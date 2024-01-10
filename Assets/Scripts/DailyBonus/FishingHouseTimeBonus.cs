using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class FishingHouseTimeBonus
{
    private const string name = "FishingHouse";

    private static string format = "yyyy-MM-dd HH:mm:ss";

    public static UnityEvent OnGet = new UnityEvent();


    public static DateTime LastOpenTime
    {
        get
        {
            if (!PlayerPrefs.HasKey(name + "DailyBonusLastOpenTime"))
            {
                LastOpenTime = DateTime.Now.AddDays(-2);
            }
            return DateTime.ParseExact(PlayerPrefs.GetString(name + "DailyBonusLastOpenTime"), format, System.Globalization.CultureInfo.InvariantCulture);
        }
        set
        {
            PlayerPrefs.SetString(name + "DailyBonusLastOpenTime", value.ToString(format));
        }
    }

    public static void GetBonus()
    {
        LastOpenTime = DateTime.Now;
        OnGet.Invoke();
    }
}
