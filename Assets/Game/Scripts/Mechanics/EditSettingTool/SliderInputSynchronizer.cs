using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderInputSynchronizer : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public Slider sensitivitySlider;

    [Range(0.2f, 2f)]
    [HideInInspector]public float value = 1f;

    private void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        nameInputField.onEndEdit.AddListener(OnInputChanged);
    }

    private void OnSensitivityChanged(float value)
    {
        this.value = Mathf.Clamp(value * 1.8f + 0.2f, 0.2f, 2f);
        nameInputField.text = this.value.ToString("F2");
    }

    private void OnInputChanged(string text)
    {
        if(float.TryParse(text, out float parsedValue))
        {
            value = Mathf.Clamp(parsedValue, 0.2f, 2f);
            sensitivitySlider.value = (value - 0.2f) / 1.8f;
        }
        else
        {
            nameInputField.text = value.ToString("F2");
        }
    }
}
