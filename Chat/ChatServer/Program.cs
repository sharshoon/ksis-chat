using System;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static ServerObject server;
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();

                Thread test = new Thread(new ThreadStart(server.ReceiveUdpMessage));
                test.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                server.ProgramExit();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
