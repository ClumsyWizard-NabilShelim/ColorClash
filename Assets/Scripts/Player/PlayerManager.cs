using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum StatusEffect
{
    TimeFreeze,
    DoubleCoin,
    Burn,
    CoinFreeze,
    PowerUpFreeze,
}

public class PlayerManager : Singleton<PlayerManager>
{
    private AudioManager audioManager;
    [Header("Stats")]
    [SerializeField] private int sliceScore;
    private int health;
    private float currentScore;
    private int coin;
    private bool recoveryInvinsibility;

    //Buffs
    private float goodChainCoinMultiplier;
    private VisualFeedbackManager feedbackManager;

    [Header("Timing")]
    [SerializeField] private int gameMinute;
    private int currentMinute;
    private float currentSecond;
    [SerializeField] private int sliceMissTimePenality;

    [Header("UI")]
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private Transform healthHolder;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI timerText;

    private ClumsyDictionary<StatusEffect, float> statusEffects = new ClumsyDictionary<StatusEffect, float>();
    [SerializeField] private ClumsyDictionary<StatusEffect, List<StatusEffectVisual>> statusEffectsVisuals= new ClumsyDictionary<StatusEffect, List<StatusEffectVisual>>();

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        health = PlayerDataManager.PlayerStats.MaxHealth;
        UpdateHealth();
        currentScore = 0;
        currentMinute = gameMinute - 1;
        currentSecond = 60.0f;

        goodChainCoinMultiplier = 1;

        UpdateUI();

