namespace AtlassianCloudBackupsLibrary
{
    public class JiraService : IAtlassianService
    {
        public string BaseUrl { get; set; }
        public string AuthUrl { get; set; }
        public string DownloadUrlBase { get; set; }
        public string BackupTriggerUrl { get; set; }
        public string BackupProgressUrl { get; set; }
        public string LastTaskIdUrl { get; set; }

        public JiraService()
        {
            BaseUrl = "https://{0}.atlassian.net/";
            AuthUrl = "rest/auth/1/session";
            BackupTriggerUrl = "rest/backup/1/export/runbackup";
            BackupProgressUrl = "rest/backup/1/export/getProgress?taskId={0}";
            DownloadUrlBase = "plugins/servlet/export/download/";
            LastTaskIdUrl = "rest/backup/1/export/lastTaskId";
        }
    }
}
