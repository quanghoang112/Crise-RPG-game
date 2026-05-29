using UnityEngine;

public class PlayerAnimTrigger : EntityAnimTrigger
{
    private Player player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    private void ThrowSword() => player.skillManager.throwSword.throwSword();
}
