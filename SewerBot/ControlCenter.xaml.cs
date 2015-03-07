using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SewerBot
{
    /// <summary>
    /// Interaction logic for ControlCenter.xaml
    /// </summary>
    public partial class ControlCenter : Window
    {
        public TcpClient client { get; set; }
        public ControlCenter(TcpClient client)
        {
            InitializeComponent();
            this.client = client;
        }

        private void CameraView_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Console.WriteLine("stream failed");
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            StartCamera();
        }

        private string Communicate(string command)
        {
            command = command + "\n";
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            this.client = new TcpClient((IPAddress.Parse(((IPEndPoint)this.client.Client.RemoteEndPoint).Address.ToString()).ToString()), 1234);
            NetworkStream stream = this.client.GetStream();
            stream.Write(data, 0, data.Length);
            data = new Byte[512];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return responseData;
        }

        private void StartCamera()
        {
            string port = Communicate("CAM_ON");
            if (port == String.Empty) {
                Console.WriteLine("Communication Failed");
                return;
            }
            System.Threading.Thread.Sleep((int)System.TimeSpan.FromSeconds(5).TotalMilliseconds); //Find a better way to wait for stream.
            string uriString = "http://"+(IPAddress.Parse (((IPEndPoint)this.client.Client.RemoteEndPoint).Address.ToString()).ToString())+":"+port+"/";
            Uri uri = new Uri(uriString);
            CameraView.Source = uri;
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            string response = Communicate("MV_127_127");
            if ((response == String.Empty) || (response == "f"))
            {
                Console.WriteLine("Communication Failed");
                return;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            string response = Communicate("MV_64_64");
            if ((response == String.Empty) || (response == "f"))
            {
                Console.WriteLine("Communication Failed");
                return;
            }
        }
    }
}
