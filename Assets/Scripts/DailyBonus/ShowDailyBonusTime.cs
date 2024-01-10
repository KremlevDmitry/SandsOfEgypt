using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDailyBonusTime : MonoBehaviour
{
    [SerializeField]
    private Text _timerText = default;
    [SerializeField]
    private Button _openDailyBonusButton = default;

    private int _remainSeconds = default;
    private IEnumerator _settingTimer = default;


    private void OnEnable()
    {
        StartCoroutine(_settingTimer = SettingTimer());

        DailyBonus.OnGet.AddListener(UpdateRemainSeconds);
    }

    private void OnDisable()
    {
        if (_settingTimer != null)
        {
            StopCoroutine(_settingTimer);
            _settingTimer = null;
        }
    }

    private void UpdateRemainSeconds()
    {
        var remainTime = (DailyBonus.LastOpenTime.AddDays(1) - DateTime.Now);
        _remainSeconds = (int)remainTime.TotalSeconds;

        UpdateGraphic();
    }

    private IEnumerator SettingTimer()
    {
        var waitSecond = new WaitForSecondsRealtime(1f);
        UpdateRemainSeconds();
        while (true)
        {
            UpdateGraphic();
            yield return waitSecond;
            _remainSeconds--;
        }
    }

    private void UpdateGraphic()
    {
        if (_remainSeconds > 0)
        {
            int hours = _remainSeconds / 60 / 60;
            int minutes = _remainSeconds / 60 % 60;
            int seconds = _remainSeconds % 60;

            _timerText.text =
                $"{(hours > 9 ? "" : "0")}{hours}:" +
                $"{(minutes > 9 ? "" : "0")}{minutes}:" +
                $"{(seconds > 9 ? "" : "0")}{seconds}";

            _openDailyBonusButton.interactable = false;
        }
        else
        {
            _timerText.text = "TAKE";

            _openDailyBonusButton.interactable = true;
        }
    }
}
