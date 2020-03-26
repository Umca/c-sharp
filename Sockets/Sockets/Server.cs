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
        IPEndPoint m_ipPoint;
        string m_host;
        public Server(int port, string host)
        {
            m_port = port;
            m_host = host;
            m_ipPoint = new IPEndPoint(IPAddress.Parse(m_host), m_port);
            m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            m_listenSocket.Bind(m_ipPoint);
            m_listenSocket.Listen(10);
            Print("Server is listening...");

            Run();
        }
        void Run()
        {

            try
            {
                while (true)
                {
                    Socket handler = m_listenSocket.Accept();
                    connections++;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Print("Amount of connections " + connections);
                    Console.ResetColor();

                    Thread th = new Thread(() =>
                    {
                        try
                        {
                            while (true)
                            {
                                Print("New incomming message.");
                                StringBuilder builder = new StringBuilder();
                                int bytes = 0;
                                do
                                {
                                    byte[] buffer = new byte[1024];
                                    bytes = handler.Receive(buffer);
                                    builder.Append(Encoding.Unicode.GetString(buffer));
                                } while (handler.Available > 0);
                                string msg = DateParser.Parse(builder.ToString());
                                handler.Send(Encoding.Unicode.GetBytes(msg));
                            }
                        }
                        catch(SocketException err)
                        {
                            if (err.ErrorCode == 10053)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Print("Disconnected!");
                                Console.ResetColor();
                            }
                            else Print(err.ToString());
                        }
                    });
                    th.Start();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }
        void Print(string msg)
        {
            Console.WriteLine("{0} : {1}", DateTime.Now.ToShortTimeString(), msg);
        }



    }
}
