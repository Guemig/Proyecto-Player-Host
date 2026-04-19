using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>();

    public event Action<string> OnMessageReceived;

    public async Task StartServer(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        Debug.Log("[Server] Started");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            clients.Add(client);

            Debug.Log("[Server] Client connected");

            _ = HandleClient(client);
        }
    }

    async Task HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
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

                Debug.Log("[Server] " + msg);

                OnMessageReceived?.Invoke(msg);
                Broadcast(msg);
            }
        }
    }

    public async void Broadcast(string msg)
    {
        msg += "\n";
        byte[] data = Encoding.UTF8.GetBytes(msg);

        foreach (var client in clients)
        {
            if (client != null && client.Connected)
            {
                await client.GetStream().WriteAsync(data, 0, data.Length);
            }
        }
    }
}