using UnityEngine;

public class FieldOfViewRuntime : MonoBehaviour
{
    private FieldOfView fieldOfView;

    // Reference to the LineRenderer prefab
    public LineRenderer lineRendererPrefab;

    // Material for the lines
    public Material lineMaterial;

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
        // Find and destroy existing LineRenderer objects
        LineRenderer[] existingLines = GetComponentsInChildren<LineRenderer>();
        foreach (var lineRenderer in existingLines)
        {
            Destroy(lineRenderer.gameObject);
        }
    }

    void DrawLineToTarget(Transform target)
    {
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab, transform);
        lineRenderer.material = lineMaterial;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position);
    }
}
