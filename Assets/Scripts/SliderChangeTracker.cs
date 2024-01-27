using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class SliderChangeTracker : MonoBehaviour
{
    [SerializeField]
    private VisualEffect m_VisualEffect;
    [SerializeField]
    private Slider m_Slider;

    private void Awake()
    {
        m_Slider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(OnSliderValueChanged));
    }

    private void OnSliderValueChanged(float _value)
    {
        m_VisualEffect.transform.position = m_Slider.handleRect.position;
        m_VisualEffect.Play();
    }
}
