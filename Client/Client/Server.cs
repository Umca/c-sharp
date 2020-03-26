using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Client
{
    class Server
    {
        int m_port;
        Socket m_socket;
        IPEndPoint m_ipPoint;
        string m_host;
        public Server(int port, string host)
        {
            m_port = port;
            m_host = host;
            m_ipPoint = new IPEndPoint(IPAddress.Parse(m_host), m_port);
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            try
            {
                m_socket.Connect(m_ipPoint);
                SendMsg("Client launched.");
                ReadFile();
            }
            catch (SocketException err)
            {
                Console.WriteLine(err);
            }
            catch (IOException err)
            {
                Console.WriteLine(err);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
        void SendMsg(string msg)
        {
            Console.WriteLine("{0} : {1}", DateTime.Now.ToShortTimeString(), msg);
        }

        void ReadFile()
        {
            const Int32 BufferSize = 1024;
            using (var fileStream = File.OpenRead("data.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {

                    Thread.Sleep(500);
                    ReceiveSendSocket(line);
                }
                m_socket.Shutdown(SocketShutdown.Send);
                m_socket.Close();
            }
        }

        void ReceiveSendSocket(string line)
        {
            byte[] data = Encoding.Unicode.GetBytes(line);
            m_socket.Send(data);

            data = new byte[256];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = m_socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (m_socket.Available > 0);
            SendMsg("Client received: " + builder.ToString());
        }
    }
}
