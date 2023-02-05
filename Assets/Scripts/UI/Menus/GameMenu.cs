using EcxUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameMenu : MonoBehaviour
{
    private const string _resumeButtonID = "resume__button";
    private const string _exitButtonID = "exit__button";

    private UIDocument _gameDocument;
    private VisualElement _root;
    private VisualElement _pauseMenuRef;
    private Button _resumeButtonRef;
    private Button _exitButtonRef;

    private bool _isGameOver;


    [Header("Game Menus")] 
    [Tooltip("String IDs to query Visual Elements")] 
    [SerializeField]
    string PauseMenuID = "PauseScreen";

    [Header("Blur")] 
    [Tooltip("Volume used to blur")] 
    [SerializeField]
    Volume Volume;

    public static event Action GamePaused;
    public static event Action GameResumed;
    public static event Action GameRestarted;

    void OnEnable()
    {
        SetVisualElements();
        RegisterButtonCallbacks();

        if (Volume == null)
            Volume = FindObjectOfType<Volume>();

        BlurBackground(false);

    }

    /// <summary>
    /// On frame update, check for key presses
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("GameMenu.cs: Escape key pressed");
            ShowPauseMenu();
        }
    }

    /// <summary>
    /// The general workflow uses string IDs to query the VisualTreeAsset and find matching Visual Elements in the UXML.
    /// Customize this for each MenuScreen subclass to identify any functional Visual Elements (buttons, controls, etc.).
    /// </summary>
    void SetVisualElements()
    {
        _gameDocument = GetComponent<UIDocument>();
        _root = _gameDocument.rootVisualElement;

        _pauseMenuRef = _root.Query(PauseMenuID);
        _resumeButtonRef = _root.Query<Button>(_resumeButtonID);
        _exitButtonRef = _root.Query<Button>(_exitButtonID);
    }

    /// <summary>
    /// Once visual elements are set, register callbacks for any buttons.
    /// </summary>
    void RegisterButtonCallbacks()
    {
        _resumeButtonRef?.RegisterCallback<ClickEvent>(ResumeGame);
        _exitButtonRef?.RegisterCallback<ClickEvent>(ExitGame);
    }

    /// <summary>
    /// Shows a visual element
    /// </summary>
    /// <param name="visualElement">The element to be shown</param>
    /// <param name="state">True if the element is to be shown, False otherwise</param>
    void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
    }

    /// <summary>
    /// Shows Pause Menu
    /// </summary>
    void ShowPauseMenu()
    {
        Debug.Log("GameMenu.cs: Game Paused");
        GamePaused?.Invoke();

        ShowVisualElement(_pauseMenuRef, true);
        BlurBackground(true);
    }

    /// <summary>
    /// Resumes Game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ResumeGame(ClickEvent clickEvent)
    {
        Debug.Log("GameMenu.cs: Game Resumed");
        GameResumed?.Invoke();

        ShowVisualElement(_pauseMenuRef, false);
        BlurBackground(false);
    }

    /// <summary>
    /// Exits Game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ExitGame(ClickEvent clickEvent)
    {
        Application.Quit();
    }

    /// <summary>
    /// Blurs the background
    /// </summary>
    /// <param name="state">True if the background is to be blurred, False otherwise</param>
    /// <remarks>This was an attempt at using the global volume depth of field to apply a Bokeh blur but it didn't seem to work</remarks>
    void BlurBackground(bool state)
    {
        if (Volume == null)
            return;


        if (Volume.profile.TryGet(out DepthOfField blurDepthOfField))
        {
            blurDepthOfField.active = state;
        }

        if (Volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.active = state;
        }
    }

}
