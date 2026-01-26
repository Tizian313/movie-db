using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_MovieDb.Models.Items
{
    public class LabeledCheckBox
    {
        public ICommand? OnToggleCommand { get; set; }
        public string? Text { get; set; }
        public bool IsChecked { get; set; }
    }
}
