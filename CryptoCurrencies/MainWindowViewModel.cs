using BL;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencies
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        FileProcessing fileProcessing = new FileProcessing();

        private ObservableCollection<CurrencyModel> _currenciesCollection;
        public ObservableCollection<CurrencyModel> CurrenciesCollection
        {
            get { return _currenciesCollection; }
            set
            {
                _currenciesCollection = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public MainWindowViewModel()
        {
            GetCurrencies();
        }

        private void GetCurrencies()
        {
            CurrenciesCollection = new ObservableCollection<CurrencyModel>();
            CurrenciesCollection = fileProcessing.GetCurrency();
        }
    }
}
