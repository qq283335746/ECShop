using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TygaSoft.WebHelper
{
    public class ImagesHelper
    {
        /// <summary>
        /// 创建规定大小的图像(源图像只能是JPG格式)  
        /// </summary>    
        /// <param name="fromPath">源图像绝对路径</param>    
        /// <param name="toPath">生成图像绝对路径</param>    
        /// <param name="width">生成图像的宽度</param>    
        /// <param name="height">生成图像的高度</param>    
        public void CreateThumbnailImage(string fromPath, string toPath, int width, int height)
        {
            Bitmap originalBmp = new Bitmap(fromPath);
            // 源图像在新图像中的位置    
            int left, top;

            if (originalBmp.Width <= width && originalBmp.Height <= height)
            {
                // 原图像的宽度和高度都小于生成的图片大小    
                left = (int)Math.Round((decimal)(width - originalBmp.Width) / 2);
                top = (int)Math.Round((decimal)(height - originalBmp.Height) / 2);


                // 最终生成的图像    
                Bitmap bmpOut = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(bmpOut))
                {
                    // 设置高质量插值法    
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 清空画布并以白色背景色填充    
                    graphics.Clear(Color.White);
                    // 把源图画到新的画布上    
                    graphics.DrawImage(originalBmp, left, top, originalBmp.Width, originalBmp.Height);
                }
                bmpOut.Save(toPath);
                bmpOut.Dispose();

                return;
            }

            // 新图片的宽度和高度，如400*200的图像，想要生成160*120的图且不变形，    
            // 那么生成的图像应该是160*80，然后再把160*80的图像画到160*120的画布上    
            int newWidth, newHeight;
            if (width * originalBmp.Height < height * originalBmp.Width)
            {
                newWidth = width;
                newHeight = (int)Math.Round((decimal)originalBmp.Height * width / originalBmp.Width);
                // 缩放成宽度跟预定义的宽度相同的，即left=0，计算top    
                left = 0;
                top = (int)Math.Round((decimal)(height - newHeight) / 2);
            }
            else
            {
                newWidth = (int)Math.Round((decimal)originalBmp.Width * height / originalBmp.Height);
                newHeight = height;
                // 缩放成高度跟预定义的高度相同的，即top=0，计算left    
                left = (int)Math.Round((decimal)(width - newWidth) / 2);
                top = 0;
            }

            // 生成按比例缩放的图，如：160*80的图    
            Bitmap bmpOut2 = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(bmpOut2))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
            }
            // 再把该图画到预先定义的宽高的画布上，如160*120    
            Bitmap lastbmp = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(lastbmp))
            {
                // 设置高质量插值法    
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 清空画布并以白色背景色填充    
                graphics.Clear(Color.White);
                // 把源图画到新的画布上    
                graphics.DrawImage(bmpOut2, left, top);
            }
            lastbmp.Save(toPath);
            lastbmp.Dispose();
        }   
    }
}
