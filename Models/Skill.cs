using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Skill
    {
        public Skill(int skill_ID, string skillDescrition) 
        {
            this.Skill_ID = skill_ID;
                this.SkillDescrition = skillDescrition;
               
        }
        [Key]
        public int Skill_ID{get;set;}
        [Required]
        public string SkillDescrition{get;set;}
    }
}