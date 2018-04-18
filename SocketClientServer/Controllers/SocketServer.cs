using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClientServer.Controllers
{
    class SocketServer
    {

        TcpListener serverSocket;
        int requestCount = 0;
        TcpClient clientSocket;
        bool listen = false;
        NetworkStream networkStream;
        string dataFromClient;


        public  async void TestAsync()
        {
          string n =  await WaitForMessage();
        }

        public void InitServer()
        {
            serverSocket = new TcpListener(8888);
            clientSocket = default(TcpClient);
        }


        public async void StartListen()
        {
            try
            {
                listen = true;
                serverSocket.Start();
                clientSocket = await InitSocket();
            }
            catch(Exception ex)
            {

            }
        }

        public void StopListen()
        {
            listen = false;
            clientSocket?.Close();
            serverSocket.Stop();
        }

        private Task<TcpClient> InitSocket()
        {
            return Task<TcpClient>.Run(()=> {
                try
                {
                    return serverSocket.AcceptTcpClient();
                }
                catch
                {
                    return null;
                }
            });
        }




        public Task<string> WaitForMessage()
        {

            return Task<string>.Run(() =>
            {
                while(listen && clientSocket == null)
                {
                    Thread.Sleep(100);
                }
                if (!listen)
                    return "stoped";
                try
                {


                    do
                    {
                        networkStream = clientSocket.GetStream();
                        byte[] bytesFrom = new byte[10025];
                        networkStream.Read(bytesFrom, 0, 10025);
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Replace("\0", "");
                              
                    } while (string.IsNullOrWhiteSpace(dataFromClient));

                    dataFromClient = dataFromClient.Trim();
                    return dataFromClient;
                    
                    //string serverResponse = "Last Message from client" + dataFromClient;
                    //Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    //networkStream.Write(sendBytes, 0, sendBytes.Length);


                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                finally
                {
                    networkStream.Flush();
                }
            });
            
        }

    }
}
