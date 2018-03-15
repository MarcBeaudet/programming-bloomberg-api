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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
            fields.Add(mm.Blgfield);

            Dictionary<string, Dictionary<string, List<HistoData>>> res_histo = new Dictionary<string, Dictionary<string, List<HistoData>>>(); 

            if(wrap.GetHistoricalData(tickers, fields, BlgPeriodOpt.periodicity_sel, BlgPeriods.daily))
            {
                res_histo = wrap.ResultHisto;
                rm.Donnees1 = res_histo[fields.First()].Values.First();
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
                rm.Nom = res_ref["NAME"].Values.First();
                rm.Description = res_ref["CIE_DES"].Values.First();
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
    }
}
