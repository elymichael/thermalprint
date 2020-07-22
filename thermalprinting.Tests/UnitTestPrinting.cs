
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
            Controllers.PrintController print = new Controllers.PrintController();
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
                        new JProperty("saleOrderID", "10"),
                        new JProperty("salesOrderControlID", "3"),
                        new JProperty("companyID", "1"),
                        new JProperty("patientID", "1"),
                        new JProperty("statusId", "P"),
                        new JProperty("totalAmount", "450"),
                        new JProperty("quantity", "3"),
                        new JProperty("data", JObject.Parse(invoice_data)),
                        new JProperty("creationDate", DateTime.Now),
                        new JProperty("userID", "1")
                    )));

            string dataprint = data.ToString();

            print.Invoice(data);            
        }

        [TestMethod]
        public void TestMethodPrintingTickets()
        {
            Controllers.PrintController print = new Controllers.PrintController();
        }

        [TestMethod]
        public void TestMethodPrintingReceipts()
        {
            Controllers.PrintController print = new Controllers.PrintController();

            string dataprint = @"{
  ""company"": {
    ""CompanyID"": 1,
    ""Name"": ""Facultad de Odontologia"",
    ""data"": {
      ""address"": ""Calle Cristobal de Llerenas, Campus Alma Mater, Zona Universitaria."",
      ""phone"": ""(809) 535-8274 ext. 3184"",
      ""contact"": ""Angel Nadal"",
      ""rnc"": ""00117292896"",
      ""companyGroupID"": 1
    }
  },
  ""receipt"": {
    ""id"": 1,
    ""date"": ""2020-07-22T01:22:28.782Z"",
    ""ticketnumber"": ""r04dvs6o8dkcwomkev"",
    ""totalAmount"": 250,
    ""totalPayment"": 200,
    ""data"": {
      ""payments"": {
        ""customer"": {
          ""ID"": ""C2"",
          ""customerID"": 2,
          ""patientID"": 0,
          ""type"": ""C"",
          ""searchresult"": ""Miledys Taveras (001151945780) "",
          ""name"": ""Miledys Taveras"",
          ""documentId"": ""001151945780""
        },
        ""data"": {
          ""payments"": {
            ""taxreceiptnumbertype"": ""00"",
            ""details"": [
              {
                ""payment"": {
                  ""amount"": 50,
                  ""paymentmethod"": {
                    ""value"": ""E"",
                    ""text"": ""Efectivo""
                  },
                  ""change"": -50
                }
              }
            ]
          }
        },
        ""amount"": ""50"",
        ""description"": ""Pago de tercera cuota"",
        ""applyApyment"": true,
        ""created"": ""2020-07-22T01:22:23.002Z""
      },
      ""customer"": {
        ""id"": ""C2"",
        ""rnc"": ""001151945780"",
        ""name"": ""Miledys Taveras""
      },
      ""cashier"": {
        ""name"": ""Ely Michael Núñez De la Rosa""
      }
    }
  }
}";

            JObject data = JObject.Parse(dataprint);

            print.Receipts(data);
        }
    }
}
