using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

/// <summary>
///  returns current location data of the device (coordinates in latitude, longitude and timestamp)
/// </summary>
public class LocationService : MonoBehaviour
{
    public double LAT;
    public double LON;
    public bool CHECKSUN;
    public DateTime sunrise;
    public DateTime sunset;
    public DateTime time;
    public District[] allDistricts;
    public District curDistrict;
    public int curDistrictNumber;
    Board board;
    public string City;
    public string Clouds;

    private const string APPID = "1fd19b4506a1e2fc4127a81babde32e9";
    public Text location;
    public Text level;
    public static int levelDifficulty;
    public int neededScore;


    /// <summary>
    /// sets up game board
    /// </summary>
    public void SetUpBoard()
    {
        GameObject boardObject = GameObject.Find("Board");
        board = boardObject.GetComponent<Board>();
    }

    /// <summary>
    /// gets device location (coordinates) if user permitted
    /// </summary>
    public void SendWeatherRequest()
    {
        SetDefaultCoords();
        SetUpBoard();
        curDistrictNumber = LevelSelection.districtNum;
        if (Matchmaking.role == "guest")
        {
            curDistrictNumber = Matchmaking.level;
        }
        DistrictArray.GetAllDistricts();

        curDistrict = DistrictArray.GetDistrict(curDistrictNumber);

        LAT = curDistrict.Latitude;
        LON = curDistrict.Longitude;

        GetWeatherData(curDistrict.Latitude, curDistrict.Longitude);
        SetLevelDifficulty();
        SetNeededScore();
        level.text = "LEVEL: " + levelDifficulty.ToString();
        level.color = SetLevelTextColor();
        var moves = 20;
        if(StartGame.activeUpgrade != null)
        {
            moves += StartGame.activeUpgrade.bonusMoves;
        }
        board.Setup(7, 7, moves, neededScore, levelDifficulty);
    } /// GetDeviceLocation method

    private Color32 SetLevelTextColor()
    {
        Color32 c = new Color32(255, 255, 255, 255);
        switch (levelDifficulty)
        {
            case 1:
                /// easy = green
                c = new Color32(20, 180, 80, 255);
                break;
            case 2:
                /// less easy = blue
                c = new Color32(66, 160, 222, 255);
                break;
            case 3:
                /// middle = purple
                c = new Color(166, 66, 222, 255);
                break;
            case 4:
                /// bit harder = orange
                c = new Color(222, 122, 70, 255);
                break;
            case 5:
                /// hard = red
                c = new Color(190, 30, 5, 255);
                break;
            default:
                break;
        }
        /// night with level 6 or default = white
        return c;
    }

    /// <summary>
    /// sends request to OpenWeatherMap API for weather data at device location
    /// </summary>
    private void GetWeatherData(double _lat, double _lon)
    {
        using (WebClient webClient = new WebClient())
        {
            /**
             * https://api.openweathermap.org/data/2.5/weather?lat=48.5113&lon=14.5048&APPID=1fd19b4506a1e2fc4127a81babde32e9
    */
            string url = string.Format("https://api.openweathermap.org/data/2.5/weather?lat=" 
                + _lat + "&lon=" + _lon + "&APPID=" + APPID);
            var json = webClient.DownloadString(url);
            var result = JSON.Parse(json);

            Clouds = result["clouds"]["all"].Value;
            string tempStr = result["main"]["temp"].Value;
            string sunriseStr = result["sys"]["sunrise"].Value;
            sunrise = UnixTimestampToDateTime(sunriseStr);
            string sunsetStr = result["sys"]["sunset"].Value;
            sunset = UnixTimestampToDateTime(sunsetStr);
        }
    } /// GetWeatherData method

