namespace NestNotification.API.Enums;

public enum EmailPriority
{
    High = 0, // Dərhal göndərilməli (şifrə sıfırlama, 2FA)
    Normal = 1, // Standart
    Low = 2 // Planlaşdırılmış və ya marketing emailləri
}