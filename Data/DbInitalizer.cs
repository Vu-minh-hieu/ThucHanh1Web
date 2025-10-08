using Microsoft.EntityFrameworkCore;
using ThucHanh1.Models.Entities;
using ThucHanh1.Data;

namespace ThucHanh1.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new SchoolContext(
                serviceProvider.GetRequiredService<DbContextOptions<SchoolContext>>()))
            {
                // Tạo database nếu chưa có
                context.Database.EnsureCreated();

                // Nếu đã có dữ liệu Major thì return
                if (context.Majors.Any())
                {
                    return;
                }

                // Seed dữ liệu cho Major
                var majors = new Major[]
                {
                new Major { MajorName = "IT" },
                new Major { MajorName = "Economics" },
                new Major { MajorName = "Mathematics" },
                };
                foreach (var major in majors)
                {
                    context.Majors.Add(major);
                }
                context.SaveChanges();

                // Seed dữ liệu cho Learner
                var learners = new Learner[]
                {
                new Learner { FirstMidName = "Carson", LastName = "Alexander",
                              EnrollmentDate = DateTime.Parse("2005-09-01"), MajorID = 1 },
                new Learner { FirstMidName = "Meredith", LastName = "Alonso",
                              EnrollmentDate = DateTime.Parse("2002-09-01"), MajorID = 2 }
                };
                foreach (var learner in learners)
                {
                    context.Learners.Add(learner);
                }
                context.SaveChanges();

                // Seed dữ liệu cho Course
                var courses = new Course[]
                {
                new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
                new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
                new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 }
                };
                foreach (var course in courses)
                {
                    context.Courses.Add(course);
                }
                context.SaveChanges();

                // Seed dữ liệu cho Enrollment (liên kết Learner - Course)
                var enrollments = new Enrollment[]
                {
                new Enrollment { LearnerID = 1, CourseID = 1050, Grade = 5.5f },
                new Enrollment { LearnerID = 1, CourseID = 4022, Grade = 7.5f },
                new Enrollment { LearnerID = 1, CourseID = 1050, Grade = 3.5f },
                new Enrollment { LearnerID = 2, CourseID = 4041, Grade = 7f }
                };
                foreach (var e in enrollments)
                {
                    context.Enrollments.Add(e);
                }
                context.SaveChanges();
            }
        }
    }

}