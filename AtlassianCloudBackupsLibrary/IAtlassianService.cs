namespace AtlassianCloudBackupsLibrary
{
    public interface IAtlassianService
    {
        string BaseUrl { get; set; }
        string AuthUrl { get; set; }
        string BackupTriggerUrl { get; set; }
        string BackupProgressUrl { get; set; }
        string DownloadUrlBase { get; set; }
        string LastTaskIdUrl { get; set; }
    }
}
