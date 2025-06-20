namespace NestNotification.API.Services;

public class EmailService : IEmailService
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IEmailLogRepository _emailLogRepository;
    private readonly ILogger<EmailService> _logger;
    private readonly IMapper _mapper;

    public EmailService(
        IEmailQueueRepository emailQueueRepository,
        IEmailTemplateRepository emailTemplateRepository,
        IEmailLogRepository emailLogRepository,
        ILogger<EmailService> logger,
        IMapper mapper)
    {
        _emailQueueRepository = emailQueueRepository;
        _emailTemplateRepository = emailTemplateRepository;
        _emailLogRepository = emailLogRepository;
        _logger = logger;
        _mapper = mapper;
    }

    #region Base Email Sending

    public async Task<string> EmailSender(MailRequest request)
    {
        try
        {
            var emailProvider = Configurations.GetConfiguration<NotificationProviders>().Email;

            var smtpClient = new SmtpClient
            {
                Host = emailProvider.Primary.SmtpSettings.Host,
                Port = emailProvider.Primary.SmtpSettings.Port,
                Credentials = new NetworkCredential(emailProvider.Primary.SmtpSettings.Username, emailProvider.Primary.SmtpSettings.Password),
                EnableSsl = emailProvider.Primary.SmtpSettings.EnableSsl,
                Timeout = emailProvider.Primary.SmtpSettings.TimeoutInSeconds
            };

            var message = new MailMessage
            {
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = request.Body.Contains("</html>") || request.Body.Contains("</body>"),
                From = new(emailProvider.Primary.FromEmail, emailProvider.Primary.DisplayName)
            };
            message.To.Add(request.ToEmail);

            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var attachment in request.Attachments)
                {
                    using var ms = new MemoryStream(attachment.File);
                    message.Attachments.Add(new(ms, attachment.FileName, attachment.FileMimeType));
                }
            }

            await smtpClient.SendMailAsync(message);

            var map = _mapper.Map<EmailLog>(request);

            await _emailLogRepository.AddAsync(map);

            await _emailLogRepository.SaveChangesAsync();

            return map.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email sending failed to {Email}", request.ToEmail);
            throw;
        }
    }

    #endregion Base Email Sending

    #region Basic Email Sending

    public async Task<ResponseDto> SendEmailAsync(SendEmailDto emailDto)
    {
        var map = _mapper.Map<EmailQueue>(emailDto);

        await _emailQueueRepository.AddAsync(map);
        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Email queued with ID: {EmailId}", map.Id);
        return new()
        {
            IsSuccess = true,
            Message = "Email queued successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                EmailQueueId = map.Id
            }
        };
    }

    public async Task<ResponseDto> SendTemplatedEmailAsync(SendTemplatedEmailDto emailDto)
    {
        var template = await _emailTemplateRepository.GetByIdAsync(emailDto.TemplateId);
        if (template == null)
        {
            throw new EntityNotFoundException($"This email template not found");
        }

        var body = template.Body;

        foreach (var param in emailDto.Placeholders)
        {
            body = body.Replace(string.Concat("{{", param.Key, "}}"), param.Value);
        }

        var map = _mapper.Map<EmailQueue>(emailDto);

        map.Subject = template.Subject;
        map.Body = body;
        map.IsHtml = template.IsHtml;

        await _emailQueueRepository.AddAsync(map);
        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Templated email queued with ID: {EmailId}", map.Id);

        return new()
        {
            IsSuccess = true,
            Message = "Templated email queued successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                EmailQueueId = map.Id
            }
        };
    }

    public async Task<ResponseDto> SendBulkEmailAsync(SendBulkEmailDto emailDto)
    {
        var emailQueues = new List<EmailQueue>();

        foreach (var recipient in emailDto.Recipients)
        {
            var map = _mapper.Map<EmailQueue>(emailDto);
            map.ToEmail = recipient;
            emailQueues.Add(map);
        }

        await _emailQueueRepository.AddRangeAsync(emailQueues);
        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Bulk email queued for {Count} recipients", emailDto.Recipients.Count);

        return new()
        {
            IsSuccess = true,
            Message = "Bulk email queued successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> SendBulkTemplatedEmailAsync(SendBulkTemplatedEmailDto emailDto)
    {
        var template = await _emailTemplateRepository.GetByIdAsync(emailDto.TemplateId);
        if (template == null)
        {
            throw new EntityNotFoundException($"This email template not found");
        }

        var emailQueues = new List<EmailQueue>();

        var body = template.Body;
        foreach (var param in emailDto.Placeholders)
        {
            body = body.Replace($"{{{param.Key}}}", param.Value);
        }

        foreach (var recipient in emailDto.Recipients)
        {
            emailQueues.Add(new()
            {
                ToEmail = recipient,
                Subject = template.Subject,
                Body = body,
                IsHtml = template.IsHtml,
                Priority = emailDto.Priority
            });
        }

        await _emailQueueRepository.AddRangeAsync(emailQueues);
        await _emailQueueRepository.SaveChangesAsync();
        _logger.LogInformation("Bulk templated email queued for {Count} recipients", emailDto.Recipients.Count);

        return new()
        {
            IsSuccess = true,
            Message = "Bulk email queued successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public ResponseDto GetEmailLogs(int page, int pageSize)
    {
        Expression<Func<EmailLog, object>> order = x => x.SentAt;

        var res = _emailLogRepository.GetAll(page, pageSize, false, order).Where(e => e != null).ToList();
        var map = _mapper.Map<List<GetEmailLogDto>>(res);
        return new()
        {
            IsSuccess = true,
            Message = "Email logs retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = map
        };
    }

    public async Task<ResponseDto> GetEmailStatusAsync(string emailId)
    {
        var email = await _emailQueueRepository.GetByIdAsync(emailId);

        if (email == null)
        {
            throw new EntityNotFoundException($"Email with ID {emailId} not found");
        }

        return new()
        {
            IsSuccess = true,
            Message = "Email status retrieved successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                EmailStatus = email?.Status.ToString()
            }
        };
    }

    public async Task<ResponseDto> ResendFailedEmailAsync(string emailId)
    {
        var email = await _emailQueueRepository.GetByIdAsync(emailId);
        if (email == null || email.Status != EmailStatus.Failed)
        {
            throw new($"Email with ID {emailId} not found or is not in Failed status");
        }

        email.Status = EmailStatus.Pending;
        email.RetryCount = 0;
        email.ErrorMessage = null;
        email.LastAttempt = DateTime.UtcNow;

        _emailQueueRepository.Update(email);
        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Failed email {EmailId} marked for resending", emailId);

        return new()
        {
            IsSuccess = true,
            Message = "Failed email marked for resending",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> RetryAllFailedEmailsAsync(int maxRetries = 3)
    {
        var failedEmails =
            _emailQueueRepository.GetAllByExpression(x => x.Status == EmailStatus.Failed && x.RetryCount < maxRetries);

        if (failedEmails.Items != null)
        {
            foreach (var email in failedEmails.Items)
            {
                if (email != null)
                {
                    email.RetryCount = 0;
                    email.Status = EmailStatus.Pending;
                    email.ErrorMessage = null;
                    _emailQueueRepository.Update(email);
                    _emailQueueRepository.Update(email);
                }
            }

            _logger.LogInformation("Marked {Count} failed emails for retry", failedEmails.Count);
            await _emailQueueRepository.SaveChangesAsync();

            return new()
            {
                IsSuccess = true,
                Message = "All failed emails marked for retry",
                StatusCode = StatusCodes.Status200OK
            };
        }

        return new()
        {
            IsSuccess = false,
            Message = "No failed emails found",
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    public async Task<ResponseDto> CancelPendingEmailAsync(string emailId)
    {
        var email = await _emailQueueRepository.GetByIdAsync(emailId);
        if (email == null || email.Status != EmailStatus.Pending)
        {
            throw new EntityNotFoundException($"Email with ID {emailId} not found or is not in Pending status");
        }

        email.Status = EmailStatus.Cancelled;
        _emailQueueRepository.Update(email);
        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Pending email {EmailId} cancelled", emailId);

        return new()
        {
            IsSuccess = true,
            Message = "Pending email cancelled",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> CancelAllPendingEmailsAsync(string recipientEmail)
    {
        var pendingEmails = _emailQueueRepository.GetAllByExpression(x => x.ToEmail == recipientEmail);

        if (pendingEmails.Count == 0)
        {
            throw new EntityNotFoundException($"No pending emails found for recipient {recipientEmail}");
        }

        foreach (var email in pendingEmails.Items)
        {
            if (email != null)
            {
                email.Status = EmailStatus.Cancelled;
                _emailQueueRepository.Update(email);
            }
        }

        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Cancelled {Count} pending emails for recipient {Recipient}",
            pendingEmails.Count,
            recipientEmail);

        return new()
        {
            IsSuccess = true,
            Message = "All pending emails cancelled",
            StatusCode = StatusCodes.Status200OK
        };
    }

    #endregion Basic Email Sending

    #region Scheduled Email Methods

    public async Task<ResponseDto> ScheduleEmailAsync(SendScheduledEmailDto emailDto)
    {
        var map = _mapper.Map<EmailQueue>(emailDto);

        await _emailQueueRepository.AddAsync(map);
        await _emailQueueRepository.SaveChangesAsync();
        _logger.LogInformation("Email scheduled for {ScheduledTime} with ID: {EmailId}", emailDto.ScheduledAt, map.Id);

        return new()
        {
            IsSuccess = true,
            Message = "Email scheduled successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                EmailQueueId = map.Id
            }
        };
    }

    public async Task<ResponseDto> ScheduleTemplatedEmailAsync(SendScheduledTemplateEmailDto emailDto)
    {
        var template = await _emailTemplateRepository.GetByIdAsync(emailDto.TemplateId);
        if (template == null)
        {
            throw new EntityNotFoundException($"This email template not found");
        }

        var body = template.Body;

        foreach (var param in emailDto.Placeholders)
        {
            body = body.Replace($"{{{param.Key}}}", param.Value);
        }

        var map = _mapper.Map<EmailQueue>(emailDto);

        map.Subject = template.Subject;
        map.Body = body;
        map.IsHtml = template.IsHtml;

        await _emailQueueRepository.AddAsync(map);
        await _emailQueueRepository.SaveChangesAsync();

        _logger.LogInformation("Templated email scheduled for {ScheduledTime} with ID: {EmailId}",
            emailDto.ScheduledAt,
            map.Id);

        return new()
        {
            IsSuccess = true,
            Message = "Templated email scheduled successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                EmailQueueId = map.Id
            }
        };
    }

    public async Task<ResponseDto> ScheduleBulkEmailAsync(SendScheduleBulkEmailDto emailDto)
    {
        var emailQueues = new List<EmailQueue>();

        foreach (var recipient in emailDto.Recipients)
        {
            emailQueues.Add(new()
            {
                ToEmail = recipient,
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsHtml = emailDto.IsHtml,
                Priority = EmailPriority.Scheduled,
                ScheduledAt = emailDto.ScheduledAt
            });
        }

        await _emailQueueRepository.AddRangeAsync(emailQueues);
        await _emailQueueRepository.SaveChangesAsync();
        _logger.LogInformation("Bulk email scheduled for {ScheduledTime} for {Count} recipients",
            emailDto.ScheduledAt,
            emailDto.Recipients.Count);

        return new()
        {
            IsSuccess = true,
            Message = "Bulk email scheduled successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> ScheduleBulkTemplatedEmailAsync(SendScheduleBulkTemplatedEmailDto emailDto)
    {
        var template = await _emailTemplateRepository.GetByIdAsync(emailDto.TemplateId);
        if (template == null)
        {
            throw new EntityNotFoundException($"This email template not found");
        }

        var emailQueues = new List<EmailQueue>();

        foreach (var recipient in emailDto.Recipients)
        {
            // Parametrləri şablona yerləşdir
            var body = template.Body;
            foreach (var param in emailDto.Placeholders)
            {
                body = body.Replace($"{{{param.Key}}}", param.Value);
            }

            emailQueues.Add(new()
            {
                ToEmail = recipient,
                Subject = template.Subject,
                Body = body,
                IsHtml = template.IsHtml,
                Priority = EmailPriority.Scheduled,
                ScheduledAt = emailDto.ScheduledAt
            });
        }

        await _emailQueueRepository.AddRangeAsync(emailQueues);
        await _emailQueueRepository.SaveChangesAsync();
        _logger.LogInformation("Bulk templated email scheduled for {ScheduledTime} for {Count} recipients",
            emailDto.ScheduledAt,
            emailDto.Recipients.Count);

        return new()
        {
            IsSuccess = true,
            Message = "Bulk email scheduled successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    #endregion Scheduled Email Methods

    #region Process Emails Queue

    public async Task ProcessEmailQueueAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // 1. Əvvəlcə yüksək prioritetli emailləri emal et
                var highPriorityEmails = _emailQueueRepository.GetAllByExpression(1,
                    10,
                    x => x.Priority == EmailPriority.High &&
                         x.Status == EmailStatus.Pending,
                    false).Items;

                foreach (var email in highPriorityEmails)
                {
                    if (email != null)
                    {
                        await ProcessSingleEmailAsync(email);
                    }
                }

                // 2. Planlaşdırılmış emaillərdən vaxtı çatanları emal et
                var scheduledEmails = _emailQueueRepository.GetAllByExpression(
                    x => x.Status == EmailStatus.Pending &&
                         x.Priority == EmailPriority.Scheduled &&
                         x.ScheduledAt.HasValue &&
                         x.ScheduledAt.Value.Date == DateTime.UtcNow.Date &&
                         x.ScheduledAt.Value.Hour == DateTime.UtcNow.Hour &&
                         x.ScheduledAt.Value.Minute == DateTime.UtcNow.Minute,
                    false).Items;

                foreach (var email in scheduledEmails)
                {
                    if (email != null)
                    {
                        await ProcessSingleEmailAsync(email);
                    }
                }

                // 3. Normal prioritetli emailləri emal et
                var normalPriorityEmails = _emailQueueRepository.GetAllByExpression(1,
                    20,
                    x => x.Priority == EmailPriority.Normal &&
                         x.Status == EmailStatus.Pending,
                    true).Items;

                foreach (var email in normalPriorityEmails)
                {
                    if (email != null)
                    {
                        await ProcessSingleEmailAsync(email);
                    }
                }

                // 4. Aşağı prioritetli emailləri emal et
                var lowPriorityEmails = _emailQueueRepository.GetAllByExpression(1,
                    20,
                    x => x.Priority == EmailPriority.Low &&
                         x.Status == EmailStatus.Pending,
                    false).Items;

                foreach (var email in lowPriorityEmails)
                {
                    if (email != null)
                    {
                        await ProcessSingleEmailAsync(email);
                    }
                }

                Console.WriteLine(DateTime.Now.ToLongTimeString());

                var retryPolicy = Configurations.GetConfiguration<ResiliencePatterns>().RetryPolicy;
                await Task.Delay(retryPolicy.RetryDelayMilliseconds, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email queue");
            }
        }
    }

    private async Task ProcessSingleEmailAsync(EmailQueue email)
    {
        try
        {
            MailRequest mailRequest = new()
            {
                ToEmail = email.ToEmail,
                Subject = email.Subject,
                Body = email.Body,
                QueueId = email.Id
            };

            await EmailSender(mailRequest);

            email.Status = EmailStatus.Success;
            email.LastAttempt = DateTime.UtcNow;
            _emailQueueRepository.Update(email);
        }
        catch (Exception ex)
        {
            email.RetryCount++;
            email.ErrorMessage = ex.Message;

            var retryPolicy = Configurations.GetConfiguration<ResiliencePatterns>().RetryPolicy;

            var retryCount = retryPolicy.MaxRetryAttempts;

            if (email.RetryCount >= retryCount)
            {
                email.Status = EmailStatus.Failed;
            }

            _emailQueueRepository.Update(email);
            _logger.LogError(ex, "Failed to send email {EmailId} to {Recipient}", email.Id, email.ToEmail);
        }

        await _emailQueueRepository.SaveChangesAsync();
    }

    #endregion Process Emails Queue
}