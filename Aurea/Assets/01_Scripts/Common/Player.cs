﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Singelton
    [SerializeField]
    public static PlayerData _instance;
    public static PlayerData Instance
    {
        get
        {
            if (_instance == null)
                _instance = StateManager.LoadPlayer();
            return _instance;
        }
    }
    #endregion

    public string GameSceneName = "Game";

    void Start()
    {
        Debug.Log(Player.Instance);
        DontDestroyOnLoad(this);
        SceneManager.LoadSceneAsync(GameSceneName);
    }
}