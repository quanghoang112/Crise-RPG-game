using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Portal Scroll", fileName = "Item effect data - Portal Scroll")]

public class ItemEffect_Portal : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        // base.ExecuteEffect();
        // Debug.Log("asd");

        if(SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("Cannot open portal in town!");
            return;
        }

        Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(player.facingDir * 1.5f, 0);

        ObjectPortal.instance.ActivatePortal(portalPosition, player.facingDir);
    }
}
