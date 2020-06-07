
namespace thermalprinting.Tests
{
    using System;
    using System.Web;
    
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    
    [TestClass]
    public class UnitTestPrinting
    {
        [TestMethod]
        public void TestMethodPrintingInvoice()
        {
            thermalprinting.Controllers.PrintController print = new Controllers.PrintController();
            string invoice_data = @"{    ""payments"": {      ""taxreceiptnumbertype"": ""00"",      ""details"": [        {          ""payment"": {            ""amount"": 450,            ""paymentmethod"": {              ""value"": ""E"",              ""text"": ""Efectivo""            }          }        }      ]    },    ""items"": [      {        ""productID"": 19,        ""name"": ""Equilibración de oclusión"",        ""price"": ""100"",        ""description"": ""Equilibración de oclusión"",        ""quantity"": 2,        ""itbis"": 0      },      {        ""productID"": 18,        ""name"": ""Ferulización"",        ""price"": ""250"",        ""description"": ""Ferulización"",        ""quantity"": 1,        ""itbis"": 0      }    ]  }";
            

            JObject data = new JObject(
                new JProperty("company", 
                    new JObject(
                        new JProperty("CompanyID","1"),
                        new JProperty("Name", "Facultad de Odontologia"),
                        new JProperty("data", new JObject(
                            new JProperty("address", "Calle Cristóbal de Llerenas, Campus Alma Mater, Zona Universitaria"),
                            new JProperty("rnc", "00117292896")
                            )))),
               new JProperty("sales", 
                    new JObject(
                        new JProperty("SaleOrderID", "10"),
                        new JProperty("SalesOrderControlID", "3"),
                        new JProperty("CompanyID", "1"),
                        new JProperty("PatientID", "1"),
                        new JProperty("StatusId", "P"),
                        new JProperty("TotalAmount", "450"),
                        new JProperty("Quantity", "3"),
                        new JProperty("data", JObject.Parse(invoice_data)),
                        new JProperty("CreationDate", DateTime.Now),
                        new JProperty("UserID", "1")
                    )));

            string dataprint = data.ToString();

            print.Send(data);
            //var task = Task.Run(async () => await print.Send(data));


        }
    }
}
