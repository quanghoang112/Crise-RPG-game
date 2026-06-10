using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;

    public UI ui;
    private Vector3 lastPlayerPosition;

    private string lastScenePlayed = "Level_0";
    private bool dataLoaded;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        ui = FindAnyObjectByType<UI>();
    }

    // public void SetLastDeathPosition(Vector3 position) => lastPlayerPosition = position;
    
    public void ContinuePlay()
    {
        Debug.Log(lastScenePlayed);
        ChangeScene(lastScenePlayed, RespawnType.None);
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.None);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        //trước đó dataLoaded = true
        SaveManager.instance.SaveGame();// vào đây sẽ đổi về false, nhưng mọi thứ được thực hiện trong 1 frame
        //nên giá trị dataLoaded nhận được ở StartCoroutine lại là true
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName,respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        //Fade effect
        UI_FadeScreen fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeOut();

        yield return fadeScreen.fadeEffectCo;

        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);

        //nếu không set dòng này, hoặc đợi 1 frame thì khả năng cao là data k có thời gian để load
        // không chạy được dòng while
        dataLoaded = false; // becomes true when SceneLoadScene -> load data game from save manager
        yield return null; // sẽ luôn để delay ít nhất 1 frame để loadScene

        while(dataLoaded == false)
        {
            yield return null;
        }

        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn();
        
        Player player = Player.instance;

        if(player == null)
            yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if(position != Vector3.zero)
            Player.instance.TeleportPLayer(position);
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        return FindAnyObjectByType<UI_FadeScreen>();
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if(type == RespawnType.Portal)
        {
            ObjectPortal portal = ObjectPortal.instance;

            Vector3 position = portal.GetPosition();

            portal.SetTrigger(false);
            portal.DisableIfNeeded();

            return position;
        }
        if(type == RespawnType.None)
        {
            var data =SaveManager.instance.GetGameData();
            var checkpoints =  FindObjectsByType<ObjectCheckpoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();

            var enterWaypoints = FindObjectsByType<ObjectWaypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
                .Select(wp => wp.GetPosition())
                .ToList();

            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList();

            if(selectedPositions.Count == 0)
                return Vector3.zero;
            
            return selectedPositions.OrderBy(postion => Vector3.Distance(postion, lastPlayerPosition)).First();
        }
        return GetWaypointPosition(type);;
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<ObjectWaypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if(point.GetWaypointType() ==type)
            {
                point.SetCanBeTriggered(false);
                return point.GetPosition();
            }
        }
        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;

        Debug.Log(string.IsNullOrEmpty(lastScenePlayed));

        // if(string.IsNullOrEmpty(lastScenePlayed))
        //     lastScenePlayed = "Level_0";
        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "MainMenu")
        {
            return;
        }

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;
        dataLoaded = false;
    }
}
