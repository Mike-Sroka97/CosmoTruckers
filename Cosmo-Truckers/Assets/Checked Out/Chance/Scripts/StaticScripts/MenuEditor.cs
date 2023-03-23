#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MenuEditor
{
    [MenuItem("Load Level /To Menu")]
    public static void MoveLevel()
    {
        EditorSceneManager.OpenScene("Assets/Checked Out/Chance/Scenes/Menu.unity");
        EditorApplication.EnterPlaymode();
    }
}
#endif
