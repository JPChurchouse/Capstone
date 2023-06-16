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
using XKarts.Logging;

namespace TestPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logger steve = new Logger();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var colour = XKarts.Identifier.Colour.Red;
            ulong integer = 0xFF0000;

            steve.log($"Testing RED: integer=={integer}, colour=={colour}, colasint=={(ulong)colour},equal=={(ulong)colour==integer}");

            steve.open();
        }
    }
}
