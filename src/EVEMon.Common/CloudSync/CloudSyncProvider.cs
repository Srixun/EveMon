using System;
using System.IO;
using System.Threading.Tasks;

namespace EVEMon.Common.CloudSync
{
    public interface ICloudSyncProvider
    {
        string Name { get; }
        bool IsConfigured { get; }
        Task<bool> BackupSettingsAsync(string sourceDirectory);
        Task<bool> RestoreSettingsAsync(string targetDirectory);
    }

    public class LocalFolderSyncProvider : ICloudSyncProvider
    {
        private string _targetSyncFolder;

        public LocalFolderSyncProvider(string targetFolder)
        {
            _targetSyncFolder = targetFolder;
        }

        public string Name => "Local Sync Folder (OneDrive/GDrive/iCloud)";

        public bool IsConfigured => !string.IsNullOrEmpty(_targetSyncFolder) && Directory.Exists(_targetSyncFolder);

        public async Task<bool> BackupSettingsAsync(string sourceDirectory)
        {
            if (!IsConfigured) return false;

            return await Task.Run(() =>
            {
                try
                {
                    string backupDir = Path.Combine(_targetSyncFolder, "EVEMon_Backup");
                    if (!Directory.Exists(backupDir))
                    {
                        Directory.CreateDirectory(backupDir);
                    }

                    // Simple copy of XML config files
                    foreach (var file in Directory.GetFiles(sourceDirectory, "*.xml"))
                    {
                        string fileName = Path.GetFileName(file);
                        File.Copy(file, Path.Combine(backupDir, fileName), true);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public async Task<bool> RestoreSettingsAsync(string targetDirectory)
        {
            if (!IsConfigured) return false;

            return await Task.Run(() =>
            {
                try
                {
                    string backupDir = Path.Combine(_targetSyncFolder, "EVEMon_Backup");
                    if (!Directory.Exists(backupDir)) return false;

                    foreach (var file in Directory.GetFiles(backupDir, "*.xml"))
                    {
                        string fileName = Path.GetFileName(file);
                        File.Copy(file, Path.Combine(targetDirectory, fileName), true);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}