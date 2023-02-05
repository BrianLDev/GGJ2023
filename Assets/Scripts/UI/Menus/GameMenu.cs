using EcxUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameMenu : MonoBehaviour
{
    private const string _resumeButtonID = "resume__button";
    private const string _restartButtonID = "restart__button";
    private const string _continueButtonID = "continue__button";
    private const string _exitMissionButtonID = "exitMission__button";
    private const string _continueDialogueButtonID = "continueDialogue__button";
    private const string _playerDialogueLabelID = "playerDialogue__label";

    private const string _playerShieldBarID = "playerShieldBar";
    private const string _playerHealthBarID = "playerHealthBar";
    private const string _playerAmmoBarID = "playerAmmoBar";
    private const float _barSize = 100;

    private UIDocument _gameDocument;
    private VisualElement _root;
    private VisualElement _pauseMenuRef;
    private VisualElement _gameWonMenuRef;
    private VisualElement _gameLostMenuRef;
    private VisualElement _playerDialogueComponentRef;
    private VisualElement _playerHealthBarRef;
    private VisualElement _playerShieldBarRef;
    private VisualElement _playerAmmoBarRef;

    private Button _resumeButtonRef;
    private Button _restartButtonRef;
    private Button _continueButtonRef;
    private Button _exitMissionButtonRef;
    private Button _continueDialogueButtonRef;

    private Label _playerDialogueLabelRef;

    private bool _isGameOver;


    [Header("Game Menus")] 
    [Tooltip("String IDs to query Visual Elements")] 
    [SerializeField]
    string PauseMenuID = "PauseMenu";

    [SerializeField]
    string GameWonMenuID = "GameWonMenu";

    [SerializeField]
    string GameLostMenuID = "GameLostMenu";

    [SerializeField]
    string PlayerDialogueComponentID = "PlayerDialogue";

    [Header("Blur")] 
    [Tooltip("Volume used to blur")] 
    [SerializeField]
    Volume Volume;

    public static event Action GamePaused;
    public static event Action GameResumed;
    public static event Action GameRestarted;
    public static event Action GameWon;
    public static event Action GameLost;

    void OnEnable()
    {
        SetVisualElements();
        RegisterButtonCallbacks();
        InitializePlayerBars();

        if (Volume == null)
            Volume = FindObjectOfType<Volume>();

        BlurBackground(false);

    }

    ///// <summary>
    ///// On frame update, check for key presses
    ///// </summary>
    //void Update()
    //{
    //    if (Input.GetKeyDown("escape") || Input.GetKeyDown("p"))
    //    {
    //        Debug.Log("GameMenu.cs: Escape key pressed");
    //        ShowPauseMenu();
    //    }

    //    if (Input.GetKeyDown("m"))
    //    {
    //        Debug.Log("GameMenu.cs: M Pressed");
    //        ShowPlayerDialogue("What happened... is that an alien");
    //    }

    //    if (Input.GetKeyDown("k"))
    //    {
    //        Debug.Log("GameMenu.cs: K Pressed");
    //        ReduceHealth(10);

    //    }

    //    if (Input.GetKeyDown("l"))
    //    {
    //        Debug.Log("GameMenu.cs: L Pressed");
    //        GainHealth(10);
    //    }

    //    if (Input.GetKeyDown("o"))
    //    {
    //        Debug.Log("GameMenu.cs: K Pressed");
    //        ReduceAmmo(10);

    //    }
    //}

    /// <summary>
    /// The general workflow uses string IDs to query the VisualTreeAsset and find matching Visual Elements in the UXML.
    /// Customize this for each MenuScreen subclass to identify any functional Visual Elements (buttons, controls, etc.).
    /// </summary>
    void SetVisualElements()
    {
        _gameDocument = GetComponent<UIDocument>();
        _root = _gameDocument.rootVisualElement;

        _pauseMenuRef = _root.Query(PauseMenuID);
        _gameWonMenuRef = _root.Query(GameWonMenuID);
        _gameLostMenuRef = _root.Query(GameLostMenuID);
        _playerDialogueComponentRef = _root.Query(PlayerDialogueComponentID);
        _playerShieldBarRef = _root.Query<VisualElement>(_playerShieldBarID);
        _playerHealthBarRef = _root.Query<VisualElement>(_playerHealthBarID);
        _playerAmmoBarRef = _root.Query<VisualElement>(_playerAmmoBarID);

        _resumeButtonRef = _root.Query<Button>(_resumeButtonID);
        _restartButtonRef = _root.Query<Button>(_restartButtonID);
        _continueButtonRef = _root.Query<Button>(_continueButtonID);
        _exitMissionButtonRef = _root.Query<Button>(_exitMissionButtonID);
        _continueDialogueButtonRef = _root.Query<Button>(_continueDialogueButtonID);

        _playerDialogueLabelRef = _root.Query<Label>(_playerDialogueLabelID);


    }

    /// <summary>
    /// Once visual elements are set, register callbacks for any buttons.
    /// </summary>
    void RegisterButtonCallbacks()
    {
        _resumeButtonRef?.RegisterCallback<ClickEvent>(ResumeGame);
        _continueButtonRef?.RegisterCallback<ClickEvent>(ContinueGame);
        _restartButtonRef?.RegisterCallback<ClickEvent>(RestartGame);
        _exitMissionButtonRef?.RegisterCallback<ClickEvent>(ExitMission);
        _continueDialogueButtonRef?.RegisterCallback<ClickEvent>(ContinueDialogue);
    }

    /// <summary>
    /// Initializes the player bars
    /// </summary>
    /// <remarks>Shield is hidden and width is set to the bar size constant</remarks>
    void InitializePlayerBars()
    {
        _playerShieldBarRef.style.width = new StyleLength(new Length(0, LengthUnit.Pixel));
        _playerHealthBarRef.style.width = new StyleLength(new Length(_barSize, LengthUnit.Pixel));
        _playerAmmoBarRef.style.width = new StyleLength(new Length(_barSize, LengthUnit.Pixel));
    }

    /// <summary>
    /// Increases the health bar
    /// </summary>
    /// <param name="healthReceived">The health received</param>
    public void GainHealth(float healthReceived)
    {
        Debug.Log($"GameMenu.cs: Health gained {healthReceived}");
        float healthBarValue = _playerHealthBarRef.style.width.value.value;

        if ((healthBarValue + healthReceived) >= _barSize)
        {
            _playerHealthBarRef.style.width = new StyleLength(new Length(_barSize, LengthUnit.Pixel));
            return;
        }

        _playerHealthBarRef.style.width = new StyleLength(new Length(healthBarValue + healthReceived, LengthUnit.Pixel));
    }

    /// <summary>
    /// Increases the shield bar
    /// </summary>
    /// <param name="shieldGained">The shield gained</param>
    public void GainShield(float shieldGained)
    {
        Debug.Log($"GameMenu.cs: Shield gained {shieldGained}");
        float shieldBarValue = _playerShieldBarRef.style.width.value.value;

        if ((shieldBarValue + shieldGained) >= _barSize)
        {
            _playerShieldBarRef.style.width = new StyleLength(new Length(_barSize, LengthUnit.Pixel));
            return;
        }

        _playerShieldBarRef.style.width = new StyleLength(new Length(shieldBarValue + shieldGained, LengthUnit.Pixel));
    }

    /// <summary>
    /// Reduces the health bar and shield bar as needed
    /// </summary>
    /// <param name="damageTaken">The damage taken</param>
    /// <remarks>If a shield is active, the shield is reduced instead of the health.
    /// Once the health reaches 0, the game over menu is shown.
    /// </remarks>
    public void ReduceHealth(float damageTaken)
    {
        Debug.Log($"GameMenu.cs: Health reduced {damageTaken}");
        float shieldBarValue = _playerShieldBarRef.style.width.value.value;
        float healthBarValue  = _playerHealthBarRef.style.width.value.value;

        VisualElement barToReduceRef = _playerHealthBarRef;
        float barValueToCompare = healthBarValue;

        if (healthBarValue <= 0)
        {
            ShowGameLostMenu();
            return;
        }

        if (shieldBarValue > 0)
        {
            barToReduceRef = _playerShieldBarRef;
            barValueToCompare = shieldBarValue;
        }

        if ((barValueToCompare - damageTaken) <= 0)
        {
            barToReduceRef.style.width = new StyleLength(new Length(0, LengthUnit.Pixel));
            return;
        }

        barToReduceRef.style.width = new StyleLength(new Length(barValueToCompare - damageTaken, LengthUnit.Pixel));

    }

    
    /// <summary>
    /// Increases the ammo bar
    /// </summary>
    /// <param name="ammo">The ammo received</param>
    public void GainAmmo(float ammo)
    {
        Debug.Log($"GameMenu.cs: Ammo gained {ammo}");
        float ammoBarValue = _playerAmmoBarRef.style.width.value.value;

        if ((ammoBarValue + ammo) >= _barSize)
        {
            _playerAmmoBarRef.style.width = new StyleLength(new Length(_barSize, LengthUnit.Pixel));
            return;
        }

        _playerAmmoBarRef.style.width = new StyleLength(new Length(ammoBarValue + ammo, LengthUnit.Pixel));
    }

    /// <summary>
    /// Reduces the ammo bar
    /// </summary>
    /// <param name="ammoExpended">The ammo expended</param>
    public void ReduceAmmo(float ammoExpended)
    {
        Debug.Log($"GameMenu.cs: Ammo expended {ammoExpended}");
        float ammoBarValue = _playerAmmoBarRef.style.width.value.value;


        if ((ammoBarValue - ammoExpended) <= 0)
        {
            _playerAmmoBarRef.style.width = new StyleLength(new Length(0, LengthUnit.Pixel));
            return;
        }

        _playerAmmoBarRef.style.width = new StyleLength(new Length(ammoBarValue - ammoExpended, LengthUnit.Pixel));
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
    /// Shows Game Won Menu
    /// </summary>
    void ShowGameWonMenu()
    {
        Debug.Log("GameMenu.cs: Game Won");
        GameWon?.Invoke();

        ShowVisualElement(_gameWonMenuRef, true);
        BlurBackground(true);
    }

    /// <summary>
    /// Shows Game Lost Menu
    /// </summary>
    void ShowGameLostMenu()
    {
        Debug.Log("GameMenu.cs: Game Won");
        GameLost?.Invoke();

        ShowVisualElement(_gameLostMenuRef, true);
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

    public void ShowPlayerDialogue(string dialogueText)
    {
        Debug.Log($"GameMenu.cs: Showing dialogue {dialogueText}");

        _playerDialogueLabelRef.text = dialogueText;
        ShowVisualElement(_playerDialogueComponentRef, true);
    }

    void ContinueDialogue(ClickEvent clickEvent)
    {
        Debug.Log("GameMenu.cs: Dialogue continued");
        // Add continue logic

        ShowVisualElement(_playerDialogueComponentRef, false);
    }

    /// <summary>
    /// Continues Game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ContinueGame(ClickEvent clickEvent)
    {
        Debug.Log("GameMenu.cs: Game Continued");
        // Add continue logic

        ShowVisualElement(_gameWonMenuRef, false);
        BlurBackground(false);
    }

    /// <summary>
    /// Restarts Game
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void RestartGame(ClickEvent clickEvent)
    {
        Debug.Log("GameMenu.cs: Game Continued");
        // Add restart logic

        ShowVisualElement(_gameLostMenuRef, false);
        BlurBackground(false);
    }

    /// <summary>
    /// Exits Mission
    /// </summary>
    /// <param name="clickEvent">The click event object</param>
    void ExitMission(ClickEvent clickEvent)
    {
        SceneManager.LoadScene("MainMenu");
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
