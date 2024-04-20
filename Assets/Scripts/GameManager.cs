using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Game_SM _gameSM => Game_SM.Instance;

    [SerializeField] GameObject _levelSpace;

    [SerializeField] GameObject _startGameButton;


    private void Awake()
    {
        _startGameButton.SetActive(false);
    }

    void Start()
    {
        _gameSM.Initialize();
        Instance = this;
    }

    private void OnEnable()
    {
        _gameSM.GameState_PreGame.OnEnter += OnGameSceneEnter;
    }

    private void OnGameSceneEnter()
    {
        _levelSpace.GetComponent<LevelGenerator>().BuildLevel();

        _startGameButton.SetActive(true);

        Debug.Log(_gameSM.GetCurrentState().GetName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
