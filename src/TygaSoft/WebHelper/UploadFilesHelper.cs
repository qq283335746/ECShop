using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

namespace TygaSoft.WebHelper
{
    public class UploadFilesHelper
    {
        string uploadRoot = ConfigHelper.GetValueByKey("UploadFilesSavePath");
        HttpContext context;

        public enum FileExtension
        {
            jpg = 255216, gif = 7173, bmp = 6677, png = 13780, xls = 208207, doc = 208207, docx = 8075, xlsx = 8075
            // 255216 jpg;7173 gif;6677 bmp;13780 png; 7790 exe dll; 8297 rar; 6063 xml;6033 html;239187 aspx;117115 cs;119105 js;210187 txt;255254 sql;xls = 208207 
        }

        /// <summary>
        /// 使用文件固定字节法验证文件是否合法
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileLen"></param>
        /// <returns></returns>
        private bool IsFileValidated(Stream stream, int fileLen)
        {
            if (fileLen == 0) return false;

            //自定义一个数组，包含所有允许上传的文件扩展名，这里只定义xls扩展名
            FileExtension[] fes = { FileExtension.gif, FileExtension.bmp, FileExtension.jpg, FileExtension.png, FileExtension.xls,FileExtension.xlsx, FileExtension.doc, FileExtension.docx };

            byte[] imgArray = new byte[fileLen];
            stream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileBuffer = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileBuffer = buffer.ToString();
                buffer = br.ReadByte();
                fileBuffer += buffer.ToString();
            }
            catch
            {
            }
            br.Close();
            ms.Close();
            foreach (FileExtension item in fes)
            {
                if (Int32.Parse(fileBuffer) == (int)item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 使用FileUpload服务器控件上传文件
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool UploadFile(FileUpload fu,string configKey, ref Dictionary<string,string> dic)
        {
            if (!fu.HasFile) return false;

            //判断文件的类型和大小
            string type = fu.PostedFile.ContentType;
            int size = fu.PostedFile.ContentLength;
            //获取客户端上的文件的完全限定名称
            string fileOriginalName = fu.PostedFile.FileName;
            //获取文件扩展名
            string fileExtension = Path.GetExtension(fu.PostedFile.FileName).ToLower();
            //获取不包含文件扩展名的文件名
            string fileName = CustomsHelper.CreateDateTimeString();
            //文件保存路径
            string fileUrl = string.Empty;
            int fileLen = fu.PostedFile.ContentLength;

            if (IsFileValidated(fu.PostedFile.InputStream, fileLen))
            {
                //创建保存文件的路径
                string path = ConfigHelper.GetFullPath(configKey);
                string fullPath = path + fileName + fileExtension;
                try
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    //上载文件到硬盘
                    fu.SaveAs(fullPath);

                    fileUrl = ConfigHelper.GetValueByKey(configKey) + fileName + fileExtension;

                    dic.Add("FileName", fileName);
                    dic.Add("FileExtension", fileExtension);
                    dic.Add("FileUrl", fileUrl);
                    dic.Add("FileOriginalName", fileOriginalName);

                    return true;
                }
                catch 
                {
                    
                }
            }

            return false;
        }

        /// <summary>
        /// 使用FileUpload服务器控件上传文件
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="dic"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool UploadFile(FileUpload fu, string configKey, ref Dictionary<string, string> dic, ref string errorMsg)
        {
            if (!fu.HasFile) return false;

            //判断文件的类型和大小
            string type = fu.PostedFile.ContentType;
            int size = fu.PostedFile.ContentLength;
            //获取客户端上的文件的完全限定名称
            string fileOriginalName = fu.PostedFile.FileName;
            //获取文件扩展名
            string fileExtension = Path.GetExtension(fu.PostedFile.FileName).ToLower();
            //获取不包含文件扩展名的文件名
            string fileName = CustomsHelper.CreateDateTimeString();
            //文件保存路径
            string fileUrl = string.Empty;
            int fileLen = fu.PostedFile.ContentLength;

            if (!IsFileValidated(fu.PostedFile.InputStream, fileLen))
            {
                errorMsg = "上传的文件为禁止的文件！";
                return false;
            }

            //创建保存文件的路径
            string path = ConfigHelper.GetFullPath(configKey);
            string fullPath = path + fileName + fileExtension;
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //上载文件到硬盘
                fu.SaveAs(fullPath);

                fileUrl = ConfigHelper.GetValueByKey(configKey) + fileName + fileExtension;

                dic.Add("FileName", fileName);
                dic.Add("FileExtension", fileExtension);
                dic.Add("FileUrl", fileUrl);
                dic.Add("FileOriginalName", fileOriginalName);

                return true;
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// 使用FileUpload服务器控件上传文件
        /// </summary>
        /// <param name="fu"></param>
        /// <param name="path"></param>
        /// <param name="dic"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Upload(FileUpload fu, string path, ref Dictionary<string, string> dic, ref string errorMsg)
        {
            if (!fu.HasFile) return false;

            //判断文件的类型和大小
            string type = fu.PostedFile.ContentType;
            int size = fu.PostedFile.ContentLength;
            //获取客户端上的文件的完全限定名称
            string fileOriginalName = fu.PostedFile.FileName;
            //获取文件扩展名
            string fileExtension = Path.GetExtension(fu.PostedFile.FileName).ToLower();
            //获取不包含文件扩展名的文件名
            string fileName = CustomsHelper.CreateDateTimeString();
            //文件保存路径
            string fileUrl = string.Empty;

            if (!IsFileValidated(fu.PostedFile.InputStream, size))
            {
                errorMsg = "上传的文件为禁止的文件！";
                return false;
            }

            string dirPath = HttpContext.Current.Server.MapPath(path);
            //创建保存文件的路径
            string fullPath = path + fileName + fileExtension;
            try
            {
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

                //上载文件到硬盘
                fu.SaveAs(HttpContext.Current.Server.MapPath(fullPath));

                fileUrl = fullPath;

                dic.Add("FileName", fileName);
                dic.Add("FileExtension", fileExtension);
                dic.Add("FileUrl", fileUrl);
                dic.Add("FileOriginalName", fileOriginalName);

                return true;
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
            }

            return false;
        }

        /// <summary>
        /// 上传文件到临时存储，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadToTemp(HttpPostedFile file)
        {
            if (file == null || file.ContentLength == 0)
            {
                throw new ArgumentException("没有获取到任何上传的文件", "file");
            }
            int size = file.ContentLength;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!IsFileValidated(file.InputStream, size))
            {
                throw new ArgumentException("上传文件不在规定的上传文件范围内");
            }

            uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
            string dirName = "Temp/";

            string[] childDirs = Directory.GetDirectories(HttpContext.Current.Server.MapPath(uploadRoot + dirName));
            foreach (string item in childDirs)
            {
                DirectoryInfo di = new DirectoryInfo(item);
                TimeSpan ts = DateTime.Now - di.CreationTime;
                if (ts.Days > 2)
                {
                    Directory.Delete(item, true);
                }
            }

            string fName = CustomsHelper.GetFormatDateTime();
            string fileUrl = string.Empty;
            string saveVirtualPath = uploadRoot + dirName + fName.Substring(0,8) + "";
            string fullPath = HttpContext.Current.Server.MapPath(saveVirtualPath);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            fullPath += "/" + fName + fileExtension;
            file.SaveAs(fullPath);

            fileUrl = saveVirtualPath + "/" + fName + fileExtension;
            return fileUrl;
        }

        /// <summary>
        /// 将临时存储文件转移到商品文件，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        /// </summary>
        /// <param name="tempVirtualPath"></param>
        /// <returns></returns>
        public string FromTempToProduct(string tempVirtualPath)
        {
            uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
            string pVirtualPath = uploadRoot + "Product/" + CustomsHelper.CreateDateTimeString().Substring(0, 6) + "/";
            return TempFileToUseFile(tempVirtualPath, pVirtualPath);
        }

        /// <summary>
        /// 将临时文件转移到实际应用文件位置存储，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        /// </summary>
        /// <param name="tempVirtualPath"></param>
        /// <param name="useVirtualPath"></param>
        /// <returns></returns>
        private string TempFileToUseFile(string tempVirtualPath, string useVirtualPath)
        {
            string tempPath = HttpContext.Current.Server.MapPath(tempVirtualPath);
            if (!File.Exists(tempPath))
            {
                throw new ArgumentException("文件不存在", "tempVirtualPath");
            }
            string fileName = VirtualPathUtility.GetFileName(tempVirtualPath);
            string usePath = HttpContext.Current.Server.MapPath(useVirtualPath);
            string useDir = Path.GetDirectoryName(usePath);
            if (!Directory.Exists(useDir))
            {
                Directory.CreateDirectory(useDir);
            }
            string fullPath = Path.Combine(useDir, fileName);
            File.Move(tempPath, fullPath);

            return useVirtualPath.Replace("~","") + fileName;
        }

        /// <summary>
        /// 创建商品缩略图
        /// 返回值：主图（220*220）、大图（800*800）、中图（350*350）、小图（50*50）
        /// </summary>
        /// <param name="virtualPath">商品主图片的路径，该路径是带有“~”符号的路径</param>
        /// <returns></returns>
        public string[] GetProductThumbnailImages(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return null;
            }
            context = HttpContext.Current;
            string siteRootPath = context.Server.MapPath("~");
            string fullPath = context.Server.MapPath(virtualPath);
            string dir = Path.GetDirectoryName(fullPath);
            string fName = Path.GetFileNameWithoutExtension(fullPath);
            string fExtension = Path.GetExtension(fullPath).ToLower();
            string newDir = dir + "\\" + fName;
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }
            ImagesHelper ih = new ImagesHelper();
            //创建220*220 主图
            string sImages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sImages, 220, 220);
            sImages = sImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
            //创建800*800 大图
            string sLImages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sLImages, 800, 800);
            sLImages = sLImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
            //创建350*350 中图
            string sMimages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sMimages, 350, 350);
            sMimages = sMimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
            //创建50*50 小图
            string sSimages = newDir + "\\" + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sSimages, 50, 50);
            sSimages = sSimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");

            string[] images = {sImages, sLImages, sMimages, sSimages };
            return images;
        }

        /// <summary>
        /// 创建商品缩略图
        /// 返回值：主图（220*220）、大图（800*800）、中图（350*350）、小图（50*50）
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="mainFileName"></param>
        /// <returns></returns>
        public string[] GetProductThumbnailImages(string virtualPath,string mainFileName)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return null;
            }
            context = HttpContext.Current;
            string siteRootPath = context.Server.MapPath("~");
            string fullPath = context.Server.MapPath(virtualPath);
            string dir = Path.GetDirectoryName(fullPath);
            string fExtension = Path.GetExtension(fullPath).ToLower();
            string newDir = dir + mainFileName;
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }
            ImagesHelper ih = new ImagesHelper();
            //创建220*220 主图
            string sImages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sImages, 220, 220);
            sImages = sImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
            //创建800*800 大图
            string sLImages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sLImages, 800, 800);
            sLImages = sLImages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
            //创建350*350 中图
            string sMimages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sMimages, 350, 350);
            sMimages = sMimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");
            //创建50*50 小图
            string sSimages = newDir + CustomsHelper.GetFormatDateTime() + fExtension;
            ih.CreateThumbnailImage(fullPath, sMimages, 50, 50);
            sSimages = sSimages.Replace(siteRootPath, "/").Trim().Replace(@"\", @"/");

            string[] images = {sImages, sLImages, sMimages, sSimages };
            return images;
        }

        /// <summary>
        /// 删除当前商品图片,并删除由当前图片创建的缩略图
        /// </summary>
        /// <param name="virtualPath"></param>
        public void DeleteProductImage(string virtualPath)
        {
            context = HttpContext.Current;
            if (string.IsNullOrEmpty(virtualPath)) return;
            string fullPath = context.Server.MapPath(virtualPath);
            string fName = Path.GetFileNameWithoutExtension(fullPath);
            string dir = Path.GetDirectoryName(fullPath);
            if(File.Exists(fullPath)) File.Delete(fullPath);
            if (Directory.Exists(dir + "\\" + fName)) Directory.Delete(dir + "\\" + fName, true);
        }

        public string GetProductImgMain(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath)) return string.Empty;
            context = HttpContext.Current;
            string fullPath = context.Server.MapPath(virtualPath);
            string dir = Path.GetDirectoryName(fullPath);
            dir = dir + "\\" + Path.GetFileNameWithoutExtension(fullPath);
            string[] files = Directory.GetFiles(dir);
            foreach (string item in files)
            {
                Bitmap bmp = new Bitmap(item);
                int width = bmp.Width;
                int height = bmp.Height;

                if (width == 220 && height == 220)
                {
                    return item.Replace(context.Server.MapPath("~"), "/");
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 上传文件，并返回存储文件的虚拟路径，该虚拟路径包含根操作符（代字号 [~]）
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public string Upload(HttpPostedFile file, string dirName)
        {
            if (file == null || file.ContentLength == 0)
            {
                throw new ArgumentException("没有获取到任何上传的文件", "file");
            }
            dirName = ""+dirName.Trim('/')+"/";
            int size = file.ContentLength;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!IsFileValidated(file.InputStream, size))
            {
                throw new ArgumentException("上传文件不在规定的上传文件范围内");
            }
            uploadRoot = VirtualPathUtility.AppendTrailingSlash(uploadRoot);
            string fileUrl = string.Empty;
            string fName = CustomsHelper.GetFormatDateTime();
            string saveVirtualPath = uploadRoot + dirName+fName.Substring(0,8)+"/";
            string fullPath = HttpContext.Current.Server.MapPath(saveVirtualPath);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            fullPath += fName+fileExtension;
            file.SaveAs(fullPath);

            fileUrl = saveVirtualPath + fName+fileExtension;
            return fileUrl;
        }

        /// <summary>
        /// 将临时文件移到正式使用的位置
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public string TempFileMoveToUseFile(string tempPath, string replaceTempName)
        {
            if (string.IsNullOrEmpty(tempPath)) return "";
            tempPath = HttpContext.Current.Server.MapPath(tempPath);
            string fileName = Path.GetFileName(tempPath);
            string directoryName = Path.Combine(ConfigHelper.GetFullPath("UploadFilesSavePath"), replaceTempName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            string fileUrl = Path.Combine(directoryName, fileName);
            File.Move(tempPath, fileUrl);

            return fileUrl.Replace(HttpContext.Current.Server.MapPath("~"), "~/").Trim().Replace(@"\", @"/");
        }

        /// <summary>
        /// 返回临时文件存放位置
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public string GetFileTempUrl(string fileExtension)
        {
            string sDay = DateTime.Now.Day.ToString();
            string dir = ConfigHelper.GetFullPath("UploadFilesSavePath");
            dir = Path.Combine(dir, "temp");
            string cDir = Path.Combine(dir, sDay);
            if (!Directory.Exists(cDir))
            {
                Directory.CreateDirectory(cDir);
            }
            string[] childDirs = Directory.GetDirectories(dir);
            foreach (string item in childDirs)
            {
                if (item != (cDir))
                {
                    Directory.Delete(item, true);
                }
            }
            string fileName = CustomsHelper.GetFormatDateTime() + fileExtension.ToLower();
            string fileUrl = Path.Combine(dir,sDay,fileName);
            return fileUrl.Replace(HttpContext.Current.Server.MapPath("~"), "~/").Trim().Replace(@"\", @"/");
        }

    }
}
