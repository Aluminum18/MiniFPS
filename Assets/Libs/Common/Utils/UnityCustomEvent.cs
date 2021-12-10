using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject>
{
    
}

[System.Serializable]
public class Unity2GameObjectsEvent : UnityEvent<GameObject, GameObject>
{

}