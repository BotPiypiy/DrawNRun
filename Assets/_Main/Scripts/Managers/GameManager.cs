using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected static GameManager _Instance;
    public static Action<Transform> GameOver;


    private void Awake()
    {
        if(_Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
}
