using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float minUnFreezeDelay;
    [SerializeField] private float maxUnFreezeDelya;
    [SerializeField] private float timeToMinUnFreezeDelay;
    private float unFreezeDelya;
    [SerializeField] private float fallGravityMultiplier;
    private float previousFallGravityMultiplier;
    [SerializeField] private int coinMultiplier;
    [SerializeField] private int scoreMultiplier;
    public static bool IsGameOver { get; set; }
    public static bool IsPaused { get; set; }

    protected override void Awake()
    {
        base.Awake();
        IsGameOver = false;
        unFreezeDelya = maxUnFreezeDelya;
    }

    public static float UnFreezeDelay { get => Instance.unFreezeDelya; }
    public static float FallGravityMultiplier { get => Instance.fallGravityMultiplier; }
    public static int CoinMultiplier { get => Instance.coinMultiplier; }
    public static int ScoreMultiplier { get => Instance.scoreMultiplier; }

    private void Update()
    {
        if(unFreezeDelya < minUnFreezeDelay)
            unFreezeDelya = minUnFreezeDelay;
        else if(unFreezeDelya > minUnFreezeDelay)
            unFreezeDelya -= Time.deltaTime / (timeToMinUnFreezeDelay * 60.0f);
    }
    public static void SlowDownFall()
    {
        Instance.previousFallGravityMultiplier = Instance.fallGravityMultiplier;
        Instance.fallGravityMultiplier = 0.5f;
    }
    public static void NormalizeFall()
    {
        Instance.fallGravityMultiplier = Instance.previousFallGravityMultiplier;
    }
}
