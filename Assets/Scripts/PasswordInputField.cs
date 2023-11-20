using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class PasswordInputField : MonoBehaviour
{
    private TMP_InputField inputField;
    private readonly int charLimit = 8;

    private void Awake()
    {
        if (!TryGetComponent(out inputField)) Debug.LogError("No input field was found!");
    }

    // Start is called before the first frame update
    void Start()
    {
        inputField.characterLimit = charLimit;

        // Validation
        inputField.onValueChanged.AddListener(changedValue =>
        {
            string validatedInput = "";
            foreach (char character in changedValue)
            {
                if (char.IsLetterOrDigit(character))
                {
                    validatedInput += character;
                }
            }

            inputField.text = validatedInput.ToUpper();
        });
    }
}
