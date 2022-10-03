using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public float timeToTransition = 0.75f;
    //public float timeBetweenTransitions = 10;

    public GameStateTracker gameStateComponent;
    public LevelSceneLoader levelSceneLoader;

    public AudioSource victoryMusic;

    private Coroutine gameRoutine;

    private float timeLeftBeforeTransition;
    private static GameRunner _instance;

    private bool isCountingDown = true;

    public static GameRunner GetInstance()
    {
        return _instance;
    }

    public static void ResetInstance()
    {
        _instance = null;
    }

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                // Grandfathered instance found. Self-destruct!
                Destroy(this.gameObject);
            }
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameStateComponent = GameStateTracker.GetInstance();
        gameRoutine = StartCoroutine(DoGameLoop());

        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        GameState gs = gameStateComponent.GetState();

        if (Time.time > gs.nextTransitionTime)
        {
            string levelScene = levelSceneLoader.LoadNextScene();
            gs.UpdateNextSceneTransition();
            gs.currentLevel = levelScene;
        }
        */

        if (isCountingDown)
        {
            timeLeftBeforeTransition -= Time.deltaTime;
        }
    }

    IEnumerator DoGameLoop()
    {
        while (true) // TODO - ???
        {
            GameState gs = gameStateComponent.GetGameState();

            // Wait for the next level transition
            while (0 < timeLeftBeforeTransition)
            {
                yield return new WaitForEndOfFrame();
            }

            victoryMusic.Stop();

            // Begin level transition
            gs.betweenLevels = true;
            yield return new WaitForEndOfFrame();

            levelSceneLoader.UnloadCurrentScene();
            gameStateComponent.SetLevelState(null);

            yield return new WaitForSeconds(timeToTransition);

            if (gs.playerHearts <= 0)
            {
                gs.playerHearts = gs.playerTotalHearts;
            }
            string level = levelSceneLoader.LoadNextScene();
            gs.betweenLevels = false;
            BeginCountdownToNextTransition();
            // End level transition

            yield return new WaitForEndOfFrame();
        }
    }

    public void NotifyLevelWinCondition()
    {
        AudioSource source = Util.FetchMusicPlayer();
        source.Stop();

        timeLeftBeforeTransition = gameStateComponent.GetGameState().timeForVictory;

        levelSceneLoader.RemoveCurrentSceneFromLoadList();
        victoryMusic.Play();
    }

    public void BeginCountdownToNextTransition()
    {
        timeLeftBeforeTransition = gameStateComponent.GetGameState().timeBetweenTransitions;
    }

    public float GetTimeBeforeNextTransition()
    {
        return timeLeftBeforeTransition;
    }
}
