using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var socket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Loopback, 1234);
            try
            {
                Console.WriteLine("Client has started");
                socket.Connect(endPoint);
                Console.WriteLine("Напишіть 'DATA'  щоб отримати дату, або 'TIME' щоб отримати час.");
                string message = Console.ReadLine();
                socket.Send(Encoding.UTF8.GetBytes(message));
                var buf= new byte[1024];
                int bytesRead= socket.Receive(buf);
                string responseMessage= Encoding.UTF8.GetString(buf, 0, bytesRead);
                Console.WriteLine($"Отримано від {socket?.RemoteEndPoint?.ToString()}: {responseMessage}");
                socket?.Shutdown(SocketShutdown.Both);
                socket?.Close();

            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
            Console.WriteLine("\nНатисни будь яку кнопку щоб вийти....");
            Console.ReadKey();
        }
    }
}
