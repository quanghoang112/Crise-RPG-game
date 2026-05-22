using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


[Serializable]
public class Buff
{
    public StatsType buffType;
    public float buffValue;    
}

public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer sr;
    private EntityStats statsToModify;


    [Header ("Buff details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    // [SerializeField] private float buffValue;
    [SerializeField] private float buffDuration = 4;
    [SerializeField] private bool CanBeUsed = true;

    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0,yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CanBeUsed == false)  return;

        statsToModify = collision.GetComponent<EntityStats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        CanBeUsed = false;
        sr.color = Color.clear;
        Debug.Log("Buff applied for 4s");

        ApplyBuff(true);
        
        yield return new WaitForSeconds(duration);
        
        ApplyBuff(false);
        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach(var buff in buffs)
        {
            if(apply)
            {
                statsToModify.GetStatByType(buff.buffType).AddModifier(buff.buffValue,buffName);
            }
            else
            {
                statsToModify.GetStatByType(buff.buffType).RemoveModifier(buffName);
            }
        }
    }
}
