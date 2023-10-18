using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // Permite arrastrar el archivo CSV directamente a este campo en el Inspector.

    public string oculusValue;
    public string ipAddressValue;
    public string metaQuestValue;
    public string storageValue;

    public string direccionMac;

    public string encendido;

    public string fecha;

    public string porcentajeBateria;

    public string red;

    public TMP_Text infoCasco;  

    public TMP_Text ultimoUso;         

    void Start()
    {
        Debug.Log("Holaaaaaaaaaaaaaaaaaaaaaa");
        if (csvFile != null)
        {
            // Obtén el contenido del archivo CSV como una cadena.
            string csvText = csvFile.text;

            // Divide el contenido del archivo CSV en líneas.
            string[] lines = csvText.Split('\n');

            if (lines.Length > 0)
            {
                // Divide la primera línea en sus valores utilizando comas como separadores.
                string[] values = lines[1].Split(',');

                // Imprime los valores específicos que deseas de la primera línea.
                if (values.Length >= 22)
                {
                    // Datos Estaticos
                    oculusValue = char.ToString(values[1][7]);
                    ipAddressValue = values[18];
                    direccionMac = values[19];
                    metaQuestValue = values[3];

                    // Datos Dinamicos
                    encendido = values[8];
                    fecha =values[7];
                    porcentajeBateria = values[13];
                    storageValue = values[12];
                    red = values[14];


                    // Asignar los valores de texto al TMP estatico
                    infoCasco.text = "Información del casco: " + oculusValue + Environment.NewLine+ encendido +Environment.NewLine + Environment.NewLine + "Dirección IP: " + ipAddressValue +
                    Environment.NewLine + "Direccion MAC: " + direccionMac + Environment.NewLine + "Modelo: " + metaQuestValue ;

                    // Asignar los valores de texto al TMP dinamico
                    ultimoUso.text = "Ultimo uso: " + fecha + Environment.NewLine + Environment.NewLine + "Porcentaje de Bateria: " + porcentajeBateria +
                    Environment.NewLine + "Almacenamiento: " + storageValue  + Environment.NewLine+  "Red: " + red ;

                }
                else
                {
                    Debug.LogWarning("La primera línea del archivo CSV no tiene suficientes valores.");
                }
            }
            else
            {
                Debug.LogWarning("El archivo CSV está vacío.");
            }
        }
        else
        {
            Debug.LogError("No se ha asignado ningún archivo CSV. Arrastra el archivo CSV al campo 'csvFile' en el Inspector.");
        }
    }
}
