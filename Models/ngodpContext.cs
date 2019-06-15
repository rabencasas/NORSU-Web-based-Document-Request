using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NGODP.Models
{
    public partial class ngodpContext : DbContext
    {
        public ngodpContext()
        {
        }

        public ngodpContext(DbContextOptions<ngodpContext> options)
            : base(options)
        {
        }

        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Datasource=ngodp.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => e.Timestamp);

                entity.ToTable("history");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .ValueGeneratedNever();

                entity.Property(e => e.Requestref).HasColumnName("requestref");

                entity.Property(e => e.Student).HasColumnName("student");

                entity.Property(e => e.Timedate)
                    .IsRequired()
                    .HasColumnName("timedate");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(e => e.Refno);

                entity.ToTable("requests");

                entity.Property(e => e.Refno)
                    .HasColumnName("refno")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Comments).HasColumnName("comments");

                entity.Property(e => e.Feedback).HasColumnName("feedback");

                entity.Property(e => e.Filedate).HasColumnName("filedate");

                entity.Property(e => e.Lacking).HasColumnName("lacking");

                entity.Property(e => e.Notification).HasColumnName("notification");

                entity.Property(e => e.Purpose).HasColumnName("purpose");

                entity.Property(e => e.Releasedate).HasColumnName("releasedate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Student).HasColumnName("student");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Uname);

                entity.ToTable("students");

                entity.Property(e => e.Uname)
                    .HasColumnName("uname")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Birthdate).HasColumnName("birthdate");

                entity.Property(e => e.Birthplace).HasColumnName("birthplace");

                entity.Property(e => e.CivilStatus).HasColumnName("civilstat");

                entity.Property(e => e.Course).HasColumnName("course");

                entity.Property(e => e.Elemaddr).HasColumnName("elemaddr");

                entity.Property(e => e.Elemname).HasColumnName("elemname");

                entity.Property(e => e.Elemyr).HasColumnName("elemyr");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Fname).HasColumnName("fname");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Graduateyr).HasColumnName("graduateyr");

                entity.Property(e => e.Isgraduate).HasColumnName("isgraduate");

                entity.Property(e => e.Lname).HasColumnName("lname");

                entity.Property(e => e.Major).HasColumnName("major");

                entity.Property(e => e.Mname).HasColumnName("mname");

                entity.Property(e => e.Mobileno).HasColumnName("mobileno");

                entity.Property(e => e.Pwd).HasColumnName("pwd");

                entity.Property(e => e.Secaddr).HasColumnName("secaddr");

                entity.Property(e => e.Secname).HasColumnName("secname");

                entity.Property(e => e.Secyr).HasColumnName("secyr");

                entity.Property(e => e.Teraddr).HasColumnName("teraddr");

                entity.Property(e => e.Tername).HasColumnName("tername");

                entity.Property(e => e.Teryr).HasColumnName("teryr");

                entity.Property(e => e.Suffix).HasColumnName("Suffix");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uname);

                entity.ToTable("users");

                entity.Property(e => e.Uname)
                    .HasColumnName("uname")
                    .ValueGeneratedNever();

                entity.Property(e => e.Fname).HasColumnName("fname");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Lname).HasColumnName("lname");

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("pwd");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("feedbacks");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Message).HasColumnName("message");

                entity.Property(e => e.TimeAndDate).HasColumnName("timeanddate");
            });
        }
    }
}
