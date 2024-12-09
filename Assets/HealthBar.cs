using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        Debug.Log($"HealthBar ustawiony na max: {health}");
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        Debug.Log($"HealthBar aktualne zdrowie: {health}");
    }

}