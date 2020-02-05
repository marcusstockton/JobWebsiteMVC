using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JobWebsiteMVC.Areas.Identity.Controllers
{
    public class JobCreateController: Controller
    {
        public List<string> GetJobTitleAutoComplete(string jobTitle)
        {
            var url = "http://api.dataatwork.org/v1/jobs/autocomplete?contains=" + jobTitle;
            string html = string.Empty;
            var words = new List<string>();

            var request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        var deserialisedData = JsonConvert.DeserializeObject<List<JobTitleAutoComplete>>(html);
                        words.AddRange(deserialisedData.Select(x=>x.suggestion));
                    }
                }
            }
            return words;
        }

        public List<string> GetJobSkillAutoComplete(string skillName) // NOT YET USED
        {
            var url = "http://api.dataatwork.org/v1/skills/autocomplete?contains=" + skillName;
            string html = string.Empty;
            var words = new List<string>();

            var request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        var deserialisedData = JsonConvert.DeserializeObject<List<SkillAutoComplete>>(html);
                        words.AddRange(deserialisedData.Select(x=>x.skill_name));
                    }
                }
            }
            return words;
        }
    }
    public class JobTitleAutoComplete
    {
        public Guid uuid { get; set; }
        public string suggestion { get; set; }
        public string normalized_job_title{get;set;}
        public Guid parent_uuid{get;set;}
    }
    public class SkillAutoComplete
    {
        public Guid skill_uuid{get;set;}
        public string skill_name{get;set;}
        public string normalized_job_title{get;set;}
    }
}