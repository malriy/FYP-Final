using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEvent
{
    public delegate void TriggerAction();

    public static event TriggerAction OnTrigger;

    public static void Trigger()
    {
        OnTrigger?.Invoke();
    }
}

