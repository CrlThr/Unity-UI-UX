using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTemp : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            sliderText.text = v.ToString("0");
            sliderText.text += " °C";
        });
    }

}
