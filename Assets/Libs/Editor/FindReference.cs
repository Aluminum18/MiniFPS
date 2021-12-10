using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;

public static class FindReference
{
    [MenuItem("Utils/Find Reference")]
    public static void FindReferenceOnScene()
    {
        var selectedObj = Selection.activeObject;
        if (selectedObj == null)
        {
            return;
        }
        Debug.Log($"Start Searching for [{selectedObj.name}]-----------------------------------------");

        GameObject[] rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < rootObjs.Length; i++)
        {
            FindObjRefOfType(rootObjs[i], selectedObj);
        }
        Debug.Log("End Searching-----------------------------------------");
    }

    private static void FindObjRefOfType(GameObject sceneObject, Object selectedObj)
    {
        Component[] components = sceneObject.GetComponents(typeof(Component));

        for (int i = 0; i < components.Length; i++)
        {
            FindRefInComponent(components[i], selectedObj);
        }

        int childCount = sceneObject.transform.childCount;
        if (childCount == 0)
        {
            return;
        }

        for (int i = 0; i < childCount; i++)
        {
            var childObj = sceneObject.transform.GetChild(i).gameObject;
            FindObjRefOfType(childObj, selectedObj);
        }
    }

    private static void FindRefInComponent(Component component, Object obj)
    {
        FieldInfo[] fieldInfos = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            var field = fieldInfos[i].GetValue(component);
            if (field == null)
            {
                continue;
            }

            if (!(field is Object))
            {
                continue;
            }

            Object fieldObj = (Object)field;

            if (fieldObj.GetInstanceID().Equals(obj.GetInstanceID()))
            {               
                Debug.Log($"Reference in component [{component.GetType().ToString()}] in object [{component.name}]", component.gameObject);
            }
        }
    }
}
