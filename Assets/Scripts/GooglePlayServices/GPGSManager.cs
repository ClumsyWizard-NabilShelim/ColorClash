using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using TMPro;
using ClumsyWizard.Utilities;

public class LeaderBoardData
{
    public IUserProfile UserProfile { get; private set; }
    public int Rank { get; private set; }
    public string Score { get; private set; }

    public LeaderBoardData(IUserProfile userProfile, int rank, string score)
    {
        UserProfile = userProfile;
        Rank = rank;
        Score = score;
    }
}

public class GPGSManager : Persistant<GPGSManager>
{
    public static bool IsPlayerAuthenticated { get; private set; }

    [SerializeField] private GameObject inputBlockPanel;

    [SerializeField] private GameObject manualSignInButton;
    [SerializeField] private GameObject leaderBoardButton;
    [SerializeField] private GameObject achievementButton;

    [SerializeField] private LeaderBoardManager leaderBoard;
    private bool showingLeaderboard;

    protected override void Awake()
    {
        base.Awake();
        ToggleSignedInButtons(false);
        manualSignInButton.SetActive(false);
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
        if (!IsPlayerAuthenticated || showingLeaderboard)
            return;

        showingLeaderboard = true;
        //PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highscore);

        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_highscore, LeaderboardStart.TopScores, 20, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
        (data) =>
        {
            List<string> userIds = new List<string>();

            foreach (IScore score in data.Scores)
            {
                userIds.Add(score.userID);
            }

            PlayGamesPlatform.Instance.LoadUsers(userIds.ToArray(), (users) =>
            {
                StartCoroutine(LoadUsers(data, users));
            });
        });
    }

    private IEnumerator LoadUsers(LeaderboardScoreData data, IUserProfile[] users)
    {
        Dictionary<string, LeaderBoardData> leaderBoardDatas = new Dictionary<string, LeaderBoardData>();

        for (int i = 0; i < data.Scores.Length;)
        {
            IScore score = data.Scores[i];
            IUserProfile user = FindUser(score.userID, users);

            if (user == null)
                continue;

            while (user.image == null)
            {
                yield return null;
            }
            leaderBoardDatas.Add(user.id, new LeaderBoardData(user, score.rank, score.formattedValue));
            i++;
        }

        leaderBoard.Show(leaderBoardDatas);
        showingLeaderboard = false;
    }

    private IUserProfile FindUser(string id, IUserProfile[] users)
    {
        for (int i = 0; i < users.Length; i++)
        {
            if (users[i].id == id)
                return users[i];
        }

        return null;
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
