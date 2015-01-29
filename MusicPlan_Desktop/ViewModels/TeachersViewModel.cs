using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.ViewModels
{
    public class TeachersViewModel : ICrudViewModel<Teacher>
    {
        #region Private fields
        #endregion

        #region Public properties
        public int SelectedItemIndex
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Teacher SelectedItem
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObservableCollection<Teacher> ItemsList
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string BtnAddButtonContent
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ICommand AddUpdateCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ICommand DeleteItemCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ICommand CancelSelectionCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ICommand SelectItemCommand
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        #endregion

        #region ViewModel methods
        public void PrepareViewModel()
        {
            throw new NotImplementedException();
        }

        public void UnselectItem()
        {
            throw new NotImplementedException();
        }

        public void SelectItem(Teacher item)
        {
            throw new NotImplementedException();
        }

        public void BindItems()
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(Teacher item)
        {
            throw new NotImplementedException();
        }

        public void AddUpdateItem(Teacher item)
        {
            throw new NotImplementedException();
        }

        public void ReBindItems(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
