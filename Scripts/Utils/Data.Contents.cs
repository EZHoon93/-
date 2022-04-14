using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;
using UnityEngine.Events;
namespace Data
{


    [System.Serializable] public class floatEvent : UnityEvent<float> { }

    [System.Serializable] public class PhotonInstantiateEvent : UnityEvent<PhotonMessageInfo> 
    {
        public static PhotonInstantiateEvent operator +(PhotonInstantiateEvent a , UnityAction<PhotonMessageInfo> b)
        {
            a.AddListener(b);
            return a ;
        }
        public static PhotonInstantiateEvent operator -(PhotonInstantiateEvent a, UnityAction<PhotonMessageInfo> b)
        {
            a.RemoveListener(b);
            return a;
        }
    }

    [System.Serializable]
    public class PhotonDestoryEvent : UnityEvent<PhotonView>
    {
        public static PhotonDestoryEvent operator +(PhotonDestoryEvent a, UnityAction<PhotonView> b)
        {
            a.AddListener(b);
            return a;
        }
        public static PhotonDestoryEvent operator -(PhotonDestoryEvent a, UnityAction<PhotonView> b)
        {
            a.RemoveListener(b);
            return a;
        }
    }
    

    #region User

    [Serializable]
    public class UserData
    {
        public string key;
        public string nickName;
        public int level;
        public int coin;
        public int gem;
        public int exp;
        public int maxExp;

        public List<AvaterSlotInfo> avaterList;
        //최초 생성
        public UserData(string _key, string _nickName)
        {
            key = _key;
            nickName = _nickName;
            level = 1;
            coin = 0;
            exp = 0;
            gem = 0;
            maxExp = 10;
            avaterList = new List<AvaterSlotInfo>()
            {
                new AvaterSlotInfo()
            };
        }
        public void     OnPho(PhotonMessageInfo photonMessageInfo)
        {

        }
        public AvaterSlotInfo GetCurrentAvater()
        {
            var index = GetCurrentAvaterIndex();

            if(avaterList.Count > index)
            {
                return avaterList[index];
            }
            //만약 없으면..
            return null;
        }

        public int GetCurrentAvaterIndex()
        {
            return PlayerPrefs.GetInt("av");
        }

        public bool UseNewCharacterAvater(int index)
        {
            if(index > avaterList.Count)
            {
                //실패 
                return false;
            }
            else
            {
                PlayerPrefs.SetInt("av", index);

                return true;
            }
        }

    }
    [Serializable]
    public class ServerKey
    {
        public bool isUsing;
        public string avaterSeverKey;
        public string weaponSeverKey;
        public string accesoryKey;
        public ServerKey(string newServerKey, string newWeaponServerKey, bool newUsing)
        {
            avaterSeverKey = newServerKey;
            weaponSeverKey = newWeaponServerKey;
            isUsing = newUsing;
        }
    }

    [Serializable]
    public class AvaterSlotInfo
    {
        public string characterAvaterKey;
        public string weaponKey;
        public string accesoryKey;
        public bool isSelect;

        public AvaterSlotInfo()
        {
            characterAvaterKey = "Ch01";
            weaponKey = "Wm01";
            accesoryKey = null;
            isSelect = false;
        }
    }

    [Serializable]
    public class SkinHasData
    {
        public bool isUsing;
        public string avaterKey;

        public SkinHasData(string key, bool newUsing)
        {
            isUsing = newUsing;
            avaterKey = key;
        }
    }



    [System.Serializable]
    public class OptionData
    {
        public float bgmValue;
        public float soundValue;
        public bool isLeftHand;
        //public List<InputUIInfo> joystickSettings = new List<InputUIInfo>();
        
        
        //기본값 초기 생성 값
        public OptionData()
        {
            bgmValue = .5f;
            soundValue = .5f;
            isLeftHand = false;

//            joystickSettings.Clear();

            //foreach(var joystick in Managers.Input.Controllers)
            //{
            //    var ultiMateJoystick = joystick.UltimateJoystick;
            //    var name = ultiMateJoystick.joystickName;
            //    var pos = new Vector2( ultiMateJoystick.positionHorizontal , ultiMateJoystick.positionVertical);
            //    var size = ultiMateJoystick.joystickSize;
            //    var newInputInfo = new InputUIInfo(name, pos, size);
            //    joystickSettings.Add(newInputInfo);
            //}
        }
    }

    [Serializable]
    public class InputUIInfo
    {
        public string name;
        public Vector2 pos;
        public float size;

        public InputUIInfo(string _name , Vector2 _pos , float _size)
        {
            name = _name;
            pos= _pos;
            size = _size;
        }
    }

    [Serializable]
    public class CharcterStatInfo
    {
        public string info;
    }

    #endregion

}