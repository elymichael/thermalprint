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
        public IHttpActionResult Send(JObject data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                
                ThermalReport report = new ThermalReport();
                string invoice = report.generateInvoice(data);

                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string subFolderPath = Path.Combine(path, "Ofidental");
                string pathtoPrint = string.Format("{0}{1}{2}.txt",
                   subFolderPath,
                   DateTime.Now.ToString("yyyyMMddhhmmss"),
                   Guid.NewGuid().ToString());

                System.IO.File.WriteAllText(pathtoPrint, invoice);

                Print(pathtoPrint);                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }

        private void Print(string path)
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
    }
}
