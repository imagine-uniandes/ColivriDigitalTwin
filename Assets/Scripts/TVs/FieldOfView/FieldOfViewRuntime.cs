using UnityEngine;
using System.Collections;

public class FieldOfViewRuntime : MonoBehaviour
{
    private FieldOfView fieldOfView;

    private void Start()
    {
        // Get the FieldOfView component on the same GameObject
        fieldOfView = GetComponent<FieldOfView>();
    }

    private void OnDrawGizmos()
    {
        // Draw the red lines to visible targets during runtime
        Gizmos.color = Color.red;
        foreach (Transform visibleTarget in fieldOfView.visibleTargets)
        {
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }
}
