using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Game_SM _gameSM => Game_SM.Instance;
    private RoundManager _roundManager;

    private int _midRoundDuration = 5;
    private int _preRoundDuration = 3;
    public int _currentPreRoundTime;
    public int _currentMidRoundTime;
    public string _startText = "Start";
    private string _timesUpText = "Time's Up";

    private TMP_Text _timerText;

    private void Awake()
    {
        gameObject.SetActive(false);
        _timerText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _roundManager = FindObjectOfType<RoundManager>();
    }

    public void Timer_PreRound()
    {
        StartCoroutine(PreRoundCountDown());
    }

    public void Timer_MidRound()
    {
        StartCoroutine(MidRoundCountDown());
    }

    private IEnumerator PreRoundCountDown()
    {
        _currentPreRoundTime = _preRoundDuration;
        while (_currentPreRoundTime > -1)
        {
            if (_currentPreRoundTime == 0)
            {
                _timerText.text = _startText;
            }
            else
            {
                _timerText.text = _currentPreRoundTime.ToString();
            }
            yield return new WaitForSeconds(1);
            _currentPreRoundTime--;
        }
        _gameSM.TryChangeState(_gameSM.GameState_MidRound);
    }

    private IEnumerator MidRoundCountDown()
    {
        _currentMidRoundTime = _midRoundDuration;
        while (_currentMidRoundTime > -1)
        {
            if (_currentMidRoundTime == 0)
            {
                _timerText.text = _timesUpText;
            }
            else
            {
                _timerText.text = _currentMidRoundTime.ToString();
                yield return new WaitForSeconds(1);
            }
            _currentMidRoundTime--;
        }
        DangerManager.Instance.DeactivateWarnings();
        DangerManager.Instance.ShootLasers();
        //shoot laser
        //determine win or lose
        //if win, go to next round
    }
}
