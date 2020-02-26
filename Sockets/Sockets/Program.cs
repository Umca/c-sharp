using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sockets
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8005;
            string host = "127.0.0.1";
            SocketServer server = new SocketServer(port, host);
            ClientServer client = new ClientServer(port, host);

            Thread serverTh = new Thread(new ThreadStart(server.Start));
            Thread clientTh = new Thread(new ThreadStart(client.Start));

            //serverTh.Start();
            //Thread.Sleep(2000);
            //clientTh.Start();

            string res = DateParser.Parse("climbers 03/06/2017 with");
            Console.WriteLine(res);
        }
    }
}
