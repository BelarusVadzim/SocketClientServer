using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer.Controllers
{
    class SocketClient
    {

        public SocketClient()
        {

        }

        public void SendData(string text)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000));
            Stream sr = GenerateStreamFromString(text);
            NetworkStream NS = null;
            string data = "";

            byte[] buffer = new byte[1500];
            int bytesSent = 0;

            while (bytesSent < sr.Length)
            {
                int bytesRead = sr.Read(buffer, 0, 1500);
                NS = tcpClient.GetStream();
                NS.Write(buffer, 0, bytesRead);
                //Console.WriteLine(bytesRead + " bytes sent.");

                bytesSent += bytesRead;
            }
            bytesSent = 0;
            NS.ReadTimeout = 5000;
            try
            {
                while ((bytesSent = NS.Read(buffer, 0, buffer.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesSent);
                    System.Windows.Forms.MessageBox.Show(data);
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("bip");
            }
            finally
            {
                tcpClient.Close();
            }



        }

        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }


    }
}
