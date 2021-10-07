using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject hero;
    public GameObject demon;
    public GameObject breath;
    public GameObject powerBar;
    public Slider powerBarSlider { get; set; }


    private void Awake()
    {
        Instance = this;
        powerBarSlider = powerBar.GetComponent<Slider>();
    }
}
