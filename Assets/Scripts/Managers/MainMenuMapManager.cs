using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMapManager : MonoBehaviour
{
    private static MainMenuMapManager instance;

    [SerializeField] private GameObject mapMenuItemPrefab;
    [SerializeField] private Transform mapMenuItems;

    public static MainMenuMapManager Instance => instance;

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
        FileManager.Instance.onListAllDataComplete += FillMenuItems;
        
    }

    private void FillMenuItems()
    {
        foreach (FileManager.ImageInfo imageInfo in FileManager.Instance.AllImageInfos)
        {
            GameObject item = Instantiate(mapMenuItemPrefab, mapMenuItems);
            item.GetComponent<MapMenuItemFiller>().FillMenuItem(imageInfo.texture, imageInfo.originalSize, imageInfo.fileName);
        }
    }

    public void OnPreviousButtonPressed()
    {
        MainMenuUIManager.Instance.EnableCanvasGroup(MainMenuUIManager.Instance.CanvasGroup);
        MainMenuUIManager.Instance.DisableCanvasGroup(gameObject.GetComponent<CanvasGroup>());
    }

    public void OnUploadButtonPressed()
    {
        foreach (Transform child in mapMenuItems)
        {
            Destroy(child.gameObject);
        }

        mapMenuItems.DetachChildren();

        FileManager.Instance.UploadNewMaps();
    }
}
