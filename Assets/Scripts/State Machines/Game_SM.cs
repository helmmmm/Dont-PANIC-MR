using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SM : StateMachine
{
    private static Game_SM _instance;
    public static Game_SM Instance => _instance ??= new Game_SM();


    // States
    private GameState_PreGame _statePreGame;
    private GameState_PreRound _statePreRound;
    private GameState_MidRound _stateMidRound;
    private GameState_PostRound _statePostRound;
    private GameState_GameOver _stateGameOver;
    private GameState_Pause _statePause;


    // Create states
    public GameState_PreGame GameState_PreGame => _statePreGame ??= new GameState_PreGame(this);
    public GameState_PreRound GameState_PreRound => _statePreRound ??= new GameState_PreRound(this);
    public GameState_MidRound GameState_MidRound => _stateMidRound ??= new GameState_MidRound(this);
    public GameState_PostRound GameState_PostRound => _statePostRound ??= new GameState_PostRound(this);
    public GameState_GameOver GameState_GameOver => _stateGameOver ??= new GameState_GameOver(this);
    public GameState_Pause GameState_Pause => _statePause ??= new GameState_Pause(this);


    public bool IsPreGame => GetCurrentState() == GameState_PreGame;
    public bool IsPreRound => GetCurrentState() == GameState_PreRound;
    public bool IsMidRound => GetCurrentState() == GameState_MidRound;
    public bool IsPostRound => GetCurrentState() == GameState_PostRound;
    public bool IsGameOver => GetCurrentState() == GameState_GameOver;
    public bool IsPause => GetCurrentState() == GameState_Pause;


    public void Initialize()
    {
        SetInitialState(GameState_PreGame);
    }
}
