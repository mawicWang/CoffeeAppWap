using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PublicResource
{
    public static class PicHandle
    {
        static Regex regPicSize = new Regex(@"/(small|middle|[0-9]+_[0-9x]+)/", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.RightToLeft);
        public static readonly int MaxLength = 4;
        public static readonly string SessionKey = "GengerP$#";
        public static bool IsValidString(string key)
        {
            if (String.IsNullOrEmpty(key)) return false;
            string args = System.Web.HttpContext.Current.Session[SessionKey] as string;
            if (String.IsNullOrEmpty(args)) return false;
            if (key.ToLower().Trim().Equals(args.ToLower()))
            {
                System.Web.HttpContext.Current.Session[SessionKey] = null;
                return true;
            }
            return false;
        }
        public static string GetString()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 1; i <= MaxLength; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else if (number % 3 == 0)
                    code = (char)('a' + (char)(number % 26));
                else
                    code = (char)('A' + (char)(number % 26));
                if ((code == '0') || (code == 'i') || (code == 'I') || (code == 'O') || (code == 'o') || (code == '1') || (code == 'l'))
                {
                    --i;
                    continue;
                }
                checkCode += code.ToString();
            }
            System.Web.HttpContext.Current.Session[SessionKey] = checkCode;
            return checkCode;
        }

        public static bool CreateThumbnailImage(string sourcefile, string destinationfile, int nWidth, int nHeight)
        {
            //图片放大，将变得不清晰，所以不考虑放大效果。只有缩略图.
            byte[] barrImgData = File.ReadAllBytes(sourcefile);
            MemoryStream ms = new MemoryStream(barrImgData);
            Image originalImage = null;
            try
            {
                originalImage = Image.FromStream(ms);
            }
            catch
            {
                if (originalImage != null)
                    originalImage.Dispose();
                return false;
            }
            bool isCreate = false;
            //图片原始大小
            int w = originalImage.Width;
            int h = originalImage.Height;

            //控制目标图片的大小范围
            if (nWidth > w)
                nWidth = w;
            else if (nWidth < 0)
                nWidth = 0;
            if (nHeight > h)
                nHeight = h;
            else if (nHeight < 0)
                nHeight = 0;

            int thumbnailW = 0, thumbnailH = 0;                     //缩略图大小

            if (nWidth == 0 && nHeight == 0)                        //原图大小
            {
                nWidth = w;
                nHeight = h;
            }
            else if ((nWidth == 0 && nHeight > 0) || nHeight == h)  //按高度等比
            {
                thumbnailH = nHeight;
                thumbnailW = (int)(((double)w / h) * nHeight);
            }
            else if ((nWidth > 0 && nHeight == 0) || nWidth == w)   //按宽度等比
            {
                thumbnailW = nWidth;
                thumbnailH = (int)(((double)h / w) * nWidth);
            }
            else
            {
                if ((double)(nHeight / h) <= (double)(nWidth / w) && (((double)w / h) >= ((double)nWidth / nHeight))) //长宽都有输入 选择差比最小的为标准
                {
                    thumbnailH = nHeight;
                    thumbnailW = (int)(((double)w / h) * nHeight);
                }
                else
                {
                    thumbnailW = nWidth;
                    thumbnailH = (int)(((double)h / w) * nWidth);
                }
            }
            System.Drawing.Image bitmap = null;
            Graphics g = null;

            //产生缩率图（如果高宽都不一致）
            if (thumbnailH != 0 && thumbnailW != 0)
            {
                bitmap = new System.Drawing.Bitmap(thumbnailW, thumbnailH);
                g = System.Drawing.Graphics.FromImage(bitmap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                //产生缩略图
                g.DrawImage(originalImage, new Rectangle(0, 0, thumbnailW, thumbnailH),
                    new Rectangle(0, 0, w, h), GraphicsUnit.Pixel);
            }
            //如果输入参数中宽高都有，并且高宽某一个属性达标后，切割图片
            if ((thumbnailH >= nHeight || thumbnailW >= nWidth) && nWidth > 0 && nHeight > 0)
            {
                originalImage = bitmap ?? originalImage;
                bitmap = new System.Drawing.Bitmap(nWidth, nHeight);
                g = System.Drawing.Graphics.FromImage(bitmap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                int x = (thumbnailW < nWidth) ? 0 : (thumbnailW - nWidth) / 2;
                int y = (thumbnailH < nHeight) ? 0 : (thumbnailH - nHeight) / 2;
                //产生切割图
                g.DrawImage(originalImage, new Rectangle(0, 0, nWidth, nHeight),
                    new Rectangle(x, y, nWidth, nHeight), GraphicsUnit.Pixel);
            }
            //没有缩略图 也没有切割
            if (bitmap == null)
            {
                bitmap = originalImage;
            }
            try
            {
                isCreate = true;
                if (!Directory.Exists(Path.GetDirectoryName(destinationfile))) Directory.CreateDirectory(Path.GetDirectoryName(destinationfile));
                if (Path.GetExtension(destinationfile).ToLower() == ".gif")
                    bitmap.Save(destinationfile, System.Drawing.Imaging.ImageFormat.Gif);
                else if ((Path.GetExtension(destinationfile).ToLower() == ".jpg") || (Path.GetExtension(destinationfile).ToLower() == ".jpeg"))
                    bitmap.Save(destinationfile, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (Path.GetExtension(destinationfile).ToLower() == ".png")
                    bitmap.Save(destinationfile, System.Drawing.Imaging.ImageFormat.Png);
                else if (Path.GetExtension(destinationfile).ToLower() == ".bmp")
                    bitmap.Save(destinationfile, System.Drawing.Imaging.ImageFormat.Bmp);
                else
                    isCreate = false;
            }
            catch
            {
                isCreate = false;
            }
            finally
            {
                if (originalImage != null)
                    originalImage.Dispose();
                if (bitmap != null)
                    bitmap.Dispose();
                if (g != null)
                    g.Dispose();
            }
            return isCreate;
        }

        public static string GetSmallPath(string path)
        {
            if (String.IsNullOrEmpty(path) || path.IndexOf(".") < 0) return path;
            return path.Insert(path.IndexOf("."), "_Small");
        }
        /// <summary>
        /// 输入新文件路径，判断文件夹，返回一个新的不重复地址
        /// </summary>
        /// <param name="tfullPath">文件路径</param>
        /// <param name="replaceUrl">需要模式匹配的字符串</param>
        /// <returns></returns>
        public static string GetLastPath(string tfullPath, string replaceUrl)
        {
            if (String.IsNullOrEmpty(tfullPath) || String.IsNullOrEmpty(replaceUrl)) return null;
            string fullPath = Path.Combine(replaceUrl, tfullPath);
            if (!File.Exists(fullPath)) return fullPath;
            int loopFrom = 0;
            string exts = Path.GetExtension(fullPath).Trim().ToLower();
            string oldPath = fullPath.Trim().ToLower().Substring(0, fullPath.Length - exts.Length);
            oldPath = Regex.Replace(oldPath, @"\[\d*\]$", "");
            string Opath = fullPath;

            while (File.Exists(oldPath + "[" + (++loopFrom).ToString() + "]" + exts)) { }
            string OFilePath = (oldPath + "[" + loopFrom.ToString() + "]" + exts);
            return OFilePath.ToLower().Replace(replaceUrl.ToLower(), "").Trim('/', '\\');
        }

        /// <summary>
        /// 判断是否是图片的流
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidPicture(Stream str)
        {
            if (null == str)
                return false;
            else
            {
                bool isTrue = false;
                using (System.Drawing.Image im = System.Drawing.Bitmap.FromStream(str))
                {
                    isTrue = (
                        im.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png) ||
                        im.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg) ||
                        im.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif) ||
                        im.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp)
                        );
                }
                return isTrue;
            }
        }
        public static bool IsValidPicture(byte[] source)
        {
            return IsValidPicture(IOHelper.BytesToStream(source));
        }
        public static void MarkWater(string filepath, string waterfile)
        {
            byte[] barrImgData = File.ReadAllBytes(filepath);
            byte[] barrWaterImgData = File.ReadAllBytes(waterfile);
            MemoryStream msImg = new MemoryStream(barrImgData);
            MemoryStream msWaterImg = new MemoryStream(barrWaterImgData);
            Image img = null;
            Image markImg = null;

            try
            {
                img = Image.FromStream(msImg);
                markImg = Image.FromStream(msWaterImg);
                float[][] ptsArray ={ 
                                            new float[] {1, 0, 0, 0, 0},
                                            new float[] {0, 1, 0, 0, 0},
                                            new float[] {0, 0, 1, 0, 0},
                                            new float[] {0, 0, 0, 1.0f, 0}, //注意：此处为0.0f为完全透明，1.0f为完全不透明
                                            new float[] {0, 0, 0, 0, 1}};
                ColorMatrix colorMatrix = new ColorMatrix(ptsArray);
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.SkipGrays, ColorAdjustType.Bitmap);
                Bitmap newBitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format48bppRgb);
                newBitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                Graphics g = Graphics.FromImage(newBitmap);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                if (markImg.Width > (img.Width - 5) || markImg.Height > (img.Height - 5))
                {

                }
                else
                {
                    int markx = img.Width - markImg.Width - 5;
                    int marky = img.Height - markImg.Height - 5;

                    g.DrawImage(markImg, new Rectangle(markx, marky, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, GraphicsUnit.Pixel, imageAttributes);
                    g.Dispose();
                    ImageFormat igf = img.RawFormat;
                    if (img != null) img.Dispose();
                    newBitmap.Save(filepath, igf);
                    newBitmap.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                if (img != null) img.Dispose();
                if (markImg != null) markImg.Dispose();
            }

        }

        /// <summary>
        /// 返回小图片路径
        /// </summary>
        /// <param name="sPath">原图片路径</param>
        /// <returns></returns>
        public static string GetSmallPicPath(string sPath)
        {
            if (string.IsNullOrEmpty(sPath) || sPath.IndexOf("/") < 0)
            {
                return sPath;
            }
            sPath = sPath.ToLower().StartsWith("http://") ? sPath : (AppHelper.StaticUploadUrl + sPath.TrimStart('/'));
            return sPath.Insert(sPath.LastIndexOf("/"), "/Small");

        }

        /// <summary>
        /// 返回中图片路径
        /// </summary>
        /// <param name="sPath">原图片路径</param>
        /// <returns></returns>
        public static string GetMiddlePicPath(string sPath)
        {
            if (string.IsNullOrEmpty(sPath) || sPath.IndexOf("/") < 0)
            {
                return sPath;
            }
            sPath = sPath.ToLower().StartsWith("http://") ? sPath : (AppHelper.StaticUploadUrl + sPath.TrimStart('/'));
            return sPath.Insert(sPath.LastIndexOf("/"), "/Middle");
        }

        /// <summary>
        /// 返回文件服务器下的图片地址
        /// </summary>
        /// <param name="sPath">图片地址</param>
        /// <returns></returns>
        public static string GetUplodPicPath(string sPath)
        {
            if (string.IsNullOrEmpty(sPath))
            {
                return sPath;
            }
            sPath = sPath.ToLower().StartsWith("http://") ? sPath : (AppHelper.StaticUploadUrl + sPath.TrimStart('/'));
            return sPath;
        }

        /// <summary>
        /// 取文件服务器各种尺寸的图片地址
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sSize"></param>
        /// <returns></returns>
        public static string GetUploadPicPath(string sPath, string sSize)
        {
            if (string.IsNullOrEmpty(sPath))
            {
                return sPath;
            }
            sPath = sPath.ToLower().StartsWith("http://") ? sPath : (AppHelper.StaticUploadUrl + sPath.TrimStart('/'));
            if (!string.IsNullOrEmpty(sSize))
            {               
                if (regPicSize.IsMatch(sPath))
                {
                    sPath = regPicSize.Replace(sPath, "/" + sSize+"/");
                }
                else
                {
                    sPath = sPath.Insert(sPath.LastIndexOf("/"), "/" + sSize);
                }
            }

            return sPath;
        }


    }
}
