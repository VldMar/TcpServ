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
    class Program
    {
        static Thread listenThread;
        static Servak serv;
        static void Main(string[] args)
        {
            
            try
            {
                serv = new Servak();
                listenThread = new Thread(new ThreadStart(serv.Listen));
                listenThread.Start();
            }
            catch(Exception ex)
            {
                serv.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
