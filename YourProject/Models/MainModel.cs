using NeutralObjects;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourProject.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        private string _ticker;
        private string _blgfield;
        private string _name;
        private string _description;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
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

        public string Blgfield
        {
            get
            {
                return _blgfield;
            }
            set
            {
                _blgfield = value;
                NotifyPropertyChanged("Blgfield");
            }
        }

        public string Ticker
        {
            get
            {
                return _ticker;
            }
            set
            {
                _ticker = value;
                NotifyPropertyChanged("Ticker");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(info));
        }

    }
}
