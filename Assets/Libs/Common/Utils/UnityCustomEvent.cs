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

[System.Serializable]
public class UnityRaycastEvent : UnityEvent<Vector3, Collider>
{

}

[System.Serializable]
public class Unity3Vector3Event : UnityEvent<Vector3>
{

}