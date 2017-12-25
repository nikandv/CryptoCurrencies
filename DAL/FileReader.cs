using Contracts;
using Contracts.Models;
using Microsoft.Office.Interop.Word;
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
            //REVIEW: А если path == null - тут будет Exception.
            var dir = new FileInfo(path).Directory.FullName;
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException($"Директория {dir} не найдена");

            System.Threading.Tasks.Task.Run(() =>
            {
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
                catch (Exception ex)
                {
                    Logger.Log.Error($"Произошла ошибка при сериализации: {ex.Message}");
                }
            });
        }

        public ObservableCollection<ShortNames> GetData(string path)
        {
            ObservableCollection<ShortNames> shortNames = new ObservableCollection<ShortNames>();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("Имя файла не может быть пустым");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл {path} не найден");
            Task<ObservableCollection<ShortNames>> t = new Task<ObservableCollection<ShortNames>>(() =>
            {
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
                catch (Exception ex)
                {
                    Logger.Log.Error($"Произошла ошибка при десериализации: {ex.Message}");
                }
                return shortNames;
            });
            t.Start();
            return t.Result;
        }

        public void SaveAsWord(string text2save)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    Logger.Log.Info("Начало сохранения данных в документ Word");
                    Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                    Document doc = app.Documents.Add(Visible: true);
                    Range text = doc.Range();
                    text.Text = text2save;
                    doc.Save();
                    doc.Close();
                    app.Quit();
                    Logger.Log.Info("Сохранение данных в документ Word произведено успешно");
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Произошла ошибка при сохранении файла в формате word: {ex.Message}");
                }
            });
        }
    }
}
