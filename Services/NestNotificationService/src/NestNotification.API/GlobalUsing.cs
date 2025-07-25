﻿global using System.Linq.Expressions;
global using System.Net;
global using System.Net.Mail;
global using AutoMapper;
global using Consul;
global using EventBus.Abstractions.Abstractions;
global using EventBus.Abstractions.Abstractions;
global using EventBus.Abstractions.Events;
global using EventBus.RabbitMq;
global using EventBus.RabbitMq.Abstractions;
global using EventBus.RabbitMq.Connections;
global using EventBus.RabbitMq.Subscriptions;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Nest.Shared;
global using Nest.Shared.Abstractions;
global using Nest.Shared.Entities;
global using Nest.Shared.Exceptions;
global using Nest.Shared.Utils;
global using NestNotification.API.Abstractions.Repositories;
global using NestNotification.API.Abstractions.Services;
global using NestNotification.API.ConfigurationModels;
global using NestNotification.API.Consts;
global using NestNotification.API.Context;
global using NestNotification.API.DTOs.EmailTemplates;
global using NestNotification.API.DTOs.SendEmails;
global using NestNotification.API.DTOs.SendEmails.BasicEmail;
global using NestNotification.API.DTOs.SendEmails.BulkEmail;
global using NestNotification.API.DTOs.SendEmails.ScheduledEmail;
global using NestNotification.API.Entities;
global using NestNotification.API.Enums;
global using NestNotification.API.Extensions;
global using NestNotification.API.Extensions.BackgroundServices;
global using NestNotification.API.IntegrationHandlers;
global using NestNotification.API.Middlewares;
global using NestNotification.API.ModelConfigurations;
global using NestNotification.API.Repositories;
global using NestNotification.API.Services;
global using NestNotification.API.Validation.EmailTemplates;
global using RabbitMQ.Client;