using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
    private TcpListener server;
    private TcpClient client;

    private void Start()
    {
        server = new TcpListener(IPAddress.Any, 8080); // Escucha en el puerto 12345
        server.Start();

        Debug.Log("Esperando una conexión...");
        client = server.AcceptTcpClient();
        Debug.Log("Cliente conectado.");

        // Stream para recibir datos del cliente
        NetworkStream stream = client.GetStream();
        byte[] data = new byte[1024];
        int bytesRead = stream.Read(data, 0, data.Length);
        string message = Encoding.UTF8.GetString(data, 0, bytesRead);
        Debug.Log("Mensaje recibido: " + message);

        client.Close();
        server.Stop();
    }
}