using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;

    public event Action<string> OnMessageReceived;

    public async Task ConnectToServer(string ip, int port)
    {
        client = new TcpClient();

        await client.ConnectAsync(ip, port);

        stream = client.GetStream();

        Debug.Log("[Client] Connected");

        _ = ReceiveLoop();
    }

    async Task ReceiveLoop()
    {
        byte[] buffer = new byte[1024];
        StringBuilder builder = new StringBuilder();

        while (client.Connected)
        {
            int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);

            if (bytes == 0) break;

            string chunk = Encoding.UTF8.GetString(buffer, 0, bytes);
            builder.Append(chunk);

            while (builder.ToString().Contains("\n"))
            {
                string full = builder.ToString();
                int index = full.IndexOf("\n");

                string msg = full.Substring(0, index);
                builder.Remove(0, index + 1);

                Debug.Log("[Client] " + msg);

                OnMessageReceived?.Invoke(msg);
            }
        }
    }
    public async void Send(string msg)
    {
        msg += "\n";

        byte[] data = Encoding.UTF8.GetBytes(msg);

        await stream.WriteAsync(data, 0, data.Length);

        Debug.Log("[Client] SENT: " + msg);
    }
}