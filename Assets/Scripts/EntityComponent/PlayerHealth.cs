using UnityEngine;

public class PlayerHealth : EntityHealth
{

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Die();
        }
    }
    protected override void Die()
    {
        base.Die();

        // GameManager.instance.SetLastDeathPosition(transform.position);
        // GameManager.instance.RestartScene();
        Player.instance.ui.OpenDeathSreenUI();
    }
}
