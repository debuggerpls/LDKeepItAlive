using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorFunctions : EditorWindow
{
    [MenuItem("Window/Edit Mode Functions")]
    public static void ShowWindow()
    {
        GetWindow<EditorFunctions>("Edit Mode Functions");
    }
 
    private void OnGUI()
    {
        if (GUILayout.Button("Update/Show Keys"))
        {
            ShowKeys();
        }
    }

    private void ShowKeys()
    {
        var keys = GameObject.FindObjectsOfType<Key>();
    }
    
}
