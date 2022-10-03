using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Exposed state object, just for tracking the state of the game.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "New GameState", menuName = "LudumDare51/Create Game State", order = 51)]
public class GameState : ScriptableObject
{

    //public const float TIME_BETWEEN_TRANSITIONS = 10; // ten seconds

    public float timeToTransition = 0.75f;
    public float timeBetweenTransitions = 10f;
    public float timeForVictory = 3f;
    //public float nextTransitionTime = 0f;
    public bool betweenLevels = false;

    public int playerTotalHearts; // Total Health
    public int playerHearts; // Health, in half-increments?
    public int playerArmor; // Armor, in half-increments
    public int playerLives;

    public int coin = 0;

    public string currentLevel;

}
