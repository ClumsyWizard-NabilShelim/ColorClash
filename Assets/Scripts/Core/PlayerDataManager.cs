using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int HighScore;
    public int Coin;

    public ClumsyDictionary<string, int> UnlockedPowerUps;
    public ClumsyDictionary<string, int> UnlockedUpgrades;
    public List<string> EquippedPowerUps;
    public List<string> UnlockedArenas;

    public PlayerData() { }

    public PlayerData(PlayerData data, ClumsyDictionary<string, int> unlockedPowerUps, List<string> equippedPowerUps, ClumsyDictionary<string, int> unlockedUpgrades, List<string> unlockedArenas)
    {
        HighScore = data.HighScore;
        Coin = data.Coin;
        UnlockedPowerUps = data.UnlockedPowerUps == null ? unlockedPowerUps : data.UnlockedPowerUps;
        EquippedPowerUps = data.EquippedPowerUps == null ? equippedPowerUps : data.EquippedPowerUps;
        UnlockedUpgrades = data.UnlockedUpgrades == null ? unlockedUpgrades : data.UnlockedUpgrades;
        UnlockedArenas = data.UnlockedArenas == null ? unlockedArenas : data.UnlockedArenas;
    }

    public PlayerData(int highScore, int coin, ClumsyDictionary<string, int> unlockedPowerUps, List<string> equippedPowerUps, ClumsyDictionary<string, int> unlockedUpgrades, List<string> unlockedArenas)
    {
        HighScore = highScore;
        Coin = coin;
        UnlockedPowerUps = unlockedPowerUps;
        EquippedPowerUps = equippedPowerUps;
        UnlockedUpgrades = unlockedUpgrades;
        UnlockedArenas = unlockedArenas;
    }

    public void AddCoin(int amount)
    {
        Coin += amount;
        PlayerDataManager.PlayerAchievements.AddNetCoin(amount);
        HUD.UpdateUI();
        PlayerDataManager.Save();
    }
    public void UseCoin(int amount)
    {
        Coin -= amount;
        if (Coin < 0)
            Coin = 0;

        HUD.UpdateUI();
        PlayerDataManager.Save();
    }
    public void CheckAndSetHighScore(int score)
    {
        if (score > HighScore)
            HighScore = score;
        PlayerDataManager.Save();
    }

    public void AddPowerUp(string idName)
    {
        if (UnlockedPowerUps.ContainsKey(idName))
            UnlockedPowerUps[idName]++;
        else
            UnlockedPowerUps.Add(idName, 1);
        PlayerDataManager.Save();
    }

    public void EquipPowerUp(string idName)
    {
        EquippedPowerUps.Add(idName);
        PlayerDataManager.Save();
    }

    public void RemovePowerUp(string idName)
    {
        EquippedPowerUps.Remove(idName);
        PlayerDataManager.Save();
    }

    public void AddUpgrade(string idName)
    {
        if (UnlockedUpgrades.ContainsKey(idName))
            UnlockedUpgrades[idName]++;
        else
            UnlockedUpgrades.Add(idName, 1);

        PlayerDataManager.Save();
    }

    public void AddArena(string idName)
    {
        if (UnlockedArenas.Contains(idName))
        {
            Debug.Log("This should not happend. UnlockedArenas already contains: " + idName);
            return;
        }

        UnlockedArenas.Add(idName);
    }
}

[System.Serializable]
public class PlayerStats
{
    public int MaxHealth;
    public int Blades;
    public int MaxPowerUpSlots;
    public PlayerStats(int maxHealth, int blades, int maxPowerUpSlots)
    {
        MaxHealth = maxHealth;
        Blades = blades;
        MaxPowerUpSlots = maxPowerUpSlots;
    }

    public void AddHealth()
    {
        MaxHealth++;
        PlayerDataManager.Save();
    }

    public void AddBlade()
    {
        Blades++;
        PlayerDataManager.Save();
    }

    public void AddSlot()
    {
        MaxPowerUpSlots++;
        PlayerDataManager.Save();
    }
}

[System.Serializable]
public class PlayerAchievements
{
    public int NetCoin;
    public int EnemiesKilled;
    public PlayerAchievements(int netCoin, int enemiesKilled)
    {
        NetCoin = netCoin;
        EnemiesKilled = enemiesKilled;
    }

    public void AddNetCoin(int amount)
    {
        NetCoin += amount;
        PlayerDataManager.Save();
    }
    public void AddEnemiesKilled(int amount)
    {
        EnemiesKilled += amount;
        PlayerDataManager.Save();
    }
}

public class PlayerDataManager : Persistant<PlayerDataManager>
{
    public static PlayerData PlayerData { get; private set; }
    public static PlayerStats PlayerStats { get; private set; }
    public static PlayerAchievements PlayerAchievements { get; private set; }

    [Header("Starting Data")]
    [SerializeField] private int startingCoin;
    [SerializeField] private int startingMaxPowerUpSlots;
    [SerializeField] private ClumsyDictionary<string, int> startingUnlockedPowerUps;
    [SerializeField] private ClumsyDictionary<string, int> startingUnlockedUpgrades;
    [SerializeField] private List<string> startingEquippedPowerUps;
    [SerializeField] private List<string> startingArenas;

    [Header("Starting Stats")]
    [SerializeField] private int startingHealth;
    [SerializeField] private int startingBlades;
    [SerializeField] private int startingPowerUpSlots;

    protected override void Awake()
    {
        base.Awake();
        Load();

        SaveLoadManager.OnSave += Save;
    }

    public static void Save()
    {
        SaveLoadManager.SaveData(PlayerData, SaveLoadKey.PlayerData);
        SaveLoadManager.SaveData(PlayerStats, SaveLoadKey.PlayerStats);
        SaveLoadManager.SaveData(PlayerAchievements, SaveLoadKey.PlayerAchievements);
    }

    private void Load()
    {
        SaveLoadManager.LoadData(SaveLoadKey.PlayerData, (PlayerData data) =>
        {
            if(data == null)
            {
                PlayerData = new PlayerData(0, startingCoin, startingUnlockedPowerUps, startingEquippedPowerUps, startingUnlockedUpgrades, startingArenas);
            }
            else
            {
                PlayerData = new PlayerData(data, startingUnlockedPowerUps, startingEquippedPowerUps, startingUnlockedUpgrades, startingArenas);
            }
        });
        SaveLoadManager.LoadData(SaveLoadKey.PlayerStats, (PlayerStats data) =>
        {
            if (data == null)
            {
                PlayerStats = new PlayerStats(startingHealth, startingBlades, startingPowerUpSlots);
            }
            else
            {
                PlayerStats = data;
            }
        });
        SaveLoadManager.LoadData(SaveLoadKey.PlayerAchievements, (PlayerAchievements data) =>
        {
            if (data == null)
            {
                PlayerAchievements = new PlayerAchievements(0, 0);
            }
            else
            {
                PlayerAchievements = data;
            }
        });
    }

    protected override void CleanUp()
    {

    }
}