using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float _time = default;
    [SerializeField]
    private Image _timerImage = default;
    private IEnumerator _timing = default;
    public UnityEvent OnTimeOff = new UnityEvent();


    public void StartTimer()
    {
        StopTimer();
        StartCoroutine(_timing = Timing());
    }

    public void StopTimer()
    {
        if (_timing != null)
        {
            StopCoroutine(_timing);
            _timing = null;
        }
    }

    private IEnumerator Timing()
    {
        for (float t = 0; t < _time; t += Time.deltaTime)
        {
            _timerImage.fillAmount = 1 - (t / _time); 
            yield return null;
        }
        _timerImage.fillAmount = 0f;
        OnTimeOff.Invoke();
    }
}
