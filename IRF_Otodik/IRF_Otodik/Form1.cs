using IRF_Otodik.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace IRF_Otodik
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Rates;
            //GetRates();

            ReadXml();
            chartRateData.DataSource = Rates;
            chartRateData.Series[0].ChartType = SeriesChartType.Line;
            chartRateData.Series[0].XValueMember = "date";
            chartRateData.Series[0].YValueMembers = "value";
            chartRateData.Series[0].BorderWidth = 2;
            chartRateData.Legends[0].Enabled = false;
            chartRateData.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRateData.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chartRateData.ChartAreas[0].AxisY.IsStartedFromZero = false;
        }

        private void ReadXml()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(GetRates());
            foreach (XmlElement item in xml.DocumentElement)
            {
                RateData rd = new RateData();
                Rates.Add(rd);
                rd.Currency = item.ChildNodes[0].Attributes["curr"].Value;
                rd.Date = Convert.ToDateTime(item.Attributes["date"].Value);
                decimal unit = Convert.ToDecimal(item.ChildNodes[0].Attributes["unit"].Value);
                decimal value = Convert.ToDecimal(item.ChildNodes[0].InnerText);
                if (unit != 0)
                {
                    rd.Value = value / unit;
                }
                else
                {
                    rd.Value = value;
                }
            }
        }

        private static string GetRates()
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();

            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };

            GetExchangeRatesResponseBody response = mnbService.GetExchangeRates(request);

            string result = response.GetExchangeRatesResult;

            //MessageBox.Show(result);

            return result;
        }
    }
}