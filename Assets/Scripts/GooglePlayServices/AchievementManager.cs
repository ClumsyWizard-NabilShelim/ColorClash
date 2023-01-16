using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public static class AchievementManager
{
    public static void UnlockAchievement(string id)
    {
        if (!GPGSManager.IsPlayerAuthenticated)
            return;

        Social.ReportProgress(id, 100.0f, (bool success) => {});
    }
    public static void IncrementAchievement(string id, int amount)
    {
        if (!GPGSManager.IsPlayerAuthenticated)
            return;

        PlayGamesPlatform.Instance.IncrementAchievement(id, amount, (bool success) =>{});
    }
}
