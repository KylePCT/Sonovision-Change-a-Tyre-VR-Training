using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManager))]

public class RoomManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        //Draw the default inspector AND customisation.
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This script is responsible for connecting to the rooms from the Editor.", MessageType.Info);

        //Reference original target class.
        RoomManager roomManager = (RoomManager)target;

        ////Connect anonymously using Photon server through button interaction.
        //if (GUILayout.Button("Join Random Room"))
        //{
        //    roomManager.JoinRandomRoom();
        //}

        if (GUILayout.Button("Join Bronze Tier Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Bronze();
        }

        if (GUILayout.Button("Join Silver Tier Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Silver();
        }

        if (GUILayout.Button("Join Gold Tier Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Gold();
        }
    }
}
