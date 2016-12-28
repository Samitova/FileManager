using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager.Model
{
    public class FileSystemExplorerService
    {

        /// <summary>
        /// Get the local drivers of the system
        /// </summary>
        /// <returns>Return the list of local drivers</returns>
        public static IList<DriveInfo> GetLocalDrivers()
        {
            return DriveInfo.GetDrives().ToList();
        }


        /// <summary>
        /// Get the list of files 
        /// </summary>
        /// <param name="path">The directory to get the files from</param>
        /// <returns>Returns the List of File info for this directory. Return null if an exception is raised</returns>
        public static IList<FileInfo> GetChildFiles(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                return dir.GetFiles().ToList();               
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
        /// <param name="path">The directory to get the files from</param>
        /// <returns>Returns the List of directories info for this directory. Return null if an exception is raised</returns>
        public static IList<DirectoryInfo> GetChildDirectories(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                return dir.GetDirectories().ToList();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return new List<DirectoryInfo>();
        }     
    }
}
