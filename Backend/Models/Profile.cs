using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string? Idcard { get;set;}
        [NotMapped]
        public object backImage { get;set;}
        [NotMapped]
        public object frontImage { get;set;}
    }
}