using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Sockets
{
    class Server
    {
        int connections;
        int m_port;
        Socket m_listenSocket;
        Socket handler;
        IPEndPoint m_ipPoint;
        string m_host;
        public Server(int port, string host)
        {
            m_port = port;
            m_host = host;
            m_ipPoint = new IPEndPoint(IPAddress.Parse(m_host), m_port);
            m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            try
            {
                m_listenSocket.Bind(m_ipPoint);
                m_listenSocket.Listen(10);
                SendMsg("Server launched.");
                Run();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
        void Run()
        {
            
            while (true)
            {
                connections++;
                handler = m_listenSocket.Accept();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Amount of connections {0}\n", connections);
                Console.ResetColor();

                new Thread(() => {

                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] data = new byte[1024];

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    SendMsg("Server received new message.");

                    string msg = DateParser.Parse(builder.ToString());
                    data = Encoding.Unicode.GetBytes(msg);
                    handler.Send(data);

                }).Start();
            } 
        }
        void SendMsg(string msg)
        {
            Console.WriteLine("{0} : {1}", DateTime.Now.ToShortTimeString(), msg);
        }



    }
}
