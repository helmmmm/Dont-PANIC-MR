using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Singleton
    public static SceneManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Change to game scene
    public void ToGameScene()
    {
        Debug.Log("Changing to Game Scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
