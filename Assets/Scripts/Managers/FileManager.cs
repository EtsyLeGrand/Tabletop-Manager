using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    private static FileManager instance;

    private string saveFilePath;
    public List<MapSettings> mapSettings = new List<MapSettings>();

    // Getters / setters
    public static FileManager Instance => instance;

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

        // D�finir le chemin du fichier de sauvegarde dans le dossier persistentDataPath
        saveFilePath = Path.Combine(Application.persistentDataPath, "ImageData.json");

        DontDestroyOnLoad(gameObject);

        // Charger les donn�es au d�marrage de l'application
        LoadData();
    }

    public void SaveData()
    {
        // Convertir la liste d'informations d'image en format JSON
        string jsonData = JsonUtility.ToJson(mapSettings);

        // �crire les donn�es JSON dans le fichier
        File.WriteAllText(saveFilePath, jsonData);
    }

    public void LoadData()
    {
        // V�rifier si le fichier existe
        if (File.Exists(saveFilePath))
        {
            // Lire les donn�es JSON du fichier
            string jsonData = File.ReadAllText(saveFilePath);

            // Convertir les donn�es JSON en liste d'informations d'image
            mapSettings = JsonUtility.FromJson<List<MapSettings>>(jsonData);
        }
    }
}
