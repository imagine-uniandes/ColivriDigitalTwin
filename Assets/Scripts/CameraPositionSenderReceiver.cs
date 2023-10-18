using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class CameraPositionSenderReceiver : MonoBehaviour
{
    private string serverIP = "192.168.0.100"; // IP del otro dispositivo
    private int serverPort = 12345; // Puerto para el servidor en el otro dispositivo

    private UdpClient udpClient;

    void Start()
    {   
        Debug.Log("Start");
        udpClient = new UdpClient();
    }

    void Update()
    {
        Debug.Log("Update");
        // Envía la posición de la cámara al otro dispositivo
        SendCameraPosition(transform.position);

        // Recibe la posición de la cámara del otro dispositivo
        //Vector3 receivedPosition = ReceiveCameraPosition();

        // Haz algo con la posición recibida, como actualizar la posición de la cámara.
        //Debug.Log("Posición recibida: " + receivedPosition);
    }

    private void SendCameraPosition(Vector3 cameraPosition)
    {
        Debug.Log("Send ");
        string positionData = string.Format("{0},{1},{2}", cameraPosition.x, cameraPosition.y, cameraPosition.z);
        byte[] data = Encoding.UTF8.GetBytes(positionData);

        udpClient.Send(data, data.Length, serverIP, serverPort);
        Debug.Log("Posición enviada: " + cameraPosition);
    }

/**
    private Vector3 ReceiveCameraPosition()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
        byte[] receivedBytes = udpClient.Receive(ref endPoint);
        string receivedData = Encoding.UTF8.GetString(receivedBytes);

        // Procesa los datos recibidos y conviértelos en una posición
        string[] positionValues = receivedData.Split(',');
        if (positionValues.Length == 3)
        {
            float x = float.Parse(positionValues[0]);
            float y = float.Parse(positionValues[1]);
            float z = float.Parse(positionValues[2]);
            Vector3 receivedPosition = new Vector3(x, y, z);
            Debug.Log("Datos recibidos: " + receivedData);
            return receivedPosition;
        }

        return Vector3.zero;
    }**/
}
