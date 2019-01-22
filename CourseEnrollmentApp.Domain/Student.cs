using System;
using System.Collections.Generic;

namespace CourseEnrollmentApp.Domain
{
    public class Student
    {

        public int StudentID { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
