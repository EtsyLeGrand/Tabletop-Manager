using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject mapObject;

    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Image lockButtonImage;
    [SerializeField] private TMP_InputField sizeInputField;
    [SerializeField] private MapPasswordInputField mapPasswordInputField;
    [SerializeField] private Button[] buttonArray; // Tous les boutons sauf lock

    [SerializeField] private Button cursorMouseModeButton;
    [SerializeField] private Button panMouseModeButton;

    [SerializeField] private Color buttonEnabledColor;
    [SerializeField] private Color buttonDisabledColor;

    private MouseMode mouseMode = MouseMode.Cursor;

    private bool isLocked = false;
    private bool areKeyShortcutsEnabled = true;
    private int currentRotation = 0;
    private float scrollSpeed = 0.0025f;
    private Vector3 originalScale = Vector3.one;

    private Action onLockButtonClicked;
    private Action<MouseMode> onMouseModeChanged;
    private Action<int> onRotationChanged;

    public enum MouseMode
    {
        Cursor = 0,
        Pan = 1
    }


    // Getters / setters
    public static UIManager Instance => instance;
    public MapPasswordInputField MapPasswordInputField => mapPasswordInputField;

    public Action OnLockButtonClicked { get => onLockButtonClicked; set => onLockButtonClicked = value; }
    public Action<MouseMode> OnMouseModeChanged { get => onMouseModeChanged; set => onMouseModeChanged = value; }
    public Action<int> OnRotationChanged { get => onRotationChanged; set => onRotationChanged = value; }

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

    void Start()
    {
        originalScale = transform.localScale; // � changer quand on pourra loader des maps
        cursorMouseModeButton.interactable = false; // Mouse mode cursor
    }

    void Update()
    {
        // Hide UI
        if (Input.GetKeyDown(KeyCode.H) && areKeyShortcutsEnabled)
        {
            canvasObject.SetActive(!canvasObject.activeSelf);
        }

        // Lock Map
        if (Input.GetKeyDown(KeyCode.L) && areKeyShortcutsEnabled)
        {
            OnMapLockButtonClicked();
        }

        // Open Password Field
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.M))
        {
            OnMapPasswordButtonClicked();
        }

        if (!isLocked)
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            sizeSlider.value += scrollDelta * scrollSpeed;
            sizeSlider.value = Mathf.Clamp(sizeSlider.value, sizeSlider.minValue, sizeSlider.maxValue);
        }
    }

    private void ToggleKeyShortcuts(bool toggle)
    {
        areKeyShortcutsEnabled = toggle;
    }

    // Public functions
    // Buttons
    public void RotateObject(int angle)
    {
        if (!isLocked) 
        {
            mapObject.transform.Rotate(new Vector3(0, 0, angle));
            currentRotation = (int)mapObject.transform.eulerAngles.z;

            onRotationChanged?.Invoke(currentRotation);
        }
    }

    public void FlipObjectHorizontal()
    {
        if (!isLocked)
        {
            Vector3 currentScale = mapObject.transform.localScale;
            currentScale.x *= -1;
            mapObject.transform.localScale = currentScale;
        } 
    }

    public void FlipObjectVertical()
    {
        if (!isLocked)
        {
            Vector3 currentScale = mapObject.transform.localScale;
            currentScale.y *= -1;
            mapObject.transform.localScale = currentScale;
        }
    }

    public void OnMapSizeSliderChanged()
    {
        mapObject.transform.localScale = sizeSlider.value * originalScale;
    }

    public void OnMapLockButtonClicked()
    {
        isLocked = !isLocked;

        foreach (Button btn in buttonArray) btn.interactable = !isLocked;

        sizeSlider.interactable = !isLocked;
        sizeInputField.interactable = !isLocked;

        // MouseMode
        if (!isLocked) 
        {
            switch (mouseMode)
            {
                case MouseMode.Cursor: OnCursorModeButtonClicked(); break;
                case MouseMode.Pan: OnPanModeButtonClicked(); break;
            }
        }

        onLockButtonClicked?.Invoke();
    }

    public void OnCursorModeButtonClicked()
    {
        mouseMode = MouseMode.Cursor;
        cursorMouseModeButton.interactable = false;
        panMouseModeButton.interactable = true;

        onMouseModeChanged?.Invoke(mouseMode);
    }

    public void OnPanModeButtonClicked()
    {
        mouseMode = MouseMode.Pan;
        panMouseModeButton.interactable = false;
        cursorMouseModeButton.interactable = true;

        onMouseModeChanged?.Invoke(mouseMode);
    }

    public void OnCenterImageButtonClicked()
    {
        mapObject.transform.position = Vector3.zero;
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnMapPasswordButtonClicked()
    {
        OnMapLockButtonClicked();
        mapPasswordInputField.gameObject.SetActive(!mapPasswordInputField.gameObject.activeInHierarchy);
        ToggleKeyShortcuts(!areKeyShortcutsEnabled);
        mapPasswordInputField.InitField();
    }

    public void OnMusicPasswordButtonClicked()
    {

    }
}
