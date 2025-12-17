using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTemp : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    private void Start()
    {
        
        UpdateTemperature(slider.value);

        slider.onValueChanged.AddListener(UpdateTemperature);
    }

    void UpdateTemperature(float value)
    {
        sliderText.text = value.ToString("0") + " °C";

        if (TemperatureManager.Instance != null)
            TemperatureManager.Instance.temperature = value;
    }
}
