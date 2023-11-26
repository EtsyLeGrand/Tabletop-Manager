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

    // Fill Menu Items on load
    private void OnEnable()
    {
        FillMenuItems();
    }

    private void FillMenuItems()
    {
        foreach (FileManager.ImageInfo imageInfo in FileManager.Instance.AllImageInfos)
        {
            GameObject item = Instantiate(mapMenuItemPrefab, mapMenuItems);
            item.GetComponent<MapMenuItemFiller>().FillMenuItem(
                imageInfo.path, imageInfo.texture, imageInfo.originalSize, imageInfo.fileName, imageInfo.mapSettings.password);
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

    public void OnSaveAllButtonPressed()
    {
        foreach(MapMenuItemFiller menuItem in mapMenuItems.GetComponentsInChildren<MapMenuItemFiller>())
        {
            FileManager.Instance.SaveMap(menuItem.ImagePath, menuItem.PasswordField.text);
        }
    }
}
