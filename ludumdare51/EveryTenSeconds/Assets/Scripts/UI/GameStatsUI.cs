using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    private GameStateTracker gameState;

    public GameObject gameStatsCanvas;

    public TMP_Text goldText;

    private int lastCoin = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameStateTracker.GetInstance();

        goldText.text = lastCoin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GameState gs = gameState.GetGameState();
        LevelState ls = gameState.GetLevelState();

        if (ls == null)
        {
            if (!gameStatsCanvas.activeSelf)
                gameStatsCanvas.SetActive(true);
        }
        else if (ls.hidesGameStats == gameStatsCanvas.activeSelf)
        {
            gameStatsCanvas.SetActive(!ls.hidesGameStats);
        }

        if (lastCoin != gs.coin)
        {
            int diff = gs.coin - lastCoin;
            int add = diff / Mathf.Abs(diff);
            lastCoin += add;

            goldText.text = lastCoin.ToString(); // might be too fast if frame by frame.
        }
    }
}
