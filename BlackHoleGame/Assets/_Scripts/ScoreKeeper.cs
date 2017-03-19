using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be a simple score keeping between the players.
/// When one player gets sucked into a hole/trap, the other player gets \
/// a point
/// </summary>
public class ScoreKeeper : MonoBehaviour
{
    #region Fields

    // The static reference
    public static ScoreKeeper _current_score;

    // The winnign score
    [SerializeField]
    private int winningScore = 10;  // Default value is 10

    // Score integers
    private int _player_score_1;    // Current player 1 score
    private int _player_score_2;    // Current player 2 score

    // UI elements
    public UnityEngine.UI.Text score_text_1;    // Player 1 score text
    public UnityEngine.UI.Text score_text_2;    // Player 2 score text
    public UnityEngine.UI.Text winningText;     // The winning text

    #endregion

    /// <summary>
    /// Make sure that there is only one of these
    /// on load, and initalize the score variables
    /// </summary>
    void Awake()
    {
        // Make sure that we only have one of these on load
        if (_current_score == null)
        {
            // Set the currenent static referece
            _current_score = this;
        }
        else if (_current_score != this)
            Destroy(gameObject);

        // Initalize the score variables
        _player_score_1 = 0;
        _player_score_2 = 0;

        // Set the UI elements
        SetScoreUI();
	}	

    /// <summary>
    /// Add the score amount to the specified player
    /// </summary>
    /// <param name="playerNum">Which player we are adding score to</param>
    /// <param name="scoreAmount">The amount of score that we want to add</param>
    public void AddScore(int playerNum, int scoreAmount)
    {
        // Add to the score
        switch (playerNum)
        {
            case (1):
                _player_score_2 += scoreAmount; 
                break;
            case (2):
                _player_score_1 += scoreAmount;
                break;
            default:
                // There is no player!!
                break;
        }

        // Check if we won 
        if(_player_score_1 >= winningScore)
        {
            // Set win text
            Win(1);
        }
        else if(_player_score_2 >= winningScore)
        {
            Win(2);
        }

        // Update the UI
        SetScoreUI();
    }

    /// <summary>
    /// Update the score texts based on the current score fields
    /// </summary>
    private void SetScoreUI()
    {
        // Set the player 1 score text
        score_text_1.text = _player_score_1.ToString();
        // Set the player 2 score text
        score_text_2.text = _player_score_2.ToString();
    }

    /// <summary>
    /// Set the win text element to the given player
    /// number
    /// </summary>
    /// <param name="winningPlayerNum">Which player won?</param>
    private void Win(int winningPlayerNum)
    {
        // Set the winning text
        winningText.text = "Player " + winningPlayerNum.ToString() + " Wins!";
    }
}
