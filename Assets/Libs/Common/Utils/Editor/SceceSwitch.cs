using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceceSwitch : MonoBehaviour
{
    private const string SCENE_PATH = "Assets/1_Game/Scenes/";
    private const string GAME_FIELD_SCENE = "GameField.unity";
    private const string TITLE_SCENE = "Title.unity";
    private const string GAME_FIELD_VR_SCENE = "GameFieldVRScene.unity";
    private const string BUFFER_SCENE = "BufferScene.unity";

    [MenuItem("ChangeScene/GameField")]
    public static void ToGameFieldScene()
    {
        OpenScene(GAME_FIELD_SCENE);
    }

    [MenuItem("ChangeScene/GameFieldVr")]
    public static void ToGameFieldVrScene()
    {
        OpenScene(GAME_FIELD_VR_SCENE);
    }

    [MenuItem("ChangeScene/Title")]
    public static void ToTitleScene()
    {
        OpenScene(TITLE_SCENE);
    }

    [MenuItem("ChangeScene/Buffer")]
    public static void ToBufferScene()
    {
        OpenScene(BUFFER_SCENE);
    }

    [MenuItem("ChangeScene/Play From StartMenu")]
    public static void PlayFromStart()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }

        ToTitleScene();
        EditorApplication.isPlaying = true;
    }

    private static void OpenScene(string scenePath)
    {
        if (EditorApplication.isPlaying)
        {
            Debug.Log("Stop app to switch scene!");
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(SCENE_PATH + scenePath);
    }
}
