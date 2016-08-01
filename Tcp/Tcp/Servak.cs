using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpServer
{
    public class Servak
    {
        static TcpListener listener;
        private List<ClientObj> clients = new List<ClientObj>();

        protected internal void AddConnection(ClientObj obj)
        {
            clients.Add(obj);
        }

        protected internal void RemoveConnection(ClientObj obj)
        {
            if (clients != null)
                clients.Remove(obj);
        }

        protected internal void Listen()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 8005);
                listener.Start();
                
                Console.WriteLine("Сервер запущен, ожидание подключения...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    if (client.Connected)
                    {
                        
                        Console.WriteLine("клиент  подключен");
                        NumberOfClient();

                        ClientObj clientObj = new ClientObj(client, this);
                        Thread clientTh = new Thread(new ThreadStart(clientObj.Process));
                        clientTh.Start();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                Disconnect();
            }       
        }

        protected internal void Disconnect()
        {
            listener.Stop();

            for(int i=0; i<clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);

        }
        protected internal void BroadcastRequest(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for(int i=0; i<clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                    Console.WriteLine("Client " + clients[i] + " recieved: " + message);
                }
            }
        }


        protected internal int NumberOfClient()
        {
            int numb = 0;
            for(int i=0;i<clients.Count;i++)
            {
                if (i  % 2 == 0)
                {
                    numb = 1;
                }
                else if(i%2 ==1)
                {
                    numb = 2;
                }
            }

            return numb;
        }

    }
}
