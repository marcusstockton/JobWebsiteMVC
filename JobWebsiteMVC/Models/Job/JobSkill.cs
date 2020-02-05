using System.Collections.Generic;

namespace JobWebsiteMVC.Models.Job
{
    public class JobSkill : Base  
    {
        public string Description { get; set; }
        public virtual ICollection<Job_JobSkill> Job_JobSkills { get; set; }
    }
}