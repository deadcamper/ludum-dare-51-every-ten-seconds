using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TvSnowUI : MonoBehaviour
{

    public GameObject tvSnow;
    public TMP_Text countDownText;

    private GameRunner gameRunner;

    // Start is called before the first frame update
    void Start()
    {
        gameRunner = GameRunner.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        GameState gs = gameRunner.gameStateComponent.GetGameState();

        if (gs.betweenLevels)
        {
            countDownText.text = "";
        }
        else
        {
            int countDownRoundUp = Mathf.CeilToInt(gameRunner.GetTimeBeforeNextTransition());
            countDownText.text = countDownRoundUp.ToString();
        }

        if (tvSnow.gameObject.activeSelf != gs.betweenLevels)
            tvSnow.gameObject.SetActive(gs.betweenLevels);
    }
}
