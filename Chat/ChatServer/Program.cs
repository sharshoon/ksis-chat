using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static ServerSubj server;
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerSubj();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();

                Thread test = new Thread(new ThreadStart(server.ReceiveUdpMessage));
                test.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
