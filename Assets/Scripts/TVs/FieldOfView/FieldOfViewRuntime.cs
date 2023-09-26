using UnityEngine;

public class FieldOfViewRuntime : MonoBehaviour
{
    private FieldOfView fieldOfView;

    // Reference to the line renderer prefab or material
    public LineRenderer lineRendererPrefab;

    private void Start()
    {
        // Get the FieldOfView component on the same GameObject
        fieldOfView = GetComponent<FieldOfView>();
    }

    private void Update()
    {
        // Clear previous lines
        ClearVisibleLines();

        // Draw new lines to visible targets
        foreach (Transform visibleTarget in fieldOfView.visibleTargets)
        {
            DrawLineToTarget(visibleTarget);
        }
    }

    void ClearVisibleLines()
    {
        // Find and destroy existing line renderer objects
        LineRenderer[] existingLines = GetComponentsInChildren<LineRenderer>();
        foreach (var lineRenderer in existingLines)
        {
            Destroy(lineRenderer.gameObject);
        }
    }

    void DrawLineToTarget(Transform target)
    {
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position);
    }
}
