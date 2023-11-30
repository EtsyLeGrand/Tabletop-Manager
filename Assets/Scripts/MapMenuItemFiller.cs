using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuItemFiller : MonoBehaviour
{
    [SerializeField] private Image thumbnailImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text sizeText;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button loadButton;

    private string imagePath;

    public TMP_InputField PasswordField => passwordField;
    public string ImagePath => imagePath;

    public void SavePasswordButton()
    {
        string newPassword = passwordField.text;
        FileManager.Instance.SaveMap(imagePath, newPassword);
    }

    public void DeleteItemButton()
    {
        FileManager.Instance.DeleteMap(imagePath);
    }

    public void FillMenuItem(string path, Texture2D texture, Vector2Int size, string fileName, string password)
    {
        imagePath = path;
        thumbnailImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        titleText.text = fileName;
        sizeText.text = size.x + "x" + size.y;
        passwordField.text = password;
    }
}
