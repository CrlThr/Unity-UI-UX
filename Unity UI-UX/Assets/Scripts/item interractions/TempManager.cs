using UnityEngine;

public class TemperatureManager : MonoBehaviour
{
    public static TemperatureManager Instance;

    [Range(0, 100)]
    public float temperature = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public float GetCookingMultiplier()
    {
        return temperature / 100f;
    }
}
