using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace POCPrint
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Raw_print_esc_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            
            try
            {
                Socket pSocket = new Socket(SocketType.Stream, ProtocolType.IP);

                pSocket.SendTimeout = 1500;

                pSocket.Connect("192.168.100.25", 9100);

                List<byte> outputList = new List<byte>();
                outputList.Add(0x1B);
                outputList.Add(0x40);

                outputList.Add(0x1B);
                outputList.Add(0x61);
                outputList.Add(0x31);
                outputList.AddRange(System.Text.Encoding.UTF8.GetBytes("hello world"));
                outputList.Add(0x0A);
                outputList.Add(0x0A);
                outputList.Add(0x0A);
                outputList.Add(0x0A);
                outputList.Add(0x0A);
                outputList.Add(0x0A);

                outputList.Add(0x1D);
                outputList.Add(0x56);
                outputList.Add(0x00);

                // Send the command to the printer
                pSocket.Send(outputList.ToArray());

                pSocket.Close();
                button.Text = "sent";
            }
            catch (SocketException ex)
            {
                Console.WriteLine("SocketError: " + ex.Message);
                button.Text = ex.Message;
            }
        }
    }
}
