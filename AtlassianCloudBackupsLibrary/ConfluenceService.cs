namespace AtlassianCloudBackupsLibrary
{
    public class ConfluenceService : IAtlassianService
    {
        public string BaseUrl { get; set; }
        public string AuthUrl { get; set; }
        public string BackupTriggerUrl { get; set; }
        public string BackupProgressUrl { get; set; }
        public string DownloadUrlBase { get; set; }
        public string LastTaskIdUrl { get; set; }

        public ConfluenceService()
        {
            BaseUrl = "https://{0}.atlassian.net/";
            AuthUrl = "rest/auth/1/session";
            BackupTriggerUrl = "wiki/rest/obm/1.0/runbackup";
            BackupProgressUrl = "wiki/rest/obm/1.0/getprogress";
            DownloadUrlBase = "wiki/download/";
            LastTaskIdUrl = string.Empty;
        }
    }
}
