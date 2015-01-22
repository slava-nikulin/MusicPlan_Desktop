using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MusicPlan_Desktop.ViewModels
{
    interface ICrudViewModel<T>
    {
        int SelectedItemIndex { get; set; }
        T SelectedItem { get; set; }
        ObservableCollection<T> ItemsList { get; set; }
        string BtnAddButtonContent { get; set; }
        ICommand AddUpdateCommand { get; set; }
        ICommand DeleteItemCommand { get; set; }
        ICommand CancelSelectionCommand { get; set; }
        ICommand SelectItemCommand { get; set; }

        void PrepareViewModel();
        void UnselectItem();
        void SelectItem(T item);
        void BindItems();
        void DeleteItem(T item);
        void AddUpdateItem(T item);
        void ReBindItems(object obj);
    }
}
