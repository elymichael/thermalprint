

namespace thermalprinting
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public class ThermalReport
    {
        /// <summary>
        /// Add Pad to both side of the string.
        /// </summary>
        /// <param name="source">content to add pad.</param>
        /// <param name="length">length of the content.</param>
        /// <returns></returns>
        private string PadBoth(string source, int length)
        {

            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft).PadRight(length);
        }

        /// <summary>
        /// Generate the invoice report.
        /// </summary>
        /// <param name="data">Json data</param>
        /// <returns>string report.</returns>
        public string generateInvoice(JObject data)
        {
            const int COST_TAM = 30;

            StringBuilder report = new StringBuilder();
            try
            {
                JToken company = data.GetValue("company");
                JToken sales = data.GetValue("sales");

                report.AppendLine(PadBoth(company["Name"].Value<string>(), COST_TAM));
                if (company["data"].HasValues)
                {
                    JObject oData = company["data"].Value<JObject>();

                    if (oData["address"] != null)
                    {
                        IEnumerable<string> lines = SplitLine(oData["address"].Value<string>(), COST_TAM);
                        foreach (string str in lines)
                        {
                            report.AppendLine(PadBoth(str, COST_TAM));
                        }
                    }
                    if (oData["rnc"] != null)
                    {
                        report.AppendLine(PadBoth(("RNC:" + oData["rnc"].Value<string>()), COST_TAM));
                    }
                    report.AppendLine(" ");
                    if (oData["authorized"] != null)
                    {
                        report.AppendLine(PadBoth("AUTORIZADO POR DGII", COST_TAM));
                    }
                }
                report.AppendLine(" ");
                if (sales["CreationDate"] != null)
                {
                    report.AppendLine(Convert.ToDateTime(sales["CreationDate"].Value<string>()).ToString("dd/MM/yyyy hh:mm:ss").PadRight(COST_TAM));
                }
                if (company["data"].HasValues)
                {
                    if (company["data"]["nif"] != null)
                    {
                        report.AppendLine(PadBoth(("NIF:" + company["data"]["nif"].Value<string>()), COST_TAM));
                    }
                }

                if (sales["data"].HasValues)
                {
                    if (sales["data"]["payments"] != null)
                    {
                        if (sales["data"]["payments"]["taxreceiptnumber"] != null)
                        {
                            report.AppendLine("NCF:" + sales["data"]["payments"]["taxreceiptnumber"].Value<string>().PadRight(COST_TAM));
                        }
                        if (sales["data"]["batch"] != null)
                        {
                            if (sales["data"]["batch"]["name"] != null)
                            {
                                report.AppendLine("Caja #:" + sales["data"]["batch"]["name"].Value<string>().PadRight(COST_TAM));
                            }
                        }
                    }
                }

                if (sales["data"]["invoiceid"] != null)
                {
                    report.AppendLine(("Venta No:" + sales["data"]["invoiceid"].Value<string>()).PadRight(COST_TAM));
                }
                if (sales["data"].HasValues)
                {
                    if (sales["data"]["customer"] != null)
                    {
                        if (sales["data"]["customer"]["rnc"] != null)
                        {
                            report.AppendLine("RNC/Cliente:" + sales["data"]["customer"]["rnc"].Value<string>().PadRight(COST_TAM));
                        }
                    }
                }

                report.AppendLine("---------------------------------------");
                //"   FACTURA PARA CONSUMIDORES FINALES   "
                report.AppendLine(PadBoth("FACTURA DE VENTAS", COST_TAM));
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

                        report.AppendLine(" ");
                        string strLineaFinal = "TOTAL A PAGAR".PadRight(name_length, ' ');
                        report.Append(strLineaFinal.PadRight(first_col_pad));


                        report.Append(string.Format("{0:N2}", "0").PadLeft(second_col_pad));
                        report.AppendLine(string.Format("{0:N2}", sales["totalAmount"].Value<string>()).PadLeft(third_col_pad));


                        report.AppendLine(" ");
                        strLineaFinal = "TOTAL PAGADO".PadRight(first_col_pad, ' ');
                        report.Append(strLineaFinal.PadRight(first_col_pad));
                        report.Append(" ".PadLeft(second_col_pad));
                        report.AppendLine(string.Format("{0:N2}", 0).PadLeft(third_col_pad));

                        strLineaFinal = "CAMBIO".PadRight(name_length, ' ');
                        report.Append(strLineaFinal.PadRight(first_col_pad));
                        report.Append(" ".PadLeft(second_col_pad));
                        report.AppendLine(string.Format("{0:N2}", 0).PadLeft(third_col_pad));
                        report.AppendLine("---------------------------------------");

                        if (sales["data"]["payments"] != null)
                        {
                            foreach (JToken item in sales["data"]["payments"]["details"])
                            {
                                report.Append(item["payment"]["paymentmethod"]["text"].Value<string>().PadRight(name_length, ' ').PadRight(first_col_pad));
                                report.Append(" ".PadLeft(second_col_pad));
                                report.AppendLine(string.Format("{0:N2}", item["payment"]["amount"].Value<string>()).PadLeft(third_col_pad));
                            }
                        }
                        report.AppendLine("---------------------------------------");
                        //  strLinea.AppendLine(" ITEMS * SON GRAVADOS CON 18% DE ITBIS");
                        //   strLinea.AppendLine(" ITEMS # SON GRAVADOS CON 16% DE ITBIS");
                        strLineaFinal = "       ITBIS ( 18% ) ";
                        report.Append(strLineaFinal.PadRight(first_col_pad));
                        report.AppendLine(string.Format("{0:N2}", 0.ToString("N2").PadLeft(third_col_pad)));
                        report.AppendLine(" ");

                        report.AppendLine("TOTAL ARTICULOS VENDIDOS = " + sales["quantity"].Value<string>());

                        report.AppendLine(" ");
                        report.AppendLine("      GRACIAS POR SU VISITA          ");
                        report.AppendLine(" ");
                        report.AppendLine(" ");

                        if (sales["data"]["cashier"] != null)
                        {
                            report.AppendLine("Atendido Por: " + sales["data"]["cashier"]["name"].Value<string>());
                        }
                        report.AppendLine(" ");
                        report.AppendLine(" ");

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
        /// Split a string by chunk site.
        /// </summary>
        /// <param name="str">string to split.</param>
        /// <param name="maxChunkSize">Maximun chunk size.</param>
        /// <returns>return string array.</returns>
        private IEnumerable<string> SplitLine(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
    }
}