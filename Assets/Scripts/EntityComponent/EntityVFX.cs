using UnityEngine;
using System.Collections;

public class EntityVFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.2f;
    private Material defaultMaterial;
    private Coroutine onDamageVFXCoroutine = null;

    
    [Header("On Doing Damage VFX")]
    [SerializeField] private Color onHitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [Space]
    [SerializeField] private Color onCritHitVFXColor = Color.red;
    [SerializeField] private GameObject CritHitVFX;
    [Space]
    // [SerializeField] private GameObject lightningVFX;

    [Header("Elemental color")]
    [SerializeField] private Color chillVFX = Color.cyan;
    [SerializeField] private Color burnVFX = Color.red;
    [SerializeField] private Color electrifyVFX = Color.yellow;
    [SerializeField] private Color originalHitVFXColor;

    private Coroutine StatusVfxCo;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        defaultMaterial = sr.material;
        originalHitVFXColor = onHitVFXColor;
    }


    public void PlayStatusVfx(float duration, ElementType element)
    {
        if(StatusVfxCo != null)
        {
            StopCoroutine(StatusVfxCo);
        }
        if(element == ElementType.Ice)
        {
            // Debug.Log("asdasd");
            StatusVfxCo=StartCoroutine(PlayStatusVfxCo(duration,chillVFX));
        }
        else if (element == ElementType.Fire)
            StatusVfxCo=StartCoroutine(PlayStatusVfxCo(duration,burnVFX));
        else if (element == ElementType.Lightning)
            StatusVfxCo = StartCoroutine(PlayStatusVfxCo(duration,electrifyVFX));

    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sr.color=Color.white;
        sr.material = defaultMaterial;
    }
    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0;
        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 0.9f;

        bool toggle = false;

        while (timeHasPassed < duration)
        {
            sr.color = toggle ? lightColor:darkColor;
            toggle =!toggle;
            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }
        sr.color = Color.white;
    }
    public void CreateOnHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX,target.position,Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = onHitVFXColor;
        
    }
    public void CreateOnCritHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(CritHitVFX,target.position,Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = onCritHitVFXColor;
        if(entity.facingDir == -1)
            vfx.transform.Rotate(0f,180f,0f);
            
    }

    // public void LightningVFX(Transform target)
    // {
    //     GameObject vfx = Instantiate(lightningVFX,target.position,Quaternion.identity);
    //     // vfx.GetComponentInChildren<SpriteRenderer>().color = onHitVFXColor;
        
    // }

    public void updateOnHitVFXColor(ElementType element)
    {
        if (element == ElementType.Ice)
            onHitVFXColor = chillVFX;

        if(element == ElementType.None)
            onHitVFXColor = originalHitVFXColor;
    }
    private IEnumerator onDamageVFXCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = defaultMaterial;
    }
    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);
        onDamageVFXCoroutine = StartCoroutine(onDamageVFXCo());
    }
}
