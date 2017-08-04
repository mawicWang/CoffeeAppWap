using System;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.IO;
namespace PublicResource
{

    public class MyEncoder
    {
        public MyEncoder()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private static string sEncryptKey = "850705t7e5l7e7";
        private static byte[] GetKey(string sTemp, int IVLength) //通过字符获取加密解密数组
        {
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        public static string Encrypto(string Source)//加密
        {
            SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
            
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetKey(sEncryptKey, mobjCryptoService.Key.Length);
            mobjCryptoService.IV = GetKey(sEncryptKey, mobjCryptoService.IV.Length);
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        public static string Encrypto(string Source, string sKey)//加密
        {
            SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetKey(sKey, mobjCryptoService.Key.Length);
            mobjCryptoService.IV = GetKey(sKey, mobjCryptoService.IV.Length);
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        public static string Decrypto(string Source)//解密
        {
            try
            {
                SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetKey(sEncryptKey, mobjCryptoService.Key.Length);
                mobjCryptoService.IV = GetKey(sEncryptKey, mobjCryptoService.IV.Length);
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
            }
            return null;
        }
        public static string Decrypto(string Source, string sKey)//解密
        {
            try
            {
                SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();

                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetKey(sKey, mobjCryptoService.Key.Length);
                mobjCryptoService.IV = GetKey(sKey, mobjCryptoService.IV.Length);
                ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

    }

}
