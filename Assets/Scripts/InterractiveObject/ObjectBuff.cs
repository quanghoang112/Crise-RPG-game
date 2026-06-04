using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;




public class ObjectBuff : MonoBehaviour
{
    private PlayerStats statsToModify;


    [Header ("Buff details")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    // [SerializeField] private float buffValue;
    [SerializeField] private float buffDuration = 4;
    
    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0,yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        statsToModify = collision.GetComponent<PlayerStats>();
        
        if(statsToModify.CanApplyBufffOf(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }
}
