using System;
using System.IO;
using UnityEngine;

public class FileDataHandler //class xử lý việc tạo file, fill data trong class gameData và serialize 
// tương tự với chiều load game
{
    private string fullPath;
    private bool encryptData;
    private string codeWord = "quanghoang112";

    public FileDataHandler(string dataDirPath, string dataFileName, bool ecryptData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
        this.encryptData = ecryptData;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToSave = JsonUtility.ToJson(gameData, true);

            if(encryptData)
                dataToSave = EncryptDecrypt(dataToSave);
            //Create/Open a new file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file: "+fullPath + "\n" + e);            
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error on trying to load data from file: " + fullPath + "\n" +e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for(int i = 0;i< data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i%codeWord.Length]);
        }

        return modifiedData;
    }
}
