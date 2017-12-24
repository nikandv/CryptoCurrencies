using Contracts;
using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public class FileReader
    {
        public void SaveData(ObservableCollection<CurrencyModel> collection, string path)
        {
            //if (!Directory.Exists(path))
            //    throw new DirectoryNotFoundException("Директория не найдена");

            try
            {
                Logger.InitLogger();
                Logger.Log.Info("Начало сериализации данных");
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<CurrencyModel>));
                using (FileStream fs = File.Create(path))
                {
                    serializer.Serialize(fs, collection);
                }
                Logger.Log.Info("Сериализация данных произведена успешно");
            }
            catch(Exception ex)
            {
                Logger.Log.Error($"Произошла ошибка при сериализации: {ex.Message}");
            }
        }

        public ObservableCollection<ShortNames> GetData(string path)
        {
            var x = Path.GetFullPath(path);
            ObservableCollection<ShortNames> shortNames = new ObservableCollection<ShortNames>();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("Имя файла не может быть пустым");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл {path} не найден");
            try
            {
                Logger.Log.Info("Начало десериализации данных");
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ShortNames>));
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    shortNames = (ObservableCollection<ShortNames>)serializer.Deserialize(fs);
                }
                Logger.Log.Info("Десериализация данных произведена успешно");
            }
            catch(Exception ex)
            {
                Logger.Log.Error($"Произошла ошибка при десериализации: {ex.Message}");
            }
            return shortNames;
        }
    }
}
