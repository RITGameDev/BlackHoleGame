using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This game manager will have a reference to anything that is moving
/// moving 
/// Author: Ben Hoffman
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager gameManager;  // Static reference to this object

    #region Fields
    private GameState currentGameState; // Current game state of the game

    public int numberOfPlayers; // The number of players in the game

    // UI elements
    public Text winningText;    // The winning text
    public Button resetButton;  // The reset button
    public Button quitButton;   // The quit button
    public Button startButton;  // The start button

    public UnityStandardAssets.ImageEffects.BlurOptimized camBlur;  // The camera blur effect for the menu

    public GameState CurrentGameState { get { return currentGameState; } }
    #endregion


    /// <summary>
    /// Set the game starte to the menu statemenu state,
    /// set the static reference to this game manager object
    /// so that other things can see the current state of the game.
    /// Set our menu UI up. Enable camera blur for the menu efffect
    /// </summary>
    void Awake ()
    {
        // Make sure tha thtis is the only one of these objects in the scene
        if (gameManager == null)
        {
            // Set the currenent referece
            gameManager = this;
        }
        // If there is something else in this scene that this script is already on, then destroy this
        else if (gameManager != this)
            Destroy(gameObject);

        // The game starts in the start menu
        currentGameState = GameState.StartMenu;

        // Show the start and quit buttons to begin
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        // Make sure that the camera blur is on for the start menu
        camBlur.enabled = true;

    }

    /// <summary>
    /// Select the start button at the beginnging
    /// </summary>
    void Start()
    {
        // Select the start button so that you can use the controller for UI
        startButton.Select();
    }

    /// <summary>
    /// Check for input from the player to resume the game
    /// </summary>
    void Update()
    {
        // If the player presses escape or the start button on the controller...
        if (Input.GetButtonDown("Cancel"))
        {
            // If we are paused, then resume the game
            if(currentGameState == GameState.Paused)
            {
                // Resume the game
                PlayGame();
            }
            // If we are playing, then pause the game
            else if(currentGameState == GameState.Playing)
            {
                // Pause the game
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Stop all things that need to move in scene
    /// </summary>
    private void PauseGame()
    {
        // The game state is not paused
        currentGameState = GameState.Paused;

        // Show the pause/ start menu UI
        resetButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        // Set the reset button as selected so that you can use the controller 
        resetButton.Select();

        // Enable camera blur
        camBlur.enabled = true;
    }

    /// <summary>
    /// This will change the game state to playing
    /// </summary>
    public void PlayGame()
    {
        // Unpause the game state
        currentGameState = GameState.Playing;

        // Get rid of the pause/ start menu UI
        startButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        // Disable camera blur
        camBlur.enabled = false;
    }

    /// <summary>
    /// Stop moving all the objects in the scene
    /// </summary>
    public void GameOver(int winningPlayer)
    {
        // Set the gamestate to game over
        currentGameState = GameState.GameOver;

        // Show the reset, main menu, and quit button, display who won
        // Set the winning text
        winningText.text = "Player " + winningPlayer.ToString() + " Wins!";

        // Show the pause/ start menu UI
        resetButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        // Set the reset button as selected so that you can use the controller 
        resetButton.Select();

        // Enable camera blur
        camBlur.enabled = true;
    }


    /// <summary>
    /// Quit out of the applicatoin, this will be called from a quit button 
    /// in the UI
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Reset the current level by reloading the level
    /// </summary>
    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);
    }

}
