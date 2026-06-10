using UnityEngine;

public class EntitySFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("SFX Names")]
    [SerializeField] private string attackHit;
    [SerializeField]private string attackMiss;
    [Space]
    private float soundDistance = 15f;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(attackHit,audioSource,soundDistance);
    }

    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX(attackMiss,audioSource,soundDistance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color =Color.cyan;
        Gizmos.DrawWireSphere(transform.position,soundDistance);
    }

}
