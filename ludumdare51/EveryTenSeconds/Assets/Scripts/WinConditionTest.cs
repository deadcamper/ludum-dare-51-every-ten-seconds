using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionTest : MonoBehaviour
{
    public int numberToCompareAgainst = 3;

    private int currentCount = 0;

    public void AddToWinCondition(int count)
    {
        currentCount += count;
        if (currentCount >= numberToCompareAgainst)
        {
            TriggerWinCondition();
        }
    }

    public void TriggerWinCondition()
    {
        GameRunner.GetInstance().NotifyLevelWinCondition();
    }
}
