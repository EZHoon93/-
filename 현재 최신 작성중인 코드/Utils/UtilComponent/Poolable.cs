using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    Pool _pool;
    [HideInInspector]
	public bool IsUsing;

	public virtual void Push()
    {
        _pool.Push(this);
    }
}