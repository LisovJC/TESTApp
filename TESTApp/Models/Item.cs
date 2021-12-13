using TESTApp.Services;

namespace TESTApp.Models
{
    internal class Item : Observer
    {
        //Свойства столбцов из окна параметров
        private string _name; 

        public string NameCol1
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _type;

        public string TypeCol2
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }


    }
}
