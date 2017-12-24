using BL;
using Contracts;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoCurrencies
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        FileProcessing fileProcessing;

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
            Config config = new Config();
            config.DataPath = Path.GetFullPath(CryptoCurrencies.Properties.Settings.Default.DataPath);
            fileProcessing = new FileProcessing(config);
            GetCurrencies();
        }

        private void GetCurrencies()
        {
            CurrenciesCollection = new ObservableCollection<CurrencyModel>();
            CurrenciesCollection = fileProcessing.GetCurrency();
        }

        private void SaveCurrencyData()
        {
            fileProcessing.SaveData();
        }

        private ICommand _updateValuesCommand;
        public ICommand UpdateValuesCommand => _updateValuesCommand ?? (_updateValuesCommand = new Command(delegate
        {
            GetCurrencies();
        }));

        private ICommand _saveDataCommand;
        public ICommand SaveDataCommand => _saveDataCommand ?? (_saveDataCommand = new Command(delegate
        {
            SaveCurrencyData();
        }));
    }
}
