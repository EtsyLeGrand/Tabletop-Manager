using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    private static ImageManager instance;

    public SpriteRenderer spriteRenderer;
    public FileManager fileManager;

    public static ImageManager Instance => instance;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
        private void Start()
        {
            // Charger les données au démarrage
            fileManager.LoadData();

            // Afficher la première image si elle existe
            DisplayImage();
        }

    public void LoadImage(string imagePath)
    {
        // Créer une nouvelle instance de MapSettings
        MapSettings newMapSettings = ScriptableObject.CreateInstance<MapSettings>();
        newMapSettings.filePath = imagePath;
        newMapSettings.fileName = Path.GetFileNameWithoutExtension(imagePath);

        // Ajouter l'information de la carte à la liste
        fileManager.mapSettings.Add(newMapSettings);

        // Enregistrer les données
        fileManager.SaveData();

        DontDestroyOnLoad(gameObject);

        // Afficher la carte
        DisplayImage();
    }

    private void DisplayImage()
    {
        // Afficher la première image si elle existe
        if (fileManager.mapSettings.Count > 0)
        {
            string imagePath = fileManager.mapSettings[0].filePath;
            Texture2D texture = LoadTexture(imagePath);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sprite;
        }
    }

    private Texture2D LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return texture;
    }
}
