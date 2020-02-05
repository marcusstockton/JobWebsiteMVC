using System;

namespace JobWebsiteMVC.Models.Job
{
    public class Job_JobSkill
    {
        public Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        public Guid JobSkillId { get; set; }
        public virtual JobSkill JobSkill { get; set; }
    }
}