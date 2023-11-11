using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public Vector3 destinationPosition = new Vector3(0.894f, -0.219f, 7.079f); // Establece la posición de destino

    public void TeleportObject(GameObject objectToTeleport)
    {

        objectToTeleport.transform.position = destinationPosition;



    }
}
