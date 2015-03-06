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
        public Uri stream { get; set; }
        public ControlCenter(TcpClient client)
        {
            InitializeComponent();
            this.client = client;
            string uriString = "http://"+client.Client.RemoteEndPoint.ToString() + "5/";
            Uri uri = new Uri(uriString);
            this.stream = uri;
            CameraView.Source = uri;
            System.Threading.Thread.Sleep((int)System.TimeSpan.FromSeconds(5).TotalMilliseconds); //Find a better way to wait for stream.
        }

        private void CameraView_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Console.WriteLine("stream failed");
        }
    }
}
