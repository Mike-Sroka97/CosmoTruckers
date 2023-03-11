using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static MyIntEvent UnityIntEvent = new MyIntEvent();
    public static MyFloatEvent UnityFloatEvent = new MyFloatEvent();
    public static MyStringEvent UnityStringEvent = new MyStringEvent();
}

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
}

[System.Serializable]
public class MyStringEvent : UnityEvent<string>
{
}

[System.Serializable]
public class MyFloatEvent : UnityEvent<float>
{
}
