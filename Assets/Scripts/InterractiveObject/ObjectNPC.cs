using UnityEngine;

public class ObjectNPC : MonoBehaviour,IInteractable
{
    [Header("Quest info")]
    [SerializeField] private string npcQuestTargetId;
    [SerializeField] private RewardType rewardNpc;
    [Space]
    protected Transform player;
    protected UI ui;
    protected PlayerQuestManager questManager;
    protected EntityDropManager dropManager;

    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactToolTip;
    private bool facingRight = true;

    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        dropManager = GetComponent<EntityDropManager>();
        ui = FindAnyObjectByType<UI>();
        startPosition = interactToolTip.transform.position;
        interactToolTip.SetActive(false);
    }

    protected virtual void Start()
    {
        questManager = Player.instance.questManager;
        
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }

    private void HandleToolTipFloat()
    {
        if(interactToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = startPosition + new Vector3(0,yOffset);
        }
    }

    private void HandleNpcFlip()
    {
        if (player == null || npc == null)
            return;
        if(npc.position.x > player.position.x && facingRight)
        {
            npc.transform.Rotate(0f,180f,0f);
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && facingRight == false)
        {
            npc.transform.Rotate(0f,180f,0f);
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactToolTip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactToolTip.SetActive(false);
    }

    public virtual void Interact()
    {
        questManager.AddProgress(npcQuestTargetId,dropManager: dropManager);
        questManager.TryGiveRewardFrom(rewardNpc, dropManager: dropManager);
    }
}
