using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Socket;

class Program
{
    static void Main(string[] args)
    {
        int port = 40001;

        // Create a TCP listener
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        Console.WriteLine($"Listening on port {port}...");

        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                // Read the "Connect" request
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(request);

                if (!request.Trim().Equals("Connect", StringComparison.OrdinalIgnoreCase))
                {
                    // Emulate the daughter board behavior
                    string deviceIds = "123,124,125";
                    string startCharacter = "S:";
                    string endCharacter = ";E";
                    int length = deviceIds.Length;
                    string response = $"{startCharacter} {length} {deviceIds} {endCharacter}";

                    // Send the response
                    byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                    stream.Close();
                }
                else
                {
                    Console.WriteLine("Invalid request. Expected 'Connect'.");
                }
            }
        }

    }
}
