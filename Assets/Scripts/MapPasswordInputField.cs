using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPasswordInputField : PasswordInputField
{
    private new void Awake()
    {
        base.Awake();
    }

    private new void Start()
    {
        base.Start();

        inputField.onSubmit.AddListener(passwordValue =>
        {
            FileManager.ImageInfo imageInfo = FileManager.Instance.AllImageInfos.FirstOrDefault(info => info.mapSettings.password == passwordValue);
            
            if (!imageInfo.Equals(default(FileManager.ImageInfo)))
            {
                // Mot de passe trouvé
            }
            else
            {
                Debug.Log("Non existant");
            }

            UIManager.Instance.OnMapPasswordButtonClicked(); // Re-désactiver
        });
    }

    public void InitField()
    {
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

}
