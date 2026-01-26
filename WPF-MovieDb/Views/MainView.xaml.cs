using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MovieDb.ViewModels;

namespace WPF_MovieDb.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((MainViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void RepeatedPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((MainViewModel)this.DataContext).RepeatedPassword = ((PasswordBox)sender).Password; }
        }
    }
}
