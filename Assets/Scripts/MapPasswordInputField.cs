using System;
using System.Linq;
using UnityEngine;

public class MapPasswordInputField : PasswordInputField
{
    private Action<FileManager.ImageInfo> onSendNewMapData;

    public Action<FileManager.ImageInfo> OnSendNewMapData { get => onSendNewMapData; set => onSendNewMapData = value; }

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
            
            if (!string.IsNullOrEmpty(passwordValue) && !imageInfo.Equals(default(FileManager.ImageInfo)))
            {
                // Mot de passe trouvé
                onSendNewMapData?.Invoke(imageInfo);
            }
            else
            {
                Debug.Log("Non existant ou vide");
            }

            UIManager.Instance.OnMapPasswordButtonClicked(); // Cacher / Désactiver le field
        });
    }

    public void InitField()
    {
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

}
