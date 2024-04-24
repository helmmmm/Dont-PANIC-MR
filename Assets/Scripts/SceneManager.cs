using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Singleton
    private static SceneManager _instance;
    public static SceneManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Change to game scene
    public void ToGameScene()
    {
        Debug.Log("Changing to Game Scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
