using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StorytimeUI : MonoBehaviour
{
    public GameObject storyCardVisibility;
    public Image storyCardImage;
    public GameObject storyTimeVisibility;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;

    public float textPerSecond = 1 / 30f;

    private LevelState lastLevelSetup;

    private GameStateTracker gameState;

    private List<StoryTimeCard> cards = new List<StoryTimeCard>();

    private Coroutine storyRoutine;

    private bool playerInterrupted = false;

    public UnityEvent onCompleteStory;

    private void Start()
    {
        gameState = GameStateTracker.GetInstance();

        storyCardVisibility.SetActive(false);
        storyTimeVisibility.SetActive(false);
    }

    private void Update()
    {
        CheckForStoryTime();
        CheckForPlayerInterrupt();
    }

    private void CheckForStoryTime()
    {
        GameState gs = gameState.GetGameState();

        // HACK - level state keeps getting confused on transition, so kill it mid-transition.
        if (gs.betweenLevels)
        {
            if (storyRoutine != null)
            {
                StopCoroutine(storyRoutine);
                storyRoutine = null;
            }
            storyTimeVisibility.SetActive(false);
            return;
        }
            
        LevelState ls = gameState.GetLevelState();

        if (ls != lastLevelSetup)
        {
            // Then we setup
            LevelState currentLevelSetup = ls;

            // Clean up old story
            cards.Clear();
            if (storyRoutine != null)
            {
                StopCoroutine(storyRoutine);
                storyRoutine = null;
            }

            storyTimeVisibility.SetActive(false);

            if (currentLevelSetup != null && currentLevelSetup.usesStoryTime)
            {
                cards.AddRange(currentLevelSetup.storyTimeCards);
                storyCardVisibility.SetActive(ls.usesStoryCardBackgrounds);
                storyRoutine = StartCoroutine(DoStoryTime());
            }

            lastLevelSetup = currentLevelSetup;
        }
    }

    private void CheckForPlayerInterrupt()
    {
        playerInterrupted |= Input.anyKeyDown;
    }

    IEnumerator DoStoryTime()
    {
        playerInterrupted = false;

        characterNameText.text = "";
        dialogueText.text = "";
        storyTimeVisibility.SetActive(true);
        storyCardImage.sprite = null;

        storyCardImage.color = new Color(1,1,1,0);

        while (cards.Count > 0)
        {
            StoryTimeCard card = cards[0];

            if (card.storyCardSprite && storyCardImage.sprite != card.storyCardSprite)
            {
                
                if (storyCardImage.sprite != null)
                {
                    // Fade out
                    for (float a = 1; a > 0; a-=0.25f)
                    {
                        storyCardImage.color = new Color(1, 1, 1, a);
                        yield return new WaitForSeconds(textPerSecond);
                    }
                }

                storyCardImage.color = new Color(1, 1, 1, 0);
                storyCardImage.sprite = card.storyCardSprite;

                // Fade in
                for (float a = 0; a < 1; a += 0.25f)
                {
                    storyCardImage.color = new Color(1, 1, 1, a);
                    yield return new WaitForSeconds(textPerSecond);
                }
            }

            characterNameText.text = card.characterName;
            characterNameText.color = card.characterNameColor;
            dialogueText.text = "";

            yield return new WaitForSeconds(textPerSecond);
            playerInterrupted = false;

            for (int n=0; n<card.text.Length;n++)
            {
                dialogueText.text = card.text.Substring(0,n);
                yield return new WaitForSeconds(textPerSecond);

                if (playerInterrupted)
                {
                    playerInterrupted = false;
                    break;
                }
            }
            dialogueText.text = card.text;

            cards.RemoveAt(0);

            yield return new WaitUntil(() => { return playerInterrupted; });
            playerInterrupted = false;
        }

        onCompleteStory.Invoke();

        LevelState ls = gameState.GetLevelState();
        if (ls.winsOnStoryTimeCompletion)
        {
            GameRunner.GetInstance().NotifyLevelWinCondition();
        }
    }
}
