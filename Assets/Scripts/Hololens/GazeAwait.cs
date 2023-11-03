using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeAwait : MonoBehaviour


{
    public GameObject RamInfo;
    public GameObject CpuInfo;
    public GameObject DiskInfo;
    public float delayBeforeHide = 2.0f;

    // Start is called before the first frame update

    public void hideRam()
    {
        if (RamInfo != null)
        {
            StartCoroutine(HideAfterDelayRam(RamInfo, delayBeforeHide));
        }
    }

    private IEnumerator HideAfterDelayRam(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        RamInfo.SetActive(false);
    }

    public void hideCPU()
    {
        if (RamInfo != null)
        {
            StartCoroutine(HideAfterDelayCPU(CpuInfo, delayBeforeHide));
        }
    }

    private IEnumerator HideAfterDelayCPU(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        CpuInfo.SetActive(false);
    }

    public void hideDisk()
    {
        if (RamInfo != null)
        {
            StartCoroutine(HideAfterDelayDisk(DiskInfo, delayBeforeHide));
        }
    }

    private IEnumerator HideAfterDelayDisk(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DiskInfo.SetActive(false);
    }

}
