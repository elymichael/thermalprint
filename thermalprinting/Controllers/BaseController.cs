namespace thermalprinting.Controllers
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Web.Http;

    public class BaseController : ApiController
    {
        /// <summary>
        /// check if the json is well formed.
        /// </summary>
        /// <param name="data">Json data object</param>
        protected void load(JObject data)
        {
            if (data == null)
            {
                throw new Exception("Check if the json is well formed.");
            }
        }

        /// <summary>
        /// Generate the filename in the local app data and write the file with the invoice to print.
        /// </summary>
        /// <param name="content">Content to print.</param>
        /// <returns>Filename.</returns>
        protected string GetFile(string content)
        {
            string pathtoPrint = string.Empty;
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string subFolderPath = Path.Combine(path, "Ofidental\\");

                if (!Directory.Exists(subFolderPath))
                {
                    Directory.CreateDirectory(subFolderPath);
                }

                pathtoPrint = string.Format("{0}{1}_{2}.txt",
                   subFolderPath,
                   DateTime.Now.ToString("yyyyMMddhhmmss"),
                   Guid.NewGuid().ToString());

                File.WriteAllText(pathtoPrint, content);
            }
            catch (Exception ex)
            {
                throw new Exception("Error writing file report: " + ex.Message);
            }

            return pathtoPrint;
        }

        /// <summary>
        /// Send the file to print.
        /// </summary>
        /// <param name="path">Filename.</param>
        protected void Print(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var printJob = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = path,
                            UseShellExecute = true,
                            Verb = "print",
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            WorkingDirectory = Path.GetDirectoryName(path)
                        }
                    };

                    printJob.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error printing report: " + ex.Message);
            }
        }
    }
}
