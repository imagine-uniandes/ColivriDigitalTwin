using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ChangeCameraPosition : MonoBehaviour
{
    public GameObject mrtkXRRig;
    public GameObject slatePiso1;
    public GameObject slateSotano1;
    public GameObject cameraMain;

    public Vector3 position1 = new Vector3(9.602f, 2.341f, 6.835f);
    public Vector3 positionS1 = new Vector3(11.68F, -2.386f, 4.22f);
    


    public void CamaraPiso1()
    {
        if (mrtkXRRig != null)
        {


            //Debug.Log("RIG: " + cameraTransform.position.y);

            if (cameraMain.transform.position.y <= 0f)
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

           

            if (cameraMain.transform.position.y >= 0)
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
