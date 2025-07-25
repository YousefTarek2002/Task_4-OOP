namespace Bonus_Task
{

    class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();

        public bool Enroll(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                return true;
            }
            return false;
        }

        public string PrintDetails()
        {
            string courseNames = Courses.Count > 0 ? string.Join(", ", Courses.ConvertAll(c => c.Title)) : "None";
            return $"ID: {StudentId}, Name: {Name}, Age: {Age}, Courses: {courseNames}";
        }
    }


    class Instructor
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public string PrintDetails()
        {
            return $"ID: {InstructorId}, Name: {Name}, Specialization: {Specialization}";
        }
    }


    class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public Instructor Instructor { get; set; }

        public string PrintDetails()
        {
            string instructorName = Instructor != null ? Instructor.Name : "Unassigned";
            return $"ID: {CourseId}, Title: {Title}, Instructor: {instructorName}";
        }
    }


    class SchoolStudentManager
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        public bool AddStudent(Student student)
        {
            if (FindStudent(student.StudentId) == null)
            {
                Students.Add(student);
                return true;
            }
            return false;
        }

        public bool AddCourse(Course course)
        {
            if (FindCourse(course.CourseId) == null)
            {
                Courses.Add(course);
                return true;
            }
            return false;
        }

        public bool AddInstructor(Instructor instructor)
        {
            if (FindInstructor(instructor.InstructorId) == null)
            {
                Instructors.Add(instructor);
                return true;
            }
            return false;
        }

        public Student FindStudent(int id) => Students.Find(s => s.StudentId == id);
        public Student FindStudent(string name) => Students.Find(s => s.Name.ToLower() == name.ToLower());
        public Course FindCourse(int id) => Courses.Find(c => c.CourseId == id);
        public Course FindCourse(string title) => Courses.Find(c => c.Title.ToLower() == title.ToLower());
        public Instructor FindInstructor(int id) => Instructors.Find(i => i.InstructorId == id);

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = FindStudent(studentId);
            var course = FindCourse(courseId);
            return student != null && course != null && student.Enroll(course);
        }

        public bool IsStudentEnrolledInCourse(int studentId, string courseTitle)
        {
            var student = FindStudent(studentId);
            return student?.Courses.Exists(c => c.Title.ToLower() == courseTitle.ToLower()) ?? false;
        }

        public string GetInstructorByCourse(string courseTitle)
        {
            var course = FindCourse(courseTitle);
            return course?.Instructor?.Name ?? "Course or instructor not found.";
        }

        public bool UpdateStudent(int id, string name, int age)
        {
            var student = FindStudent(id);
            if (student != null)
            {
                student.Name = name;
                student.Age = age;
                return true;
            }
            return false;
        }

        public bool DeleteStudent(int id)
        {
            var student = FindStudent(id);
            return Students.Remove(student);
        }
    }


    class MenuActions
    {
        private SchoolStudentManager manager;

        public MenuActions(SchoolStudentManager manager)
        {
            this.manager = manager;
        }

        public void AddStudent()
        {
            Console.Write("Student ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.WriteLine(manager.AddStudent(new Student { StudentId = id, Name = name, Age = age })
                ? "Student added successfully."
                : "Student ID already exists.");
        }

        public void AddInstructor()
        {
            Console.Write("Instructor ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Specialization: ");
            string spec = Console.ReadLine();

            Console.WriteLine(manager.AddInstructor(new Instructor { InstructorId = id, Name = name, Specialization = spec })
                ? "Instructor added successfully."
                : "Instructor ID already exists.");
        }

        public void AddCourse()
        {
            Console.Write("Course ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Instructor ID: ");
            int instructorId = int.Parse(Console.ReadLine());

            var instructor = manager.FindInstructor(instructorId);
            if (instructor == null)
            {
                Console.WriteLine("Instructor not found.");
                return;
            }

            Console.WriteLine(manager.AddCourse(new Course { CourseId = id, Title = title, Instructor = instructor })
                ? "Course added successfully."
                : "Course ID already exists.");
        }

        public void EnrollStudent()
        {
            Console.Write("Student ID: ");
            int studentId = int.Parse(Console.ReadLine());
            Console.Write("Course ID: ");
            int courseId = int.Parse(Console.ReadLine());

            Console.WriteLine(manager.EnrollStudentInCourse(studentId, courseId)
                ? "Enrollment successful."
                : "Failed to enroll.");
        }

        public void ShowAllStudents()
        {
            manager.Students.ForEach(s => Console.WriteLine(s.PrintDetails()));
        }

        public void ShowAllCourses()
        {
            manager.Courses.ForEach(c => Console.WriteLine(c.PrintDetails()));
        }

        public void ShowAllInstructors()
        {
            manager.Instructors.ForEach(i => Console.WriteLine(i.PrintDetails()));
        }

        public void FindStudent()
        {
            Console.Write("Search by ID or Name? (i/n): ");
            string choice = Console.ReadLine();
            Student student = choice == "i"
                ? manager.FindStudent(int.Parse(Console.ReadLine()))
                : manager.FindStudent(Console.ReadLine());

            Console.WriteLine(student != null ? student.PrintDetails() : "Student not found.");
        }

        public void FindCourse()
        {
            Console.Write("Search by ID or Title? (i/n): ");
            string choice = Console.ReadLine();
            Course course = choice == "i"
                ? manager.FindCourse(int.Parse(Console.ReadLine()))
                : manager.FindCourse(Console.ReadLine());

            Console.WriteLine(course != null ? course.PrintDetails() : "Course not found.");
        }

        public void CheckEnrollment()
        {
            Console.Write("Student ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Course Title: ");
            string title = Console.ReadLine();

            Console.WriteLine(manager.IsStudentEnrolledInCourse(id, title)
                ? "Student is enrolled in the course."
                : "Student is not enrolled.");
        }

        public void GetInstructorByCourse()
        {
            Console.Write("Course Title: ");
            string title = Console.ReadLine();
            Console.WriteLine("Instructor: " + manager.GetInstructorByCourse(title));
        }

        public void UpdateStudent()
        {
            Console.Write("Student ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Name: ");
            string name = Console.ReadLine();
            Console.Write("New Age: ");
            int age = int.Parse(Console.ReadLine());

            Console.WriteLine(manager.UpdateStudent(id, name, age)
                ? "Student updated."
                : "Student not found.");
        }

        public void DeleteStudent()
        {
            Console.Write("Student ID to delete: ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine(manager.DeleteStudent(id)
                ? "Student deleted."
                : "Deletion failed.");
        }
    }

    class Program
    {
        static void Main()
        {
            SchoolStudentManager manager = new SchoolStudentManager();
            MenuActions actions = new MenuActions(manager);

            while (true)
            {
                Console.WriteLine("\n===== STUDENT MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Add Instructor");
                Console.WriteLine("3. Add Course");
                Console.WriteLine("4. Enroll Student in Course");
                Console.WriteLine("5. Show All Students");
                Console.WriteLine("6. Show All Courses");
                Console.WriteLine("7. Show All Instructors");
                Console.WriteLine("8. Find Student");
                Console.WriteLine("9. Find Course");
                Console.WriteLine("10. Update Student");
                Console.WriteLine("11. Delete Student");
                Console.WriteLine("12. Check if Student Enrolled in Course");
                Console.WriteLine("13. Get Instructor Name by Course");
                Console.WriteLine("14. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": actions.AddStudent(); break;
                    case "2": actions.AddInstructor(); break;
                    case "3": actions.AddCourse(); break;
                    case "4": actions.EnrollStudent(); break;
                    case "5": actions.ShowAllStudents(); break;
                    case "6": actions.ShowAllCourses(); break;
                    case "7": actions.ShowAllInstructors(); break;
                    case "8": actions.FindStudent(); break;
                    case "9": actions.FindCourse(); break;
                    case "10": actions.UpdateStudent(); break;
                    case "11": actions.DeleteStudent(); break;
                    case "12": actions.CheckEnrollment(); break;
                    case "13": actions.GetInstructorByCourse(); break;
                    case "14": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }
    }


}
