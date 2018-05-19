using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add by EP.
using HtmlAgilityPack;
using System.Net;
using Project_ClassCredit.Service;
using Project_ClassCredit.Modal;

namespace Project_ClassCredit
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: 1. Open html file.
            //       2. Using HtmlAgilityPack to parse data.
            //       3. Output.

            string storePath = @"C:\Users\yinnping\Downloads\course.txt";
            Console.Write("Plase enter a file path that can store your course information (*.txt): ");
            // string storePath = Console.ReadLine();
            string filePath = string.Empty;

            //if (args.Length != 0)
            //    filePath = args[0];
            //else
            //{
            Console.Write("Please enter the file path (*.html): ");
            filePath = Console.ReadLine();
            //}

            // File open.
            StreamReader sr = new StreamReader(filePath);
            string retu = sr.ReadToEnd();
            sr.Close();

            retu = WebUtility.HtmlDecode(retu);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(retu);

            // Start parse.
            HtmlNode[] dataGrid = htmlDoc.DocumentNode.Descendants().Where(
                                  x => (x.Id == "DataGrid1")).ToArray();
            string temp = WebUtility.HtmlDecode(dataGrid[0].InnerHtml);
            htmlDoc.LoadHtml(temp);        
            HtmlNode[] target = htmlDoc.DocumentNode.Descendants().Where(
                                x => (x.Name == "tr")).ToArray();

            Helper helper = new Helper();

            StreamWriter sw = new StreamWriter(File.Create(storePath));
            for (int i = 1; i < target.Length; i++)
            {
                HtmlNode[] courseRow = helper.GetCourseRow(target, i);
                Course course = helper.GetCourse(courseRow);
                sw.WriteLine(course.ToString());
                Console.WriteLine($"{i} done.");
            }
            
            sw.Close();

            sr = new StreamReader(File.OpenRead(storePath));
            List<Course> general = new List<Course>();
            List<Course> enneed = new List<Course>();
            List<Course> chneed = new List<Course>();
            List<Course> spneed = new List<Course>();
            List<Course> csneed = new List<Course>();
            List<Course> cschoose = new List<Course>();
            List<Course> other = new List<Course>();
            List<Course> all = new List<Course>();
            
            char[] splitChar = { '\t' };
            while (!sr.EndOfStream)
            {
                string[] data = sr.ReadLine().Split(splitChar);
                Course course = new Course();
                course.Semester = data[0];
                course.Code = data[1];
                course.Name = data[2];
                course.Credit = int.Parse(data[3].Trim());
                course.Grade = data[4];

                char[] code = data[1].ToCharArray();
                if (data[1].Contains("CC12"))
                    chneed.Add(course);
                else if (data[1].Contains("CC10") || data[1].Contains("FE"))
                    enneed.Add(course);
                else if (data[1].Contains("CC") && code[3] == '5')
                    spneed.Add(course);
                else if (data[1].Contains("GE") || data[1].Contains("SA") || code[2] == 'G')
                    general.Add(course);
                else if (data[1].Contains("CS"))
                {
                    if (code[6] == '3' || code[6] == '0')
                        csneed.Add(course);
                    else
                        cschoose.Add(course);
                }
                else if (data[1].Contains("EC"))
                    cschoose.Add(course);
                else
                    other.Add(course);
            }
            sr.Close();

            Console.WriteLine("=== 通識必修 共 {0} 學分 ===", Sum(general.ToArray()));
            ShowUp(general.ToArray());
            Console.WriteLine();

            Console.WriteLine("=== 英文必修 共 {0} 學分 ===", Sum(enneed.ToArray()));
            ShowUp(enneed.ToArray());
            Console.WriteLine();

            Console.WriteLine("=== 文學必修 共 {0} 學分 ===", Sum(chneed.ToArray()));
            ShowUp(chneed.ToArray());
            Console.WriteLine();

            Console.WriteLine("=== 體育必修 共 {0} 學分 ===", Sum(spneed.ToArray()));
            ShowUp(spneed.ToArray());
            Console.WriteLine();

            Console.WriteLine("=== 資工必修 共 {0} 學分 ===", Sum(csneed.ToArray()));
            ShowUp(csneed.ToArray());
            Console.WriteLine();

            Console.WriteLine("=== 資工選修 共 {0} 學分 ===", Sum(cschoose.ToArray()));
            ShowUp(cschoose.ToArray());
            Console.WriteLine();

            Console.WriteLine("=== 其他 共 {0} 學分 ===", Sum(other.ToArray()));
            ShowUp(other.ToArray());
            Console.WriteLine();

            all.AddRange(general);
            all.AddRange(enneed);
            all.AddRange(chneed);
            all.AddRange(spneed);
            all.AddRange(csneed);
            all.AddRange(cschoose);
            all.AddRange(other);

            Console.WriteLine(Sum(all.ToArray()));

            Console.ReadLine();
        }

        static void ShowUp(Course[] courseArr)
        {
            foreach (Course item in courseArr)
            {
                Console.WriteLine(item.ToString());
            }            
        }

        static int Sum(Course[] courseArr)
        {
            int sum = 0;
            foreach (Course item in courseArr)
            {
                char[] grade = item.Grade.ToCharArray();
                if (grade[0] < 'D')
                    sum += item.Credit;
            }
            return sum;
        }
    }
}
