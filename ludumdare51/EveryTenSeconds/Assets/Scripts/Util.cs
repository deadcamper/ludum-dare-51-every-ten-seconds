using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Transform FetchPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Could not find any player. Uh oh...");
            return null;
        }

        return playerObj.transform;
    }

    public static AudioSource FetchMusicPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("MusicPlayer");
        if (playerObj == null)
        {
            Debug.LogError("Could not find any music player. Uh oh...");
            return null;
        }

        AudioSource musicPlayer = playerObj.GetComponent<AudioSource>();

        if (musicPlayer == null)
        {
            Debug.LogError("Could not find the AudioSource component on the music player. Oops!");
            return null;
        }

        return musicPlayer;
    }
}
