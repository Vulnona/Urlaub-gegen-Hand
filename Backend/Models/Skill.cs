using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Skill
    {
        public Skill(int skill_ID, string skillDescrition, Skill parentSkill)
        {
            this.Skill_ID = skill_ID;
            this.SkillDescrition = skillDescrition;
            ParentSkill = parentSkill;
        }
        [Key]
        public int Skill_ID{get;}
        [Required]
        public string SkillDescrition{get;set;}
        public Skill ParentSkill{get;set;}
    }
}