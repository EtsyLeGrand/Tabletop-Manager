using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    private static MainMenuUIManager instance;

    private CanvasGroup canvasGroup;

    [SerializeField] private MainMenuMapManager mapManager;

    public static MainMenuUIManager Instance => instance;
    public CanvasGroup CanvasGroup => canvasGroup;
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

        if (!TryGetComponent(out canvasGroup)) Debug.LogError("No canvas group was found!");
    }

    public void OnLaunchPlayerScreen()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void OnOpenMapMenu()
    {
        DisableCanvasGroup(canvasGroup);
        EnableCanvasGroup(mapManager.gameObject.GetComponent<CanvasGroup>());
    }

    public void OnOpenMusicMenu()
    {
        //DisableCanvasGroup(canvasGroup);
    }

    public void OnExitApp()
    {
        Application.Quit();
    }

    public void DisableCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void EnableCanvasGroup(CanvasGroup cg)
    {
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
}
