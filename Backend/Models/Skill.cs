using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Skill
    {
     
        [Key]
        public int Skill_ID{get;set;}
        public string SkillDescrition{get;set;}
        public Nullable<int>  ParentSkill_ID{get;set;}
    }
}