using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Memento : MonoBehaviour
{
    public static Memento instance;

    string path;
    Settings settings;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        path = Application.persistentDataPath + "/game_save/settings_data/settings_save.txt";
         settings = Resources.Load<Settings>("ScriptableObjects/Settings");
    }

    public delegate void DelMemento();
    public DelMemento OnLoadedData;


    private void Start()
    {
        MenuUI.OnLoadData += LoadData;
    }

    public bool ExistsDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData(0);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }
#endif

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_TypeToSave">0 is for Settings</param>
    public void SaveData(int _TypeToSave)
    {
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

                string savedJson = JsonUtility.ToJson(settings);

                if (File.Exists(path))
                {
                    File.WriteAllText(path, savedJson);
                }
                else
                {
                    FileStream file = File.Create(path);
                    File.WriteAllText(path, savedJson);
                    file.Close();
                }

                Debug.Log(savedJson);

                //BinaryFormatter bf = new BinaryFormatter();
                //var json = JsonUtility.ToJson(Resources.Load(resourceSettings));
                //bf.Serialize(file, json);
                break;
            default:
                break;
        }
    }

    public void LoadData()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/game_save/settings_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save/settings_data");
        }
        //BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(path))
        {
            string file = File.ReadAllText(path);
            Debug.Log(file);
            //Settings settings = JsonUtility.FromJson<Settings>(file);
             
            OnLoadedData?.Invoke();//AudioMute,.
        }
    }
}
