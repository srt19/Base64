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

namespace Base64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool b64mode = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncRadio(object sender, RoutedEventArgs e)
        {
            b64mode = false;
            URLCek.IsEnabled = false;
            URLCek.IsChecked = false;
        }
        private void DecRadio(object sender, RoutedEventArgs e)
        {
            b64mode = true;
            URLCek.IsEnabled=true;
        }
        private void ConvertRun(object sender, RoutedEventArgs e)
        {
            string inText = InBox.Text;
            string outText;

            if (b64mode == true && URLCek.IsChecked == true)
            {
                try
                {
                    inText = inText.Replace("&", " ");
                    inText = inText.Replace("url=", "");
                    string[] list = inText.Split();
                    inText = list[1];
                    byte[]? inBytes = Convert.FromBase64String(inText);
                    outText = Encoding.UTF8.GetString(inBytes);
                    OutBox.Text = outText;
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show("Please input complete URL", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            else if (b64mode == true && URLCek.IsChecked == false)
            {
                try
                {
                    byte[]? inBytes = Convert.FromBase64String(inText);
                    outText = Encoding.UTF8.GetString(inBytes);
                    OutBox.Text = outText;
                }

                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            else if (b64mode == false && URLCek.IsChecked == false)
            {
                byte[]? inBytes = Encoding.UTF8.GetBytes(inText);
                outText = Convert.ToBase64String(inBytes);
                OutBox.Text = outText;
            }
        }
    }
}
