﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Sockets
{
    class ClientServer
    {
        
        int m_port;
        Socket m_socket;
        IPEndPoint m_ipPoint;
        string m_host;
        public ClientServer(int port, string host)
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
                string message = "Random message from client.";
                byte[] data = Encoding.Unicode.GetBytes(message);
                m_socket.Send(data);

                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = m_socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (m_socket.Available > 0);
                SendMsg("Response from server: " + builder.ToString());
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


    }
}
