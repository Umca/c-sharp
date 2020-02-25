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
            while (true)
            {
                Socket handler = m_listenSocket.Accept();
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                byte[] data = new byte[256];

                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (handler.Available > 0);
                SendMsg(builder.ToString());

                string msg = "Random message from server.";
                data = Encoding.Unicode.GetBytes(msg);
                handler.Send(data);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        void SendMsg(string msg)
        {
            Console.WriteLine("{0} : {1}", DateTime.Now.ToShortTimeString(), msg);
        } 


    }
}
