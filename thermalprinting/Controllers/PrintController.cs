namespace thermalprinting.Controllers
{    
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class PrintController : ApiController
    {
        [AllowAnonymous]
        [Route("api/Print/Send")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public async Task<IHttpActionResult> Send(JObject data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                
                ThermalReport report = new ThermalReport();
                report.generateInvoice(data);
                //System.IO.File.WriteAllText("C:\\filename.txt", "contenido");

                //PrintPreviewDialog pp = new PrintPreviewDialog();
                //PaperSize psize = new PaperSize("Custom", 100, 200);
                //printDocument.DefaultPageSettings.PaperSize = psize;
                //// 5 es el tamaño de las letras y 150 es el espacio extra
                //printDocument.DefaultPageSettings.PaperSize.Height = 900 + (dtFactura.Rows.Count * 38);
                //printDocument.DefaultPageSettings.PaperSize.Width = 300;


                //printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printPage); //add an event handler that will do the printing

                //pp.Document = printDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }
    }
}
