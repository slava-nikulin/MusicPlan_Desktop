using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlan_Desktop.ViewModels
{
    public class XamlListItem<T>
    {
        public T XListItem { get; set; }
        public bool IsSelected { get; set; }
    }
}
