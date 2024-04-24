using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    Game_SM _gameSM => Game_SM.Instance;
    public int _currentRound = 1;
    public bool _PreRoundOver = false;
    public bool _MidRoundOver = false;

    private void Start()
    {
        _gameSM.GameState_MidRound.OnExit += ExitMidRound;
    }

    private void ExitMidRound()
    {
        _currentRound++;
    }

    private void OnDisable()
    {
        _gameSM.GameState_MidRound.OnExit -= ExitMidRound;
    }
}
