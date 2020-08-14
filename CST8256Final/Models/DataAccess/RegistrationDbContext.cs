using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Final;

namespace Final.Models.DataAccess
{
    public partial class RegistrationDbContext : DbContext
    {
        public RegistrationDbContext()
        {
        }

        public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Config.ConnectionString("RegistrationDB");
                optionsBuilder.UseSqlServer(connectionString); optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;attachdbfilename=C:\\FINAL\\APPDATA\\REGISTRATIONDB.MDF;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId)
                    .HasColumnName("CourseID")
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CourseTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FeeBase).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => new { e.CourseCourseId, e.StudentStudentNum });

                entity.Property(e => e.CourseCourseId)
                    .HasColumnName("Course_CourseID")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.StudentStudentNum)
                    .HasColumnName("Student_StudentNum")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.CourseCourse)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.CourseCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_ToCourse");

                entity.HasOne(d => d.StudentStudentNumNavigation)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.StudentStudentNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_ToStudent");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentNum);

                entity.Property(e => e.StudentNum)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });
        }
    }
}
