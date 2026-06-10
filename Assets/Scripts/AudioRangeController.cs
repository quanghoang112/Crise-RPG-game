using System;
using UnityEngine;

public class AudioRangeController : MonoBehaviour
{
    private AudioSource source;
    private Transform player;

    [SerializeField]private float minDistanceToHear = 12;
    
    private float maxVolume;

    private void Start()
    {
        player = Player.instance.transform;
        source = GetComponent<AudioSource>();

        maxVolume = source.volume;
    }

    private void Update()
    {
        if(player == null)
            return;

        float distance = Vector2.Distance(player.position, transform.position);
        float t = Mathf.Clamp01(1 - (distance / minDistanceToHear));    
    
        float targetVolume = Mathf.Lerp(0,maxVolume,t*t);
        source.volume = Mathf.Lerp(source.volume,targetVolume,Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color =Color.cyan;
        Gizmos.DrawWireSphere(transform.position,minDistanceToHear);
    }
}
