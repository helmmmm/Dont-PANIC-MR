using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _startGameButton;
    [SerializeField] GameObject _timer;
    [SerializeField] GameObject _roundText;
    private Game_SM _gameSM => Game_SM.Instance;

    //singleton
    public static GameUIManager Instance;


    private void Awake()
    {
        _startGameButton.SetActive(false);
    }

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
    }

    public void GameUIManager_PreGame()
    {
        _startGameButton.SetActive(true);
    }

    public void GameUIManager_PreRound()
    {
        _timer.SetActive(true);
        _roundText.SetActive(true);
        _startGameButton.SetActive(false);
    }

    public void GameUIManager_MidRound()
    {

    }

    public void GameUIManager_PostRound() 
    {
    
    }

    public void OnStartButton()
    {
        _gameSM.TryChangeState(_gameSM.GameState_PreRound);
    }
}
