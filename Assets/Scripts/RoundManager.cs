using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    Game_SM _gameSM => Game_SM.Instance;
    public static RoundManager Instance;

    [SerializeField] TMP_Text _roundText;

    public int _currentRound = 0;
    public bool _PreRoundOver = false;
    public bool _MidRoundOver = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _currentRound = 0;
        _gameSM.GameState_PreRound.OnEnter += UpdateRoundText;
    }

    public void RoundManager_PreRound()
    {
        _currentRound++;
    }

    public void UpdateRoundText()
    {
        _roundText.text = "Round " + _currentRound;
    }

    private void OnDisable()
    {
        _gameSM.GameState_PreRound.OnEnter -= UpdateRoundText;
    }
}
