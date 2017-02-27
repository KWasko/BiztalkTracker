using BiztalkTracker.Model;
using BiztalkTracker.View;
using BiztalkTracker.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BiztalkTracker.ViewModel;

namespace BiztalkTracker.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainView()
        {
            ViewModel = new MainViewModel();
          
            InitializeComponent();

        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            ViewModel.SelectedMessage =dataGrid.SelectedCells[0].Item as SelectableMessage;
           // contextTextBox.Text = (e.AddedCells[0].Item as SelectableMessage).Context;
           // bodyTextBox.Text = (e.AddedCells[0].Item as SelectableMessage).BodyFormatted;
        }
    }
}
