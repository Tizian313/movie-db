using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_MovieDb.Models.Items
{
    public class CanvasButton
    {
        public int StarWidth { get; set; }
        public ICommand? CanvasCommand { get; set; }
        public string? CanvasButtonText { get; set; }
        public int CanvasButtonParameter { get; set; }
    }

}
