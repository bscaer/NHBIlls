using System.ComponentModel;

namespace Legiscan.Model
{
    public class Bill : INotifyPropertyChanged
    {
        private readonly string _number;
        private readonly string _title;
        private bool _isSelected;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Bill(string number, string title)
        {
            _number = number;
            _title = title;
        }

        public bool IsSelected 
        { 
            get { return _isSelected;  } 
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public string Number { get { return _number; } }
        public string Title { get { return _title; } }
    }
}
