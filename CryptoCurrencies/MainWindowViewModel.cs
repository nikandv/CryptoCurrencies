using BL;
using Contracts;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Office.Interop.Word;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
                OnPropertyChanged("CurrenciesCollection");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public MainWindowViewModel()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Config config = new Config();
            config.DataPath = Path.GetFullPath(CryptoCurrencies.Properties.Settings.Default.DataPath);
            //REVIEW: Path.Combine
            config.NamesPath = Path.GetFullPath("../../" + Properties.Settings.Default.NamesPath);
            fileProcessing = new FileProcessing(config);
            GetCurrencies();
        }

        private void GetCurrencies()
        {
            CurrenciesCollection = new ObservableCollection<CurrencyModel>();
            CurrenciesCollection = fileProcessing.GetCurrency();
            ObservableCollection<ShortNames> shortNames = fileProcessing.GetShortNames();
            foreach (var item in CurrenciesCollection)
            {
                foreach (var item1 in shortNames)
                {
                    if (item.Name == item1.Name)
                        item.Symbol = item1.Symbol;
                }
            }
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

        private ICommand _saveToWordCommand;
        public ICommand SaveToWordCommand => _saveToWordCommand ?? (_saveToWordCommand = new Command(delegate
        {
            string text2save = string.Empty;
            foreach (var item in CurrenciesCollection)
            {
                text2save += item.ToString() + "\r\n";
            }
            fileProcessing.SaveAsWord(text2save);
        }));
    }
}
