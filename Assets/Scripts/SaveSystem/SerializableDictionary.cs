using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]//Custom lại dictionary để Inspector trên unity editor
public class SerializableDictionary<Tkey,Tvalue>: Dictionary<Tkey,Tvalue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<Tvalue> values = new List<Tvalue>();    
    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
            Debug.Log("Keys count is not equal to value count");

        for (int i = 0;i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<Tkey, Tvalue> pairs in this)
        {
            keys.Add(pairs.Key);
            values.Add(pairs.Value);
        }
    }
}
