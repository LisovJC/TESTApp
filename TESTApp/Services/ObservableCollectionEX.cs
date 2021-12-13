using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace TESTApp.Services
{
    internal class ObservableCollectionEX<T> : ObservableCollection<T> //Измененный класс наблюдаемой коллекции, для реакции на изменения элементов этой коллекции
    {
        public ObservableCollectionEX() : base()
        {
            CollectionChanged += ObservableCollection_CollectionChanged;
        }

        public string JsonPath { get; set; }

        public void Serialize() //Метод сохранения данных в json файл
        {
            if (JsonPath != null)
            {
                string dir = Path.GetDirectoryName(JsonPath);
                if (!Directory.Exists(dir))
                {
                    _ = Directory.CreateDirectory(dir);
                }

                using (StreamWriter file = File.CreateText(JsonPath)) //Сериализация
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, this);
                }
            }
        }

        private void ObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) //Наблюдение изменения коллекции
        {
            if (e.OldItems != null)
            {
                foreach (T item in e.OldItems)
                {
                    if (item != null && item is INotifyPropertyChanged i)
                    {
                        i.PropertyChanged -= Element_PropertyChanged;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (T item in e.NewItems)
                {
                    if (item != null && item is INotifyPropertyChanged i)
                    {
                        i.PropertyChanged -= Element_PropertyChanged;
                        i.PropertyChanged += Element_PropertyChanged;
                    }
                }
            }
            Serialize();
        }

        private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Serialize();
        }
    }
}
