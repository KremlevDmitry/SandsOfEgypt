using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class DailyBonus
{
    private static string format = "yyyy-MM-dd HH:mm:ss";

    public static UnityEvent OnGet = new UnityEvent();


    public static DateTime LastOpenTime
    {
        get
        {
            if (!PlayerPrefs.HasKey("DailyBonusLastOpenTime"))
            {
                LastOpenTime = DateTime.Now.AddDays(-2);
            }
            return DateTime.ParseExact(PlayerPrefs.GetString("DailyBonusLastOpenTime"), format, System.Globalization.CultureInfo.InvariantCulture);
        }
        set
        {
            PlayerPrefs.SetString("DailyBonusLastOpenTime", value.ToString(format));
        }
    }

    public static void GetBonus()
    {
        LastOpenTime = DateTime.Now;
        OnGet.Invoke();
    }
}
