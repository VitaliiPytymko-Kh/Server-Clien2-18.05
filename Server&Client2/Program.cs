using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Any, 1234);
            socket.Bind(endPoint);
            socket.Listen(10);
            try 
            {
                Console.WriteLine("Server has started");
                while (true)
                {
                    var client = socket.Accept();
                    var buf = new byte[1024];
                    int bytesRead = client.Receive(buf);
                    string receivedMessage = Encoding.UTF8.GetString(buf, 0, bytesRead);
                    Console.WriteLine($"Отримано від  {client?.RemoteEndPoint?.ToString()}: {receivedMessage}");

                    string responseMessage;
                    if (receivedMessage.Trim().ToUpper() == "DATA")
                    {
                        responseMessage = DateTime.Now.ToString("yyy-MM-dd");
                    }
                    else if (receivedMessage.Trim().ToUpper() == "TIME")
                    {
                        responseMessage = DateTime.Now.ToString("HH:mm:ss");
                    }
                    else
                    {
                        responseMessage = "Invalid request";
                    }
                    client?.Send(Encoding.UTF8.GetBytes(responseMessage));
                    client?.Shutdown(SocketShutdown.Both);
                    client?.Close();
                }
            }
            catch ( Exception ex) { Console.WriteLine(ex.Message); }
            socket.Close();
            Console.WriteLine("\nPress any key to exit....");
            Console.ReadKey();
        }
    }
}
