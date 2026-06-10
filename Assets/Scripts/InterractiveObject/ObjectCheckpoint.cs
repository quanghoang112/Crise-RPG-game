using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCheckpoint : MonoBehaviour, ISaveable
{
    public bool isActive {get;private set;}
    private Animator anim;
    // private ObjectCheckpoint[] allCheckpoints;
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;

    private AudioSource fireAudioSource;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        fireAudioSource = GetComponent<AudioSource>();
        // allCheckpoints = FindObjectsByType<ObjectCheckpoint>(FindObjectsSortMode.None);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        // if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
        // {
        //     // Nếu là file gốc trong thư mục Project, hãy xóa trắng ID đi để khi kéo vào Level nó tự sinh mã mới
        //     checkpointId = "";
        //     return;
        // }
        if(string.IsNullOrEmpty(checkpointId))
        {
            checkpointId =SceneManager.GetActiveScene().name + "_" + Guid.NewGuid().ToString();
        }
#endif
    }

    public string GetCheckpointId() => checkpointId;

    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;

    public void ActivateCheckpoint(bool activate)
    {
        isActive =activate;
        anim.SetBool("Enable",activate);

        if(isActive && fireAudioSource.isPlaying == false)
            fireAudioSource.Play();
        if(!isActive)
            fireAudioSource.Stop();
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
