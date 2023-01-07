using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ClumsyWizard.Utilities;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AssetLoader : Persistant<AssetLoader>
{
	private static bool initialized;
	private static Action onInitialized;

	protected override void Awake()
	{
		base.Awake();

		//Initializing the addresables. This is important to do, cause without this line of code, the addresables does not work consistantly.
		AsyncOperationHandle operation = Addressables.InitializeAsync();
		operation.Completed += (AsyncOperationHandle result) =>
		{
			initialized = true;
			onInitialized?.Invoke();
			onInitialized = null;
		};
	}

	public static void LoadAssetsByTag<T>(string tag, Action<List<T>> onAssetLoaded)
	{
		Load(new string[] { tag }, onAssetLoaded);
	}

	public static void Load<T> (string key, Action<T> onAssetLoaded, Action onAssetLoadOver)
	{
		if (key == "" || key == null)
		{
			onAssetLoadOver?.Invoke();
			return;
		}

		CWString.NormalizeString(ref key);

		if (!initialized)
		{
			onInitialized += () =>
			{
				Load(key, onAssetLoaded, onAssetLoadOver);
			};

			return;
		}

		AsyncOperationHandle operation = Addressables.LoadAssetAsync<T>(key);
		operation.Completed += (AsyncOperationHandle result) =>
		{
			if (result.Result != null)
				onAssetLoaded?.Invoke((T)result.Result);
			else
				Debug.Log("Failed to load asset: '" + key + "'!");

			onAssetLoadOver?.Invoke();
		};
	}

	public static void Load<T>(string[] tags, Action<List<T>> onAssetLoaded)
	{
		if (tags == null || tags.Length == 0)
			return;

		CWString.NormalizeString(ref tags);

		if (!initialized)
		{
			onInitialized += () =>
			{
				Load(tags, onAssetLoaded);
			};

			return;
		}

		AsyncOperationHandle<IList<T>> operation = Addressables.LoadAssetsAsync<T>(tags, null, Addressables.MergeMode.Intersection, true);

		operation.Completed += (AsyncOperationHandle<IList<T>> result) =>
		{
			if (result.Result != null)
			{
				List<T> datas = (List<T>)result.Result;
				onAssetLoaded.Invoke(datas);
			}
			else
			{
				Debug.Log("Failed to load asset with key: '" + tags.ToString() + "'!");
			}
		};
	}

	public static void RecursiveLoad<T>(string[] keys, Action<List<T>> onAssetLoaded)
    {
		RecursiveLoad(keys, null, onAssetLoaded);
	}

	public static void RecursiveLoad<T>(string[] keys, Action<T> onEveryIteration, Action<List<T>> onAssetLoaded)
	{
		CWString.NormalizeString(ref keys);

		if (!initialized)
		{
			onInitialized += () =>
			{
				RecursiveLoad(keys, onEveryIteration, onAssetLoaded);
			};

			return;
		}
		RecursiveLoad(keys, new List<T>(), onEveryIteration, onAssetLoaded, 0);
	}

	private static void RecursiveLoad<T>(string[] keys, List<T> data, Action<T> onEveryIteration, Action<List<T>> onAssetLoaded, int index)
	{
		if (keys == null || keys.Length == 0 || index >= keys.Length)
		{
			onAssetLoaded(data);
			return;
		}

		AsyncOperationHandle operation = Addressables.LoadAssetAsync<T>(keys[index]);
		operation.Completed += (AsyncOperationHandle result) =>
		{
			if (result.Result != null)
			{
				data.Add((T)result.Result);
				index++;
				onEveryIteration?.Invoke((T)result.Result);
				RecursiveLoad(keys, data, onEveryIteration, onAssetLoaded, index);
			}
			else
			{
				Debug.Log("Failed to load asset: '" + keys[index] + "'!");
			}
		};
	}

	protected override void CleanUp()
	{
		onInitialized = null;
	}
}