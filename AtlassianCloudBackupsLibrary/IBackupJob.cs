namespace AtlassianCloudBackupsLibrary
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;

    public interface IBackupJob
    {
        Func<IConfigurationSection, Task> PrepareFunction { get; set; }
        Func<IConfigurationSection, Task> CleanUpFunction { get; set; }

        Task<IBackupJob> Execute(bool runCleanupOnly = false);
        Task<IBackupJob> Prepare();
        Task<IBackupJob> CleanUp();
    }
}
