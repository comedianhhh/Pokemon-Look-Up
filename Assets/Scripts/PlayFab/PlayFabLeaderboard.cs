using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLeaderboard : MonoBehaviour
{
    public TMPro.TMP_InputField HighScoreText;

    [UnityEngine.Serialization.FormerlySerializedAs("PostLeaderboard")]
    public Button PostLeaderboardButton;
    public Button GetLeaderboardButton;

    void Start()
    {
        PostLeaderboardButton.enabled = false;
        GetLeaderboardButton.enabled = false;
        HighScoreText.enabled = false;
    }

    private void FixedUpdate()
    {
        if (PlayFabManager.Instance.state == PlayFabManager.LoginState.Success)
        {
            PostLeaderboardButton.enabled = true;
            GetLeaderboardButton.enabled = true;
            HighScoreText.enabled = true;
        }
        else
        {
            PostLeaderboardButton.enabled = false;
            GetLeaderboardButton.enabled = false;
            HighScoreText.enabled = false;
        }
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        Debug.LogError($"PlayFab Error: {obj.ErrorMessage}");
    }


    #region Post High Score via API
    public void PostHighScore()
    {
        int score = -1;
        if (int.TryParse(HighScoreText.text, out score) == false)
        {
            Debug.LogWarning($"HighScoreText has an Invalid Number: {HighScoreText.text}");
            return;
        }

        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate { StatisticName = "high_score", Value = score }
                }
            },
            OnUpdatePlayerStatistic,
            OnPlayFabError
        );
    }

    private void OnUpdatePlayerStatistic(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Update Player Statistic Success");
    }
    #endregion


    #region Get Leaderboard via API
    public void GetLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest
            {
                StatisticName = "high_score",
                StartPosition = 0,
                MaxResultsCount = 5
            },
            OnGetLeaderboardCallback,
            OnPlayFabError
        );
    }

    private void OnGetLeaderboardCallback(GetLeaderboardResult obj)
    {
        string leaderboardName = (obj.Request as GetLeaderboardRequest).StatisticName;
        Debug.Log($"Get Leaderboard success: {leaderboardName}");

        foreach (PlayerLeaderboardEntry playerDetails in obj.Leaderboard)
        {
            Debug.Log($"Player {playerDetails.DisplayName} has a rank of {playerDetails.Position} with a score of {playerDetails.StatValue}");
        }
    }
    #endregion


    #region Get Current Player Statistic

    [System.Serializable]
    public class PlayerHighScore
    {
        public int high_score;
    }

    public void GetPlayerHighScore()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest
            {
                StatisticNames = new List<string>() { "high_score" }
            },
            success =>
            {
                foreach (var stat in success.Statistics)
                {
                    if (stat.StatisticName == "high_score")
                    {
                        Debug.Log($"Player high score: {stat.Value}");
                    }
                }
            },
            OnPlayfabError
        );
    }

    private void OnPlayfabError(PlayFabError error)
    {
        Debug.Log("Error on stat update/get");
    }


    #endregion


    ///////////////////////////////////////////////////////////////////
    /// Local update ends here
    /// 
    ///
    /// Cloud script version begins here
    ///////////////////////////////////////////////////////////////////


    // Note: because Playfab requires an Azure server to do cloud scripting,
    // we will not pursue this lesson at this time as this may incur a 
    // financial cost to the students
    //
    // Below is an example of cloud scripting

    public void PostHighScoreViaCloudScript()
    {
        int score = -1;
        if (int.TryParse(HighScoreText.text, out score) == false)
        {
            Debug.LogWarning($"HighScoreText has an Invalid Number: {HighScoreText.text}");
            return;
        }

        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "updatePlayerStat",
                FunctionParameter = new { high_score = score },
            },
            result =>
            {
                if (result != null && result.FunctionResult != null)
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.LogError($"Post high score error");
                }
            },
            OnPlayFabError
        );
    }

    // below is what should be on the server 
    /*
    handlers.updatePlayerStat = function(args, context)
    {

        var player_daily_leaderboard_Request = {
        PlayFabId: currentPlayerId, 
        Statistics:
        [
            {
        StatisticName: "high_score",
              Value: args.high_score
            }
	     // Add more here for more updates
        ]
    };
    server.UpdatePlayerStatistics(player_daily_leaderboard_Request);    

    return 1;
}; 
    */



}

