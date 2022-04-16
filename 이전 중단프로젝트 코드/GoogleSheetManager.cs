using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class serverDataEtc
{
    public string key;
    public string value;
}


[System.Serializable]
public class GoogleData
{
    public List<serverCharacterData> dataCharacter;
    public List<serverProjectileData> dataProjectile;
    public List<serverDataEtc> dataEtc;
}
public class GoogleSheetManager : MonoBehaviour
{
    public GoogleData GD;
    const string URL = "https://script.google.com/macros/s/AKfycbyhAgHCJuntujgoQS2tD7WhJ5avJ7FmUJlDEAL6sB60fTbEoSNrhELjEaU-YQKIiSaF/exec";


    public bool isDataOk;
    void SetData()
    {
        //캐릭터 데이터 셋업
        foreach (var c in GD.dataCharacter)
        {
            var characterScriptableData = DataEtc.Instance.GetCharacterDataByServerKey(c.key);
            characterScriptableData.SetupProductData(c);
        }

        //무기 데이터 셋업
        foreach (var p in GD.dataProjectile)
        {
            var projectileScriptableData = DataEtc.Instance.GetProjectileDataByServerKey(p.key);
            projectileScriptableData.SetupData(p);

        }

        foreach (var e in GD.dataEtc)
        {
            var etcScriptableData = DataEtc.Instance.GetEtcDataByServerKey(e.key);
            etcScriptableData.SetupData(e);
        }

        AbilitySkill_HardCoding();
    }
    

    /// <summary>
    /// 스킬 데이터 강제로 하드코딩해서 /.
    /// </summary>
    void AbilitySkill_HardCoding()
    {

        var run = DataContainer.Instance.GetAbilityByType(AbilityType.AP_r) as SkillContainer;
        var dark = DataContainer.Instance.GetAbilityByType(AbilityType.AP_d) as SkillContainer;
        var heal = DataContainer.Instance.GetAbilityByType(AbilityType.AP_h) as SkillContainer;
        var shield = DataContainer.Instance.GetAbilityByType(AbilityType.AP_s) as SkillContainer;

        run.SetUpHardCoding(DataEtc.Instance.sRunCoolTime, DataEtc.Instance.sRunDurationTime);
        dark.SetUpHardCoding(DataEtc.Instance.sDarkCoolTime, DataEtc.Instance.sDarkDurationTime);
        heal.SetUpHardCoding(DataEtc.Instance.sHealCoolTime, 0);
        shield.SetUpHardCoding(DataEtc.Instance.sShieldCoolTime, DataEtc.Instance.sShieldDurationTime);






    }




    public void GetServerData()
    {
        isDataOk = false;
        WWWForm form = new WWWForm();
        form.AddField("order", "data");
        StartCoroutine(Post(form));
    }


    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
            if (www.isDone) Response(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
    }

    void Response(string json)
    {
        isDataOk = true;
        GD = JsonUtility.FromJson<GoogleData>(json);    //Json으로 파싱
        SetData(); //데이터 세팅
    }


}
