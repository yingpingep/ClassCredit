using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add by EP.
using Project_ClassCredit.Modal;
using HtmlAgilityPack;
using System.Net;

namespace Project_ClassCredit.Service
{
    public class Helper
    {
        HtmlDocument htmlDoc;
        /// <summary>
        /// Get course form html file (by row).
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public HtmlNode[] GetCourseRow(HtmlNode[] input, int index)
        {
            htmlDoc = new HtmlDocument();
            string temp = WebUtility.HtmlDecode(input[index].InnerHtml);
            htmlDoc.LoadHtml(temp);
            HtmlNode[] data = htmlDoc.DocumentNode.Descendants().Where(
                                 x => (x.ParentNode.Name == "td" && x.Name == "font")).ToArray();
            return data;
        }

        /// <summary>
        /// Get actual course information using course row.
        /// </summary>
        /// <param name="input">Course row.</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Course GetCourse(HtmlNode[] input)
        {            
            Course course = new Course();

            course.Semester = input[1].InnerText.Trim();
            course.Code = input[2].InnerText.Trim();
            course.Name = input[3].InnerText.Trim();
            course.Credit = int.Parse(input[4].InnerText.Trim());
            course.Grade = input[5].InnerText.Trim();            

            return course;
        }
    }
}
