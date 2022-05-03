using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JobWebsiteMVC.Areas.Identity.Controllers
{
    public class JobController : Controller
    {
        public async Task<Dictionary<string, string>> GetJobTitleAutoCompleteAsync(string jobTitle)
        {
            var url = "http://api.dataatwork.org/v1/jobs/autocomplete?contains=" + jobTitle;
            string html = string.Empty;
            var words = new Dictionary<string, string>();

            var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var request = await myClient.GetAsync(url);

            if (request.IsSuccessStatusCode)
            {
                using (Stream stream = await request.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        var deserialisedData = JsonConvert.DeserializeObject<List<JobTitleAutoComplete>>(html);
                        words = deserialisedData.ToDictionary(x => x.uuid.ToString(), x => x.suggestion);
                    }
                }
            }
            return words;
        }

        public async Task<List<string>> GetJobSkillAutoCompleteAsync(string jobId)
        {
            var url = $"http://api.dataatwork.org/v1/jobs/{Guid.Parse(jobId).ToString("n")}/related_skills";
            string html = string.Empty;
            var skills = new List<string>();

            var myClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            var request = await myClient.GetAsync(url);

            if (request.IsSuccessStatusCode)
            {
                using (Stream stream = await request.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        var deserialisedData = JsonConvert.DeserializeObject<SkillAutoComplete>(html);
                        skills = deserialisedData.skills.Where(x => double.Parse(x.importance) > 4.0).Select(x => x.description).ToList();
                    }
                }
            }

            return skills;
        }
    }

    internal class JobTitleAutoComplete
    {
        public Guid uuid { get; set; }
        public string suggestion { get; set; }
        public string normalized_job_title { get; set; }
        public Guid parent_uuid { get; set; }
    }

    [JsonObject]
    internal class SkillAutoComplete
    {
        public Guid job_uuid { get; set; }
        public string job_title { get; set; }
        public string normalized_job_title { get; set; }
        public List<Skill> skills { get; set; }
    }

    [JsonObject]
    internal class Skill
    {
        public Guid skill_uuid { get; set; }
        public string skill_name { get; set; }
        public string description { get; set; }
        public string importance { get; set; }
    }
}