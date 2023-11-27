using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TeleportButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public Color32 m_HoverColor = Color.blue;
    public Color32 m_DownColor = Color.white;

    private Image m_Image = null;
    public GameObject objectToTeleport; // Asigna el objeto que quieres teletransportar en el Inspector
    public Vector3 destinationPosition = new Vector3(0.894f, -0.219f, 7.079f); // Establece la posición de destino

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    private void TeleportObject()
    {
        if (objectToTeleport != null)
        {
            objectToTeleport.transform.position = destinationPosition;
        }
        else
        {
            Debug.LogError("No se ha asignado un objeto para teletransportar en el Inspector.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click");
        m_Image.color = m_HoverColor;
        TeleportObject(); // Llama a la función de teletransporte cuando se hace clic
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down");
        m_Image.color = m_DownColor;
    }
}