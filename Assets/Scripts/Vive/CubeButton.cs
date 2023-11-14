using UnityEngine;

public class CubeButton : MonoBehaviour
{
    private int clickCount = 0;
    private int keyPressCount = 0;

    private void OnMouseDown()
    {
        // Incrementa el contador de clics
        clickCount++;

        // Llama a diferentes funciones según el número de clics
        if (clickCount == 1)
        {
            FirstClick();
        }
        else if (clickCount == 2)
        {
            SecondClick();
        }
    }

        private void Update()
    {
        // Verifica si se ha presionado una tecla
        if (Input.anyKeyDown)
        {
            keyPressCount++; // Incrementa el contador de pulsaciones de teclas

            // Decide qué función llamar en función del número de teclas presionadas
            if (keyPressCount == 1)
            {
                FirstClick();
            }
            else if (keyPressCount == 2)
            {
                SecondClick();
            }
        }
    }

    private void FirstClick()
    {
        // Lógica para el primer clic aquí
        Debug.Log("Primer clic en " + gameObject.name);
        GameObjectManager.instance.StartMovementOnObjects();
    }

    private void SecondClick()
    {
        // Lógica para el segundo clic aquí
        Debug.Log("Segundo clic en " + gameObject.name);
        GameObjectManager.instance.StartMovementOnObjectsRevert();

        // Restablece el contador de clics para permitir más clics
        clickCount = 0;
        keyPressCount = 0;
    }
}
