using Legiscan.Model;
using MvvmHelpers.Commands;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace Legiscan.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICollection<Bill> _bills = new ObservableCollection<Bill>();
        private readonly ICollection<Bill> _selectedBills = new ObservableCollection<Bill>();
        private readonly ICommand _fetchCommand;
        private readonly ICommand _hideCommand;
        private readonly ICommand _filterCommand;
        private bool _showBills = true;
        private string _filterText = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            _fetchCommand = new AsyncCommand(async () =>
            {
                Bill[] bills = await BillService.GetBillsAsync();
                foreach (Bill bill in bills)
                {
                    _bills.Add(bill);
                    _selectedBills.Add(bill);
                    bill.PropertyChanged += Bill_PropertyChanged;
                }

                Bills.Filter = new Predicate<object>(b => 
                    FilterText == "" || (b as Bill).Title.Contains(FilterText) || (b as Bill).Number.Contains(FilterText));

                SelectedBills.Filter = new Predicate<object>(b => (b as Bill).IsSelected);
            });

            _hideCommand = new DelegateCommand(() =>
            {
                ShowBills = !ShowBills;
            });


            _filterCommand = new DelegateCommand(() =>
            {
                Console.WriteLine("Filter");
            });
        }

        public ICommand FetchCommand { get { return _fetchCommand; } }

        public ICommand HideCommand { get { return _hideCommand; } }

        public ICommand FilterCommand { get { return _filterCommand; } }

        public static string WindowTitle { get { return "New Hampshire Bills"; } }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
               _filterText = value.Trim();
               Bills.Refresh();
            }
        }

        public bool ShowBills 
        { 
            get { return _showBills; }
            set
            {
                if (_showBills != value)
                {
                    _showBills = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowBills)));
                }
            }
        }

        public ICollectionView Bills
        {
            get { return CollectionViewSource.GetDefaultView(_bills); }
        }

        public ICollectionView SelectedBills
        {
            get { return CollectionViewSource.GetDefaultView(_selectedBills); }
        }

        private void Bill_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SelectedBills.Refresh();
        }
    }
}
