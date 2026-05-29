using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillThrowSword : SkillBase
{
    private SkillObject_ThrowSword currentSword;
    // private int constantPower=10;

    [Range(0f,10f)]
    [SerializeField] private float throwPower = 5f;
    [SerializeField] private float swordGravity = 3.5f;
    [SerializeField] private GameObject swordPrefab;
    
    [Header("Pierce Sword Upgrade")]
    [SerializeField] private GameObject pierceSwordPrefab;
    [SerializeField] public int amountToPierce = 2;

    [Header("Spin Sword Upgrade")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int maxDistance=5;
    public float attacksPerSecond=6;
    public float maxSpinDuration =3;

    [Header("Bounce Sword Upgrade")]
    [SerializeField] private GameObject bounceSwordPrefab;
    public int bounceCount;
    public float bounceSpeed;

    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = .05f;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    
    protected override void Awake()
    {
        base.Awake();
        // skillType = SkillType.SwordThrow;
        // upgradeType = SkillUpgradeType.None;
        // cooldown = 0;
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;

        dots = GenerateDots();
    }

    public void throwSword()
    {
        // Debug.Log("throwSword");
        GameObject newSword = Instantiate(GetSwordPrefab(), dots[1].position, Quaternion.identity);
        currentSword = newSword.GetComponent<SkillObject_ThrowSword>();
        currentSword.SetupSword(this,GetThrowPower());
    
    }

    private GameObject GetSwordPrefab()
    {
        if(Unlocked(SkillUpgradeType.SwordThrow))
        {
            return swordPrefab;
        }
        if(Unlocked(SkillUpgradeType.SwordThrowPierce))
        {
            return pierceSwordPrefab;
        }
        if(Unlocked(SkillUpgradeType.SwordThrowSpin))
        {
            return spinSwordPrefab;
        }
        if(Unlocked(SkillUpgradeType.SwordThrowBounce))
        {
            return bounceSwordPrefab;
        }
        Debug.Log("No valid sword upgrade");
        return null;
    }

    private Vector2 GetThrowPower() => confirmedDirection * (throwPower * 10);


    public override bool CanUseSkill()
    {
        if(currentSword != null)    
        {
            currentSword.GetSwordBackToPlayer();
            return false;
        }
        return base.CanUseSkill();

    }
    
    public void PredictTrajectory (Vector2 direction)
    {
        for(int i = 0;i< dots.Length;i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i*spaceBetweenDots);
        }    
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = throwPower * 10;

        Vector2 initialVeclocity = direction * scaledThrowPower;

        Vector2 gravityEffect = .5f * Physics2D.gravity * swordGravity * (t*t);
    
        Vector2 predictedPoint = (initialVeclocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;
    public void EnableDots(bool enable)
    {
        foreach(Transform t in dots)
            t.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for(int i = 0;i< numberOfDots;i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }
        return newDots;
    }
}
