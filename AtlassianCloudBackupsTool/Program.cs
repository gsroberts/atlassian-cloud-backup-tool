namespace AtlassianCloudBackupsTool
{
    using System;
    using System.Threading.Tasks;
    using AtlassianCloudBackupsLibrary;
    using Microsoft.Extensions.Configuration;
    using AtlassianCloudBackupsLibrary.Helpers;
    using System.IO;

    class Program
    {
        private static string _logLabel;
        private static IConfigurationRoot _config;

        public static void Main(string[] args)
        {
            ParameterCollection parameters = new ParameterCollection(args);

            var logLocation = parameters.GetParameterValue<string>("logPath") ?? Path.GetTempPath();
            var logFileName = parameters.GetParameterValue<string>("logFileName") ?? "atlassian-could-backup-tool.log";
            Console.WriteLine($"Log file can be found at {logLocation}/{logFileName}");
            
            _logLabel = "Backup-Tool";

            Logger.Current.Init(logLocation, logFileName);
            Logger.Current.Log(_logLabel, "======================================================================");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);

            _config = builder.Build();

            try
            {
                RunBackupAsync(parameters).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Current.Log(_logLabel, string.Format("EXCEPTION: {0} - {1}", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Async task that runs the bulk of the backup operation
        /// 
        /// This is only required because you can't have an async entry point into a console application
        /// </summary>
        /// <param name="args">ParameterCollection parsed from the parameter array passed in via the command line</param>
        /// <returns>Awaitable task</returns>
        public static async Task RunBackupAsync(ParameterCollection args)
        {
            var runConfluenceBackup = args.GetParameterValue<bool>("backupConfluence");
            var runJiraBackup = args.GetParameterValue<bool>("backupJira");
            var runBitBucketBackup = args.GetParameterValue<bool>("backupBitBucket");
            var runCleanUpOnly = args.GetParameterValue<bool>("runCleanUpOnly");

            var account = _config["cloudAccount"];
            var username = _config["userName"];
            var password = _config["password"];

            if (runConfluenceBackup)
            {
                var confluenceConfig = _config.GetSection("confluenceBackupConfig");
                var backupConfluence = new BackupAtlassianService<ConfluenceService>(confluenceConfig)
                {
                    Account = account,
                    UserName = username,
                    Password = password,
                    CleanUpFunction = TrimBackups
                };

                await backupConfluence.Execute(runCleanUpOnly);
            }

            if (runJiraBackup)
            {
                var jiraConfig = _config.GetSection("jiraBackupConfig");
                var backupJira = new BackupAtlassianServiceV2<JiraService>(jiraConfig)
                {
                    Account = account,
                    UserName = username,
                    Password = password,
                    CleanUpFunction = TrimBackups
                };

                await backupJira.Execute(runCleanUpOnly);
            }

            if (runBitBucketBackup)
            {
                var bitbucketConfig = _config.GetSection("bitbucketBackupConfig");
                var backupBitBucket = new BackupBitBucketService(bitbucketConfig);

                await backupBitBucket.Execute(runCleanUpOnly);
            }
        }

        /// <summary>
        /// Task designed to trim backups to a maintainable size
        /// </summary>
        /// <param name="config">IConfigurationSection that contains the properties referenced in the task</param>
        /// <returns>Awaitable task</returns>
        private static async Task TrimBackups(IConfigurationSection config)
        {          
            var backupDir = Directory.GetFiles(config["destination"]);
            
            Logger.Current.Log(_logLabel, string.Format("Cleaning up archived backups located at {0}", config["destination"]));
            
            var today = DateTime.Today.Date;

            var backupsToKeep = int.Parse(config["backupsToKeep"]);

            if (backupsToKeep != 0)
            {
                var oldestDateToKeep = today.AddDays(backupsToKeep * -1);

                var archivePath = Path.Combine(config["destination"], "archive");

                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }

                foreach (var file in backupDir)
                {
                    var fileInfo = new FileInfo(file);

                    // Archive old backups beyond the keep threshold specified in appsettings.config for this job
                    if (fileInfo.LastWriteTime < oldestDateToKeep)
                    {
                        fileInfo.CopyTo(Path.Combine(archivePath, fileInfo.Name));

                        fileInfo.Delete();
                        Logger.Current.Log(_logLabel, string.Format("Archived old backup file {0}", fileInfo.FullName));
                    }
                }

                // Trim archived backups based on max age of archived backups specified in appsettings.config for this job
                var maxAgeArchivedBackups = int.Parse(config["maxAgeArchivedBackups"]);
                var ageThreshold = today.AddDays(maxAgeArchivedBackups*-1);

                var entriesToRemove = Directory.GetFiles(archivePath);

                foreach (var entry in entriesToRemove)
                {
                    var lastWriteTime = File.GetLastWriteTime(entry);

                    if (lastWriteTime < ageThreshold)
                    {
                        File.Delete(entry);
                    }
                }
            }

            await Task.Yield();
        }
    }
}