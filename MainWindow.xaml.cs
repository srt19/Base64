using System;
using System.IO;
using System.Text;
using System.Windows;

namespace Base64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool b64mode = false;
        private String outFile = null!;
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
            string outText = "0";

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

            if (FileCek.IsChecked == true)
                WriteTXT(outText, inText);
            else
                return;
        }

        private void WriteTXT(string outText, string inText)
        {
            try
            {
                if (outFile.EndsWith("txt"))
                {
                    StreamWriter file = File.AppendText(outFile);
                    file.WriteLine(outText);
                }

                else
                {
                    if (File.Exists(outFile) != true)
                    {
                        var headings = string.Format("{0};{1}", "Input", "Output" + Environment.NewLine);
                        var outs = string.Format("{0};{1}", inText, outText + Environment.NewLine);
                        File.WriteAllText(outFile, headings);
                        File.AppendAllText(outFile, outs);
                    }

                    else
                    {
                        var outs = string.Format("{0};{1}", inText, outText + Environment.NewLine);
                        File.AppendAllText(outFile, outs);
                    }
                }
            }

            catch(System.IO.IOException)
            {
                MessageBox.Show("File being used in another program", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void File_Tick(object sender, RoutedEventArgs e)
        {
            if (SelFile.IsEnabled == false)
                SelFile.IsEnabled = true;
            else
                return;
        }

        private void SelFile_Clicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFile = new()
            {
                Title = "Save txt file",
                DefaultExt = "txt",
                Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv"
            };

            Nullable<bool> result = saveFile.ShowDialog();

            if (result == true)
            {
                outFile = saveFile.FileName;
            }
            else
                return;
        }
    }
}
