using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace ServerForChorch
{
    public partial class MainWindow : Window
    {
        private IPAddress[] IP => Dns.GetHostAddresses(Dns.GetHostName());
        private Socket socket { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            logList.DataContext = new Log[0];

            for (int i = 0; i < IP.Length; i++)
            {
                IPComboBox.Items.Add(IP[i]);
            }
            IPComboBox.SelectedIndex = 0;
        }

        private async void Start(object sender, RoutedEventArgs e)
        {
            if (IPComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Выберите IP адресс", "Ошибка");
                return;
            }
            else if (IP[IPComboBox.SelectedIndex] == IPAddress.Parse("127.0.0.1"))
            {
                System.Windows.MessageBox.Show("Подключитесь к локальной сети", "Ошибка");
                return;
            }
            else if (numeric.Value == 0)
            {
                System.Windows.MessageBox.Show("Введите порт подключения", "Ошибка");
                return;
            }
            IPComboBox.IsEnabled = false;
            numeric.IsEnabled = false;
            start.IsEnabled = false;
            stop.IsEnabled = true;

            if (IP[IPComboBox.SelectedIndex].IsIPv6LinkLocal)
                socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            else
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(new IPEndPoint(IP[IPComboBox.SelectedIndex], numeric.Value));
            socket.Listen(5);
            try
            {
                while (true)
                {
                    Socket handler = await socket.AcceptAsync();

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[1024];
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    try
                    {
                        SendKeys.SendWait(builder.ToString());

                        if (logList.Items.Count >= 6)
                            logList.Items.RemoveAt(0);

                        logList.Items.Add(new Log(handler.RemoteEndPoint.ToString(), builder.ToString()));
                    }
                    catch (Exception)
                    {
                        if (logList.Items.Count >= 6)
                            logList.Items.RemoveAt(0);

                        logList.Items.Add(new Log(handler.RemoteEndPoint.ToString(), $"Not processed: {builder}"));
                    }

                    

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                IPComboBox.IsEnabled = true;
                numeric.IsEnabled = true;
                start.IsEnabled = true;
                stop.IsEnabled = false;
                if (socket.Connected)
                {
                    System.Windows.MessageBox.Show("Error", ex.Message);
                    socket.Close();
                }
            }

        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            IPComboBox.IsEnabled = true;
            numeric.IsEnabled = true;
            start.IsEnabled = true;
            stop.IsEnabled = false;
            socket.Close();
        }

        private void IPComboBox_Open(object sender, EventArgs e)
        {
            IPComboBox.Items.Clear();
            for (int i = 0; i < IP.Length; i++)
            {
                IPComboBox.Items.Add(IP[i]);
            }
        }

        private void MenuItemWhite(object sender, RoutedEventArgs e)
        {
            whiteMenuItem.IsChecked = true;
            blackMenuItem.IsChecked = false;
            Background = (Brush)System.Windows.Application.Current.Resources["windowWhitebgColor"];
            menu.Style = (Style)System.Windows.Application.Current.Resources["whiteMenu"];
            textPort.Foreground = (Brush)System.Windows.Application.Current.Resources["whitefgColor"];
            IPComboBox.Style = (Style)System.Windows.Application.Current.Resources["whiteComboBox"];
            stop.Style = (Style)System.Windows.Application.Current.Resources["whiteButtonStyle"];
            start.Style = (Style)System.Windows.Application.Current.Resources["whiteButtonStyle"];
            logList.Style = (Style)System.Windows.Application.Current.Resources["whiteListBox"];
            numeric.setWhiteStyle();
        }
        private void MenuItemBlack(object sender, RoutedEventArgs e)
        {
            whiteMenuItem.IsChecked = false;
            blackMenuItem.IsChecked = true;
            Background = (Brush)System.Windows.Application.Current.Resources["windowBlackbgColor"];
            menu.Style = (Style)System.Windows.Application.Current.Resources["blackMenu"];
            textPort.Foreground = (Brush)System.Windows.Application.Current.Resources["blackfgColor"];
            IPComboBox.Style = (Style)System.Windows.Application.Current.Resources["blackComboBox"];
            stop.Style = (Style)System.Windows.Application.Current.Resources["blackButtonStyle"];
            start.Style = (Style)System.Windows.Application.Current.Resources["blackButtonStyle"];
            logList.Style = (Style)System.Windows.Application.Current.Resources["blackListBox"];
            numeric.setBlackStyle();
        }
    }
}
