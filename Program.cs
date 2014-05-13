using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace DeleteCurrentlyPlaying
{
    class Program
    {
        static string[] fileExtensions = new string[] { ".mp3", ".wma", "aac", "m4a", "wav" };
        const string deleteFolder = "To Delete"; 
        const int inputIndex = 21; 

        static void Main(string[] args)
        {
            string fileToDelete = null;
           
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                if (line.Length > inputIndex && (fileExtensions.Any<string>(ext => line.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))))
                {
                    fileToDelete = line.Substring(inputIndex); 
                }
            }

            if (fileToDelete == null) {
                Console.WriteLine("No currently playing media file found."); 
                return; 
            }

            try
            {
                Console.WriteLine("Found media file: " + fileToDelete);  
                while (FileIsLocked(fileToDelete)) {
                    Console.WriteLine("File is in use. Waiting..."); 
                    Thread.Sleep(5000); 
                }
                if (!Directory.Exists(deleteFolder))
                {
                    Directory.CreateDirectory(deleteFolder); 
                }
                File.Move(fileToDelete, deleteFolder + "\\"+ fileToDelete.Substring(fileToDelete.LastIndexOf("\\")+1));
                Console.WriteLine("File moved to \""+deleteFolder+"\".");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to delete " + fileToDelete);
                Console.WriteLine(ex.Message); 
            }    
        }

        static bool FileIsLocked(string path)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);

                return false;
            }
            catch
            {
                return true;
            }
            finally
            {
                if (fs != null) fs.Close(); 
            }
        }
    }
}

