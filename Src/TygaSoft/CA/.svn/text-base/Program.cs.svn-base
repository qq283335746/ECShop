using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace TygaSoft.CA
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] A = { "a", "b", "c", "d" };
            string[] B = { "a", "b", "e", "h" };
            List<string> aList = new List<string>();
            aList.Add("a");
            aList.Add("b");
            aList.Add("c");
            aList.Add("d");
            List<string> bList = new List<string>();
            bList.Add("a");
            bList.Add("b");
            bList.Add("e");
            bList.Add("h");

            string[] C = A.Intersect(B).ToArray();
            string[] D = A.Except(B).ToArray();
            string[] E = B.Except(A).ToArray();
            string[] F = A.Except(C).ToArray();
            string[] H = B.Except(C).ToArray();

            List<string> CC = aList.Intersect(bList).ToList<string>();
            List<string> DD = aList.Except(bList).ToList<string>();

            string fromPath = @"E:\Visual Studio 2010 Workspace\电子商务类项目\珠宝类\JewelryShop\JewelryShop.Web\UploadRoot\Product\201310\20131014_1450422799";
            string[] files = Directory.GetFiles(fromPath);
            foreach (string item in files)
            {
                Bitmap bmp = new Bitmap(item);
                int width = bmp.Width;
                int height = bmp.Height;

                if (width == 220 && height == 220)
                {
                    Console.WriteLine("找到了");
                }
            }

        }
    }
}
