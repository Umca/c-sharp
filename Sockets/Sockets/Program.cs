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
            int port = 13000;
            string host = "127.0.0.1";
            Server server = new Server(port, host);
        }
    }
}
