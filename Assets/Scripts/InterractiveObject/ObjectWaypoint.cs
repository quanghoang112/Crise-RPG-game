using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectWaypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [SerializeField] private TextMeshPro transferToSceneName;
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType connectedWaypoint;
    [SerializeField] private Transform respawnPoint;
    
    [SerializeField] private bool canBeTriggered = true;


    public void SetCanBeTriggered(bool canBeTriggered) => this.canBeTriggered = canBeTriggered;

    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + transferToScene + " - " + waypointType.ToString();
        transferToSceneName.text = transferToScene;

        if(waypointType == RespawnType.Enter)
            connectedWaypoint = RespawnType.Exit;

        if(waypointType == RespawnType.Exit)
            connectedWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(canBeTriggered == false)
            return;
        // SaveManager.instance.SaveGame();
        // SceneManager.LoadScene(transferToScene);
        GameManager.instance.ChangeScene(transferToScene, connectedWaypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
    
    public RespawnType GetWaypointType() => waypointType;

    public Vector3 GetPosition()
    {
        canBeTriggered = false;
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

}
