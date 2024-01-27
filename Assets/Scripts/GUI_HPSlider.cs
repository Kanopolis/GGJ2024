using UnityEngine;
using UnityEngine.UI;

public class GUI_HPSlider : MonoBehaviour
{
    [SerializeField]
    private Slider m_Slider;

    private static GUI_HPSlider instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public static void SetupSlider(float _maxValue)
    {
        if (instance.m_Slider.maxValue != _maxValue)
            instance.m_Slider.maxValue = _maxValue;
    }

    public static void SetNewValue(float _value)
    {
        if (instance.m_Slider.value != _value)
            instance.m_Slider.value = _value;
    }
}
