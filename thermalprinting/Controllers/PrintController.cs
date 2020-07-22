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

    public class PrintController : BaseController
    {

        [AllowAnonymous]
        [Route("api/Print/Invoice")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public IHttpActionResult Invoice([FromBody]JObject data)
        {
            try
            {
                base.Load(data);

                ThermalReport report = new ThermalReport(data);
                string invoice = report.generateInvoice();

                string pathtoPrint = GetFile(invoice);

                Print(pathtoPrint);
            }
            catch (Exception ex)
            {
                return BadRequest(string.Format("Error:{0}, Stacktrace:{1}", ex.Message, ex.StackTrace));
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("api/Print/Tickets")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public IHttpActionResult Tickets([FromBody]JObject data)
        {
            try
            {
                Load(data);

                ThermalReport report = new ThermalReport(data);
                string invoice = report.generateTicket();

                string pathtoPrint = GetFile(invoice);

                Print(pathtoPrint);
            }
            catch (Exception ex)
            {
                return BadRequest(string.Format("Error:{0}, Stacktrace:{1}", ex.Message, ex.StackTrace));
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("api/Print/Receipts")]
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        public IHttpActionResult Receipts([FromBody]JObject data)
        {
            try
            {
                Load(data);

                ThermalReport report = new ThermalReport(data);
                string invoice = report.generateReceipts();

                string pathtoPrint = GetFile(invoice);

                Print(pathtoPrint);
            }
            catch (Exception ex)
            {
                return BadRequest(string.Format("Error:{0}, Stacktrace:{1}", ex.Message, ex.StackTrace));
            }

            return Ok();
        }
    }
}
