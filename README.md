# Atlassian Cloud Backups Tool #

This repo contains a .NET Core 2 library written in C# that duplicates the work done by the powershell script published (but not maintained) by Atlassian for backing up its cloud offerings.

Also included is a .NET Console application that leverages the library to accomplish backups for both JIRA and Confluence as well as a bare-clone backup of BitBucket repos.  No other service is currently supported but the library will eventually support all Atlassian cloud services.  While it should serve most needs, the console application is intended as an example of how to use the library to perform backups.

# Getting Started #

(Download links coming soon.  In the meantime, feel free to clone the repo and build it yourself!)

In order to use the console application, you will first need to create an appsettings.config file in the same directory as the executable DLL with the appropriate values configured (account, username and password, etc.)

```json
{
  "cloudAccount": "XXXXXXXXXX",
  "userName": "XXXXXXXXX",
  "password": "xxxxxxxxxxxxxx",
  "jiraBackupConfig": {
    "authUrl": "rest/auth/1/session",
    "backupProgressUrl": "rest/obm/1.0/getprogress",
    "backupTriggerUrl": "rest/obm/1.0/runbackup",
    "baseUrl": "https://{0}.atlassian.net/",
    "destination": "/backup-location/jira",
    "downloadUrlBase": "",
    "fileName": "jira-backup-{0}.zip",
    "backupsToKeep": 0,
    "maxAgeArchivedBackups": 30
  },
  "confluenceBackupConfig": {
    "authUrl": "rest/auth/1/session",
    "backupProgressUrl": "wiki/rest/obm/1.0/getprogress",
    "backupTriggerUrl": "wiki/rest/obm/1.0/runbackup",
    "baseUrl": "https://{0}.atlassian.net/",
    "destination": "/backup-location/confluence",
    "downloadUrlBase": "wiki/download/",
    "fileName": "confluence-backup-{0}.zip",
    "backupsToKeep": 0,
    "maxAgeArchivedBackups": 30
  },
  "bitbucketBackupConfig": {
    "backupRepoWikis": true,
    "destination": "/backup-location/bitbucket",
    "password": "XXXXXXXXX",
    "teamName": "XXXXXXXXX",
    "user": "XXXXXXXXXX",
    "userName": "XXXXXXXXX",
    "useTeam": true
  }
}

```

The file paths for the 'destination' properties can be either Unix style paths (shown in the above config example) or Windows style.  For Windows syle paths, you must escape the backslashes like this:

```
"destination": "c:\\backup-location\\bitbucket",
```

The style you use will depend on the platform that runs the .NET Core installation this tool will execute under.

Once you've created the config file, you can then run the executable like this: 

```sh
dotnet AtlassianCloudBackupsTool.dll /backupJira /backupConfluence /backupBitBucket /logPath:z:\backups /logFileName:backup.log
```

Note that the /logPath parameter is where the tool will write it's log so make sure the user executing the tool has write permissions to that location on disk, as well as the locations specified in the "destination" properties of each config object in the appsettings.config file.

The flags 'backupBitBucket', 'backupJira' and 'backupConflence' enable the respective jobs within the console application.  You can exclude any one of them from the command.
