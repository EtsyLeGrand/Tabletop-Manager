using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Settings", menuName = "Maps/MapSettings")]
public class MapSettings : ScriptableObject
{
    public string fileName;
    public string filePath;

    public void DisplayImage(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null)
        {
            // Charger la texture depuis le chemin du fichier
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            // Créer un sprite à partir de la texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // Afficher le sprite dans le SpriteRenderer
            spriteRenderer.sprite = sprite;
        }
    }
}
