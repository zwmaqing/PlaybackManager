using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Helper.DiskFile
{
    public struct DiskFile
    {
        public string PathName;
        public string FullName;
        public string Name;
        public string ExtensionName;
        public DiskFile(string pathName)
        {
            PathName = pathName;
            FullName = Path.GetFileName(pathName);
            Name = Path.GetFileNameWithoutExtension(pathName);
            ExtensionName = Path.GetExtension(pathName).ToLower();
        }

    }

    /// <summary>
    /// 文件操作夹
    /// </summary>
    public static class DiskDirFileIo
    {
        #region 格式化磁盘

        //[DllImport("shell32.dll", EntryPoint = "SHFormatDrive", CallingConvention = CallingConvention.Cdecl)]
        [DllImport("shell32.dll")]
        private static extern uint SHFormatDrive(IntPtr hWnd, uint drive, SHFormatFlags fmtID, SHFormatOptions Options);

        [Flags]
        private enum SHFormatOptions : uint
        {
            /// <summary>
            /// Full formatting
            /// </summary>
            SHFMT_OPT_COMPLETE = 0x0,
            /// <summary>
            /// Quick Format
            /// </summary>
            SHFMT_OPT_FULL = 0x1,
            /// <summary>
            /// MS-DOS System Boot Disk
            /// </summary>
            SHFMT_OPT_SYSONLY = 0x2
        }

        private enum SHFormatFlags : uint
        {
            SHFMT_ID_DEFAULT = 0xFFFF,
            /// <summary>
            /// A general error occured while formatting. This is not an indication that the drive cannot be formatted though.
            /// </summary>
            SHFMT_ERROR = 0xFFFFFFFF,
            /// <summary>
            /// The drive format was cancelled by user/OS.
            /// </summary>
            SHFMT_CANCEL = 0xFFFFFFFE,
            /// <summary>
            /// A serious error occured while formatting. The drive is unable to be formatted by the OS.
            /// </summary>
            SHFMT_NOFORMAT = 0xFFFFFFD
        }

        public static bool FormatDriver(IntPtr hWnd, string disk, bool quickFormat = true, string label = "")
        {
            bool state = false;
            if (disk.Length < 2)
            {
                return state;
            }
            List<string> zimu = new List<string> { "a:", "b:", "c:", "d:", "e:", "f:", "g:", "h:", "i:", "j:", "k:", "l:", "m:", "n:", "o:", "p:", "q:", "r:", "s:", "t:", "u:", "v:", "w:", "x:", "y:", "z:" };
            string d = disk.ToLower().Substring(0, 2);
            int driveIndex = zimu.FindIndex(x => x == d);
            if (driveIndex >= 0)
            {
                try
                {
                    var options = SHFormatOptions.SHFMT_OPT_COMPLETE;
                    if (quickFormat)
                        options = SHFormatOptions.SHFMT_OPT_FULL;
                    uint returnCode = SHFormatDrive(hWnd, (uint)driveIndex, SHFormatFlags.SHFMT_ID_DEFAULT, options);

                    state = returnCode == 0 ? true : false;
                    ChangeDiskVolumeLabel(disk, label);
                }
                catch
                {
                    state = false;
                }
            }
            return state;
        }


        #endregion 格式化磁盘

        #region 获取系统磁盘信息列表
        /// <summary>
        /// 获取系统磁盘信息列表
        /// </summary>
        /// <returns>DriveInfo[]</returns>

        public static DriveInfo[] GetSysDiskInfoList()
        {
            return DriveInfo.GetDrives();
        }

        #endregion 获取系统磁盘信息

        #region 获取指定磁盘卷标的磁盘实例

        /// <summary>
        /// 获取指定磁盘卷标的磁盘实例
        /// 不存在，则返回NULL
        /// 不扫描未准备好的磁盘
        /// </summary>
        /// <param name="diskVolumeLabel"></param>
        /// <returns></returns>
        public static DriveInfo GetDiskInfo(string diskVolumeLabel)
        {
            foreach (DriveInfo VARIABLE in DriveInfo.GetDrives())
            {
                if (VARIABLE.IsReady)
                {
                    if (VARIABLE.VolumeLabel == diskVolumeLabel)
                    {
                        return VARIABLE;
                    }
                }

            }
            return null;
        }

        #endregion 获取指定磁盘卷标的磁盘实例

        #region 获取指定磁盘的卷标

        /// <summary>
        /// 返回给定磁盘的卷标
        /// 如： D
        /// "ProGraming"
        /// 磁盘名无效返回""
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns></returns>
        public static string GetDiskVolumeLabel(string diskName)
        {
            try
            {
                var diskDriver = new DriveInfo(diskName);
                return diskDriver.VolumeLabel;
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        #endregion 获取指定磁盘的卷标

        #region 修改指定磁盘的卷标

        public static bool ChangeDiskVolumeLabel(string diskName, string volumeLabel)
        {
            try
            {
                var diskDriver = new DriveInfo(diskName);
                diskDriver.VolumeLabel = volumeLabel;
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        #endregion

        #region 检测指定目录是否存在
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region 检测指定文件是否存在,如果存在返回true
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }
        #endregion

        #region 获取指定目录中的文件列表
        /// <summary>
        /// 获取指定目录中所有文件列表 含路径
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFilePathNames(string directoryPath)
        {
            string[] files = null;
            try
            {
                //获取文件列表
                files = Directory.GetFiles(directoryPath);
            }
            catch 
            {
                return null;
            }
            return files;
        }

        /// <summary>
        /// 获取指定目录中所有文件列表含扩展名 不含路径
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            string[] files = Directory.GetFiles(directoryPath);
            int i = 0;
            foreach (string file in files)
            {
                files[i] = Path.GetFileName(file);
                i++;
            }
            //获取文件列表
            return files;
        }

        #endregion

        #region 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取指定目录及子目录中所有文件列表
        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 检测指定目录是否为空
        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFilePathNames(directoryPath);
                if (fileNames != null && fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //这里记录日志
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }
        #endregion

        #region 检测指定目录中是否存在指定的文件
        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion

        #region 创建目录
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        #endregion

        #region 删除目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(dir))
                Directory.Delete(dir);
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (System.IO.File.Exists(file))
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch
                {
                    return;
                }
            }
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="dir">带后缀的文件名</param>
        /// <param name="pagestr">文件内容</param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
                CreateDir(dir.Substring(0, dir.LastIndexOf("\\")));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(dir, false, System.Text.Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }
        #endregion

        #region 移动文件(剪贴--粘贴)
        /// <summary>
        /// 移动文件(剪贴--粘贴)
        /// </summary>
        /// <param name="dir1">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="newName">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (System.IO.File.Exists(dir1))
                System.IO.File.Move(dir1, dir2);
        }

        #endregion

        #region 文件重命名(移动)

        public static void FileReName(string dir1, string newName)
        {
            dir1 = dir1.Replace("/", "\\");
            string dir2 = Path.GetDirectoryName(dir1);
            dir2 += "\\" + newName;
            if (System.IO.File.Exists(dir1))
                System.IO.File.Move(dir1, dir2);
        }

        #endregion 文件重命名(移动)

        #region 复制文件
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="destFileName">Name of the dest file.</param>
        public static void CopyFile(string sourceFileName, string destFileName)
        {
            sourceFileName = sourceFileName.Replace("/", "\\");
            destFileName = destFileName.Replace("/", "\\");
            if (System.IO.File.Exists(sourceFileName))
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
            }
        }

        /// <summary>
        /// 复制大文件
        /// CopyFile(@"D:\SQLSVRENT_2008R2_CHS.iso", @"F:\SQLSVRENT_2008R2_CHS.iso", 1024 * 1024 * 5)
        /// </summary>
        /// <param name="fromPath">源文件的路径</param>
        /// <param name="toPath">文件保存的路径</param>
        /// <param name="eachBlackSizeByte">每次读取的长度</param>
        /// <returns>是否复制成功</returns>
        public static bool CopyBigFile(string fromPath, string toPath, int eachBlackSizeByte, bool isCover = false)
        {
            if (DiskDirFileIo.IsExistFile(toPath) && !isCover)
            {
                return false;
            }
            //将源文件 读取成文件流
            FileStream fromFile = null;
            //已追加的方式 写入文件流
            FileStream toFile = null;
            try
            {
                fromFile = new FileStream(fromPath, FileMode.Open, FileAccess.Read);

                toFile = new FileStream(toPath, FileMode.Append, FileAccess.Write);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //实际读取的文件长度
            int toCopyLength = 0;
            //如果每次读取的长度小于 源文件的长度 分段读取
            try
            {
                if (fromFile != null && toFile != null && eachBlackSizeByte < fromFile.Length)
                {
                    byte[] buffer = new byte[eachBlackSizeByte];
                    long copied = 0;
                    while (copied <= fromFile.Length - eachBlackSizeByte)
                    {
                        toCopyLength = fromFile.Read(buffer, 0, eachBlackSizeByte);
                        fromFile.Flush();
                        toFile.Write(buffer, 0, eachBlackSizeByte);
                        toFile.Flush();
                        //流的当前位置
                        toFile.Position = fromFile.Position;
                        copied += toCopyLength;

                    }
                    int left = (int)(fromFile.Length - copied);
                    toCopyLength = fromFile.Read(buffer, 0, left);
                    fromFile.Flush();
                    toFile.Write(buffer, 0, left);
                    toFile.Flush();

                }
                else
                {
                    //如果每次拷贝的文件长度大于源文件的长度 则将实际文件长度直接拷贝
                    byte[] buffer = new byte[fromFile.Length];
                    fromFile.Read(buffer, 0, buffer.Length);
                    fromFile.Flush();
                    toFile.Write(buffer, 0, buffer.Length);
                    toFile.Flush();

                }
            }
            catch (Exception ex)
            {
                fromFile.Close();
                toFile.Close();
                throw ex;
            }
            fromFile.Close();
            toFile.Close();
            return true;
        }


        #endregion

        #region 根据时间得到目录名 / 格式:yyyyMMdd 或者 HHmmssff
        /// <summary>
        /// 根据时间得到目录名yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 根据时间得到文件名HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }
        #endregion

        #region 复制文件夹
        /// <summary>
        /// 复制文件夹(递归)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    System.IO.File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        #endregion

        #region 检查文件,如果文件不存在则创建
        /// <summary>
        /// 检查文件,如果文件不存在则创建  
        /// </summary>
        /// <param name="FilePath">路径,包括文件名</param>
        public static void ExistsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //以上写法会报错,详细解释请看下文.........   
            if (!System.IO.File.Exists(FilePath))
            {
                FileStream fs = System.IO.File.Create(FilePath);
                fs.Close();
            }
        }
        #endregion

        #region 删除指定文件夹对应其他文件夹里的文件
        /// <summary>
        /// 删除指定文件夹对应其他文件夹里的文件
        /// </summary>
        /// <param name="varFromDirectory">指定文件夹路径</param>
        /// <param name="varToDirectory">对应其他文件夹路径</param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    System.IO.File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        /// <summary>
        /// 复制文件参考方法,页面中引用
        /// </summary>
        /// <param name="cDir">新路径</param>
        /// <param name="TempId">模板引擎替换编号</param>
        public static void CopyFiles(string cDir, string TempId)
        {
            //if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            //{
            //    string TempStr = string.Empty;
            //    StreamWriter sw;
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //}
        }

        #region 创建一个目录
        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 创建一个文件
        /// <summary>
        /// 创建一个文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //关闭文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //写入二进制流
                    fs.Write(buffer, 0, buffer.Length);

                    //关闭文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 获取文本文件的行数
        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中
            string[] rows = System.IO.File.ReadAllLines(filePath);

            //返回行数
            return rows.Length;
        }
        #endregion

        #region 获取一个文件的长度
        /// <summary>
        /// 获取一个文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小
            return (int)fi.Length;
        }
        #endregion

        #region 读取文本文件

        /// <summary>
        /// Gets the string file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="count">The count.</param>
        /// <returns>System.String.</returns>
        public static string GetTextFileString(string fileName, int count = 1024)
        {
            byte[] byData = new byte[count];
            char[] charData = new char[count];
            using (FileStream sFile = new FileStream(fileName, FileMode.Open))
            {
                try
                {
                    sFile.Seek(0, SeekOrigin.Begin);
                    sFile.Read(byData, 0, count);
                }
                catch (IOException e)
                {
                    Console.WriteLine("An IO exception has been thrown!");
                    Console.WriteLine(e.ToString());
                    Console.ReadLine();
                    return "";
                }
            }

            Decoder d = Encoding.UTF8.GetDecoder();
            d.GetChars(byData, 0, byData.Length, charData, 0);
            StringBuilder strBuff = new StringBuilder();
            foreach (char VARIABLE in charData)
            {
                if (VARIABLE != '\0')
                {
                    strBuff.Append(VARIABLE);
                }
            }
            return strBuff.ToString();
        }


        #endregion 读取文本文件

        #region 获取指定目录中的子目录列表
        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 向文本文件写入内容

        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            //向文件写入内容
            System.IO.File.WriteAllText(filePath, text, encoding);
        }
        #endregion

        #region 向文本文件的尾部追加内容
        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            System.IO.File.AppendAllText(filePath, content);
        }
        #endregion

        #region 将现有文件的内容复制到新文件中
        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            System.IO.File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion

        #region 将文件移动到指定目录
        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录
                System.IO.File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        #endregion

        #region 从文件的绝对路径中获取扩展名
        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion

        #region 清空指定目录

        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件
                string[] fileNames = GetFilePathNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    try
                    {
                        DeleteFile(fileNames[i]);
                    }
                    catch (Exception)
                    {

                    }
                }

                //删除目录中所有的子目录
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }

        #endregion

        #region 清空文件内容

        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件
            System.IO.File.Delete(filePath);

            //重新创建该文件
            CreateFile(filePath);
        }

        #endregion

        #region 删除指定目录
        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion
    }


    public class DiskFilseInfo
    {
        private string diskLogicName = "";//默认为存储器根目录

        List<string> _objectJpgFilseList = new List<string>();//jpg 文件列表
        List<string> _objectWavFilseList = new List<string>();//Wav  文件列表
        List<string> _objectOtherFilseList = new List<string>();//其他 文件列表 带扩展名
        List<string> _subDirectoriesList = new List<string>();//子目录列表

        List<string> _nonExistentJpgFilseList = new List<string>();//不存在的jpg 文件列表

        List<string> _nonExistentWavFilseList = new List<string>();//不存在的Wav 文件列表

        List<string> _spareJpgFilseList = new List<string>();//多余的的jpg 文件列表

        List<string> _spareWavFilseList = new List<string>();//多余的的Wav 文件列表

        List<string> _spareOtherFilseList = new List<string>();//多余的的Wav 文件列表

        List<string> _nonExistentOtherFilseList = new List<string>();//不存在的其他 文件列表  带扩展名

        #region Attribute def

        public string DiskLogicName
        {
            get { return diskLogicName; }
            set { diskLogicName = value; }
        }

        /// <summary>
        /// Gets or sets the object JPG filse list.
        /// 带扩展名
        /// </summary>
        /// <value>The object JPG filse list.</value>
        public List<string> ObjectJpgFilseList
        {
            get { return _objectJpgFilseList; }
            set { _objectJpgFilseList = value; }
        }

        /// <summary>
        /// Gets or sets the object wav filse list.
        /// 带扩展名
        /// </summary>
        /// <value>The object wav filse list.</value>
        public List<string> ObjectWavFilseList
        {
            get { return _objectWavFilseList; }
            set { _objectWavFilseList = value; }
        }

        /// <summary>
        /// Gets or sets the object other filse list.
        /// 带扩展名
        /// </summary>
        /// <value>The object other filse list.</value>
        public List<string> ObjectOtherFilseList
        {
            get { return _objectOtherFilseList; }
            set { _objectOtherFilseList = value; }
        }

        public List<string> NonExistentJpgFilseList
        {
            get { return _nonExistentJpgFilseList; }
            set { _nonExistentJpgFilseList = value; }
        }

        public List<string> NonExistentWavFilseList
        {
            get { return _nonExistentWavFilseList; }
            set { _nonExistentWavFilseList = value; }
        }

        public List<string> SpareJpgFilseList
        {
            get { return _spareJpgFilseList; }
            set { _spareJpgFilseList = value; }
        }

        public List<string> SpareWavFilseList
        {
            get { return _spareWavFilseList; }
            set { _spareWavFilseList = value; }
        }

        public List<string> SpareOtherFilseList
        {
            get { return _spareOtherFilseList; }
            set { _spareOtherFilseList = value; }
        }


        #endregion //Attribute

        /// <summary>
        /// Initializes a new instance of the <see cref="DiskFilseInfo"/> class.
        /// </summary>
        /// <param name="scdDiskVolumeLabel">The SCD disk volume label.</param>
        public DiskFilseInfo(String logicName)
        {
            diskLogicName = logicName;
        }

        /// <summary>
        /// 获取ScdDiskLocation指定位置的文件分类列表
        /// jpgFiles，wavFiles不需要输入扩展名;otherFiles 需要指定扩展名
        /// </summary>
        /// <param name="jpgFiles">The JPG files.</param>
        /// <param name="wavFiles">The wav files.</param>
        /// <param name="otherFiles">The other files.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetDiskFilstClassList(List<string> jpgFiles, List<string> wavFiles, List<string> otherFiles)
        {
            if (diskLogicName != "")
            {
                string[] filsList = DiskDirFileIo.GetFilePathNames(diskLogicName);
                foreach (string s in filsList)
                {
                    string fileNameWithExtension = Path.GetFileName(s);
                    string fileName = Path.GetFileNameWithoutExtension(s);
                    string filseExtension = Path.GetExtension(s).ToLower();
                    switch (filseExtension)
                    {
                        case ".jpg":
                            {
                                _objectJpgFilseList.Add(fileName);
                                if (!jpgFiles.Exists(x => x == fileName))
                                {
                                    _spareJpgFilseList.Add(fileName);//
                                }
                                break;
                            }
                        case ".wav":
                            {
                                _objectWavFilseList.Add(fileName);
                                if (!wavFiles.Exists(x => x == fileName))
                                {
                                    _spareWavFilseList.Add(fileName);//
                                }
                                break;
                            }
                        default:
                            {
                                _objectOtherFilseList.Add(fileNameWithExtension);
                                if (!otherFiles.Exists(x => x == fileNameWithExtension))
                                {
                                    _spareOtherFilseList.Add(fileNameWithExtension);//
                                }//带扩展名
                                break;
                            }
                    }
                }
                _subDirectoriesList.AddRange(DiskDirFileIo.GetDirectories(diskLogicName));//子目录

                foreach (string fileName in jpgFiles)
                {
                    if (!_objectJpgFilseList.Exists(x => x == fileName))
                    {
                        _nonExistentJpgFilseList.Add(fileName);//
                    }
                }
                foreach (string fileName in wavFiles)
                {
                    if (!_objectWavFilseList.Exists(x => x == fileName))
                    {
                        _nonExistentWavFilseList.Add(fileName);//
                    }
                }
                foreach (string file in otherFiles)
                {
                    if (!_objectOtherFilseList.Exists(x => x == file))
                    {
                        _nonExistentOtherFilseList.Add(file);//
                    }
                }
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Class TxtFileEncoding.
    /// txt文件生成时的编码
    /// </summary>
    public class TxtFileEncoding
    {
        public TxtFileEncoding()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }

        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }

        /// <summary>
        /// 取得一个文本文件的编码方式。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }

        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }

                //根据文件流的前4个字节判断Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }

                //恢复Seek位置      
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
    }
}