    public static int GetLevelDifficulty()
    {
        return levelDifficulty;
    }
    /// <summary>
    /// chooses level difficulty according to weather data
    /// </summary>
    public void SetLevelDifficulty()
    {
        SetCheckSun();
        double dailyTotal = GetDailyTotal();
        if (CHECKSUN == true)
        {
            if (dailyTotal != 0)
            {               
                if (dailyTotal > 0.00 && dailyTotal <= 25.00) {
                    levelDifficulty = 1;
                }
                if (dailyTotal > 25.00 && dailyTotal <= 50.00) {
                    levelDifficulty = 2;
                }
                if (dailyTotal > 50.00 && dailyTotal <= 75.00) {
                    levelDifficulty = 3;
                }
                if (dailyTotal > 75.00 && dailyTotal <= 100.00) {
                    levelDifficulty = 4;
                }
                if (dailyTotal > 100) {
                    levelDifficulty = 5;
                }
            }
        }
        else
        {
            levelDifficulty = 6;
        }
    } /// SetLevelDifficulty method

    /// <summary>
    /// chooses score needed to complete level
    /// </summary>
    public void SetNeededScore()
    {
        if (curDistrictNumber <= 5)
        {
            neededScore = 12;
        }
        else if (curDistrictNumber <= 10)
        {
            neededScore = 15;
        }
        else if (curDistrictNumber <= 15)
        {
            neededScore = 18;
        }
        else if (curDistrictNumber <= 20)
        {
            neededScore = 21;
        }
        else if (curDistrictNumber <= 25)
        {
            neededScore = 24;
        }
        else
        {
            neededScore = 27;
        }
    } /// SetNeededScore;

    /// <summary>
    /// sets default coordinates in case location goes wrong
    /// </summary>
    private void SetDefaultCoords()
    {
        if (LAT == 0)
        {
            LAT = 48.5113;
        }
        if (LON == 0)
        {
            LON = 14.5048;
        }
        levelDifficulty = 6;
    } /// SetDefaultCoords method

    /// <summary>
    /// checks if it is daytime or not
    /// </summary>
    private void SetCheckSun()
    {
        DateTime localDate = DateTime.Now;
        if (localDate >= sunrise && localDate <= sunset)
        {
            CHECKSUN = true;
        }
        else
        {
            /// should be FALSE, change TO True FOR TESTING PURPOSES
            CHECKSUN = false;
        }
    } /// SetCheckSun method

    /// <summary>
    /// converts UnixTimestamp to unity DateTime format
    /// </summary>
    /// <returns>DateTime object</returns>
    private DateTime UnixTimestampToDateTime(string _unixTimestamp)
    {
        /// Unix timestamp is seconds past epoch
        double timestamp = int.Parse(_unixTimestamp);
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
        return dtDateTime;
    } /// UnixTimestampToDateTime method

    /// <summary>
    /// sets the average number of hours in a day
    /// depending on the current month
    /// </summary>
    /// <returns>number of hours</returns>
    private double GetDayTimeHours()
    {
        double dayTimeHours = 8;
        switch (DateTime.Now.Month)
        {
            case 1:
                dayTimeHours = 8.51;
                break;
            case 2:
                dayTimeHours = 10.17;
                break;
            case 3:
                dayTimeHours = 11.54;
                break;
            case 4:
                dayTimeHours = 13.42;
                break;
            case 5:
                dayTimeHours = 15.15;
                break;
            case 6:
                dayTimeHours = 16.07;
                break;
            case 7:
                dayTimeHours = 15.46;
                break;
            case 8:
                dayTimeHours = 14.25;
                break;
            case 9:
                dayTimeHours = 12.41;
                break;
            case 10:
                dayTimeHours = 10.56;
                break;
            case 11:
                dayTimeHours = 9.19;
                break;
            case 12:
                dayTimeHours = 8.26;
                break;
            default:
                dayTimeHours = 8.51;
                break;
        }
        return dayTimeHours;
    } /// GetDayTimeHours method

    /// <summary>
    /// calculates sum of usful daily hours
    /// </summary>
    public double GetDailyTotal()
    {
        double dayTime = GetDayTimeHours();

        /// sunny = 100 - current cloud percentage
        double howSunny = 100 - double.Parse(Clouds);
        double dailyTotal = (dayTime * howSunny) / curDistrict.PVs;
        return dailyTotal;
    }  /// GetDailyTotal method
}

