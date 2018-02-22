using NeutralObjects;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourProject.Models
{
    public class ResultModel : INotifyPropertyChanged
    {
        private List<HistoData> _donnees1;
        private string _nom;
        private string _description;

        public string Nom
        {
            get
            {
                return _nom;
            }
            set
            {
                _nom = value;
                NotifyPropertyChanged("Nom");
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public List<HistoData> Donnees1
        {
            get
            {
                return _donnees1;
            }
            set
            {
                _donnees1 = value;
                NotifyPropertyChanged("Donnees1");
            }
        }

        public List<DataPoint> DonneesChartable
        {
            get
            {
                if (!_donnees1.Any())
                    return null;
                else
                {
                    return _donnees1.ConvertAll<DataPoint>(new Converter<HistoData, DataPoint>(HistoDataToDataPoint));
                }
            }
        }

        private DataPoint HistoDataToDataPoint(HistoData pf)
        {
            return new DataPoint(pf.Date.ToOADate(), pf.Price);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public ResultModel()
        {
            _donnees1 = new List<HistoData>();
        }
    }
}
