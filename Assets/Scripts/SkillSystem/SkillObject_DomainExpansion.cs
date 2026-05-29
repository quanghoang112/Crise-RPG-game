using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private SkillDomain domainManager;

    private float expandSpeed = 2;
    private float slowDownPercent = .9f;
    private float duration;


    private bool isShrinking;
    private Vector3 targetScale;



    public void SetupDomain (SkillDomain domainManager)
    {
        this.domainManager = domainManager;

        float maxSize = domainManager.maxDomainSize;
        duration = domainManager.GetDomainDuration();
        expandSpeed = domainManager.expandSpeed;
        slowDownPercent = domainManager.GetSlowPercentage();


        targetScale = Vector3.one * maxSize;

        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > .1f;

        if(shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);
        
        if(isShrinking && sizeDifference < .1f)
        {
            domainManager.ClearTargets();
            Destroy(gameObject);
        }
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy == null)
            return;
        domainManager.AddTarget(enemy);
        enemy.slowDownEntityBy(duration,slowDownPercent,true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy == null)
            return;
        
        enemy.StopSlowDownEntityBy();
    }

}
