using TESTApp.Services;

namespace TESTApp.Models
{
    internal class ItemList : Observer
    {
        //Свойство столбца из окна списка значений
        private string _listValue;

        public string ListValue
        {
            get => _listValue;
            set { _listValue = value; OnPropertyChanged(); }
        }
    }
}
