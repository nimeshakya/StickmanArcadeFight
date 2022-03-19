using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeEvent : MonoBehaviour
{
    [SerializeField] private Text gameTimerText;
    private GameOverEvent gameOverEvent;

    public int totalGameTime = 120;
    public int currentGameTime;
    private float passedTime;

    private void Awake()
    {
        gameOverEvent = GetComponent<GameOverEvent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameTime = totalGameTime;
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseGameTime();
    }

    void DecreaseGameTime()
    {
        if (!gameOverEvent._isGameOver)
        {
            passedTime += Time.deltaTime;
            if(passedTime >= 1)
            {
                currentGameTime = Mathf.Clamp(currentGameTime - 1, 0, totalGameTime);
                passedTime -= (int)passedTime;
                gameTimerText.text = currentGameTime.ToString();
                Debug.Log(currentGameTime);
            }
        }
    }
}
