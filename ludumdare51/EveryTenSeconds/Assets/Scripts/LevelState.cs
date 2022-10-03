using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StoryTimeCard
{
    public string characterName;
    public Color characterNameColor;
    [Multiline]
    public string text;

    public Sprite storyCardSprite;
}

[System.Serializable]
[CreateAssetMenu(fileName = "New LevelState", menuName = "LudumDare51/Create Level Setup State", order = 51)]
public class LevelState : ScriptableObject
{

    //public bool hidePlayer;
    public bool freezePlayer;

    public bool hidesGameStats;

    public bool usesStoryTime;
    public bool usesStoryCardBackgrounds;
    public bool canReleaseStoryTime;
    public bool winsOnStoryTimeCompletion;

    public StoryTimeCard[] storyTimeCards;

}
