using System.ComponentModel.DataAnnotations;
using UGHModels;

namespace UGHApi.Models
{
    public class Profile
    {
        [Key]
        public int Profile_ID{get;set;}
        public DateTime MembershipFirstActivation{get;}
        public User? UghUser {get;}
        public string? NickName {get;set;}
    }
}