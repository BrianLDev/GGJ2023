/*
ECX UTILITY SCRIPTS
Game Manager (Singleton)
Last updated: September 24, 2020
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcxUtilities {
  public class GameManager : Singleton<GameManager> {
    public enum GameState { MainMenu, Game, Paused, GameOver }
    private GameState currentState;
    public GameState CurrentState => currentState;
    private GameState prevState;
    private MainMenu mainMenu;
    private GameMenu gameMenu;

    private void Awake() {
      // subscribe to OnSceneLoaded event
      SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnEnable() {
      mainMenu = FindObjectOfType<MainMenu>();
      gameMenu = FindObjectOfType<GameMenu>();
    }

    private void SetState(GameState state) {
      prevState = currentState;
      currentState = state;
    }

    public void MainMenu() {
      Debug.Log("GameManager: Exiting to main menu.");
      SetState(GameState.MainMenu);
      SceneManager.LoadScene(0);
    }

    // assumes starting/restarting at level 1
    public void StartGame() {
      SetState(GameState.Game);
      SceneManager.LoadScene(1);
    }

    // specify which level to start
    public void StartGame(int level) {
      SetState(GameState.Game);
      SceneManager.LoadScene(level);
    }

    // note - Player.cs calls pause toggle since it receives Input messages
    public void PauseToggle() {
      if (currentState == GameState.Game) {
        SetState(GameState.Paused);
        gameMenu?.ShowPauseMenu();
      }
      else if (currentState == GameState.Paused) {
        SetState(GameState.Game);
        gameMenu?.ResumeGame();
      }
    }

    public void GameOver() {
      SetState(GameState.GameOver);
      gameMenu.ShowGameLostMenu();
    }

    public void Victory() {
      SetState(GameState.GameOver);
      gameMenu.ShowGameWonMenu();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (scene.buildIndex == 0)
        SetState(GameState.MainMenu);
      // note - all scenes except the 1st are assumed to be in game.  Game over screen should be UI only.
      else if (scene.buildIndex > 0)
        SetState(GameState.Game);
    }

    private void OnDestroy() {
      SceneManager.sceneLoaded -= OnSceneLoaded;  // unsubscribe to OnSceneLoaded event
    }

  }
}
