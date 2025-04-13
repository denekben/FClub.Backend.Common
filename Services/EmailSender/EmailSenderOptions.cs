using System.ComponentModel.DataAnnotations;

namespace FClub.Backend.Common.Services.EmailSender
{
    public class EmailSenderOptions
    {
        [Required]
        public string SmtpHost { get; set; }
        [Required]
        public int SmtpPort { get; set; }
        [Required]
        public string ServiceMail { get; set; }
        [Required]
        public string MailPassword { get; set; }
    }
}