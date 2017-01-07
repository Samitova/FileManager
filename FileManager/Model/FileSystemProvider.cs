using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Model
{
    public class FileSystemProvider
    {
        /// <summary>
        /// Get the local drivers of the system
        /// </summary>
        /// <returns>Return the list of local drivers</returns>
        public static IList<DriveInfo> GetLocalDrives()
        {
            return DriveInfo.GetDrives().ToList();
        }

        /// <summary>
        /// Get the  drive by name
        /// </summary>
        /// <returns>Driveinfo</returns>
        public static DriveInfo GetDrive(string path)
        {
            return new DriveInfo(path);
        }

        /// <summary>
        /// Get the directory
        /// </summary>
        /// <returns>Return the list of local drivers</returns>
        public static DirectoryInfo GetDirectory(string path)
        {
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Get the directory
        /// </summary>
        /// <returns>Return the list of local drivers</returns>
        public static FileInfo GetFile(string path)
        {
            return new FileInfo(path);
        }

        /// <summary>
        /// Get the list of files 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Return the list of children files</returns>
        public static IList<FileInfo> GetChildrenFiles(string path)
        {
            try
            {               
                return new DirectoryInfo(path).GetFiles().ToList();
            }

            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return new List<FileInfo>();
        }

        /// <summary>
        /// Get the list of directories 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Return the list of children directories</returns>
        public static IList<DirectoryInfo> GetChildrenDirectories(string path)
        {
            try
            {
                return new DirectoryInfo(path).GetDirectories().ToList();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return new List<DirectoryInfo>();
        }
    }
}
