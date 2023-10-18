using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        string roomName = "SalaVR"; // Nombre de la sala (puedes cambiarlo)
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4, // Cambia esto según tus necesidades
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom()
    {
        string roomName = "SalaVR"; // Nombre de la sala a la que deseas unirte
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado a Photon Master Server");
        // Habilitar botones de "Crear Sala" y "Unirse a Sala" u otras acciones
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entraste a la sala: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("NombreDeTuEscenaVR"); // Cambia a tu escena VR
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al unirse a la sala: " + message);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Sala creada: " + PhotonNetwork.CurrentRoom.Name);
    }
}