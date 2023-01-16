using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using TMPro;
using ClumsyWizard.Utilities;

public class GPGSManager : Persistant<GPGSManager>
{
    public static bool IsPlayerAuthenticated { get; private set; }

    [SerializeField] private GameObject inputBlockPanel;

    [SerializeField] private GameObject manualSignInButton;
    [SerializeField] private GameObject leaderBoardButton;
    [SerializeField] private GameObject achievementButton;

    protected override void Awake()
    {
        base.Awake();
        ToggleSignedInButtons(false);
        inputBlockPanel.SetActive(true);
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

        SceneManagement.OnNewSceneLoadedCore += () =>
        {
            if(!SceneManagement.isMainMenu)
            {
                manualSignInButton.SetActive(false);
                leaderBoardButton.SetActive(false);
                achievementButton.SetActive(false);
            }
            else
            {
                if(IsPlayerAuthenticated)
                    ToggleSignedInButtons(true);
                else
                    ToggleSignedInButtons(false);
            }
        };
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        inputBlockPanel.SetActive(false);
        if(status == SignInStatus.Success)
        {
            Debug.Log("Successfully Authenticated");
            Debug.Log($"Hello {Social.localUser.userName}. You have an ID of: {Social.localUser.id}");
            IsPlayerAuthenticated = true;
            ToggleSignedInButtons(true);
        }
        else
        {
            Debug.Log("Failed to Authenticate");
            Debug.Log($"Failed to authenticate user. Reason: {status}");
            IsPlayerAuthenticated = false;
            ToggleSignedInButtons(false);
        }
    }

    public void ManualSignIn()
    {
        if (IsPlayerAuthenticated)
            return;

        inputBlockPanel.SetActive(true);
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    public void ShowAchievements()
    {
        if (!IsPlayerAuthenticated)
            return;

        Social.ShowAchievementsUI();
    }
    public void ShowLeaderBoard()
    {
        if (!IsPlayerAuthenticated)
            return;

        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highscore);
    }

    public static void PostToLeaderBoard(int amount)
    {
        if (!IsPlayerAuthenticated)
            return;

        Social.ReportScore(amount, GPGSIds.leaderboard_highscore, (bool success) => {});
    }

    private void ToggleSignedInButtons(bool active)
    {
        manualSignInButton.SetActive(!active);
        leaderBoardButton.SetActive(active);
        achievementButton.SetActive(active);
    }

    protected override void CleanUp()
    {
    }
}