        feedbackManager = GetComponent<VisualFeedbackManager>();
    }

    private void Update()
    {
        if (GameManager.IsGameOver)
            return;

        if (!statusEffects.ContainsKey(StatusEffect.TimeFreeze))
        {
            currentSecond -= Time.deltaTime;
            if(currentSecond <= 0.0f)
            {
                currentMinute--;
                if (currentMinute < 0)
                {
                    GameOver();
                    return;
                }
                currentSecond = 60.0f;
            }
            if (currentMinute < 0)
            {
                GameOver();
                return;
            }

            if (currentMinute == 0)
                audioManager.Play("TimeRunningOut");
            timerText.text = string.Format("{0:00}", currentMinute) + ":" + string.Format("{0:00}", currentSecond);
        }

        foreach (StatusEffect effect in statusEffects.Keys)
        {
            statusEffects[effect] -= Time.deltaTime;
        }

        for (int i = 0; i < statusEffects.Count; i++)
        {
            if (statusEffects.ElementAt(i).Value <= 0)
            {
                if (statusEffectsVisuals.ContainsKey(statusEffects.ElementAt(i).Key))
                {
                    foreach (StatusEffectVisual effectVisual in Instance.statusEffectsVisuals[statusEffects.ElementAt(i).Key])
                    {
                        effectVisual.Deactivate();
                    }
                }
                statusEffects.RemoveAt(i);
                continue;
            }

            i++;
        }

        ProcessStatusEffects();
    }

    private void ProcessStatusEffects()
    {
        if (statusEffects.ContainsKey(StatusEffect.Burn))
        {
            currentScore -= 5 * Time.deltaTime;
            if (currentScore < 0)
                currentScore = 0;
            UpdateUI();
        }
    }

    public static void GoodSlice(int sliceCount, int coin)
    {
        Instance.recoveryInvinsibility = false;
        Instance.currentScore += Instance.sliceScore * sliceCount * GameManager.ScoreMultiplier;
        Instance.goodChainCoinMultiplier += 0.1f;
        Instance.goodChainCoinMultiplier = float.Parse(Instance.goodChainCoinMultiplier.ToString("0.0"));
        ObjectLauncher.SuccessfullSlice = true;
        if(Instance.goodChainCoinMultiplier >= 1.5f)
        Instance.feedbackManager.Feedback(FeedbackType.CoinMultiplier, Instance.goodChainCoinMultiplier, 5);
        Instance.AddCoin(coin);
        Instance.UpdateUI();
    }

    public static void BadSlice()
    {
        Instance.recoveryInvinsibility = true; 
        Instance.audioManager.Play("Damage");
        Instance.goodChainCoinMultiplier = 1;
        Instance.health--;
        Instance.UpdateHealth();
        Instance.feedbackManager.Feedback(FeedbackType.Health, -1, 1);
        CameraShake.Shake(ShakeIntensity.Low);
        if (Instance.health <= 0)
        {
            Instance.GameOver();
        }
    }

    public static void Heal(int amount)
    {
        Instance.health += amount;
        Instance.feedbackManager.Feedback(FeedbackType.Health, 1, 1);
        Instance.UpdateHealth();
    }
    public static void AddTime(float amountInSeconds)
    {
        Instance.currentSecond += amountInSeconds;

        if (Instance.currentSecond > 60.0f)
        {
            float minute = Instance.currentSecond / 60.0f;
            float seconds = 60.0f * (minute - (int)minute);
            Instance.currentMinute += (int)minute;
            Instance.currentSecond = seconds;
        }

        Instance.feedbackManager.Feedback(FeedbackType.Time, amountInSeconds, 1);
    }

    private void GameOver()
    {
        currentMinute = 0;
        currentSecond = 0.0f;
        timerText.text = string.Format("{0:00}", currentMinute) + ":" + string.Format("{0:00}", currentSecond);
        audioManager.Stop("TimeRunningOut");
        PlayerDataManager.PlayerData.AddCoin(coin);
        PlayerDataManager.PlayerData.CheckAndSetHighScore(Mathf.CeilToInt(currentScore));
        PlayerDataManager.Save();
        GameOverMenu.Over(Mathf.CeilToInt(currentScore), coin, currentScore > PlayerDataManager.PlayerData.HighScore);
    }

    public static void SliceMissed()
    {
        if(Instance.recoveryInvinsibility)
        {
            Instance.recoveryInvinsibility = false;
            return;
        }
        Instance.audioManager.Play("Damage");
        Instance.goodChainCoinMultiplier = 1;
        DecreaseTime(Instance.sliceMissTimePenality);
        Instance.feedbackManager.Feedback(FeedbackType.Missed, Instance.sliceMissTimePenality, 0.5f);
        CameraShake.Shake(ShakeIntensity.Medium);
    }
    public static void DecreaseTime(float amountInSeconds)
    {
        if (Instance.statusEffects.ContainsKey(StatusEffect.TimeFreeze))
            return;

        Instance.currentSecond -= amountInSeconds;

        if (Instance.currentSecond < 0.0f)
        {
            float minute = 1 + Mathf.Abs(Instance.currentSecond) / 60.0f;
            float seconds = (60.0f * (minute - (int)minute));
            Instance.currentMinute -= (int)minute;
            Instance.currentSecond = 60 - seconds;
        }
    }
    private void AddCoin(int amount)
    {
        if (Instance.statusEffects.ContainsKey(StatusEffect.CoinFreeze))
            return;

        amount = amount * GameManager.CoinMultiplier;
        amount += Mathf.CeilToInt(amount * Instance.goodChainCoinMultiplier * (Instance.statusEffects.ContainsKey(StatusEffect.DoubleCoin) ? 2 : 1));
        Instance.coin += amount;
        Instance.feedbackManager.Feedback(FeedbackType.CoinEarned, amount, 1);
    }
    public static void RemoveCoin(int amount)
    {
        Instance.coin -= amount;
        if (Instance.coin < 0)
            Instance.coin = 0;
        Instance.UpdateUI();
    }

    private void UpdateUI()
    {
        coinText.text = coin.ToString();
        scoreText.text = Mathf.CeilToInt(currentScore).ToString();
    }

    private void UpdateHealth()
    {
        if(healthHolder.childCount != health)
        {
            for (int i = 0; i < healthHolder.childCount; i++)
            {
                Destroy(healthHolder.GetChild(i).gameObject);
            }

            for (int i = 0; i < health; i++)
            {
                Instantiate(healthPrefab, healthHolder);
            }
        }
    }

    public static void AddStatusEffect(StatusEffect effect, float duration)
    {
        if (effect == StatusEffect.DoubleCoin)
            Instance.feedbackManager.Feedback(FeedbackType.CoinRush, 0, duration);

        if (Instance.statusEffects.ContainsKey(effect))
        {
            Instance.statusEffects[effect] += duration;
        }
        else
        {
            Instance.statusEffects.Add(effect, duration);

            if (effect == StatusEffect.PowerUpFreeze && PlayerDataManager.PlayerData.EquippedPowerUps.Count == 0)
                return;

            if (Instance.statusEffectsVisuals.ContainsKey(effect))
            {
                foreach (StatusEffectVisual effectVisual in Instance.statusEffectsVisuals[effect])
                {
                    effectVisual.Activate();
                }
            }
        }
    }
    public static bool HasStatusEffet(StatusEffect effect)
    {
        return Instance.statusEffects.ContainsKey(effect);
    }
}