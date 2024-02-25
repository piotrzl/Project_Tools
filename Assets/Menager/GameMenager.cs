using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMenager : SingletonMenager<GameMenager>
{
    public Action<bool> PauseGameEvent;

    public bool GameIsPause => gameIsPause;

    bool gameIsPause = false;

    public void PauseGame() 
    {
        gameIsPause = !gameIsPause;

        if (gameIsPause) 
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1f;
        }

        PauseGameEvent?.Invoke(gameIsPause);
    }
}
