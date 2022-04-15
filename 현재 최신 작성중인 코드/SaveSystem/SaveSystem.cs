using System;
using System.IO;
using UnityEngine;


[CreateAssetMenu( menuName = "Single/System")]
public class SaveSystem : DescriptionBaseSO
{
    [Header("Datas")]
    [SerializeField] private UserInfoSO _userInfo = default;
    [SerializeField] private SettingInfoSO _settingInfoSO = default;

    [Header("BroadCasting")]
    [SerializeField] private VoidEventChannelSO _completeLoadDataSO;


    private readonly string jsonDataName = "saveData.json";
    //private readonly string optionDataName = "optionData.json";
    public SaveData saveData = new SaveData();


    private void OnEnable()
    {
        //_completeLoadDataSO.RaiseEvent();
    }

    private void OnDisable()
    {
        
    }




    public static bool SaveData(object userData, string name)
    {
        try
        {

            string jsonData = JsonUtility.ToJson(userData, true);
#if false   //암호화
            name += ".dat";
            string path = Path.Combine(Application.persistentDataPath, name);
            Debug.Log(path + "@@@@");

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            string code = System.Convert.ToBase64String(bytes);
            File.WriteAllText(path, code);

#endif
#if true    //비암호화
            string path = Path.Combine(Application.persistentDataPath, name);
            File.WriteAllText(path, jsonData);
#endif
        }
        catch (Exception)
        {
            //Debug.Log("저장실패");

            return false;
        }
        //string path = Path.Combine(Application.dataPath, name);
        return true;

    }

    public T LoadData<T>(string name)
    {
        if (!DoseSaveGameExist(name))
        {
            //Debug.Log("lod x");
            return default;
        }
        try
        {

            string path = Path.Combine(Application.persistentDataPath, name);
            string code = File.ReadAllText(path);
#if false //암호화
            byte[] bytes = System.Convert.FromBase64String(code);
            string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
#endif
#if true //비암호화
            string jsonData = File.ReadAllText(path);
#endif
            var userData = JsonUtility.FromJson<T>(jsonData);

            return userData;
        }
        catch (Exception)
        {
            return default;
        }


    }

    public bool DeleteSaveGame(string name)
    {
        try
        {
            //Debug.Log("삭제중");
            string path = Path.Combine(Application.persistentDataPath, name);
            File.Delete(path);
        }
        catch (Exception)
        {
            //Debug.Log("삭제실패");

            return false;
        }
        return true;
    }
    public bool DoseSaveGameExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }


    private string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name);

    }

    private void SaveSettings()
    {
        //saveData.SaveSettings(_currentSettings);

    }
}
