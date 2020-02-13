using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace POSServices.PosMsgModels
{
    public partial class HO_MsgContext : DbContext
    {
        public HO_MsgContext()
        {
        }

        public HO_MsgContext(DbContextOptions<HO_MsgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobSynchDetailDownloadStatus> JobSynchDetailDownloadStatus { get; set; }
        public virtual DbSet<JobSynchDetailUploadStatus> JobSynchDetailUploadStatus { get; set; }
        public virtual DbSet<JobTabletoSynchDetailDownload> JobTabletoSynchDetailDownload { get; set; }
        public virtual DbSet<JobTabletoSynchDetailUpload> JobTabletoSynchDetailUpload { get; set; }
        public virtual DbSet<M3tableToSynch> M3tableToSynch { get; set; }
        public virtual DbSet<TableToSynch> TableToSynch { get; set; }

        // Unable to generate entity type for table 'dbo.M3Staging'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.FTPServerTable'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.stagingtable'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=SERVER-VSU;Database=HO_Msg;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastSynch).HasColumnType("datetime");

                entity.Property(e => e.Statusjob)
                    .IsRequired()
                    .HasColumnName("statusjob")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Waiting')");

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Synchdate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<JobSynchDetailDownloadStatus>(entity =>
            {
                entity.HasKey(e => e.SynchDetail);

                entity.Property(e => e.SynchDetail).ValueGeneratedNever();
            });

            modelBuilder.Entity<JobSynchDetailUploadStatus>(entity =>
            {
                entity.HasKey(e => e.SynchDetail);

                entity.Property(e => e.SynchDetail).ValueGeneratedNever();
            });

            modelBuilder.Entity<JobTabletoSynchDetailDownload>(entity =>
            {
                entity.HasKey(e => e.SynchDetail)
                    .HasName("PK_JobTabletoSynchDetail");

                entity.HasIndex(e => e.Id)
                    .HasName("ID_idx");

                entity.Property(e => e.CreateTable)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DownloadPath)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasColumnName("StoreID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Synchdate).HasColumnType("datetime");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TablePrimarykey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobTabletoSynchDetailUpload>(entity =>
            {
                entity.HasKey(e => e.SynchDetail);

                entity.Property(e => e.SynchDetail).ValueGeneratedNever();

                entity.Property(e => e.CreateTable)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasColumnName("StoreID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Synchdate).HasColumnType("datetime");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UploadPath)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<M3tableToSynch>(entity =>
            {
                entity.ToTable("M3TableToSynch");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdentityColumn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sqlcommand)
                    .IsRequired()
                    .HasColumnName("SQLCommand")
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TablePrimarykey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TableToSynch>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Sqlcommand)
                    .IsRequired()
                    .HasColumnName("SQLCommand")
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TablePrimarykey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
