using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Helper.DataSecurity
{

    /// <summary>
    /// Class EncryptionAndDecryption
    /// </summary>
    public static class EncryptAndDecrypt
    {

        #region 常用编码转换方法

        /// <summary>
        /// Strings to hex string.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>System.String.</returns>
        public static string StringToHexString(string inputString)
        {
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            var buffByte = byteConverter.GetBytes(inputString);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buffByte)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();    //返回结果
        }

        /// <summary>
        /// Strings to encode string.
        /// </summary>
        /// <param name="stringInput">The string input.</param>
        /// <returns>System.String.</returns>
        public static string StringToEncodeString(string stringInput)
        {
            byte[] encoded = Encoding.Unicode.GetBytes(stringInput);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in encoded)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将汉字转换为Unicode
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <returns></returns>
        public static string GbToUnicode(string text)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(text);
            string lowCode = "", temp = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 2 == 0)
                {
                    temp = System.Convert.ToString(bytes[i], 16);//取出元素4编码内容（两位16进制）
                    if (temp.Length < 2) temp = "0" + temp;
                }
                else
                {
                    string mytemp = Convert.ToString(bytes[i], 16);
                    if (mytemp.Length < 2) mytemp = "0" + mytemp; lowCode = lowCode + @"\u" + mytemp + temp;//取出元素4编码内容（两位16进制）
                }
            }
            return lowCode;
        }

        /// <summary>
        /// 将Unicode转换为汉字
        /// </summary>
        /// <param name="name">要转换的字符串</param>
        /// <returns></returns>
        public static string UnicodeToGb(string text)
        {
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "([\\w]+)|(\\\\u([\\w]{4}))");
            if (mc != null && mc.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    sb.Append(Encoding.Unicode.GetString(codes));
                }
                return sb.ToString();
            }
            else
            {
                return text;
            }
        }

        public static string BytesToHexString(Byte[] inBytes)
        {
            //临时显示
            StringBuilder strGuff = new StringBuilder();
            foreach (Byte s in inBytes)
            {
                strGuff.Append(s.ToString("X") + " ");
            }
            return strGuff.ToString();
            //发送
        }

        public static Byte[] GetStrToByteArray(string inputString)
        {
            string inputStr = inputString;
            string[] Split = inputStr.Trim().Split(new Char[] { ' ', ',', '.', ':', '\t' });
            Byte[] returnByte;
            if (Split.Length > 0)
            {
                returnByte = new Byte[Split.Length];
                int i = 0;
                foreach (string s in Split)
                {
                    if (!String.IsNullOrEmpty(s.Trim()))
                    {
                        returnByte[i] = ConvertStringByte(s);
                        i++;
                    }
                }
            }
            else
            {
                returnByte = null;
            }
            return returnByte;
        }

        public static byte ConvertStringByte(string stringVal)
        {
            byte byteVal = 0;
            if (!String.IsNullOrEmpty(stringVal))
            {
                try
                {
                    byteVal = System.Convert.ToByte(stringVal, 16);
                }
                catch (System.OverflowException ex)
                {
                    throw (ex);
                }
                catch (System.FormatException ex)
                {
                    throw (ex);
                }
                catch (System.ArgumentNullException ex)
                {
                    throw (ex);
                }
            }
            return byteVal;
        }


        #endregion 常用编码转换方法

        #region MD5 加密（散列码 Hash 加密）

        /// <summary>
        /// MD5 加密（散列码 Hash 加密）
        /// </summary>
        /// <param name="code">明文</param>
        /// <returns>密文</returns>
        public static string GetStrMD5(string code)
        {
            /* 获取原文内容的byte数组 */
            byte[] sourceCode = Encoding.Default.GetBytes(code);
            byte[] targetCode;    //声明用于获取目标内容的byte数组
            /* 创建一个MD5加密服务提供者 */
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            targetCode = md5.ComputeHash(sourceCode);    //执行加密
            /* 对字符数组进行转码 */
            StringBuilder sb = new StringBuilder();
            foreach (byte b in targetCode)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the STR SHa1.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
        /// <remarks>SAP_SchoolSystem</remarks>
        public static string GetStrSHA1(string code)
        {
            /* 获取原文内容的byte数组 */
            byte[] sourceCode = Encoding.Default.GetBytes(code);
            byte[] targetCode;    //声明用于获取目标内容的byte数组
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            targetCode = SHA1.ComputeHash(sourceCode);    //执行加密
            /* 对字符数组进行转码 */
            StringBuilder sb = new StringBuilder();
            foreach (byte b in targetCode)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the M d5 from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">GetInfoFile() fail,error: + ex.Message</exception>
        /// <remarks>SAP_SchoolSystem</remarks>
        public static string GetMD5FromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetInfoFile() fail,error:" + ex.Message);
            }
        }

        public static string GetSha1FromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
                byte[] retVal = SHA1.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetInfoFile() fail,error:" + ex.Message);
            }
        }

        #endregion MD5 加密（散列码 Hash 加密）

        #region DES 加（解）密。（对称加密）

        /// <summary>
        /// DES 加密（对称加密）。使用密钥将明文加密成密文
        /// </summary>
        /// <param name="code">明文</param>
        /// <param name="sKey">密钥 8</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string code, string sKey)
        {
            /* 创建一个DES加密服务提供者 */
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //以下两个很重要 ，解决了其它语言结果不一样的问题
            // des.Mode = CipherMode.ECB;
            // des.Padding = PaddingMode.Zeros;
            /* 将要加密的内容转换成一个Byte数组 */
            byte[] inputByteArray = Encoding.Default.GetBytes(code);

            /* 设置密钥和初始化向量 */
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            /* 创建一个内存流对象 */
            MemoryStream ms = new MemoryStream();

            /* 创建一个加密流对象 */
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            /* 将要加密的文本写到加密流中 */
            cs.Write(inputByteArray, 0, inputByteArray.Length);

            /* 更新缓冲 */
            cs.FlushFinalBlock();

            /* 获取加密过的文本 */
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            /* 释放资源 */
            cs.Close();
            ms.Close();

            /* 返回结果 */
            return ret.ToString();
        }

        /// <summary>
        /// DES 解密（对称加密）。使用密钥将密文解码成明文
        /// </summary>
        /// <param name="code">密文</param>
        /// <param name="sKey">密钥 8</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string code, string sKey)
        {
            /* 创建一个DES加密服务提供者 */
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //以下两个很重要 ，解决了其它语言结果不一样的问题
            // des.Mode = CipherMode.ECB;
            // des.Padding = PaddingMode.Zeros;
            /* 将要解密的内容转换成一个Byte数组 */
            byte[] inputByteArray = new byte[code.Length / 2];

            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            /* 设置密钥和初始化向量 */
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            /* 创建一个内存流对象 */
            MemoryStream ms = new MemoryStream();

            /* 创建一个加密流对象 */
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            /* 将要解密的文本写到加密流中 */
            cs.Write(inputByteArray, 0, inputByteArray.Length);

            /* 更新缓冲 */
            cs.FlushFinalBlock();

            /* 返回结果 */
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="inFile">文件储存路径</param>
        /// <param name="outFile">储存文件复制的路径</param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static bool EncryptFileDES(string inFile, string outFile, string encryptKey)
        {
            byte[] rgbKey = ASCIIEncoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = ASCIIEncoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            try
            {
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流
                FileStream outFs = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);// 等待写入流
                outFs.SetLength(0);//帮助读写的变量
                byte[] byteIn = new byte[100];//放临时读入的流
                long readLen = 0;//读入流的长度
                long totalLen = inFs.Length;//读入流的总长度
                int everylen = 0;//每次读入流的长度
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile
                CryptoStream encStream = new CryptoStream(outFs, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, 100);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.Close();
                inFs.Close();
                outFs.Close();
                return true;//加密成功
            }
            catch (Exception ex)
            {
                throw (ex);//加密失败
            }
        }

        public static bool DecryptFileDES(string inFile, string outFile, string encryptKey)
        {
            byte[] rgbKey = ASCIIEncoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = ASCIIEncoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            try
            {
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流
                FileStream outFs = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);// 等待写入流
                outFs.SetLength(0);//帮助读写的变量
                byte[] byteIn = new byte[100];//放临时读入的流
                long readLen = 0;//读入流的长度
                long totalLen = inFs.Length;//读入流的总长度
                int everylen = 0;//每次读入流的长度
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile
                CryptoStream encStream = new CryptoStream(outFs, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, 100);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.Close();
                inFs.Close();
                outFs.Close();
                return true;//加密成功
            }
            catch (Exception ex)
            {
                throw (ex);//加密失败
            }
        }

        public static string DecryptFromFileDES(string inFile, string encryptKey)
        {
            byte[] rgbKey = ASCIIEncoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = ASCIIEncoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            try
            {
                FileStream inFs = new FileStream(inFile, FileMode.Open, FileAccess.Read);//读入流
                /* 创建一个内存流对象 */
                MemoryStream ms = new MemoryStream();

                byte[] byteIn = new byte[100];//放临时读入的流
                long readLen = 0;//读入流的长度
                long totalLen = inFs.Length;//读入流的总长度
                int everylen = 0;//每次读入流的长度
                DES des = new DESCryptoServiceProvider();//将inFile加密后放到outFile
                CryptoStream encStream = new CryptoStream(ms, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                while (readLen < totalLen)
                {
                    everylen = inFs.Read(byteIn, 0, 100);
                    encStream.Write(byteIn, 0, everylen);
                    readLen = readLen + everylen;
                }
                encStream.FlushFinalBlock();
                encStream.Close();
                inFs.Close();

                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
               throw(ex);//加密失败
            }
        }


        #endregion


        #region RSA 加（解）密。（不对称加密）

        /// <summary>
        /// Creates the RSA key.
        /// </summary>
        /// <param name="dwKeySize">Size of the dw key.</param>
        /// <returns>RSAKey.</returns>
        public static RSAKey CreateRSAKey(int dwKeySize = 1024)
        {
            RSAKey rsaKey = new RSAKey();    //声明一个RSAKey对象

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(dwKeySize);
            rsaKey.PrivateKey = rsa.ToXmlString(true);    //创建私钥
            rsaKey.PublicKey = rsa.ToXmlString(false);    //创建公钥

            return rsaKey;    //返回结果
        }

        /// <summary>
        /// RSA 加密（不对称加密）。使用公钥将明文加密成密文
        /// </summary>
        /// <param name="code">明文</param>
        /// <param name="key">公钥</param>
        /// <returns>密文</returns>
        public static string RSAEncrypt(string code, string key)
        {
            /* 将文本转换成byte数组 */
            byte[] source = Encoding.Default.GetBytes(code);
            byte[] ciphertext;    //密文byte数组

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);    //设置公钥
            ciphertext = rsa.Encrypt(source, false);    //加密，得到byte数组

            /* 对字符数组进行转码 */
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ciphertext)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();    //返回结果
        }

        /// <summary>
        /// RSA 解密（不对称加密）。使用私钥将密文解密成明文
        /// </summary>
        /// <param name="code">密文</param>
        /// <param name="key">私钥</param>
        /// <returns>明文</returns>
        public static string RSADecrypt(string code, string key)
        {
            /* 将文本转换成byte数组 */
            byte[] ciphertext = new byte[code.Length / 2];
            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                ciphertext[x] = (byte)i;
            }
            byte[] source;    //原文byte数组

            /* 创建一个RSA加密服务提供者 */
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);    //设置私钥
            source = rsa.Decrypt(ciphertext, false);    //解密，得到byte数组

            return Encoding.Default.GetString(source);    //返回结果
        }


        #endregion RSA 加（解）密。（不对称加密）

        #region RSA 数字签名与验证

        /// <summary>
        /// Hashes the and sign bytes.
        /// </summary>
        /// <param name="DataToSign">The data to sign.</param>
        /// <param name="Key">The key.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        /// <summary>
        /// Verifies the signed hash.
        /// </summary>
        /// <param name="DataToVerify">The data to verify.</param>
        /// <param name="SignedData">The signed data.</param>
        /// <param name="Key">The key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        /// <summary>
        /// Hashes the and sign bytes.
        /// </summary>
        /// <param name="dataToSignStr">The data to sign STR.</param>
        /// <param name="PraXmlKey">The pra XML key.</param>
        /// <returns>System.String.</returns>
        public static string HashAndSignBytes(string dataToSignStr, string PraXmlKey)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                //RSAalg.ImportParameters(Key);
                RSAalg.FromXmlString(PraXmlKey);
                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.

                byte[] dataToSign = new byte[dataToSignStr.Length / 2];
                for (int x = 0; x < dataToSignStr.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(dataToSignStr.Substring(x * 2, 2), 16));
                    dataToSign[x] = (byte)i;
                }
                byte[] byteBuff = RSAalg.SignData(dataToSign, new SHA1CryptoServiceProvider());

                StringBuilder sb = new StringBuilder();
                foreach (byte b in byteBuff)
                {
                    sb.AppendFormat("{0:X2}", b);
                }
                return sb.ToString();    //返回结果
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        /// <summary>
        /// Verifies the signed hash.
        /// </summary>
        /// <param name="DataToVerifyStr">The data to verify STR.</param>
        /// <param name="SignedDataStr">The signed data STR.</param>
        /// <param name="pubXmlKey">The pub XML key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool VerifySignedHash(string DataToVerifyStr, string SignedDataStr, string pubXmlKey)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                //RSAalg.ImportParameters(Key);
                RSAalg.FromXmlString(pubXmlKey);
                byte[] signedData = new byte[SignedDataStr.Length / 2];
                for (int x = 0; x < SignedDataStr.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(SignedDataStr.Substring(x * 2, 2), 16));
                    signedData[x] = (byte)i;
                }
                byte[] dataToVerifyStr = new byte[DataToVerifyStr.Length / 2];
                for (int x = 0; x < DataToVerifyStr.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(DataToVerifyStr.Substring(x * 2, 2), 16));
                    dataToVerifyStr[x] = (byte)i;
                }
                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.VerifyData(dataToVerifyStr, new SHA1CryptoServiceProvider(), signedData);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }


        #endregion RSA 数字签名与验证
    }

    /// <summary>
    /// .Net AES 加密解密类
    /// </summary>
    public class AESHelper
    {
        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string key, string vector, CipherMode mode)
        {

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Mode = mode;
            rijndaelProvider.Padding = PaddingMode.Zeros;//
            rijndaelProvider.Key = UTF8Encoding.UTF8.GetBytes(key);
            rijndaelProvider.IV = UTF8Encoding.UTF8.GetBytes(vector);
            // rijndaelProvider.Padding = PaddingMode.Zeros;  //其实这里就是抛出异常的关键所在。

            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();
            byte[] inputData = UTF8Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);

        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="strKey"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string key, string vector, CipherMode mode)
        {
            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Mode = mode;
            rijndaelProvider.Padding = PaddingMode.Zeros;//
            rijndaelProvider.Key = UTF8Encoding.UTF8.GetBytes(key);
            rijndaelProvider.IV = UTF8Encoding.UTF8.GetBytes(vector);
            //rijndaelProvider.Padding = PaddingMode.Zeros;

            ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptBytes = rijndaelDecrypt.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return UTF8Encoding.UTF8.GetString(decryptBytes);
        }

    }


    /// <summary>
    /// RSA 密钥。公钥&私钥
    /// </summary>
    public class RSAKey
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }

}
