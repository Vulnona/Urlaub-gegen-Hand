using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class PasswordResetToken
{
    [Key]
    public int TokenId{get;set;}
    public Guid user_Id{get;set;}
    public Guid Token{get;set;}
    public DateTime requestDate{get;set;}
}
