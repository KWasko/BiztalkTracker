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

namespace BiztalkTracker.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        

        public MainView()
        {
          
            InitializeComponent();

            //var messages = GetTrackedMessages(search);
            //dataGrid.ItemsSource = messages;
        }

        

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            contextTextBox.Text = (e.AddedCells[0].Item as Message).Context;
            bodyTextBox.Text = (e.AddedCells[0].Item as Message).BodyFormatted;
        }
    }
}
