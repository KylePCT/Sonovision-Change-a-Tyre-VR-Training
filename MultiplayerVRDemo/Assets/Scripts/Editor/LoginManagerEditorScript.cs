using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoginManager))]

public class LoginManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script is responsible for connecting to the Photon servers from the Editor.", MessageType.Info);

        //Reference original target class.
        LoginManager loginManager = (LoginManager)target;

        //Connect anonymously using Photon server through button interaction.
        if (GUILayout.Button("Connect Anonymously"))
        {
            loginManager.ConnectToPhotonServer();
        }
    }

}
