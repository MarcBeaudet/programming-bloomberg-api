using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlgConnector;
using NeutralObjects;
using YourProject.Models;

namespace YourProject
{
    /// <summary>
    /// Logique d'interaction pour SimpleReporting.xaml
    /// </summary>
    public partial class SimpleReporting : Window
    {
        public SimpleReporting(){
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BlgWrapper wrap = new BlgWrapper();
            List<string> tickers = new List<string>();
            List<string> fields = new List<string>();

            MainModel mm = GridMain.DataContext as MainModel;
            ResultModel rm = GridResult.DataContext as ResultModel;

            tickers.Add(mm.Ticker);
            fields.Add("PX_LAST");

            Dictionary<string, Dictionary<string, List<HistoData>>> res_histo = new Dictionary<string, Dictionary<string, List<HistoData>>>();

            if(wrap.GetHistoricalData(tickers, fields, BlgPeriodOpt.periodicity_sel, BlgPeriods.daily))
            {
                List<HistoData> ListPerf = new List<HistoData>();
                res_histo = wrap.ResultHisto;

                for (int i = 0; i < res_histo[fields.First()].Values.First().Count(); i++)
                {
                    double VLEnd = res_histo[fields.First()].Values.First()[i].Price;
                    double VLStart = res_histo[fields.First()].Values.First()[0].Price;
                    double perf = ((VLEnd - VLStart) / VLStart);
                    HistoData HistPerf = new HistoData(res_histo[fields.First()].Values.First()[i].Date, perf);
                    ListPerf.Add(HistPerf);
                }


                rm.Donnees1 = ListPerf;


                
                P1MTxt.Text = PerformanceCalculus(res_histo, 21);
                P3MTxt.Text = PerformanceCalculus(res_histo, 63);
                P6MTxt.Text = PerformanceCalculus(res_histo, 126);
                P1YTxt.Text = PerformanceCalculus(res_histo, 252);
                P3YTxt.Text = PerformanceCalculus(res_histo, 756);

                V1MTxt.Text = VolatilityCalculus(res_histo, 21);
                V3MTxt.Text = VolatilityCalculus(res_histo, 63);
                V6MTxt.Text = VolatilityCalculus(res_histo, 126);
                V1YTxt.Text = VolatilityCalculus(res_histo, 252);
                V3YTxt.Text = VolatilityCalculus(res_histo, 756);
                
            }
            else
            {
                string message = "";
                foreach(var kvp in wrap.Errors)
                {
                    message += kvp.Key + " : " + kvp.Value + Environment.NewLine;
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            pcprod.ItemsSource = null;
            pcprod.Items.Clear();
            pcprod.ItemsSource = rm.DonneesChartable;

            fields.Clear();
            fields.Add("NAME");
            fields.Add("CIE_DES");

            Dictionary<string, Dictionary<string, string>> res_ref = new Dictionary<string, Dictionary<string, string>>();

            if (wrap.GetReferenceData(tickers, fields))
            {
                res_ref = wrap.ResultRef;
                mm.Name = res_ref["NAME"].Values.First();
                mm.Description = res_ref["CIE_DES"].Values.First();
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

        private string PerformanceCalculus(Dictionary<string, Dictionary<string, List<HistoData>>> res_histo, int days)
        {

            List<double> PriceList = new List<double>();
            for (int i = 0; i < res_histo["PX_LAST"].Values.First().Count(); i++)
                PriceList.Add(res_histo["PX_LAST"].Values.First()[i].Price);

            int indexLast = PriceList.Count() - 1;
            double price = ((PriceList[indexLast] - PriceList[indexLast - days]) / PriceList[indexLast - days]);
            string priceStr = price.ToString("0.00%");
            return priceStr;
        }

        private string VolatilityCalculus(Dictionary<string, Dictionary<string, List<HistoData>>> res_histo, int days)
        {
            List<double> PriceList = new List<double>();
            double N = 252;
            double sum = 0;
            for (int i = 0; i < res_histo["PX_LAST"].Values.First().Count(); i++)
                PriceList.Add(res_histo["PX_LAST"].Values.First()[i].Price);

            int indexLast = PriceList.Count() - 1;
            for (int i = 0; i < days; i++)
                sum += Math.Pow(Math.Log(PriceList[indexLast - i] / PriceList[indexLast - i - 1]), 2);
            sum *= N / days;

            double variance = sum;

            variance = Math.Sqrt(variance);
            string varianceStr = variance.ToString("0.00%");
            return varianceStr;
        }
    }
}
