using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_ClassCredit.Modal
{
    public class Course
    {
        public string Semester { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credit { get; set; }
        public string Grade { get; set; }

        public Course()
        { }
        public Course(string semester, string code, string name, int credit, string grade)
        {
            Semester = semester;
            Code = code;
            Name = name;
            Credit = credit;
            Grade = grade;
        }

        public override string ToString()
        {
            return $"{Semester} \t{Code} \t{Name} \t{Credit.ToString()} \t{Grade}";
        }
    }
}
