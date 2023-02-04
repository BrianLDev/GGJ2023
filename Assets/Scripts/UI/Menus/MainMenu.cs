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

    private Button _startButtonRef;
    private Button _exitGameButtonRef;

    void OnEnable()
    {
        _startButtonRef.Focus();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        _startButtonRef = _root.Query<Button>(_startButtonID);
        _exitGameButtonRef = _root.Query<Button>(_exitGameButtonID);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        // register action when each button is clicked
        _startButtonRef?.RegisterCallback<ClickEvent>(StartGame);
        _exitGameButtonRef?.RegisterCallback<ClickEvent>(ExitGame);
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void StartGame(ClickEvent clickEvent)
    {
        Debug.Log("MainMenu: StartGame called, loading scene 'Test'");
        SceneManager.LoadScene("Test");
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ExitGame(ClickEvent clickEvent)
    {
        Debug.Log("MainMenu: ExitGame called");
        Application.Quit();
    }
}
