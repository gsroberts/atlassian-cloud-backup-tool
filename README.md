# Atlassian Cloud Backups Tool #

This repo contains a .NET Core 1.1 library written in C# that duplicates the work done by the powershell script published (but not maintained) by Atlassian for backing up its cloud offerings.

Also included is a .NET Console application that leverages the library to accomplish backups for both JIRA and Confluence.  No other service is currently supported but the library will eventually support all Atlassian cloud services.  While it should serve most needs, the console application is intended as an example of how to use the library to perform backups.

# Getting Started #

Download the app ([Latest Version](https://bitbucket.org/gsroberts/atlassian-cloud-backup-tool/downloads/AtlassianCloudBackupsTool-672017.zip))

In order to use the console application as-is, you will first need to supply the appropriate values in the appsettings.config file (account, username and password) and then run the executable like this: 

```sh
dotnet AtlassianCloudBackupsTool.dll /backupJira /backupConfluence /logPath:z:\backups /logFileName:backup.log
```

The flags 'backupJira' and 'backupConflence' enable the respective jobs within the console application