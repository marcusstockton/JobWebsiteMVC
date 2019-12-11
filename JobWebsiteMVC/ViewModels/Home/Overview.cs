namespace JobWebsiteMVC.ViewModels.Home
{
    public class Overview
    {
        public int ActiveJobCount { get; set; }
        public int DraftJobs{get;set;}
        public int FutureJobs{get;set;}
        public int ClosedJobs{get;set;}
        public int UserCount { get; set; }
        public int JobSeekingUserCount { get; set; }
        public int EmployerCount { get; set; }
    }
}