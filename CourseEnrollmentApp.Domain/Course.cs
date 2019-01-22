using System.Collections.Generic;

namespace CourseEnrollmentApp.Domain
{
    public class Course
    {
        public int CourseID { get; set; }

        public string Title { get; set; }

        public int Credits { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}
