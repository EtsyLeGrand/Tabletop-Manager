using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMapManager : MonoBehaviour
{
    private static MainMenuMapManager instance;

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

    public void OnPreviousButtonPressed()
    {
        MainMenuUIManager.Instance.EnableCanvasGroup(MainMenuUIManager.Instance.CanvasGroup);
        MainMenuUIManager.Instance.DisableCanvasGroup(gameObject.GetComponent<CanvasGroup>());
    }
}
