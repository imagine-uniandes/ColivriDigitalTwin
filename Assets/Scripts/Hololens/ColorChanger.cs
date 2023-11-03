using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    private Material gazeMaterial;

    private Material originalMaterial;
    private Renderer objectRenderer;
    private bool isGazeHovered = false;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    private void Update()
    {
        if (isGazeHovered)
        {
            objectRenderer.material = gazeMaterial;
        }
        else
        {
            objectRenderer.material = originalMaterial;
        }
    }

    public void Increment()
    {
        isGazeHovered = true;
    }

    public void Decrement()
    {
        isGazeHovered = false;
    }
}
