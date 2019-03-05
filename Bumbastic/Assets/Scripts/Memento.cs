using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Memento : MonoBehaviour
{
    public static Memento instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public bool ExistsDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_TypeToSave">0 is for Settings</param>
    public void SaveData(int _TypeToSave) {
        if (!ExistsDirectory())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }
        switch (_TypeToSave)    
        {
            case 0:
                if (!Directory.Exists(Application.persistentDataPath + "/game_save/settings_data"))
                {
                    Directory.CreateDirectory(Application.persistentDataPath + "/game_save/settings_data");
                }
                FileStream file = File.Create(Application.persistentDataPath + "/game_save/settings_data/settings_save.txt");
                BinaryFormatter bf = new BinaryFormatter();
                var json = JsonUtility.ToJson(Resources.Load("ScriptableObjects/Settings"));
                Debug.Log(json.ToString());
                bf.Serialize(file, json);
                file.Close();
                break;
            default:
                break;
        }
    }

    private void LoadData() {
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/settings_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/settings_data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/game_save/settings_data/settings_save.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_save/settings_data/settings_save.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), Resources.Load("ScriptableObjects/Settings"));
            file.Close();
        }  
    }
}
