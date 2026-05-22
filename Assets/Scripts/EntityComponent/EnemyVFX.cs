using Unity.VisualScripting;
using UnityEngine;

public class EnemyVFX : EntityVFX
{
    private Enemy enemy => GetComponent<Enemy>();
    
    [Header("Counter Attack Window")]
    [SerializeField] private GameObject attackAlert;
    protected override void Awake()
    {
        base.Awake();
        attackAlert.SetActive(false);
    }

    public void EnableAttackAlert(bool enable) => attackAlert.SetActive(enable);
}
