using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Application = UnityEngine.Application;

public class MainMenu : Menu
{
    private const string _startButtonID = "start__button";
    private const string _exitGameButtonID = "exit__button";
    private const string _creditsButtonID = "credits__button";
    private const string _creditsBackButtonID = "creditsBack__button";

    private VisualElement _creditsMenuRef;
    private VisualElement _mainMenuRef;

    private Button _startButtonRef;
    private Button _exitGameButtonRef;
    private Button _creditsButtonRef;
    private Button _creditsBackButtonRef;

    [Header("Game Menus")]
    [Tooltip("String IDs to query Visual Elements")]
    [SerializeField]
    string CreditsMenuID = "CreditsMenu";

    [SerializeField]
    string MainMenuID = "MainMenu";



    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        _creditsMenuRef = _root.Query<VisualElement>(CreditsMenuID);
        _mainMenuRef = _root.Query<VisualElement>(MainMenuID);

        _startButtonRef = _root.Query<Button>(_startButtonID);
        _exitGameButtonRef = _root.Query<Button>(_exitGameButtonID);
        _creditsButtonRef = _root.Query<Button>(_creditsButtonID);
        _creditsBackButtonRef = _root.Query<Button>(_creditsBackButtonID);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        // register action when each button is clicked
        _startButtonRef?.RegisterCallback<ClickEvent>(StartGame);
        _exitGameButtonRef?.RegisterCallback<ClickEvent>(ExitGame);
        _creditsButtonRef?.RegisterCallback<ClickEvent>(ShowCredits);
        _creditsBackButtonRef?.RegisterCallback<ClickEvent>(HideCredits);
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void StartGame(ClickEvent clickEvent)
    {
        Debug.Log("MainMenu.cs: StartGame called, loading scene 'Test'");
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ExitGame(ClickEvent clickEvent)
    {
        Debug.Log("MainMenu.cs: ExitGame called");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>
    /// Shows the credits
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ShowCredits(ClickEvent clickEvent)
    {
        Debug.Log("MainMenu.cs: Showing credits");
        _creditsMenuRef.style.display = DisplayStyle.Flex;
        _mainMenuRef.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Shows the credits
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void HideCredits(ClickEvent clickEvent)
    {
        Debug.Log("MainMenu.cs: Showing credits");
        _creditsMenuRef.style.display = DisplayStyle.None;
        _mainMenuRef.style.display = DisplayStyle.Flex;
    }
}
