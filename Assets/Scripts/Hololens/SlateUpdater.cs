using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlateUpdater : MonoBehaviour
{
    public TextMeshPro proyectoText;
    public TextMeshPro asignacionText;
    public TextMeshPro procesadorText;
    public TextMeshPro memoriaText;
    public TextMeshPro almacenamientoText;
    public TextMeshPro tarjetaGraficaText;
    public CSVReader csvReader;
    public int computerIndex;

    private void Start()
    {
        UpdateTitleText();
    }

    private void UpdateTitleText()
    {

        var computerDataList = csvReader.computerDataList;

        if (computerDataList != null && computerDataList.Count >= computerIndex)
        {
            var computerData = computerDataList[computerIndex];
            var proyectoCargo = computerData.ProyectoCargo;
            var asignacion = computerData.Asignacion;
            var procesador = "Procesador: " + computerData.Procesador;
            var memoria = "Memoria total: " + computerData.Memoria;
            var almacenamiento = "Almacenamiento total: " + computerData.Almacenamiento;
            var tarjetaGrafica = computerData.TarjetaGrafica;


            Debug.Log("Proyecto/Cargo: " + proyectoCargo);

            proyectoText.text = proyectoCargo;
            asignacionText.text = asignacion;
            procesadorText.text = procesador;
            memoriaText.text = memoria;
            almacenamientoText.text = almacenamiento;
            tarjetaGraficaText.text = tarjetaGrafica;
        }
        else
        {
            proyectoText.text = "No Data Available";
            asignacionText.text = "No Data Available";
            memoriaText.text = "No Data Available";
            almacenamientoText.text = "No Data Available";
            procesadorText.text = "No Data Available";
            tarjetaGraficaText.text = "No Data Available";

        }
    }
}
