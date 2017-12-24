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
                //throw new DirectoryNotFoundException("Директория не найдена");
            string DataPath = Path.GetFullPath(path);
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<CurrencyModel>));
            using (FileStream fs = File.Create(path))
            {
                serializer.Serialize(fs, collection);
            }
        }

        public void GetData(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("Имя файла не может быть пустым");
            if (!File.Exists(path))
                throw new FileNotFoundException("Файл не найден");
            string[] data = File.ReadAllLines(path);

        }
    }
}
