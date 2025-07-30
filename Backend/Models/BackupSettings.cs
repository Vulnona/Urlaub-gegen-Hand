namespace UGHApi.Models
{
    /// <summary>
    /// Configuration settings for database backup service
    /// </summary>
    public class BackupSettings
    {
        /// <summary>
        /// How often to create backups (in hours)
        /// </summary>
        public int BackupIntervalHours { get; set; } = 24;

        /// <summary>
        /// How long to keep backups (in days)
        /// </summary>
        public int RetentionDays { get; set; } = 30;

        /// <summary>
        /// S3 bucket prefix for backups
        /// </summary>
        public string S3BackupPrefix { get; set; } = "backups/database/";

        /// <summary>
        /// Whether to enable automatic backups
        /// </summary>
        public bool EnableAutomaticBackups { get; set; } = true;

        /// <summary>
        /// Maximum backup file size in MB before compression
        /// </summary>
        public int MaxBackupSizeMB { get; set; } = 100;

        /// <summary>
        /// Whether to compress backups
        /// </summary>
        public bool CompressBackups { get; set; } = false;
    }
} 