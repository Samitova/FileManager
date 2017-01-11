using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace FileManager.Model
{
    public class FileSystemProvider
    {
        public static event EventHandler FoundItem;
        public static event EventHandler ChangeDirectory;
               
        public static string CurrentSearchDir { get; set; }
        public static string CurrentFoundFile { get; set; }               

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

        /// <summary>
        /// Search files
        /// </summary>
        public static void SearchFiles(string pattern, string path, Action<int> progress, Func<bool> cancel)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Search path wasn`t found");
                return;
            }
            else
            {                
                SearchDir(pattern, path, progress, cancel);
            }
        }

        /// <summary>
        /// Recurent search files in directory and subdirectories
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <param name="cancel"></param>
        private static void SearchDir(string pattern, string path, Action<int> progress, Func<bool> cancel)
        {
            string[] dirs = Directory.GetDirectories(path);
            SearchFilesInDir(pattern, path, progress, cancel);

            foreach (string dir in dirs)
            {
                if (cancel())
                    return;

                CurrentSearchDir = dir;
                OnChangeDirectory(new EventArgs());

                try
                {
                    SearchFilesInDir(pattern, dir, progress, cancel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());                    
                }
                try
                {
                    SearchDir(pattern, dir, progress, cancel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// Search files in directory
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="dir"></param>
        /// <param name="progress"></param>
        /// <param name="cancel"></param>
        private static void SearchFilesInDir(string pattern, string dir, Action<int> progress, Func<bool> cancel)
        {
            EventArgs args = new EventArgs();
            string[] files = Directory.GetFiles(dir, pattern);
            foreach (string file in files)
            {
                if (cancel())
                    return;

                CurrentFoundFile = file;
                OnFoundItem(args);
            }
        }

        /// <summary>
        /// Occurs when file is found
        /// </summary>
        /// <param name="e"></param>
        public static void OnFoundItem(EventArgs e)
        {
            FoundItem?.Invoke(null, e);            
        }

        /// <summary>
        /// Occurs when dir is changed during searching
        /// </summary>
        /// <param name="e"></param>
        private static void OnChangeDirectory(EventArgs e)
        {
            ChangeDirectory?.Invoke(null, e);            
        }
    }
}
