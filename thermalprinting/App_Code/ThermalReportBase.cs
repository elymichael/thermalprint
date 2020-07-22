namespace thermalprinting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json.Linq;

    public class ThermalReportBase
    {
        private readonly JObject _data;
        protected JObject Data { get { return _data; } }

        public ThermalReportBase(JObject data)
        {
            if (data == null)
            {
                throw new Exception("Check if the json is well formed.");
            }

            _data = data;
        }

        protected string GetHeader()
        {
            StringBuilder report = new StringBuilder();
            try
            {
                JToken company = _data.GetValue(RestApiConstant.CompanyNode);

                report.AppendLine(PadBoth(company[RestApiConstant.Company.Name].Value<string>(), RestApiConstant.COST_TAM));
                if (company[RestApiConstant.Company.Data].HasValues)
                {
                    JObject oData = company[RestApiConstant.Company.Data].Value<JObject>();

                    if (oData[RestApiConstant.Company.Address] != null)
                    {
                        IEnumerable<string> lines = SplitLine(oData[RestApiConstant.Company.Address].Value<string>(), RestApiConstant.COST_TAM);
                        foreach (string str in lines)
                        {
                            report.AppendLine(PadBoth(str, RestApiConstant.COST_TAM));
                        }
                    }
                    if (oData[RestApiConstant.Company.RNC] != null)
                    {
                        report.AppendLine(PadBoth(("RNC:" + oData[RestApiConstant.Company.RNC].Value<string>()), RestApiConstant.COST_TAM));
                    }
                    report.AppendLine(RestApiConstant.BlankSpace);
                    if (oData[RestApiConstant.Company.Authorized] != null)
                    {
                        report.AppendLine(PadBoth("AUTORIZADO POR DGII", RestApiConstant.COST_TAM));
                    }
                }
                report.AppendLine(" ");

                return report.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error working with the header: " + ex.Message);
            }
        }


        /// <summary>
        /// Add Pad to both side of the string.
        /// </summary>
        /// <param name="source">content to add pad.</param>
        /// <param name="length">length of the content.</param>
        /// <returns></returns>
        protected string PadBoth(string source, int length)
        {

            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft).PadRight(length);
        }

        /// <summary>
        /// Split a string by chunk site.
        /// </summary>
        /// <param name="str">string to split.</param>
        /// <param name="maxChunkSize">Maximun chunk size.</param>
        /// <returns>return string array.</returns>
        protected IEnumerable<string> SplitLine(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
    }
}