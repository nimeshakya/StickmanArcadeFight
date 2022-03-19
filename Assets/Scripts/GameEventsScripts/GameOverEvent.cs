using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEvent : MonoBehaviour
{
    private GameTimeEvent gameTimeEvent;
    private EnemyController enemyController;
    private PlayerController playerController;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text winLoseDrawText;

    private bool gameOver;
    public bool _isGameOver { get { return gameOver; } }
    private bool playerWin;
    private bool enemyWin;

    private void Awake()
    {
        gameTimeEvent = GetComponent<GameTimeEvent>();
        enemyController = GameObject.FindWithTag("Enemy").GetComponent<EnemyController>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        gameOver = false;
        playerWin = false;
        enemyWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        ActionIfTimeIsUp();
        GameStatusWhenDeath();
        ActionWhenGameOverByDeath();
    }

    void ActionIfTimeIsUp()
    {
        if(gameTimeEvent.currentGameTime <= 0 && (!playerWin && !enemyWin))
        {
            gameOver = true;
            winLoseDrawText.text = "Draw!";
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over! Draw");
        }
    }

    // check who is dead and if game is over by death
    void GameStatusWhenDeath()
    {
        if (enemyController._isDead)
        {
            gameOver = true;
            playerWin = true;
            enemyWin = false;
        } else if (playerController._isDead)
        {
            gameOver = true;
            playerWin = false;
            enemyWin = true;
        }
    }

    // what to do when game is over by death
    void ActionWhenGameOverByDeath()
    {
        if(gameOver && (playerWin || enemyWin))
        {
            if (playerWin)
            {
                winLoseDrawText.text = "Player 1 Wins!";
                gameOverPanel.SetActive(true);
            }
            else
            {
                winLoseDrawText.text = "Player 2 Wins!";
                gameOverPanel.SetActive(true);
            }
        }
    }
}
