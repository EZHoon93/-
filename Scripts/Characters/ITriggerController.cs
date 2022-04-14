using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerController 
{
    void OnTriggerChangeDetected(bool enter, GameObject other);
}
