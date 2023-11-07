using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ChangeCameraPosition : MonoBehaviour
{
    public GameObject mrtkXRRig;
    public GameObject slatePiso1;
    public GameObject slateSotano1;

    public Vector3 position1 = new Vector3(9.60200024f, 0.740999997f, 6.83500004f);
    public Vector3 positionS1 = new Vector3(11.6829996f, -3.98600006f, 4.21999979f);
    public Transform cameraTransform = Camera.main.transform;


    public void CamaraPiso1()
    {
        if (mrtkXRRig != null)
        {


            //Debug.Log("RIG: " + cameraTransform.position.y);

            if (mrtkXRRig.transform.position.y <= 0f)
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

           

            if (cameraTransform.position.y >= 0)
            {
                // Cambia la posición del objeto MRTK XR Rig para ir al sótano 1
                //Debug.Log("RIG: " + cameraTransform.position.y);
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
