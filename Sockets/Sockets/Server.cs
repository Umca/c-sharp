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
        static object locker = new object();
        static int connections;
        bool m_isRunning;
        int m_port;
        string m_host;
        Socket m_listenSocket;
        IPEndPoint m_ipPoint;
        List<Socket> sockets;
        public Server(int port, string host)
        {
            m_port = port;
            m_host = host;
            m_ipPoint = new IPEndPoint(IPAddress.Parse(m_host), m_port);
            m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            m_listenSocket.Bind(m_ipPoint);
            m_listenSocket.Listen(10);
            Print("Server is listening...");
            m_isRunning = true;

            sockets = new List<Socket>();
            Thread th = new Thread(Run);
            th.Start();
            try
            {
                Console.WriteLine("Press Esc to shutdown the server.");
                ConsoleKeyInfo cki;
                do
                {
                    cki = Console.ReadKey();
                    Console.WriteLine("Please, press Ecp instead of {0}.", cki.Key.ToString());
                } while (cki.Key != ConsoleKey.Escape);
                m_isRunning = false;
                foreach (var sock in sockets)
                {
                    if (sock.Connected)
                    {
                        sock.Shutdown(SocketShutdown.Both);
                        sock.Close();
                    }
                }
                sockets.Clear();
                m_listenSocket.Shutdown(SocketShutdown.Both);
            }
            catch(SocketException err)
            {
                if (err.ErrorCode == 10057) Console.WriteLine("Server is closed. {0}", err.ErrorCode);
            }
            catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }
            finally
            {
                
                m_listenSocket.Close();
                Console.ReadKey();
            }
        }
        void Run()
        {

            try
            {
                while (m_isRunning)
                {
                    Socket handler = m_listenSocket.Accept();
                    sockets.Add(handler);
                    connections++;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Print("Amount of connections " + connections);
                    Console.ResetColor();

                    Thread th = new Thread((object sock) =>
                    {
                        Socket client = (Socket)sock;
                        try
                        {
                            while (true)
                            {

                                if (!m_isRunning)
                                {
                                    client.Shutdown(SocketShutdown.Both);
                                    client.Close();
                                    client = null;
                                    return;
                                }
                                Print("New incomming message.");
                                StringBuilder builder = new StringBuilder();
                                int bytes = 0;
                                do
                                {
                                   
                                    byte[] buffer = new byte[1024];
                                    if (client.Connected)
                                    {
                                        bytes = client.Receive(buffer);   // System.Net.Sockets.SocketException (0x80004005): Операция блокирования прервана вызовом WSACancelBlockingCall
                                                                          // в System.Net.Sockets.Socket.Receive(Byte[] buffer, Int32 offset, Int32 size, SocketFlags socketFlags)
                                                                          // я хоть и проверяю выше на то, что сервак работает, но все равно на эту строку жалуется
                                                                          // я так понимаю , мы прошли проверку, дошли до ресив. а я уже в этот момент остановила сервер и поэтому ошибка
                                                                          // я передвигала проверку ближе к ресив, но тогда есть шанс отвалится ниже на отправке. 
                                                                          // а дублирование - вещь такая(
                                                                          // может же быть такое, что прерывается коннекшн в этот момент?
                                        builder.Append(Encoding.Unicode.GetString(buffer));
                                    }
                                } while (client.Available > 0);
                                string msg = DateParser.Parse(builder.ToString());
                                if(client.Connected) client.Send(Encoding.Unicode.GetBytes(msg));
                            }
                        }
                        catch(SocketException err)
                        {
                            if (err.ErrorCode == 10053) // client disconnected
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                lock (locker)
                                {
                                    sockets.Remove(client);
                                    connections--;
                                    Print("Disconnected!" + connections + " - current amount of connections. " + err.ErrorCode);
                                }
                                Console.ResetColor();
                            }
                            else Print(err.ToString());
                        }
                    });
                    th.Start(handler);
                }
            }
            catch (SocketException err)
            {
                if (err.ErrorCode == 10004) return; //main socket is closed from other thread
                Console.WriteLine(err.ToString());
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
