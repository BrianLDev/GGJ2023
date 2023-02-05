using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public abstract class Menu : MonoBehaviour
{
    [Header("UI Management")]
    [Tooltip("Set the Main Menu here explicitly (or get automatically from current GameObject).")]
    [SerializeField] protected MainMenuUIController mainMenuUIController;

    [Tooltip("Set the UI Document here explicitly (or get automatically from current GameObject).")]
    [SerializeField] protected UIDocument _document;

    protected VisualElement _root;


    protected virtual void Awake()
    {
        // set up MainMenuUIManager and UI Document
        if (mainMenuUIController == null)
            mainMenuUIController = GetComponent<MainMenuUIController>();

        // default to current UIDocument if not set in Inspector
        if (_document == null)
            _document = GetComponent<UIDocument>();

        // alternately falls back to the MainMenu UI Document
        if (_document == null && mainMenuUIController != null)
            _document = mainMenuUIController.MainMenuDocument;

        if (_document == null)
        {
            Debug.LogWarning($"MainMenu.cs: Document is null");
            return;
        }

        else
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }
    }

    
    /// <summary>
    /// The general workflow uses string IDs to query the VisualTreeAsset and find matching Visual Elements in the UXML.
    /// Customize this for each MenuScreen subclass to identify any functional Visual Elements (buttons, controls, etc.).
    /// </summary>
    protected virtual void SetVisualElements()
    {
        // get a reference to the root VisualElement 
        if (_document != null)
            _root = _document.rootVisualElement;
    }

    /// <summary>
    /// Once visual elements are set, register callbacks for any buttons.
    /// </summary>
    protected virtual void RegisterButtonCallbacks()
    {

    }
}
