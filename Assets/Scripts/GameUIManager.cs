using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameUIManager : NetworkBehaviour
{
    [SerializeField] GameObject _timer;
    [SerializeField] GameObject _roundText;

    [SerializeField] GameObject _menuPanels;

    private Game_SM _gameSM => Game_SM.Instance;

    //singleton
    public static GameUIManager Instance;


    private void Awake()
    {
        _menuPanels.SetActive(true);
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


    public void GameUIManager_PreRound()
    {
        _timer.SetActive(true);
        _roundText.SetActive(true);
    }

    public void GameUIManager_MidRound()
    {

    }

    public void GameUIManager_PostRound() 
    {
    
    }

    [ServerRpc]
    public void OnGamePhaseServerRpc()
    {
        OnGamePhaseClientRpc();
        _menuPanels.SetActive(false);
        _gameSM.Initialize();
    }

    [ClientRpc]
    public void OnGamePhaseClientRpc()
    {
        _menuPanels.SetActive(false);
    }

    public void OnStartButton()
    {
        _gameSM.TryChangeState(_gameSM.GameState_PreRound);
    }
}
