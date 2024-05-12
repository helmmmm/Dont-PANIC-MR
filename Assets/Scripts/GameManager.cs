using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Game_SM _gameSM => Game_SM.Instance;
    private LevelGenerator _levelGenerator;

    public bool IsGameOver;
    
    [SerializeField] GameUIManager _gameUIManager;
    [SerializeField] GameObject _levelSpace;
    [SerializeField] Timer _timer;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        IsGameOver = false;
        _levelGenerator = _levelSpace.GetComponent<LevelGenerator>();
        _gameSM.GameState_PreGame.OnEnter += PreGameSequence;
        _gameSM.GameState_PreRound.OnEnter += PreRoundSequence;
        _gameSM.GameState_MidRound.OnEnter += MidRoundSequence;
        _gameSM.GameState_MidRound.OnExit += ExitMidRoundSequence;
        _gameSM.GameState_PostRound.OnEnter += PostRoundSequence;
        _gameSM.GameState_GameOver.OnEnter += GameOverSequence;
    }

    private void PreGameSequence()
    {
        _levelGenerator.BuildLevel();
    }

    public void PreRoundSequence()
    {
        RoundManager.Instance.RoundManager_PreRound();
        DangerManager.Instance.DangerManager_PreRound();
        _gameUIManager.GameUIManager_PreRound();
        _timer.Timer_PreRound();
    }

    private void MidRoundSequence()
    {
        DangerManager.Instance.DangerManager_MidRound();
        _timer.Timer_MidRound();
    }

    private void ExitMidRoundSequence()
    {
        // code
    }

    private void PostRoundSequence()
    {
        // Determine win/lose
        DangerManager.Instance.ClearRound();
        // update score
        _gameSM.TryChangeState(_gameSM.GameState_PreRound);
    }

    private void ExitPostRoundSequenceToGameOver()
    {
       
    }

    private void GameOverSequence()
    {

    }

    private void OnDisable()
    {
        _gameSM.GameState_PreGame.OnEnter -= PreGameSequence;
        _gameSM.GameState_PreRound.OnEnter -= PreRoundSequence;
        _gameSM.GameState_MidRound.OnEnter -= MidRoundSequence;
        _gameSM.GameState_MidRound.OnExit -= ExitMidRoundSequence;
        _gameSM.GameState_PostRound.OnEnter -= PostRoundSequence;
        _gameSM.GameState_GameOver.OnEnter -= GameOverSequence;
    }
}
