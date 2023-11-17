using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject mapObject;

    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Image lockButtonImage;
    [SerializeField] private TMP_InputField sizeInputField;
    [SerializeField] private Button[] buttonArray; // Tous les boutons sauf lock

    [SerializeField] private Button cursorMouseModeButton;
    [SerializeField] private Button panMouseModeButton;

    [SerializeField] private Color buttonEnabledColor;
    [SerializeField] private Color buttonDisabledColor;

    private MouseMode mouseMode = MouseMode.Cursor;

    private bool isLocked = false;
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

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale; // À changer quand on pourra loader des maps
        cursorMouseModeButton.interactable = false; // Mouse mode cursor
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            canvasObject.SetActive(!canvasObject.activeSelf);
        } // Hide UI

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnMapLockButtonClicked();
        } // Lock Map

        if (!isLocked)
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            sizeSlider.value += scrollDelta * scrollSpeed;
            sizeSlider.value = Mathf.Clamp(sizeSlider.value, sizeSlider.minValue, sizeSlider.maxValue);
        }
    }

    // Public functions
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
}
