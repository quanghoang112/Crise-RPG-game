using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;
    
    [SerializeField] private string fileName ="savefile.json";
    [SerializeField] private bool encryptData = true;

    private void Awake()
    {
        // if(instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        //     Destroy(gameObject);
        instance = this;
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath); //thống nhất folder lưu save file trên mọi hệ thống
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        allSaveables = FindISaveable();
        //có option 2 là thay vì tìm all ISaveable thì ta cho các class đăng ký
        yield return null;

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if(gameData == null)
        {
            Debug.Log("No save data found, creating new save!");
            gameData = new GameData();
            return;
        }

        foreach(var saveable in allSaveables)
            saveable.LoadData(gameData);
    }

    public void SaveGame()
    {
        Debug.Log("Saving game...");
        foreach(var saveable in allSaveables)
        {
            saveable.SaveData(ref gameData); // lưu data dưới class gameData
        }

        dataHandler.SaveData(gameData);// serialize về .json và lưu local
    }

    public GameData GetGameData() => gameData;

    [ContextMenu("Delete save data file")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();

        gameData.skillPoints = -1;
        gameData.gold = -1;
        LoadGame();
    }

    private List<ISaveable> FindISaveable() //option 1: tìm mọi gameObject active và inactive có các component này
    {
        return 
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }

}
