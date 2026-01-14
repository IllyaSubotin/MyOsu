using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeManager))]
public class NodePoolManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NodeManager manager = (NodeManager)target;

        if (GUILayout.Button("Collect nodes"))
        {
            manager.CollectNodes();
        }
    }
}
