using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPosition : MonoBehaviour
{

    public GameObject avatar;

    public TextAsset csvFile;
    // Start is called before the first frame update
    void Start()
    {
        // Obtén el contenido del archivo CSV como una cadena.
            string csvText = csvFile.text;
            string[] lines = csvText.Split('\n');

        // Verifica si hay al menos una línea en el archivo
        if (lines.Length > 0)
        {
            // Divide la primera línea en valores separados por comas
            string[] values = lines[0].Split(',');

            // Asegúrate de que haya al menos tres valores
            if (values.Length >= 3)
            {
                // Asigna los valores a variables separadas
                float value0 = float.Parse(values[0]);
                float value1 = float.Parse(values[1]);
                float value2 = float.Parse(values[2]);

                avatar.transform.position = new Vector3(value0,value1,value2);

                
            }
            else
            {
                Debug.LogError("No hay suficientes valores en la primera línea del archivo CSV.");
            }
        }
        else
        {
            Debug.LogError("El archivo CSV está vacío.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Information()
    {
       
        
        
    }
}
