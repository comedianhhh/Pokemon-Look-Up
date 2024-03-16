using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabUserSettings : MonoBehaviour
{
    public Button UpdateUsernameButton;
    public TMPro.TMP_InputField UsernameText;

    void Start()
    {
        UpdateUsernameButton.enabled = false;
        UsernameText.enabled = false;
    }

    private void FixedUpdate()
    {
        if (PlayFabManager.Instance.state == PlayFabManager.LoginState.Success)
        {
            UpdateUsernameButton.enabled = true;
            UsernameText.enabled = true;
        }
    }

    public void UpdateUserName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = UsernameText.text,
        },
        result =>
        {
            Debug.Log("Username was changed");
        },
        error =>
        {
            Debug.Log("Failed to update username");
        }
        );
    }
}
