using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_ThrowSword
{
    private int amountToPierce;

    public override void SetupSword(SkillThrowSword swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        amountToPierce = swordManager.amountToPierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // base.OnTriggerEnter2D(collision);

        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if(amountToPierce <= 0 || groundHit)
        {
            DamageEnemiesInRadius(transform,1);
            StopSword(collision);
            return;
        }

        amountToPierce--;
        DamageEnemiesInRadius(transform,1);
    }
}
