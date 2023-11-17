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

        // Définir le chemin du fichier de sauvegarde dans le dossier persistentDataPath
        saveFilePath = Path.Combine(Application.persistentDataPath, "ImageData.json");

        DontDestroyOnLoad(gameObject);

        // Charger les données au démarrage de l'application
        LoadData();
    }

    public void SaveData()
    {
        // Convertir la liste d'informations d'image en format JSON
        string jsonData = JsonUtility.ToJson(mapSettings);

        // Écrire les données JSON dans le fichier
        File.WriteAllText(saveFilePath, jsonData);
    }

    public void LoadData()
    {
        // Vérifier si le fichier existe
        if (File.Exists(saveFilePath))
        {
            // Lire les données JSON du fichier
            string jsonData = File.ReadAllText(saveFilePath);

            // Convertir les données JSON en liste d'informations d'image
            mapSettings = JsonUtility.FromJson<List<MapSettings>>(jsonData);
        }
    }
}
