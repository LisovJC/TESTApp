using TESTApp.Services;
using TESTApp.Models;
using System.Diagnostics;
using System;
using System.Windows;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace TESTApp.ViewModels
{
    internal class MainViewModel : Observer
    {
        public ObservableCollectionEX<Item> Items { get; set; } //Создание коллекции для хранения данных окна параметров
       
        private int _selectedGridIndex;

        public int SelectedGridIndex //Выбранный элемент для окна параметров
        {
            get => _selectedGridIndex;
            set { _selectedGridIndex = value; OnPropertyChanged(); }
        }
        public readonly string DataGridPath = $"{Environment.CurrentDirectory}\\GridItems.json"; //Путь рассположения файлов

        public RelayCommand AddGridItemCommand { get; set; } //Создание команд для окна параметров (добавить элемент)
        public RelayCommand RemoveGridItemCommand { get; set; } //Удалить элемент
        public RelayCommand UpGridItemCommand { get; set; } //Поднять элемент
        public RelayCommand DownGridItemCommand { get; set; } //Опустить элемент
        public RelayCommand OpenListWindowCommand { get; set; } //Открыть окно списка значений
            
        public MainViewModel()
        {
            
            Items = new ObservableCollectionEX<Item>();          
            LoadJson(DataGridPath); //Загрузка данных окна параметра из json файла

            AddGridItemCommand = new RelayCommand(o => //Добавление новой строки
            {               
                Items.Add(new Item());
            });

            RemoveGridItemCommand = new RelayCommand(o => //Удаление строки
            {
                try
                {
                    Items.Remove(Items[SelectedGridIndex]);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            UpGridItemCommand = new RelayCommand(o => //Поднятие строки вверх
            {               
                Items.Move(SelectedGridIndex, SelectedGridIndex - 1);
            }, o =>
            {
                return SelectedGridIndex != Items.Count & SelectedGridIndex - 1 >= 0;
            });

            DownGridItemCommand = new RelayCommand(o => //Спуск строки вниз
            {
                Items.Move(SelectedGridIndex, SelectedGridIndex + 1);
            }, o =>
            {
                return SelectedGridIndex + 1 < Items.Count;
            });

            OpenListWindowCommand = new RelayCommand(o => //Открытие окна списка зачений
            {
                ListWindow lw = new ListWindow();                
                lw.Owner = Application.Current.MainWindow;
                lw.Show();
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
                string json = File.ReadAllText(path);
                Items = JsonConvert.DeserializeObject<ObservableCollectionEX<Item>>(json);
            }
            Items.JsonPath = path;            
        }
    }
}
