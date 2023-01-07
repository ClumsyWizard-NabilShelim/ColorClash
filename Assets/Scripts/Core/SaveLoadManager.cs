using ClumsyWizard.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public enum SaveLoadKey
{
	PlayerData,
	PlayerStats,
	Settings
}

public class SaveLoadManager : Persistant<SaveLoadManager>
{
	private static string SAVE_FOLDER
    {
        get
        {
			return Application.persistentDataPath + "/saves/";
		}
    }
	private static readonly string FILE_EXTENSION = ".clumsywizard";
	public static Action OnSave;

	protected override void Awake()
    {
        base.Awake();
		CreateDirectory();
		Application.quitting += () =>
		{
			OnSave?.Invoke();
		};
		SceneManagement.OnLoadTriggeredCore += () =>
		{
			OnSave?.Invoke();
		};   
    }

    public static void SaveData<T>(T objectToSave, SaveLoadKey key)
	{
		CreateDirectory();
		string json = JsonConvert.SerializeObject(objectToSave, Formatting.Indented);
		File.WriteAllText(CreatePath(key), json);
	}

	public static void LoadData<T>(SaveLoadKey key, Action<T> onLoaded)
	{
		CreateDirectory();
		if (SaveExists(key))
		{
			string loadedString = File.ReadAllText(CreatePath(key));
			T loadedObject = JsonConvert.DeserializeObject<T>(loadedString);
			onLoaded?.Invoke(loadedObject);
			return;
		}
		onLoaded?.Invoke(default);
	}

	public static void DeleteFile(SaveLoadKey key)
	{
		if (SaveExists(key))
			File.Delete(CreatePath(key));
	}

	public static void DEBUG_DELETE_ALL_SAVES()
	{
		if (Directory.Exists(SAVE_FOLDER))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
			directoryInfo.Delete(true);
			Debug.Log("||******************************||All Data Erased!||********************************||");
		}
	}

	public static bool SaveExists(SaveLoadKey key)
	{
		return File.Exists(CreatePath(key));
	}
	public static string CreatePath(SaveLoadKey key)
	{
		return SAVE_FOLDER + key.ToString() + FILE_EXTENSION;
	}
	private static void CreateDirectory()
	{
		if (!Directory.Exists(SAVE_FOLDER))
		{
			Directory.CreateDirectory(SAVE_FOLDER);
		}
	}

	public static void SetInt(string id, int amount)
    {
		PlayerPrefs.SetInt(id, amount);
		PlayerPrefs.Save();
	}
	public static void IncrementInt(string id, int increment)
	{
		PlayerPrefs.SetInt(id, PlayerPrefs.GetInt(id) + increment);
		PlayerPrefs.Save();
	}
	public static void DecrementInt(string id, int decrement)
	{
		PlayerPrefs.SetInt(id, PlayerPrefs.GetInt(id) - decrement);
		PlayerPrefs.Save();
	}

	protected override void CleanUp()
	{

	}
}