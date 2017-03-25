using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int startLife = 5;  // Default value is 10

    // Score integers
    private int _player_score_1;    // Current player 1 score
    private int _player_score_2;    // Current player 2 score

    private bool _GameOver;


    #endregion

    public bool GameOver { get { return _GameOver; } }

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
        _player_score_1 = startLife;
        _player_score_2 = startLife;

        Time.timeScale = 1f;

        _GameOver = false;
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
                _player_score_1 -= scoreAmount;
                break;
            case (2):
                _player_score_2 -= scoreAmount;
                break;
            default:
                // There is no player!!
                break;
        }

        // Check if we are out of live or not
        if (_player_score_1 <= 0)
        {
            // Player 2 has won
            GameManager.gameManager.GameOver(2);
        }
        else if (_player_score_2 <= 0)
        {
            // Player 1 has won 
            GameManager.gameManager.GameOver(1);
        }
    }

}
