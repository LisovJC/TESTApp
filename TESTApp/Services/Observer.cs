using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TESTApp.Services
{
    internal class Observer : INotifyPropertyChanged //Класс слушатель, для извещения о любом изменении свойств объектов
   
    {    
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
