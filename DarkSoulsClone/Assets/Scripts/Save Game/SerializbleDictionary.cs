using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
  [System.Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // Called right before serialization
    // Saves the dictionary to lists
    public void OnBeforeSerialize()
    {
      keys.Clear();
      values.Clear();

      foreach (KeyValuePair<TKey, TValue> pair in this)
      {
        keys.Add(pair.Key);
        values.Add(pair.Value);
      }
    }

    // Called right after serialization
    // Load the dictionary FROM lists
    public void OnAfterDeserialize()
    {
      Clear();

      if (keys.Count != values.Count)
      {
        Debug.LogError("Bro, we tried to deserialize the dictionary, the amount of keys does not match the amount of values");
      }

      for (int i = 0; i < keys.Count; i++)
      {
        Add(keys[i], values[i]);
      }
    }
  }
}
