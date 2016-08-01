using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace TcpServer
{
    public class ClientObj
    {
        protected internal string Id { get; set; }
        protected internal NetworkStream Stream { get; set; }

        Servak serv;
        string username;
        TcpClient client;
        

        public ClientObj(TcpClient tcpClient, Servak obj)
        {
            Id = Guid.NewGuid().ToString();
            serv = obj;
            client = tcpClient;
            obj.AddConnection(this);
        }
        public string RecieveRequest()
        {
            byte[] data = new byte[64];
            Stream.Read(data, 0, data.Length);
            string str = Encoding.Unicode.GetString(data);
            Console.WriteLine("Client  "+ Id + "  send: " + str);
            return str;
        }

        public void SendYourNumber()
        {
            byte[] data = new byte[32];
            string number = serv.NumberOfClient().ToString();
            data = Encoding.Unicode.GetBytes(number);
            Stream.Write(data,0,data.Length);
            Console.WriteLine("Client received: " + number);
        }
        public void Process()
        {
            try
            {
               Stream = client.GetStream();
               SendYourNumber();
                while (true)
                {
                    string message = RecieveRequest();
                    serv.BroadcastRequest(message, this.Id); 

                    Console.WriteLine("Client  " + Id +"  recieved: "+ message);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                serv.RemoveConnection(this);
                Close();
            }
        }

        public void Close()
        {
            if (Stream != null)
            {
                Stream.Close();
            }
            if (client != null)
                client.Close();
        }


    }
}
