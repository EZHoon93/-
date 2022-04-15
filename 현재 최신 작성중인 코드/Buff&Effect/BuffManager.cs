using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BuffManager : MonoBehaviourPun
{
    private void Awake()
    {
    }


    //public void CheckBuff(BuffController buffController, Define.BuffType buffType , float createTime, float durationTime)
    //{
    //    var buff = buffController.buffBaseList.Find(s => s.buffType == buffType);
    //    //없는 버프라면 생성
    //    //if (buff == null)
    //    //{
    //    //    buff = Managers.Resource.Instantiate($"Buff/{buffType.ToString()}").GetComponent<BuffBase>();
    //    //    buff.Setup(buffController, buffType);
    //    //    buff.transform.ResetTransform(buffController.transform);
    //    //    buffController.AddBuff(buff);
    //    //    buff.Play(createTime , durationTime);
    //    //}
    //    //else
    //    //{
    //    //    buff.RePlay(durationTime);
    //    //}
        


    

}
