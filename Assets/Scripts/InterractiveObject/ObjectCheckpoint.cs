using System;
using UnityEngine;

public class ObjectCheckpoint : MonoBehaviour, ISaveable
{
    public bool isActive {get;private set;}
    private Animator anim;
    // private ObjectCheckpoint[] allCheckpoints;
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        // allCheckpoints = FindObjectsByType<ObjectCheckpoint>(FindObjectsSortMode.None);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if(string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = Guid.NewGuid().ToString();
        }
#endif
    }

    public string GetCheckpointId() => checkpointId;

    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;

    public void ActivateCheckpoint(bool activate)
    {
        isActive =activate;
        anim.SetBool("Enable",activate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // foreach(var point in allCheckpoints)
        //     point.ActivateCheckpoint(false);

        
        ActivateCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpoints.TryGetValue(checkpointId, out active);
        ActivateCheckpoint(active);
    }

    public void SaveData(ref GameData data)
    {
        if(isActive == false)
            return;

        if(data.unlockedCheckpoints.ContainsKey(checkpointId) == false)
        {
            data.unlockedCheckpoints.Add(checkpointId, true);
        }
    }
}
