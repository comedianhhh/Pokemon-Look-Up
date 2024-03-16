using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    public enum LoginState
    {
        Startup,
        Instantiated,
        Success,
        Failed
    };
    public LoginState state = LoginState.Startup;

    public string PlayerGUID = "";
    public bool CreateNewPlayer = false;

    private void Awake()
    {
        PlayerGUID = PlayerPrefs.GetString("PlayFabPlayerId", "");
        if (string.IsNullOrEmpty(PlayerGUID) || CreateNewPlayer == true)
        {
            CreateNewPlayer = false;
            PlayerGUID = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayFabPlayerId", PlayerGUID);
        }
    }

    private void Start()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = PlayerGUID,
            CreateAccount = true
        };
        //LoginWithCustomIDRequest request = new LoginWithCustomIDRequest {
        //    CustomId = SystemInfo.deviceUniqueIdentifier,
        //    CreateAccount = true 
        //};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        state = LoginState.Failed;

        Debug.LogWarning("Something went wrong logging into PlayFab :(");
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        state = LoginState.Success;
        Debug.Log("Congratulations, you have logged into PlayFab");
    }
}

