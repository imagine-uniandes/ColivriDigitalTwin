using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ChangeCameraPosition : MonoBehaviour
{
    public GameObject mrtkXRRig;
    public GameObject slatePiso1;
    public GameObject slateSotano1;

    public Vector3 position1 = new Vector3(0.365999997f, 0.739000022f, 0.0659999996f);
    public Vector3 positionS1 = new Vector3(2.4375f, -3.98600006f, -2.46700001f);

    
    public void CamaraPiso1()
    {
        if (mrtkXRRig != null)
        {
            float currentHeight = mrtkXRRig.transform.position.y;

            

            if (currentHeight <= 0f)
            {
                // Cambia la posición del objeto MRTK XR Rig para ir al piso 1
                mrtkXRRig.transform.position = position1;
            }
            else
            {
                slatePiso1.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado el objeto MRTK XR Rig.");
        }
    }

    //antes de subir posicion aca arriba en la vida real es -2.4

    //sueb y suposicion es 2.2
    //a la mitad de las esclares es -0.1
    //abajo es -2.3

    public void CamaraSotano1()
    {
        if (mrtkXRRig != null)
        {
            float currentHeight = mrtkXRRig.transform.position.y;

           

            if (currentHeight >= 0)
            {
                // Cambia la posición del objeto MRTK XR Rig para ir al sótano 1
                mrtkXRRig.transform.position = positionS1;
            }
            else
            {
                slateSotano1.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("No se ha asignado el objeto MRTK XR Rig.");
        }
    }
}
