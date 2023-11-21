using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Membership
    {
        [Key]
        public int MembershipID{get;set;}
        public DateTime Expiration{get;set;}
        public bool IsMembershipActive{get
        {
            return this.Expiration <= DateTime.Now;
        }}
    }
}