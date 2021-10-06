using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject hero;
    public GameObject demon;
    public GameObject breath;


    private void Awake()
    {
        Instance = this;
    }
}
