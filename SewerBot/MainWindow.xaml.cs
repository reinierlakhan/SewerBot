using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Net;
using System.Net.Sockets;


namespace SewerBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Connect("192.168.2.2", "Boop1\n");
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            FindBots("sewerbot");
        }
        private void FindBots(string pattern)
        {
            string strHostName;

            strHostName = Dns.GetHostName();

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            DeviceList.Items.Clear();

            int i = 0;
            while (i < addr.Length)
            {
                strHostName = Dns.GetHostEntry((addr[i])).HostName;

                Match match = Regex.Match(strHostName, pattern);
                if (match.Success)
                {
                    DeviceList.Items.Add(new { Hostname = strHostName, Address = addr[i].ToString() });
                   // DeviceList.Items.Add(addr[i].ToString());
                }
                i++;
            }
            ConnectButton.IsEnabled = false;
        }
        static TcpClient Connect(String server, Int32 port, String message, String ack)
        {
            try
            {
                // Create a TcpClient. 
                // Note, for this client to work you need to have a TcpServer  
                // connected to the same address as specified by the server, port 
                // combination.
                
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response. 

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);


                // Close everything.
                stream.Close();
                if (responseData.Equals(ack))
                    return client;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
            return null;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            FindBots("sewerbot");
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var device = DeviceList.SelectedItem;
            System.Type type = device.GetType();
            string host = (string)type.GetProperty("Hostname").GetValue(device, null);
            string address = (string)type.GetProperty("Address").GetValue(device, null);
            Console.WriteLine(address);
            
            TcpClient client = Connect(address, 9999, "CONN_REQ", "SB_READY");
            if (client != null)
            {
                ControlCenter control = new ControlCenter(client);
                control.Show();
                this.Close();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Cannot connect to the selected device.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectButton.IsEnabled = true;
        }
    }


}
