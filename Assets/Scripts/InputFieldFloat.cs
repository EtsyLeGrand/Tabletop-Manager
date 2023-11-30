using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldFloat : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Slider linkedSlider;

    private string previousInputFieldValue;

    // Start is called before the first frame update
    void Start()
    {
        // Valeurs de d�part
        inputField.text = (linkedSlider.value * 100).ToString();
        previousInputFieldValue = inputField.text;

        // � la fin de l'�dition du InputField
        inputField.onEndEdit.AddListener((string input) =>
        {
            if (float.TryParse(input, out float result))
            {
                linkedSlider.value = result / 100;
                previousInputFieldValue = inputField.text;
                Debug.Log("Size percentage changed");
            }
            else
            {
                inputField.text = previousInputFieldValue;
                Debug.LogWarning("Size error");
            }
        });

        // � la fin de l'�dition du Slider
        linkedSlider.onValueChanged.AddListener((float value) =>
        {
            inputField.text = Math.Round((linkedSlider.value * 100), 1).ToString();
        });
        
    }
}
