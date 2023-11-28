using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    public static GameObjectManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMovementOnObjects()
    {
        // Este método será llamado por el CubeButton para iniciar el movimiento en los objetos que lo contienen.
        MovementObject[] objectsToMove = FindObjectsOfType<MovementObject>();

        foreach (MovementObject obj in objectsToMove)
        {
            obj.StartMovement();
        }
    }

        public void StartMovementOnObjectsRevert()
    {
        // Este método será llamado por el CubeButton para iniciar el movimiento en los objetos que lo contienen.
        MovementObject2[] objectsToMove = FindObjectsOfType<MovementObject2>();

        foreach (MovementObject2 obj in objectsToMove)
        {
            obj.StartMovement();
        }
    }
}
