namespace thermalprinting.Controllers
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    public class PrintController : ApiController
    {
        [AllowAnonymous]
        [Route("api/Print/Send")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public IHttpActionResult Send([FromBody]JObject data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (data == null)
                {
                    return BadRequest("Check if the json is well formed.");
                }

                ThermalReport report = new ThermalReport();
                string invoice = report.generateInvoice(data);

                string pathtoPrint = GetFile(invoice);

                Print(pathtoPrint);
            }
            catch (Exception ex)
            {
                return BadRequest(string.Format("Error:{0}, Stacktrace:{1}", ex.Message, ex.StackTrace));
            }

            return Ok();
        }

        /// <summary>
        /// Generate the filename in the local app data and write the file with the invoice to print.
        /// </summary>
        /// <param name="content">Content to print.</param>
        /// <returns>Filename.</returns>
        private string GetFile(string content)
        {
            string pathtoPrint = string.Empty;
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string subFolderPath = Path.Combine(path, "Ofidental");
                pathtoPrint = string.Format("{0}{1}{2}.txt",
                   subFolderPath,
                   DateTime.Now.ToString("yyyyMMddhhmmss"),
                   Guid.NewGuid().ToString());

                File.WriteAllText(pathtoPrint, content);                
            }
            catch(Exception ex)
            {
                throw new Exception("Error writing file report: " + ex.Message);
            }

            return pathtoPrint;
        }

        /// <summary>
        /// Send the file to print.
        /// </summary>
        /// <param name="path">Filename.</param>
        private void Print(string path)
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
