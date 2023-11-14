using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementObject : MonoBehaviour
{
    public float moveDuration = 5;
    public float targetY = -9; 
    // Start is called before the first frame update
    public void StartMovement()
    {
        Debug.Log("Ejecutando MovementObject");
        StartCoroutine(MoveObject(targetY));
    }

    IEnumerator MoveObject(float targetYPosition)
    {
        Vector3 startPosition = transform.position;
        float timeElapsed = 0;

        while (timeElapsed < moveDuration)
        {
            Vector3 newPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);
            transform.position = Vector3.Lerp(startPosition, newPosition, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = new Vector3(transform.position.x, targetYPosition, transform.position.z);
    }
}
