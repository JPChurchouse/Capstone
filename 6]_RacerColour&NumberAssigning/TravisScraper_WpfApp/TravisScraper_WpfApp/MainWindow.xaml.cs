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
//
using System.Threading;                             //-- drag and drop photos
//

namespace TravisScraper_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ReadRunning_label.Visibility = Visibility.Hidden;
        }

        private void ReadTheWebpage_Button_Click(object sender, RoutedEventArgs e)
        {

            this.Dispatcher.Invoke(() =>
            {
                ReadRunning_label.Visibility = Visibility.Visible;
                this.UpdateLayout();
            });

            ReadRunning_label.Visibility = Visibility.Visible;
            this.UpdateLayout();

            //Thread.Sleep(2000);

            WebpageText_textBox.Text = WebpageScraper_Class.ReadUrlData(URL_textBox.Text);

            ReadRunning_label.Visibility = Visibility.Hidden;
        }

        private void ResetURL_button_Click(object sender, RoutedEventArgs e)
        {
            URL_textBox.Text = DefaultURL_textBox.Text;
        }
    }
}
