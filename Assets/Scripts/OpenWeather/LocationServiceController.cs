using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class LocationServiceController : Singleton<LocationServiceController>
{
    public bool isSearching = false;
    public float longitude;
    public float latitude;

    public TMP_Text latitudeText;
    public TMP_Text longitudeText;


    public System.Action OnLocationUpdateCallBack;
    private void Start()
    {
        latitudeText.text = "";
        longitudeText.text = "";
        GetLocation();
    }
    

    void GetLocation()
    {
        if(isSearching==false)
        {
            StartCoroutine(UpdateLoaction());
        }
    }

    private IEnumerator UpdateLoaction()
    {
        Input.location.Start();

        int waitTime = 20;
        while(Input.location.status== LocationServiceStatus.Initializing && waitTime > 0)
        {
            yield return new WaitForSeconds(1);
            waitTime--;
        }

        if(waitTime<=0)
        {
            latitudeText.text = "Time Out";
            isSearching = false;
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed||
            Input.location.status == LocationServiceStatus.Stopped)
        {
            latitudeText.text = "Failed to determin device location";
            isSearching = false;
            yield break;
        }

        latitude=Input.location.lastData.latitude;
        longitude=Input.location.lastData.longitude;

        latitudeText.text = $"Latitude:{latitude}";
        longitudeText.text = $"Longitude:{longitude}";

        Input.location.Stop();
        isSearching = false;

        if(OnLocationUpdateCallBack!=null)
        {
            OnLocationUpdateCallBack();
        }

    }
}
