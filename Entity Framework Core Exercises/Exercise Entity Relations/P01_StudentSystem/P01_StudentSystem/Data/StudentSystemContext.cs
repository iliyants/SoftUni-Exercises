using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=StudentSystem;Integrated Security=True");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurateStudentEntity(modelBuilder);
            ConfigurateCourseEntity(modelBuilder);
            ConfgurateResourceEntity(modelBuilder);
            ConfigurateHomeworkEntity(modelBuilder);
            ConfigurateStudentCoursesEntity(modelBuilder);

            modelBuilder.Seed();
        }

        private void ConfigurateHomeworkEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Homework>(e =>
            {
                e.HasKey(h => h.HomeworkId);

                e.Property(c => c.Content)
                .IsUnicode(false);

                e.Property(ct => ct.ContentType);
            });
        }

        private void ConfgurateResourceEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>(e =>
            {
                e.HasKey(r => r.ResourceId);

                e.Property(n => n.Name)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

                e.Property(u => u.Url)
                .IsUnicode(false);

                e.Property(rt => rt.ResourceType);
            });
        }

        private void ConfigurateCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(e =>
            {
                e.HasKey(c => c.CourseId);

                e.Property(n => n.Name)
                .HasColumnType("nvarchar(80)")
                .IsRequired();

                e.Property(d => d.Description)
                .IsUnicode()
                .IsRequired(false);

                e.HasMany(c => c.Resources).WithOne(r => r.Course);
                e.HasMany(c => c.StudentsEnrolled).WithOne(h => h.Course);
            });
        }

        private void ConfigurateStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(e =>
            {
                e.HasKey(s => s.StudentId);

                e.Property(n => n.Name)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

                e.Property(pn => pn.PhoneNumber)
                .HasColumnType("char(10)")
                .IsRequired(false);

                e.Property(b => b.Birthday)
                .IsRequired(false);

                e.HasMany(s => s.HomeworkSubmissions).WithOne(hs => hs.Student);
            });
        }

        private void ConfigurateStudentCoursesEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey(sc => new
                {
                    sc.StudentId,
                    sc.CourseId
                });

                e.HasOne(x => x.Student)
                .WithMany(y => y.CourseEnrollments)
                .HasForeignKey(x => x.StudentId);

                e.HasOne(x => x.Course)
                .WithMany(y => y.StudentsEnrolled)
                .HasForeignKey(x => x.CourseId);

            });
        }
    }
}
