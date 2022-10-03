using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelSceneLoader : MonoBehaviour
{
    public int startingTier = 1;

    [FormerlySerializedAs("levelsByName")]
    public List<string> tierOneLevelsByName;
    public List<string> tierTwoLevelsByName;
    public List<string> tierThreeLevelsByName;

    public string endScene = "EndScene";

    public bool randomMode = false;

    private UnityEngine.SceneManagement.Scene currentLevelScene;

    private int levelIndex = -1;
    private string lastLevelName;

    private List<string> currentLevels = new List<string>();
    private int currentTier;


    /// <summary>
    /// Tracks and loads level scenes, as needed
    /// </summary>
    void Start()
    {
        if (startingTier < 1 || startingTier > 3)
        {
            Debug.LogError(string.Format("Invalid tier selected for LevelSceneLoader ({0}). Loader will have no levels.", startingTier));
        }
        currentTier = startingTier-1;

        FishForNextLevels();
    }

    bool FishForNextLevels()
    {
        currentTier++;

        List<string> nextLevels;
        switch (currentTier)
        {
            case 1:
                nextLevels = tierOneLevelsByName;
                break;
            case 2:
                nextLevels = tierTwoLevelsByName;
                break;
            case 3:
                nextLevels = tierThreeLevelsByName;
                break;
            default:
                return false;
        }

        currentLevels.AddRange(nextLevels);
        return currentLevels.Count > 0;
    }

    public void RemoveCurrentSceneFromLoadList()
    {
        bool indexShift = currentLevels.Remove(currentLevelScene.name);
        if (indexShift)
            levelIndex--;
    }

    public string UnloadCurrentScene()
    {
        string currentLevelName = currentLevelScene.name;
        if (currentLevelScene.isLoaded && currentLevelScene.IsValid())
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentLevelScene);
            lastLevelName = currentLevelName;

            return lastLevelName;
        }

        return null;
    }

    public string LoadNextScene()
    {
        if (currentLevelScene.isLoaded && currentLevelScene.IsValid())
        {
            UnloadCurrentScene();
        }

        if (currentLevels.Count == 0)
        {
            FishForNextLevels(); // Try fishing for the next tier
        }

        string nextLevelName;

        if (currentLevels.Count == 0)
        {
            LoadEndGame();
            return null;
        }
        else if(currentLevels.Count == 1)
        {
            nextLevelName = currentLevels[0];
        }
        else if(randomMode)
        {
            do
            {
                // Pick a scene by random, must not be the same place
                int levelIndex = Random.Range(0, currentLevels.Count);
                nextLevelName = currentLevels[levelIndex];
            } while (nextLevelName == lastLevelName);
        }
        else
        {
            levelIndex = (levelIndex + 1) % currentLevels.Count;
            nextLevelName = currentLevels[levelIndex];
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        currentLevelScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(nextLevelName);

        return nextLevelName;
    }

    public void LoadEndGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(endScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
