using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This game manager will have a reference to anything that is moving
/// moving 
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

    #region Fields
    private GameState currentGameState;

    public int numberOfPlayers;

    // UI elements
    public Text winningText;     // The winning text
    public Button resetButton;
    public Button quitButton;
    public Button startButton;


    private BlackHoleManager blackHoleManager;
    public UnityStandardAssets.ImageEffects.BlurOptimized camBlur;

    public GameState CurrentGameState { get { return currentGameState; } }
    #endregion


    // I need to keep track of all things that are moving

    // Use this for initialization
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

        // Select the start button so that you can use the controller for UI
        startButton.Select();

        // Make sure that the camera blur is on for the start menu
        camBlur.enabled = true;

    }

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
        if (Input.GetButtonDown("Cancel"))
        {
            // If we are paused, then resume the game
            if(currentGameState == GameState.Paused)
            {
                PlayGame();
            }
            // If we are playing, then pause the game
            else if(currentGameState == GameState.Playing)
            {
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

        // Stop allowing players to aim and shoot

        // Stop the black hole lifetime counter

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
    /// Quit out of the applicatoin
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Reset the current level
    /// </summary>
    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);
    }

    /// <summary>
    /// This coroutine will slowly reduce the timescale
    /// so that the game ends it slows down
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowDownTime()
    {
        while (Time.timeScale > 0.3)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, Time.deltaTime);
            yield return null;
        }
        Time.timeScale = 0f;
    }

}
