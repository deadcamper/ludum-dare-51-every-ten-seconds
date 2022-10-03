using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStateLoader : MonoBehaviour
{
    public LevelState levelState;

    // Start is called before the first frame update
    void Awake()
    {
        GameStateTracker.GetInstance().SetLevelState(levelState);
        gameObject.SetActive(false);
    }
}
