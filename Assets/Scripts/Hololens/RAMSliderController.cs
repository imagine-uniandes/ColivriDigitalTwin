using UnityEngine;
using UnityEngine.UI;


public class RAMSliderController : MonoBehaviour
{
    public Slider ramSlider;

    void Update()
    {
        // Accede al valor actual del slider
        float ramValue = ramSlider.value;
        Debug.Log("slider " + ramValue);
        // Utiliza ramValue en tu lógica para controlar algo, como filtrar objetos según el valor del slider.
    }
}
