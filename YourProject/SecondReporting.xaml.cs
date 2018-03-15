using System;
using System.Windows;
using System.Windows.Media;
using System.IO.Packaging;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using BlgConnector;
using NeutralObjects;
using YourProject.Models;

namespace YourProject
{
    /// <summary>
    /// Logique d'interaction pour SecondReporting.xaml
    /// </summary>
    public partial class SecondReporting : Window
    {
        public SecondReporting()
        {
            InitializeComponent();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            BlgWrapper wrap = new BlgWrapper();
            List<string> tickers = new List<string>();
            List<string> fields = new List<string>();

            MainModel mm = GridMain.DataContext as MainModel;
            ResultModel rm = GridResult.DataContext as ResultModel;

            if (Ticker1.Text != "" && NbActions1.Text != "")
            {
                tickers.Add(Ticker1.Text);
            }
            if (Ticker2.Text != "" && NbActions2.Text != "")
            {
                tickers.Add(Ticker2.Text);
            }
            if (Ticker3.Text != "" && NbActions3.Text != "")
            {
                tickers.Add(Ticker3.Text);
            }
            if (Ticker4.Text != "" && NbActions4.Text != "")
            {
                tickers.Add(Ticker4.Text);
            }
            if (Ticker5.Text != "" && NbActions5.Text != "")
            {
                tickers.Add(Ticker5.Text);
            }

            fields.Add("PX_LAST");

            Dictionary<string, Dictionary<string, List<HistoData>>> res_histo = new Dictionary<string, Dictionary<string, List<HistoData>>>();

            List<HistoData> Portfolio = new List<HistoData>();

            if (wrap.GetHistoricalData(tickers, fields, BlgPeriodOpt.periodicity_sel, BlgPeriods.daily))
            {


                res_histo = wrap.ResultHisto;
                for (int i = 0; i < res_histo[fields.First()].Values.First().Count; i++)
                {
                    double sumValue = 0;

                    if (Ticker1.Text != "" && NbActions1.Text != "")
                    {
                        sumValue += res_histo["PX_LAST"][Ticker1.Text][i].Price * Convert.ToInt32(NbActions1.Text);
                    }
                    if (Ticker2.Text != "" && NbActions2.Text != "")
                    {
                        sumValue += res_histo["PX_LAST"][Ticker2.Text][i].Price * Convert.ToInt32(NbActions2.Text);
                    }
                    if (Ticker3.Text != "" && NbActions3.Text != "")
                    {
                        sumValue += res_histo["PX_LAST"][Ticker3.Text][i].Price * Convert.ToInt32(NbActions3.Text);
                    }
                    if (Ticker4.Text != "" && NbActions4.Text != "")
                    {
                        sumValue += res_histo["PX_LAST"][Ticker4.Text][i].Price * Convert.ToInt32(NbActions4.Text);
                    }
                    if (Ticker5.Text != "" && NbActions5.Text != "")
                    {
                        sumValue += res_histo["PX_LAST"][Ticker5.Text][i].Price * Convert.ToInt32(NbActions5.Text);
                    }
                    HistoData value = new HistoData(res_histo[fields.First()].Values.First()[i].Date, sumValue);

                    Portfolio.Add(value);
                }

                List<HistoData> ListPerf = new List<HistoData>();
                for (int i = 0; i < Portfolio.Count(); i++)
                {
                    double VLEnd = Portfolio[i].Price;
                    double VLStart = Portfolio[0].Price;
                    double perf = ((VLEnd - VLStart) / VLStart);
                    HistoData HistPerf = new HistoData(Portfolio[i].Date, perf);
                    ListPerf.Add(HistPerf);
                }

                rm.Donnees1 = ListPerf;
                pcprod.ItemsSource = null;
                pcprod.Items.Clear();
                pcprod.ItemsSource = rm.DonneesChartable;

                P1MTxt.Text = CalculPerformance(Portfolio, 21);
                P3MTxt.Text = CalculPerformance(Portfolio, 63);
                P6MTxt.Text = CalculPerformance(Portfolio, 126);
                P1YTxt.Text = CalculPerformance(Portfolio, 252);
                P3YTxt.Text = CalculPerformance(Portfolio, 756);

                V1MTxt.Text = CalculVolatility(Portfolio, 21);
                V3MTxt.Text = CalculVolatility(Portfolio, 63);
                V6MTxt.Text = CalculVolatility(Portfolio, 126);
                V1YTxt.Text = CalculVolatility(Portfolio, 252);
                V3YTxt.Text = CalculVolatility(Portfolio, 756);


            }
            else
            {
                string message = "";
                foreach (var kvp in wrap.Errors)
                {
                    message += kvp.Key + " : " + kvp.Value + Environment.NewLine;
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            fields.Clear();
            fields.Add("NAME");

            Dictionary<string, Dictionary<string, string>> res_ref = new Dictionary<string, Dictionary<string, string>>();

            List<double> PerfArray = new List<double>();

            if (wrap.GetReferenceData(tickers, fields))
            {
                res_ref = wrap.ResultRef;
                double value;

                if (Ticker1.Text != "" && NbActions1.Text != "")
                {
                    Ticker1CompositionName.Text = res_ref["NAME"][Ticker1.Text];
                    Shares1.Text = NbActions1.Text;
                    TotalValue1.Text = Convert.ToString(res_histo["PX_LAST"][Ticker1.Text][res_histo["PX_LAST"][Ticker1.Text].Count() - 1].Price * Convert.ToInt32(NbActions1.Text));
                    value = ((res_histo["PX_LAST"][Ticker1.Text][res_histo["PX_LAST"][Ticker1.Text].Count() - 1].Price * Convert.ToInt32(NbActions1.Text) * 100) / Portfolio[Portfolio.Count() - 1].Price) / 100;
                    Percentage1.Text = value.ToString("0.00%");
                }
                if (Ticker2.Text != "" && NbActions2.Text != "")
                {
                    Ticker2CompositionName.Text = res_ref["NAME"][Ticker2.Text];
                    Shares2.Text = NbActions2.Text;
                    TotalValue2.Text = Convert.ToString(res_histo["PX_LAST"][Ticker2.Text][res_histo["PX_LAST"][Ticker2.Text].Count() - 1].Price * Convert.ToInt32(NbActions2.Text));
                    value = ((res_histo["PX_LAST"][Ticker2.Text][res_histo["PX_LAST"][Ticker2.Text].Count() - 1].Price * Convert.ToInt32(NbActions2.Text) * 100) / Portfolio[Portfolio.Count() - 1].Price) / 100;
                    Percentage2.Text = value.ToString("0.00%");
                }
                if (Ticker3.Text != "" && NbActions3.Text != "")
                {
                    Ticker3CompositionName.Text = res_ref["NAME"][Ticker3.Text];
                    Shares3.Text = NbActions3.Text;
                    TotalValue3.Text = Convert.ToString(res_histo["PX_LAST"][Ticker3.Text][res_histo["PX_LAST"][Ticker3.Text].Count() - 1].Price * Convert.ToInt32(NbActions3.Text));
                    value = ((res_histo["PX_LAST"][Ticker3.Text][res_histo["PX_LAST"][Ticker3.Text].Count() - 1].Price * Convert.ToInt32(NbActions3.Text) * 100) / Portfolio[Portfolio.Count() - 1].Price) / 100;
                    Percentage3.Text = value.ToString("0.00%");
                }
                if (Ticker4.Text != "" && NbActions4.Text != "")
                {
                    Ticker4CompositionName.Text = res_ref["NAME"][Ticker4.Text];
                    Shares4.Text = NbActions4.Text;
                    TotalValue4.Text = Convert.ToString(res_histo["PX_LAST"][Ticker4.Text][res_histo["PX_LAST"][Ticker4.Text].Count() - 1].Price * Convert.ToInt32(NbActions4.Text));
                    value = ((res_histo["PX_LAST"][Ticker4.Text][res_histo["PX_LAST"][Ticker4.Text].Count() - 1].Price * Convert.ToInt32(NbActions4.Text) * 100) / Portfolio[Portfolio.Count() - 1].Price) / 100;
                    Percentage4.Text = value.ToString("0.00%");
                }
                if (Ticker5.Text != "" && NbActions5.Text != "")
                {
                    Ticker5CompositionName.Text = res_ref["NAME"][Ticker5.Text];
                    Shares5.Text = NbActions5.Text;
                    TotalValue5.Text = Convert.ToString(res_histo["PX_LAST"][Ticker5.Text][res_histo["PX_LAST"][Ticker5.Text].Count() - 1].Price * Convert.ToInt32(NbActions5.Text));
                    value = ((res_histo["PX_LAST"][Ticker5.Text][res_histo["PX_LAST"][Ticker5.Text].Count() - 1].Price * Convert.ToInt32(NbActions5.Text) * 100) / Portfolio[Portfolio.Count() - 1].Price) / 100;
                    Percentage5.Text = value.ToString("0.00%");
                }

            }
            else
            {
                string message = "";
                foreach (var kvp in wrap.Errors)
                {
                    message += kvp.Key + " : " + kvp.Value + Environment.NewLine;
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string CalculPerformance(List<HistoData> Portfolio, int days)
        {
            int indexLast = Portfolio.Count() - 1;
            double price = ((Portfolio[indexLast].Price - Portfolio[indexLast - days].Price) / Portfolio[indexLast - days].Price);
            string priceStr = price.ToString("0.00%");
            return priceStr;
        }

        private string CalculVolatility(List<HistoData> Portfolio, int days)
        {
            double N = 252;
            double sum = 0;

            int indexLast = Portfolio.Count() - 1;
            for (int i = 0; i < days; i++)
                sum += Math.Pow(Math.Log(Portfolio[indexLast - i].Price / Portfolio[indexLast - i - 1].Price), 2);
            sum *= N / days;

            double variance = sum;

            variance = Math.Sqrt(variance);
            string varianceStr = variance.ToString("0.00%");
            return varianceStr;
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
