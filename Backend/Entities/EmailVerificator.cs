using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class EmailVerificator
{
    [Key]
    public int verificationId{get;set;}
    public Guid user_Id{get;set;}
    public Guid verificationToken{get;set;}
    public DateTime requestDate{get;set;}
}