using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Action/", fileName = "Action ")]
public class ActionBase : ScriptableObject
{
    public virtual void OnUpdate()
    {

    }
    public virtual void OnStart()
    {

    }

    public virtual void OnEnd()
    {

    }
}
