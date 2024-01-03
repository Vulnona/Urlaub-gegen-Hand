using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Skill
    {
        /* public Skill(int skill_ID, string skillDescrition,int? parentSkill_id)
        {
            this.Skill_ID = skill_ID;
            this.SkillDescrition = skillDescrition;
            ParentSkill_ID = parentSkill_id;
        } */
        [Key]
        public int Skill_ID{get;set;}
        public string? SkillDescrition{get;set;}
        public Nullable<int>  ParentSkill_ID{get;set;}
    }
}