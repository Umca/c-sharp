using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 4000;
            string host = "127.0.0.1";
            Server client = new Server(port, host);
            client.Start();

        }
    }
}
