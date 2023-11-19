using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    private static FileManager instance;

    [SerializeField] private string saveFolderPath;
    [SerializeField] private string[] supportedImageExtensions;
    [SerializeField] private string[] supportedMusicExtensions;

    private string fullSavePath;

    private List<ImageInfo> allImageInfos = new();
    private List<MusicInfo> allMusicPaths = new();

    private struct ImageInfo
    {
        public Texture2D texture;
        public Vector2Int originalSize;
        public string path;
        public string fileName;
    }

    private struct MusicInfo
    {
        public AudioClip audioClip;
        public string path;
        public string fileName;
    }

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fullSavePath = Path.Combine(Application.persistentDataPath, saveFolderPath);

        if (!Directory.Exists(fullSavePath)) Directory.CreateDirectory(fullSavePath);

        ListAllData();
    }

    private async void ListAllData()
    {
        string[] allFilesInFolder = Directory.GetFiles(fullSavePath);

        foreach (string file in allFilesInFolder)
        {
            string extension = Path.GetExtension(file.ToLower());

            // Images
            if (supportedImageExtensions.Contains(extension))
            {
                byte[] imageData = File.ReadAllBytes(file);
                Texture2D txtr = new(1, 1);
                txtr.LoadImage(imageData);

                ImageInfo info = new()
                {
                    originalSize = new Vector2Int(txtr.width, txtr.height),
                    texture = txtr,
                    path = file,
                    fileName = Path.GetFileNameWithoutExtension(file)
                };

                allImageInfos.Add(info);
            }

            // Music
            else if (supportedMusicExtensions.Contains(extension))
            {
                AudioClip clip = await LoadClip(file);

                MusicInfo info = new()
                {
                    audioClip = clip,
                    path = file,
                    fileName = Path.GetFileNameWithoutExtension(file)
                };
            }

            // MapSettings
            else if (extension == ".json" && Path.GetFileNameWithoutExtension(file).Contains("mapsettings"))
            {
                // Handle MapSettings
            }

            // Invalid extensions
            else
            {
                // Handle Invalid extensions
            }
        }
    }

    async Task<AudioClip> LoadClip(string path)
    {
        AudioClip clip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);
                
                if (uwr.result == UnityWebRequest.Result.ConnectionError || 
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                    Debug.Log($"{uwr.error}");
                else 
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return clip;
    }
}
