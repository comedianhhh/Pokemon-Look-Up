using System;

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Net;

using System.IO;


public class WeatherController : MonoBehaviour
{
    [Serializable]

    public class Weather
    {
        public int id;
        public string main;
        public string description;
    }

    [Serializable]

    public class WeatherMain
    {
        public float temp;
    }

    [Serializable]

    public class WeatherInfo
    {
        public int id;
        public string name;
        public WeatherMain main;
        public List<Weather> weather;
    }

    public float UpdateTimeDelay = 10.0f;
    private float CurrentTimeDelay = 0.0f;

    public string API_KEY = "";
    public string CityId = "Oakville,CA";

    public TMP_Text CityText;
    public TMP_Text WeatherText;
    public TMP_Text TemperatureText;

    private WeatherInfo GetWeather()
    {
        string url="http://api.openweathermap.org/data/2.5/weather?q="+CityId+"&appid="+API_KEY;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());

        string jsonResponse = reader.ReadToEnd();
        return JsonUtility.FromJson<WeatherInfo>(jsonResponse);
    }

    private void Start()
    {
        WeatherInfo weatherInfo = GetWeather();

        Debug.Log($"{weatherInfo.name}'s current weather is {weatherInfo.weather[0].main} and + ${weatherInfo.weather[0].description}, with a temperature of {weatherInfo.main.temp-273.15f}");

        CityText.text = $"City: {weatherInfo.name}";
        WeatherText.text = $"{weatherInfo.weather[0].main} and {weatherInfo.weather[0].description}";
        TemperatureText.text = $"Temperature: {weatherInfo.main.temp-273.15f} ";

        CurrentTimeDelay=UpdateTimeDelay;
    }
}
