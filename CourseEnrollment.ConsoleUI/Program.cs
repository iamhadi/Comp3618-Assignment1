using CourseEnrollmentApp.Data;
using CourseEnrollmentApp.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CourseEnrollmentApp.ConsoleUI
{

    class Program
    {
        static CourseEnrollmentContext courseEnrollmentContext = new CourseEnrollmentContext();

        static void Main(string[] args)
        {

            PopulateDatabase();
            TestUpdateOpertaion();
            TestDeleteOpertaion();

          
            
            ConsoleKeyInfo key;

            Console.WriteLine("\n**These operation only works with Enrollment");
            Console.Write("\nCreat/Retrieve/Update/Delete/eXit:");
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.X)
            {
                switch (key.Key)
                {
                    case ConsoleKey.C:
                        EnrollmentOperation.Create(courseEnrollmentContext);
                        break;

                    case ConsoleKey.R:
                        EnrollmentOperation.Retrieve(courseEnrollmentContext);

                        break;
                    case ConsoleKey.U:
                        EnrollmentOperation.Update(courseEnrollmentContext);

                        break;
                    case ConsoleKey.D:
                        EnrollmentOperation.Delete(courseEnrollmentContext);
                        break;
                    default:
                        Console.WriteLine(" Try Again!");
                        break;
                }

                Console.Write("\nCreat/Retrieve/Update/Delete/eXit:");
            }

        }

        public static void PopulateDatabase()
        {
            if (courseEnrollmentContext.Enrollments.Count() == 0)
            {
                Course course1 = new Course() { Title = "Adv C#", Credits = 4 };
                Course course2 = new Course() { Title = "React/Redux", Credits = 3 };
                Course course3 = new Course() { Title = "Angular", Credits = 3 };

                Student st1 = new Student() { FirstMidName = "John", LastName = "Travelta", EnrollmentDate = DateTime.Now };
                Student st2 = new Student() { FirstMidName = "Robert", LastName = "Deniro", EnrollmentDate = DateTime.Now };
                Student st3 = new Student() { FirstMidName = "Will", LastName = "Smith", EnrollmentDate = DateTime.Now };

                Enrollment en1 = new Enrollment() { Course = course1, Student = st1 };
                Enrollment en2 = new Enrollment() { Course = course1, Student = st2 };
                Enrollment en3 = new Enrollment() { Course = course2, Student = st3 };
                Enrollment en4 = new Enrollment() { Course = course3, Student = st2 };
                Enrollment en5 = new Enrollment() { Course = course2, Student = st1 };

                courseEnrollmentContext.Enrollments.Add(en1);
                courseEnrollmentContext.Enrollments.Add(en2);
                courseEnrollmentContext.Enrollments.Add(en3);
                courseEnrollmentContext.Enrollments.Add(en4);
                courseEnrollmentContext.Enrollments.Add(en5);

                courseEnrollmentContext.SaveChanges();
                Console.WriteLine("\nDatabase populated successfully.");
                
            }
            Console.WriteLine("\n*******  Initial Students List ************");
            StudentOperation.Retrieve(courseEnrollmentContext);

            Console.WriteLine("\n*******  Initial Course List ************");
            CourseOperation.Retrieve(courseEnrollmentContext);

            Console.WriteLine("\n*******  Initial Enrolment List ************");
            EnrollmentOperation.Retrieve(courseEnrollmentContext);
        }

        public static void TestUpdateOpertaion()
        {
            /// update : Changing John Travelta -> to Kevin Travelta
            var students = courseEnrollmentContext.Students.ToList();
            var student = students.FirstOrDefault(p => p.FirstMidName == "John" && p.LastName == "Travelta");
            if (student != null)
            {
                student.FirstMidName = "Kevin";
            }
            
            courseEnrollmentContext.SaveChanges();

            Console.WriteLine("\n*******  Students List after Changing ************");
            StudentOperation.Retrieve(courseEnrollmentContext);

            Console.WriteLine("\n*******  Enrolment List after Changing ************");
            EnrollmentOperation.Retrieve(courseEnrollmentContext);
        }

        public static void TestDeleteOpertaion()
        {
            /// Delete : Deleting EnrollmentID = 1
            var enrollments = courseEnrollmentContext.Enrollments.ToList();
            var item = enrollments.FirstOrDefault(p => p.EnrollmentID == 1);
            if (item != null)
            {
                courseEnrollmentContext.Enrollments.Remove(item);
                courseEnrollmentContext.SaveChanges();

                Console.WriteLine("\n*******  New Enrolment List : ID=1 was deleted ************");
                EnrollmentOperation.Retrieve(courseEnrollmentContext);
            }


            /// Deleting Will Smith
            /// 
            var students = courseEnrollmentContext.Students.ToList();
            var student = students.FirstOrDefault(p => p.FirstMidName == "Will" && p.LastName=="Smith");
            if (student != null)
            {
                courseEnrollmentContext.Students.Remove(student);
                courseEnrollmentContext.SaveChanges();

                Console.WriteLine("\n*******  New Students List : Will Smith was deleted ************");
                EnrollmentOperation.Retrieve(courseEnrollmentContext);
            }

        }

    }

    public static class EnrollmentOperation
    {
        public static void Create(CourseEnrollmentContext courseEnrollmentContext)
        {
            StudentOperation.Retrieve(courseEnrollmentContext);

            CourseOperation.Retrieve(courseEnrollmentContext);

            Console.Write("\nStudent ID:");
            var studentID = Convert.ToInt16(Console.ReadLine());
            var student = courseEnrollmentContext.Students.FirstOrDefault(p => p.StudentID == studentID);



            Console.Write("\nCourse ID:");
            var courseID = Convert.ToInt16(Console.ReadLine());
            var course = courseEnrollmentContext.Courses.FirstOrDefault(p => p.CourseID == courseID);

            if (course != null && student != null)
            {
                courseEnrollmentContext.Enrollments.Add(new Enrollment() { Course = course, Student = student });
                courseEnrollmentContext.SaveChanges();
            }
        }

        public static void Delete(CourseEnrollmentContext courseEnrollmentContext)
        {
            Console.Write("\nEnrollment ID:");
            var id = Console.ReadLine();
            Enrollment foundItem = courseEnrollmentContext.Enrollments.FirstOrDefault(p => p.EnrollmentID == Convert.ToInt16(id));
            if (foundItem != null)
            {
                courseEnrollmentContext.Enrollments.Remove(foundItem);
                Console.WriteLine("\nRecord was deleted.");
            }
            else
                Console.WriteLine("\nRecord was not found.");

            courseEnrollmentContext.SaveChanges();
        }

        public static void Retrieve(CourseEnrollmentContext courseEnrollmentContext)
        {
            //var enrollments = courseEnrollmentContext.Enrollments.Include(p => p.Course).Include(p => p.Student).ToList();
            var enrollments = courseEnrollmentContext.Enrollments.ToList();

            Console.WriteLine("\n\nEnrollments List:");
            foreach (var enrollment in enrollments)
            {
                Console.WriteLine($"{enrollment.EnrollmentID}- {enrollment.Student.FirstMidName} {enrollment.Student.LastName} , {enrollment.Course.Title} , {enrollment.Course.Credits} credits");
            }
            Console.WriteLine();
        }

        public static void Update(CourseEnrollmentContext courseEnrollmentContext)
        {
            var enrollments = courseEnrollmentContext.Enrollments.Include(p => p.Course).Include(p => p.Student).ToList();
            Console.Write("\nEnrollment ID:");
            var id = Console.ReadLine();
            if (id != null)
            {
                var foundItem = courseEnrollmentContext.Enrollments.FirstOrDefault(p => p.EnrollmentID == Convert.ToInt16(id));
                if (foundItem != null)
                {

                    Console.WriteLine($"\n{ foundItem.Student.FirstMidName} {foundItem.Student.LastName} was enrolled for {foundItem.Course.CourseID}-{foundItem.Course.Title} ");
                    Console.Write($"\nNew Course ID:");
                    var newCourseID = Convert.ToInt16(Console.ReadLine());
                    var newCourse = courseEnrollmentContext.Courses.FirstOrDefault(p => p.CourseID == newCourseID);
                    foundItem.Course = newCourse;

                    courseEnrollmentContext.SaveChanges();
                    Console.WriteLine("\nRecord was updated successfully.");
                }
                else
                    Console.WriteLine("\nRecord was not found.");

                courseEnrollmentContext.SaveChanges();
            }
        }
    }

    public static class StudentOperation
    {
        public static void Create(CourseEnrollmentContext courseEnrollmentContext)
        {
            Student student = new Student();
            Console.Write("\nStudent Name:");
            student.FirstMidName = Console.ReadLine();
            Console.Write("\nStudent Lastname:");
            student.LastName = Console.ReadLine();
            courseEnrollmentContext.Students.Add(student);
            courseEnrollmentContext.SaveChanges();
        }

        public static void Retrieve(CourseEnrollmentContext courseEnrollmentContext)
        {
            var students = courseEnrollmentContext.Students.ToList();
            Console.WriteLine("\n\t\tStudents List:");
            foreach (var student in students)
            {               
                Console.WriteLine($"\t\t\t{student.StudentID}- {student.FirstMidName} {student.LastName}");  
            }
        }
    }

    public static class CourseOperation
    {
        public static void Create(CourseEnrollmentContext courseEnrollmentContext)
        {
            Course course = new Course();
            Console.Write("\nCourse Name:");
            course.Title = Console.ReadLine();
            Console.Write("\nCredits:");
            course.Credits = Convert.ToInt16(Console.ReadLine());
            courseEnrollmentContext.Courses.Add(course);
            courseEnrollmentContext.SaveChanges();
        }

        public static void Retrieve(CourseEnrollmentContext courseEnrollmentContext)
        {
            var courses = courseEnrollmentContext.Courses.ToList();
            Console.WriteLine("\n\t\tCourses List:");
            foreach (var course in courses)
            {
                Console.WriteLine($"\t\t\t{course.CourseID}- {course.Title}  , {course.Credits} credits");
            }
            Console.WriteLine();
        }
    }
}
