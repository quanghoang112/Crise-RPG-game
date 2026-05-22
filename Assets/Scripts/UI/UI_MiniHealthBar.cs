using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    private void OnEnable()
    {
        entity.onFlipped += HandleFlipped;        
    }
    private void OnDisable()
    {
        entity.onFlipped -= HandleFlipped;
    }
    private void HandleFlipped()
    {
        transform.rotation = Quaternion.identity; // Keep the health bar upright
    }
}
