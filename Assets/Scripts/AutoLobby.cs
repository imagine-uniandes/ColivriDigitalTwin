using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoLobby : MonoBehaviour
{
   public Button ConnectButton;
   public Button JoinRandomButton;
  // public Text Log;

   public byte maxPlayersPerRoom = 4;

   public void Connect()
   {

    if (!PhotonNetwork.IsConnected)
    {
        
        if(PhotonNetwork.ConnectUsingSettings()){

            Debug.Log("Connected to Server");
        }
        else
        {
            Debug.Log("Falling Connecting to Server");
        }
    }
   }
   
   /**public override void OnConnectedToMaster()
   {
    ConnectButton.interactable = false;
    JoinRandomButton.interactable = true;
   }**/
   public void Join()
   {
    if (!PhotonNetwork.JoinRandomRoom())
    {
        Debug.Log("Fail Joining room");
    }
   }

    /**
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Log.text += "\nNo Rooms to Join, creating one...";

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions(){ maxPlayers = maxPlayersPerRoom}))
        {
            Log.text += "nRoom Created";

        }
        else
        {
            Log.text += "\nFail Creating Room";
        }
    }**/

}
