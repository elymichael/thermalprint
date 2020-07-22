namespace thermalprinting
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class ThermalReport : ThermalReportBase
    {
        public ThermalReport(JObject data) : base(data)
        {

        }
        /// <summary>
        /// Generate the invoice report.
        /// </summary>
        /// <param name="data">Json data</param>
        /// <returns>string report.</returns>
        public string generateInvoice()
        {
            StringBuilder report = new StringBuilder();
            try
            {
                JToken company = Data.GetValue(RestApiConstant.CompanyNode);
                JToken sales = Data.GetValue(RestApiConstant.SalesNode);

                report.Append(GetHeader());

                if (sales["CreationDate"] != null)
                {
                    report.AppendLine(Convert.ToDateTime(sales["CreationDate"].Value<string>()).ToString("dd/MM/yyyy hh:mm:ss").PadRight(RestApiConstant.COST_TAM));
                }
                if (company[RestApiConstant.Company.Data].HasValues)
                {
                    if (company[RestApiConstant.Company.Data][RestApiConstant.Company.NIF] != null)
                    {
                        report.AppendLine(PadBoth(("NIF:" + company[RestApiConstant.Company.Data][RestApiConstant.Company.NIF].Value<string>()), RestApiConstant.COST_TAM));
                    }
                }

                if (sales["data"].HasValues)
                {
                    if (sales["data"]["payments"] != null)
                    {
                        if (sales["data"]["payments"]["taxreceiptnumber"] != null)
                        {
                            report.AppendLine("NCF:" + sales["data"]["payments"]["taxreceiptnumber"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                        }
                        if (sales["data"]["batch"] != null)
                        {
                            if (sales["data"]["batch"]["name"] != null)
                            {
                                report.AppendLine("Caja #:" + sales["data"]["batch"]["name"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                            }
                        }
                    }
                }

                if (sales["data"]["invoiceid"] != null)
                {
                    report.AppendLine(("Venta No:" + sales["data"]["invoiceid"].Value<string>()).PadRight(RestApiConstant.COST_TAM));
                }
                if (sales["data"].HasValues)
                {
                    if (sales["data"]["customer"] != null)
                    {
                        if (sales["data"]["customer"]["rnc"] != null)
                        {
                            report.AppendLine("RNC/Cliente:" + sales["data"]["customer"]["rnc"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                        }
                    }
                }

                report.AppendLine("---------------------------------------");
                //"   FACTURA PARA CONSUMIDORES FINALES   "
                report.AppendLine(PadBoth("FACTURA DE VENTAS", RestApiConstant.COST_TAM));
                report.AppendLine("---------------------------------------");
                report.AppendLine("DESCIPCION           ITBIS     VALOR   ");
                report.AppendLine("---------------------------------------");

                if (sales["data"].HasValues)
                {
                    if (sales["data"]["items"].HasValues)
                    {
                        const int first_col_pad = 18;
                        const int second_col_pad = 8;
                        const int third_col_pad = 10;
                        const int name_length = 16;

                        foreach (JToken item in sales["data"]["items"])
                        {
                            report.Append(item["name"].Value<string>().Length > name_length ? item["name"].Value<string>().Substring(0, name_length) : item["name"].Value<string>().PadRight(name_length, ' '));

                            report.Append(string.Format("{0:N2}", item["itbis"].Value<string>()).PadLeft(second_col_pad));
                            report.AppendLine(string.Format("{0:N2}", item["price"].Value<string>()).PadLeft(third_col_pad));
                            report.AppendLine(string.Format("{0:N2}", item["quantity"].Value<string>()).PadLeft(first_col_pad));
                        }

                        report.AppendLine(RestApiConstant.BlankSpace);
                        string strLineaFinal = "TOTAL A PAGAR".PadRight(name_length, ' ');
                        report.Append(strLineaFinal.PadRight(first_col_pad));


                        report.Append(string.Format("{0:N2}", "0").PadLeft(second_col_pad));
                        report.AppendLine(string.Format("{0:N2}", sales["totalAmount"].Value<string>()).PadLeft(third_col_pad));


                        report.AppendLine(RestApiConstant.BlankSpace);
                        strLineaFinal = "TOTAL PAGADO".PadRight(first_col_pad, ' ');
                        report.Append(strLineaFinal.PadRight(first_col_pad));
                        report.Append(RestApiConstant.BlankSpace.PadLeft(second_col_pad));
                        report.AppendLine(string.Format("{0:N2}", 0).PadLeft(third_col_pad));

                        strLineaFinal = "CAMBIO".PadRight(name_length, ' ');
                        report.Append(strLineaFinal.PadRight(first_col_pad));
                        report.Append(RestApiConstant.BlankSpace.PadLeft(second_col_pad));
                        report.AppendLine(string.Format("{0:N2}", 0).PadLeft(third_col_pad));
                        report.AppendLine("---------------------------------------");

                        if (sales["data"]["payments"] != null)
                        {
                            foreach (JToken item in sales["data"]["payments"]["details"])
                            {
                                report.Append(item["payment"]["paymentmethod"]["text"].Value<string>().PadRight(name_length, ' ').PadRight(first_col_pad));
                                report.Append(RestApiConstant.BlankSpace.PadLeft(second_col_pad));
                                report.AppendLine(string.Format("{0:N2}", item["payment"]["amount"].Value<string>()).PadLeft(third_col_pad));
                            }
                        }
                        report.AppendLine("---------------------------------------");
                        //  strLinea.AppendLine(" ITEMS * SON GRAVADOS CON 18% DE ITBIS");
                        //   strLinea.AppendLine(" ITEMS # SON GRAVADOS CON 16% DE ITBIS");
                        strLineaFinal = "       ITBIS ( 18% ) ";
                        report.Append(strLineaFinal.PadRight(first_col_pad));
                        report.AppendLine(string.Format("{0:N2}", 0.ToString("N2").PadLeft(third_col_pad)));
                        report.AppendLine(RestApiConstant.BlankSpace);

                        report.AppendLine("TOTAL ARTICULOS VENDIDOS = " + sales["quantity"].Value<string>());

                        report.AppendLine(RestApiConstant.BlankSpace);
                        report.AppendLine("      GRACIAS POR SU VISITA          ");
                        report.AppendLine(RestApiConstant.BlankSpace);
                        report.AppendLine(RestApiConstant.BlankSpace);

                        if (sales["data"]["cashier"] != null)
                        {
                            report.AppendLine("Atendido Por: " + sales["data"]["cashier"]["name"].Value<string>());
                        }
                        report.AppendLine(RestApiConstant.BlankSpace);
                        report.AppendLine(RestApiConstant.BlankSpace);

                        string strResult = report.ToString().Normalize(NormalizationForm.FormD);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating report: " + ex.Message);
            }
            return report.ToString();
        }

        /// <summary>
        /// Generate the invoice report.
        /// </summary>
        /// <param name="data">Json data</param>
        /// <returns>string report.</returns>
        public string generateTicket()
        {
            StringBuilder report = new StringBuilder();
            try
            {
                //JToken company = data.GetValue("company");
                JToken ticket = Data.GetValue("ticket");

                report.Append(GetHeader());

                if (ticket["date"] != null)
                {
                    report.AppendLine(Convert.ToDateTime(ticket["date"].Value<string>()).ToString("dd/MM/yyyy hh:mm:ss").PadRight(RestApiConstant.COST_TAM));
                }

                if (ticket["ticketnumber"] != null)
                {
                    report.AppendLine(("Venta No:" + ticket["ticketnumber"].Value<string>()).PadRight(RestApiConstant.COST_TAM));
                }

                report.AppendLine("TIPO DE ENVASE:" + ticket["boxtype"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                report.AppendLine("FECHA DE ENTREGA:" + Convert.ToDateTime(ticket["deliverydate"].Value<string>()).ToString("dd/MM/yyyy hh:mm:ss").PadRight(RestApiConstant.COST_TAM));

                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine("---------------------------------------");
                report.AppendLine("NOMBRE:" + ticket["customer"]["name"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                report.AppendLine("DOCUMENTO:" + ticket["customer"]["document"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                report.AppendLine("---------------------------------------");
                report.AppendLine(RestApiConstant.BlankSpace);

                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine("      GRACIAS POR SU VISITA          ");
                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine(RestApiConstant.BlankSpace);

                if (ticket["cashier"] != null)
                {
                    report.AppendLine("Atendido Por:" + ticket["cashier"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                }
                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine(RestApiConstant.BlankSpace);

                string strResult = report.ToString().Normalize(NormalizationForm.FormD);

            }
            catch (Exception ex)
            {
                throw new Exception("Error generating report: " + ex.Message);
            }
            return report.ToString();
        }

        /// <summary>
        /// Generate the invoice report.
        /// </summary>
        /// <param name="data">Json data</param>
        /// <returns>string report.</returns>
        public string generateReceipts()
        {
            StringBuilder report = new StringBuilder();
            try
            {
                JToken receipt = Data.GetValue("receipt");

                const int first_col_pad = 18;
                const int second_col_pad = 8;
                const int third_col_pad = 10;
                const int name_length = 16;

                report.Append(GetHeader());

                if (receipt["date"] != null)
                {
                    report.AppendLine(Convert.ToDateTime(receipt["date"].Value<string>()).ToString("dd/MM/yyyy hh:mm:ss").PadRight(RestApiConstant.COST_TAM));
                }

                if (receipt["ticketnumber"] != null)
                {
                    report.AppendLine(("Venta No:" + receipt["ticketnumber"].Value<string>()).PadRight(RestApiConstant.COST_TAM));
                }

                report.AppendLine(RestApiConstant.BlankSpace);
                string strLineaFinal = "CUOTA A PAGAR".PadRight(name_length, ' ');
                report.Append(strLineaFinal.PadRight(first_col_pad));


                report.Append(string.Format("{0:N2}", "0").PadLeft(second_col_pad));
                report.AppendLine(string.Format("{0:N2}", receipt["totalAmount"].Value<string>()).PadLeft(third_col_pad));


                report.AppendLine(RestApiConstant.BlankSpace);
                strLineaFinal = "TOTAL PAGADO".PadRight(first_col_pad, ' ');
                report.Append(strLineaFinal.PadRight(first_col_pad));
                report.Append(RestApiConstant.BlankSpace.PadLeft(second_col_pad));
                report.AppendLine(string.Format("{0:N2}", 0).PadLeft(third_col_pad));

                strLineaFinal = "CAMBIO".PadRight(name_length, ' ');
                report.Append(strLineaFinal.PadRight(first_col_pad));
                report.Append(RestApiConstant.BlankSpace.PadLeft(second_col_pad));
                report.AppendLine(string.Format("{0:N2}", 0).PadLeft(third_col_pad));
                report.AppendLine("---------------------------------------");

                report.AppendLine(RestApiConstant.BlankSpace);
                strLineaFinal = "SALDO CONTABLE".PadRight(name_length, ' ');
                report.Append(strLineaFinal.PadRight(first_col_pad));


                report.Append(string.Format("{0:N2}", "0").PadLeft(second_col_pad));
                report.AppendLine(string.Format("{0:N2}", receipt["totalPayment"].Value<string>()).PadLeft(third_col_pad));

                if (receipt["data"]["payments"] != null)
                {
                    JToken itemP = receipt["data"]["payments"];
                    if (itemP["data"]["payments"] != null)
                    {
                        foreach (JToken item in itemP["data"]["payments"]["details"])
                        {
                            report.Append(item["payment"]["paymentmethod"]["text"].Value<string>().PadRight(name_length, ' ').PadRight(first_col_pad));
                            report.Append(RestApiConstant.BlankSpace.PadLeft(second_col_pad));
                            report.AppendLine(string.Format("{0:N2}", item["payment"]["amount"].Value<string>()).PadLeft(third_col_pad));
                        }
                    }
                }
                if (receipt["data"] != null)
                {
                    report.AppendLine("---------------------------------------");
                    report.AppendLine(RestApiConstant.BlankSpace);
                    report.AppendLine("---------------------------------------");
                    report.AppendLine("NOMBRE:" + receipt["data"]["customer"]["name"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                    report.AppendLine("DOCUMENTO:" + receipt["data"]["customer"]["rnc"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                    report.AppendLine("---------------------------------------");
                    report.AppendLine(RestApiConstant.BlankSpace);
                }
                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine("      GRACIAS POR SU VISITA          ");
                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine(RestApiConstant.BlankSpace);

                if (receipt["data"] != null)
                {
                    if (receipt["data"]["cashier"] != null)
                    {
                        report.AppendLine("Atendido Por:" + receipt["data"]["cashier"]["name"].Value<string>().PadRight(RestApiConstant.COST_TAM));
                    }
                }
                report.AppendLine(RestApiConstant.BlankSpace);
                report.AppendLine(RestApiConstant.BlankSpace);

                string strResult = report.ToString().Normalize(NormalizationForm.FormD);

            }
            catch (Exception ex)
            {
                throw new Exception("Error generating report: " + ex.Message);
            }
            return report.ToString();
        }
    }
}