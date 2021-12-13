using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using TESTApp.Models;
using TESTApp.Services;

namespace TESTApp.ViewModels
{
    internal class ListViewModel : Observer
    {
        public readonly string DataListPath = $"{Environment.CurrentDirectory}\\ListItems.json"; //Путь до файла
        public ObservableCollectionEX<ItemList> ListOfValues { get; set; } //Создание коллекции для хранения данных окна списка значений

        private int _selectedListIndex;

        public int SelectedListIndex //Выбранный элемент для окна списка значений
        {
            get => _selectedListIndex;
            set { _selectedListIndex = value; OnPropertyChanged(); }

        }

        public RelayCommand AddListItemCommand { get; set; } //Команды для окна списка значений (добавить элемент)
        public RelayCommand RemoveListItemCommand { get; set; } //Удалить элемент
        public RelayCommand UpListItemCommand { get; set; } //Поднять элемент
        public RelayCommand DownListItemCommand { get; set; } //Опустить элемент
        
        
        public ListViewModel()
        {
            ListOfValues = new ObservableCollectionEX<ItemList>();
            LoadJson(DataListPath);

            AddListItemCommand = new RelayCommand(o => //Добавление новой строки
            {
                ListOfValues.Add(new ItemList());
            });

            RemoveListItemCommand = new RelayCommand(o => //Удаление строки
            {
                try
                {
                    ListOfValues.Remove(ListOfValues[SelectedListIndex]);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            UpListItemCommand = new RelayCommand(o => //Поднятие строки вверх
            {
                ListOfValues.Move(SelectedListIndex, SelectedListIndex - 1);
            }, o =>
            {
                return SelectedListIndex != ListOfValues.Count & SelectedListIndex - 1 >= 0;
            });

            DownListItemCommand = new RelayCommand(o => //Спуск строки вниз
            {
                ListOfValues.Move(SelectedListIndex, SelectedListIndex + 1);
            }, o =>
            {
                return SelectedListIndex + 1 < ListOfValues.Count;
            });
        }
        
        public static bool IsValidJson(string stringValue) //Проверка корректности json файла
        {
            if (File.Exists(stringValue))
            {
                var value = File.ReadAllText(stringValue);
                value = value.Trim();
                if ((value.StartsWith("{") && value.EndsWith("}")) ||
                    (value.StartsWith("[") && value.EndsWith("]")))
                {
                    try
                    {
                        JToken obj = JToken.Parse(value);
                        return true;
                    }
                    catch (JsonReaderException)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public void LoadJson(string path) //Метод загрузки данных из json файла
        {            
            if (IsValidJson(path))
            {
                string json = File.ReadAllText(DataListPath);
                ListOfValues = JsonConvert.DeserializeObject<ObservableCollectionEX<ItemList>>(json);
            }
            ListOfValues.JsonPath = DataListPath;
        }
    }
}
