using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.Utilities
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
	{
		protected static T Instance { get; set; }

		protected virtual void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this as T;
		}
    }

	public abstract class Persistant<T> : Singleton<T> where T : MonoBehaviour
	{
		protected override void Awake()
		{
			base.Awake();
			transform.SetParent(null);
			DontDestroyOnLoad(gameObject);

			SceneManagement.OnLoadTriggeredCore += () =>
			{
				CleanUp();
			};
		}

		protected abstract void CleanUp();
	}
}