using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using XKarts;

namespace Creator_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitLog();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var colour = XKarts.Identifier.Colour.Red;
            ulong integer = 0xFF0000;

            Log.Information($"Testing RED: integer=={integer}, colour=={colour}, colasint=={(ulong)colour},equal=={(ulong)colour==integer}");

        }
    }
}
