using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ynost.Model
{
    public partial class ModelTag : ObservableObject
    {
        [ObservableProperty]
        private string[] _titles = {}
    }
}
