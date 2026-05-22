using UnityEngine;

public class PlayerCombat : EntityCombat
{
    private Player player => GetComponent<Player>();
    public bool CounterAttackPerformed()
    {
        bool hasCounteredSomebody = false;
        foreach(var targetCollider in handleTargetDetection())
        {
            ICounterable counterable = targetCollider.GetComponent<ICounterable>();
            if(counterable == null) continue;
            if(counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasCounteredSomebody = true;
            }
        }
        return hasCounteredSomebody;
    }
    public float getCounterDuration() => player.counterAttackDuration;
}
