using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component to talk to the actual GameState, to make resets easier.
/// </summary>
public class GameStateTracker : MonoBehaviour
{
    [SerializeField]
    private GameState initialGameState;

    private GameState currentState;
    private LevelState currentLevelState;

    public static GameStateTracker GetInstance()
    {
        return GameRunner.GetInstance().gameStateComponent;
    }

    public void Awake()
    {
        ResetState();
    }

    public void ResetState()
    {
        if (currentState != null)
        {
            Destroy(currentState);
        }
        currentState = Instantiate(initialGameState);
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public LevelState GetLevelState()
    {
        return currentLevelState;
    }

    public void SetLevelState(LevelState template)
    {
        if (currentLevelState)
            Destroy(currentLevelState);

        if(template != null)
        {
            currentLevelState = Instantiate(template);
            DontDestroyOnLoad(currentLevelState); // Cleanup should be handled internally here
        }
        else
        {
            currentLevelState = null;
        }
    }
}
