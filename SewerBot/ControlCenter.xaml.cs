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
            CameraView.Source = client.Client.RemoteEndPoint;
        }
    }
}
