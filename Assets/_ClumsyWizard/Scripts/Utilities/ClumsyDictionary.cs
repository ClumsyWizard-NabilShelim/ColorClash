using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Collections;
using System.Linq;

namespace ClumsyWizard.Utilities
{
    [Serializable]
    public struct Pair<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public Pair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [Serializable]
	public class ClumsyDictionary<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField] private List<Pair<TKey, TValue>> entries = new List<Pair<TKey, TValue>>();

        private List<TKey> keys = new List<TKey>();
        private List<TValue> values = new List<TValue>();

        public List<TKey> Keys => keys;
        public List<TValue> Values => values;
        public int Count => keys.Count;

        public ClumsyDictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        public ClumsyDictionary(ClumsyDictionary<TKey, TValue> copy)
        {
            entries = copy.entries.ToList();
            keys = copy.keys.ToList();
            values = copy.values.ToList();
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            keys.Clear();
            values.Clear();

            foreach (Pair<TKey, TValue> pair in entries)
            {
                keys.Add(pair.key);
                values.Add(pair.value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if(keys.Contains(key))
            {
                Debug.LogError("Key already exitsts: " + key.ToString() + " with value(s) of: " + value.ToString());
                return;
            }

            keys.Add(key);
            values.Add(value);

            entries.Add(new Pair<TKey, TValue>(key, value));
        }

        public void Remove(TKey key)
        {
            if (!keys.Contains(key))
            {
                Debug.LogError("Key does not exitst: " + key);
                return;
            }

            int index = keys.IndexOf(key);
            keys.Remove(key);
            values.RemoveAt(index);
            entries.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            TKey key = keys[index];
            if (!keys.Contains(key))
            {
                Debug.LogError("Key does not exitst: " + key);
                return;
            }

            keys.Remove(key);
            values.RemoveAt(index);
            entries.RemoveAt(index);
        }

        public void Clear()
        {
            keys.Clear(); 
            values.Clear();
            entries.Clear();
        }

        public TKey GetKeyByValue(TValue value)
        {
            int index = values.IndexOf(value);

            if (index < keys.Count)
                return keys[index];
            else
                return default;
        }

        public KeyValuePair<TKey, TValue> ElementAt(int index)
        {
            return new KeyValuePair<TKey, TValue>(keys[index], values[index]);
        }

        public int IndexOf(TValue value)
        {
            return values.IndexOf(value);
        }

        public void UpdateKey(int index, TKey newKey)
        {
            keys[index] = newKey;
            entries[index] = new Pair<TKey, TValue>(newKey, entries[index].value);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (keys.Contains(key))
                    return values[keys.IndexOf(key)];

                return default;
            }
            set
            {
                if (keys.Contains(key))
                    values[keys.IndexOf(key)] = value;
                else
                    Debug.LogError("Dictionary does not contain key: " + key.ToString());
            }
        }

        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }

        public bool ContainsValue(TValue value)
        {
            return values.Contains(value);
        }
    }
}