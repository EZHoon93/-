﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExitGames.Client.Photon.StructWrapping;

using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T[] GetComponentsChildrenRemoveMy<T>(MonoBehaviour go) where T : UnityEngine.Component
    {
        List<T> result = new List<T>();
        go.GetComponentsInChildren<T>(result);

        if (result.Count > 1)
            result.RemoveAt(0);

        return result.ToArray();
    }

    //public static Transform FindChild(Tran string name)
    //{
    //    if (string.IsNullOrEmpty(name))
    //        return null;



    //}

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        var fields = type.GetProperties();
        foreach (var field in fields)
        {
            //Debug.Log(field.Name +"/"+field.CanRead +"/"+ field.CanWrite);
            if (field.CanWrite == false)
                continue;
            field.SetValue(copy, field.GetValue(original));
        }
    
        return copy as T;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {

            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
  
    public static void StartCoroutine(MonoBehaviour monoBehaviour, ref IEnumerator enumerator, IEnumerator nextEnumerator)
    {
        if (enumerator != null)
        {
            monoBehaviour.StopCoroutine(enumerator);
        }
        enumerator = nextEnumerator;
        monoBehaviour.StartCoroutine(enumerator);
    }

    //콜백 함수, time초뒤 해당 함수실행
    public static void CallBackFunction(MonoBehaviour monoBehaviour, float time, System.Action action)
    {
        monoBehaviour.StartCoroutine(CallBack(time, action));
    }

    static IEnumerator CallBack(float time, System.Action action)
    {
        if (time > 0)
            yield return new WaitForSeconds(time);
        else
            yield return null;
        action?.Invoke();
    }

    public static void ImageFillAmount(UnityEngine.UI.Image image, ref IEnumerator enumerator, float coolTime)
    {
        StartCoroutine(image, ref enumerator, ProcessCoolTime(image, coolTime));
    }


    static IEnumerator ProcessCoolTime(UnityEngine.UI.Image image, float coolTime)
    {
        //image.StopCoroutine()
        //GetComponent<IEnumerator>()
        image.enabled = true;
        var processCoolTime = coolTime;
        while (processCoolTime > 0)
        {
            processCoolTime -= Time.deltaTime;
            image.fillAmount = processCoolTime / coolTime;
            yield return null;
        }
        image.fillAmount = 0;
        image.enabled = false;
    }

    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static void UtillResetTransform( Transform target )
    {
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.Euler(Vector3.zero);
        
        //target.localScale = Vector3.one;
        
    }

    #region Color Text
    static string GetColorToCode(Color color)
    {
        //var result = "<"+_color.r
        string r = ((int)(color.r * 255)).ToString("X2");
        string g = ((int)(color.g * 255)).ToString("X2");
        string b = ((int)(color.b * 255)).ToString("X2");
        string a = ((int)(color.a * 255)).ToString("X2");
        string result = string.Format("{0}{1}{2}{3}", r, g, b, a);

        return result;
    }

    public static string GetColorContent(Color _color, string _content)
    {
        var colorHexCode = GetColorToCode(_color);

        var result = "<color=#" + colorHexCode + ">" + _content + "</color>";


        return result;

    }

    /// <summary>
    /// 초 => 00:00 타입 으로 전환
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    public static string GetTimeFormat(int newSec)
    {
        if (newSec < 0)
        {
            return "00:00";
        }
        int min = newSec / 60;
        int sec = newSec % 60;
        var timeStr = string.Format("{0:D1}:{1:D2}", min, (int)sec);
        return timeStr;

    }
    #endregion



    /// <summary>
    /// T열거형을 랜덤으로 1개뽑는다. 
    /// 매개변수가 있을시 해당 매개변수를 제외하고 뽑는다. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enum"></param>
    /// <returns></returns>
    public static T RandomEnum<T>(System.Enum @enum = null) where T : System.Enum
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        T result = default;
        do
        {
            int ran = UnityEngine.Random.Range(0, values.Length);
            result = (T)values.GetValue(ran);
            if(@enum == null)
            {
                return result;
            }
        } while (string.Compare(result.ToString(), @enum.ToString()) == 0);

        return result;
    }

    /// <summary>
    /// 랜덤으로 1개 타입형 선택, 인수값을 제외
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="existEnumArray"></param> 제외할 값
    /// <returns></returns>
    public static T GetRandomEnumTypeExcept<T>(T[] existEnumArray) where T : Enum
    {
        var enumAllArray = EnumToArray<T>();   //열거형 목록전부 배열ㅁ생성

        var randomSelect= enumAllArray.Where(s => existEnumArray.Contains(s) == false).OrderBy(s => Guid.NewGuid()).Single();

        return randomSelect;
    }
    public static T[] EnumToArray<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        T[] result = new T[values.Length];
        int i = 0;
        foreach(var  value in values)
        {
            result[i] = (T)value;
            i++;
        }
        return result;

    }
    public static int[] EnumToIntArray<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        int[] result = new int[values.Length];
        int i = 0;
        foreach (var value in values)
        {
            result[i] = (int)value;
            i++;
        }
        return result;

    }
    public static Enum GetEnumByIndex<T>(int index)
    {
        Array values = Enum.GetValues(typeof(T));

        foreach(var @enum  in values)
        {
            if(index == (int)@enum)
            {
                return (Enum)@enum;
            }
        }

        return null;
    }

}
