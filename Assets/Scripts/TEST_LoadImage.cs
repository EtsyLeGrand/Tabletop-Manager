using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TEST_LoadImage : MonoBehaviour
{
    private string path;
    private Texture2D texture;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        path = StandaloneFileBrowser.OpenFilePanel("Open File", Application.persistentDataPath, "", false)[0];

        byte[] fileData = System.IO.File.ReadAllBytes(path);

        texture = new Texture2D(2000, 2000);

        texture.LoadImage(fileData);

        float aspectRatio = (float)texture.width / texture.height;

        // Adjust the texture size to match the aspect ratio
        int newWidth = Mathf.RoundToInt(2000 * aspectRatio);
        int newHeight = Mathf.RoundToInt(2000);

        // Resize the texture
        texture.Reinitialize(newWidth, newHeight);

        // Recreate mip maps
        texture.Apply(true);

        // Create a sprite using the loaded texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, newWidth, newHeight), new Vector2(0.5f, 0.5f));

        img.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
