using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Game_SM _gameSM => Game_SM.Instance;
    
    [SerializeField] GameUIManager _gameUIManager;
    [SerializeField] GameObject _levelSpace;
    [SerializeField] Timer _timer;


    private void Awake()
    {

    }
    void Start()
    {
        _gameSM.Initialize();
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
        _gameSM.GameState_PreGame.OnEnter += PreGameSequence;
        _gameSM.GameState_PreRound.OnEnter += PreRoundSequence;
        _gameSM.GameState_MidRound.OnEnter += MidRoundSequence;
    }

    private void PreGameSequence()
    {
        _levelSpace.GetComponent<LevelGenerator>().BuildLevel();
        _gameUIManager.GameUIManager_PreGame();
    }

    public void PreRoundSequence()
    {
        DangerManager.Instance.DangerManager_PreRound();
        _gameUIManager.GameUIManager_PreRound();
        _timer.Timer_PreRound();
    }

    private void MidRoundSequence()
    {
        DangerManager.Instance.DangerManager_MidRound();
        _timer.Timer_MidRound();
    }

    private void OnDisable()
    {
        _gameSM.GameState_PreGame.OnEnter -= PreGameSequence;
        _gameSM.GameState_PreRound.OnEnter -= PreRoundSequence;
    }
}
