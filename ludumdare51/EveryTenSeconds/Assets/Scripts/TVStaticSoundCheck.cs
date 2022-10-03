using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVStaticSoundCheck : MonoBehaviour
{
    public AudioSource tvStaticSound;

    private GameStateTracker gameState;

    private void Start()
    {
        gameState = GameStateTracker.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        GameState gs = gameState.GetGameState();

        if (tvStaticSound.isPlaying && !gs.betweenLevels)
        {
            tvStaticSound.Stop();
        }
        else if (!tvStaticSound.isPlaying && gs.betweenLevels)
        {
            tvStaticSound.Play();
        }
        
    }
}
