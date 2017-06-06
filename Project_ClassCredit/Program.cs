using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add by EP.
using HtmlAgilityPack;
using System.Net;

namespace Project_ClassCredit
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: 1. Open html file.
            //       2. Using HtmlAgilityPack to parse data.
            //       3. Output.

            string filePath = string.Empty;

            if (args.Length != 0)
                filePath = args[0];
            else
            {
                Console.Write("Please enter the file path (*.html): ");
                filePath = Console.ReadLine();
            }

            // File open.
            StreamReader sr = new StreamReader(filePath);
            string retu = sr.ReadToEnd();

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


            for (int i = 1; i < target.Length; i++)
            {
                GetCourse(target, i);
            }
                          
                   

            Console.ReadLine();
        }

        static HtmlNode[] GetCourse(HtmlNode[] input, int index)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string temp = WebUtility.HtmlDecode(input[index].InnerHtml);
            htmlDoc.LoadHtml(temp);
            HtmlNode[] data = htmlDoc.DocumentNode.Descendants().Where(
                                 x => (x.ParentNode.Name == "td" && x.Name == "font")).ToArray();
            return data;
        }
    }
}
