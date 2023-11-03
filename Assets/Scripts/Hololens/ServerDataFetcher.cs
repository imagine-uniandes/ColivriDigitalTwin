using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class ComputerDynamicData
{
    public string CPUUsage;
    public string RAMUsage;
    public string DiskUsage;
}

public class ServerDataFetcher : MonoBehaviour
{
    public TextMeshPro[] cpuTextMeshes; // Array de TextMeshPro para CPU
    public TextMeshPro[] ramTextMeshes; // Array de TextMeshPro para RAM
    public TextMeshPro[] diskTextMeshes; // Array de TextMeshPro para Disco

    private List<ComputerDynamicData> computerDataList = new List<ComputerDynamicData>();

    private int numComputers = 26;

    void Start()
    {
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData()
    {
        while (true)
        {
            for (int i = 1; i <= numComputers; i++) // Itera a través de las computadoras remotas
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get("http://172.24.100.110:8080/data/" + i))
                {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        string jsonData = webRequest.downloadHandler.text;

                        // Parsea los datos JSON a un objeto ComputerDynamicData
                        ComputerDynamicData computerData = JsonUtility.FromJson<ComputerDynamicData>(jsonData);

                        // Actualiza los TextMeshPro correspondientes
                        cpuTextMeshes[i - 1].text = "Uso actual CPU: " + computerData.CPUUsage;
                        ramTextMeshes[i - 1].text = "Uso actual RAM: " + computerData.RAMUsage;
                        diskTextMeshes[i - 1].text = "Uso actual Disco: " + computerData.DiskUsage;

                        // Agrega los datos a la lista
                        if (i <= computerDataList.Count)
                        {
                            computerDataList[i - 1] = computerData;
                        }
                        else
                        {
                            computerDataList.Add(computerData);
                        }
                    }
                    else
                    {
                        Debug.LogError("Error al obtener datos para la computadora " + i + ": " + webRequest.error);
                    }
                }
            }

            yield return new WaitForSeconds(5f);
        }
    }
}
