using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class EmailVerificator
    {
        [Key]
        public int verivicationID{get;set;}
        public int user_Id{get;set;}
        public Guid verificationToken{get;set;}
        public DateTime requestDate{get;set;}
    }
}