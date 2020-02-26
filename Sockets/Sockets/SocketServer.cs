using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Sockets
{
    class SocketServer
    {
        int m_port;
        Socket m_listenSocket;
        IPEndPoint m_ipPoint;
        string m_host;
        public SocketServer(int port, string host)
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
            } catch(Exception err)
            {
                Console.WriteLine(err);
            }
        }
        void Run()
        {
            Socket handler = m_listenSocket.Accept();
            while (true)
            {
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

                string msg = builder.ToString() + " server mark";
                data = Encoding.Unicode.GetBytes(msg);
                handler.Send(data);
                SendMsg("Server sent message. ");
            }
        }
        void SendMsg(string msg)
        {
            Console.WriteLine("{0} : {1}", DateTime.Now.ToShortTimeString(), msg);
        } 


    }
}
