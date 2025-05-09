using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;

[Overlay(typeof(SceneView), "Location Connector", true)]
[ExecuteAlways]
public class LocationToolOverlay : Overlay
{

    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement() { name = "Location Connector Tool Root" };
        root.Add(new Label() { text = "Location Connector Tool" });
        Button connectButton = new Button() { text = "Connect selected MapLocations" };
        Button clearButton = new Button() { text = "Clear selected MapLocations" };
        Button subdivideButton = new Button() { text = "Subdivide selected MapLocations" };
        connectButton.clicked += ConnectAll;
        clearButton.clicked += ClearAll;
        subdivideButton.clicked += SubdivideMaplocation;
        root.Add(connectButton);
        root.Add(clearButton);
        //root.Add(subdivideButton);
        return root;
    }

    void ClearAll()
    {
        if (Selection.GetFiltered<MapLocation>(SelectionMode.TopLevel).Length == 0)
        {
            Debug.Log("No map locations selected!");
            return;
        }

        IEnumerable<Object> targets = Selection.GetFiltered<MapLocation>(SelectionMode.TopLevel);
        EditorGUI.BeginChangeCheck();

        foreach (var obj in targets)
        {
            if (!(obj is MapLocation platform))
                continue;

            foreach (var obj2 in targets)
            {
                if (obj == obj2)
                    continue;
                Debug.Log("Connecting nodes " + obj.name + " " + obj2.name);
                ((MapLocation)obj).GetConnectedLocations().Remove(((MapLocation)obj2));
                ((MapLocation)obj2).GetConnectedLocations().Remove(((MapLocation)obj));
                SerializedObject sobj = new UnityEditor.SerializedObject(obj);
                SerializedObject sobj2 = new UnityEditor.SerializedObject(obj2);
                sobj.ApplyModifiedProperties();
                sobj2.ApplyModifiedProperties();
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj2);
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            // Tunsave the current scene if there's any change in the editor GUI
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
    void ConnectAll()
    {
        if (Selection.GetFiltered<MapLocation>(SelectionMode.TopLevel).Length == 0)
        {
            Debug.Log("No map locations selected!");
            return;
        }

        IEnumerable<Object> targets = Selection.GetFiltered<MapLocation>(SelectionMode.TopLevel);

        foreach (var obj in targets)
        {
            if (!(obj is MapLocation platform))
                continue;

            foreach (var obj2 in targets)
            {
                if (obj == obj2)
                    continue;
                MapLocation m1 = (MapLocation)obj;
                MapLocation m2 = (MapLocation)obj2;
                // Debug.Log("Connecting nodes " + obj.name + " " + obj2.name);
                if (!m1.GetConnectedLocations().Contains(m2))
                {
                    m1.GetConnectedLocations().Add(m2);
                }
                if (!m2.GetConnectedLocations().Contains(m1))
                {
                    m2.GetConnectedLocations().Add(m1);
                }
                SerializedObject sobj = new UnityEditor.SerializedObject(obj);
                SerializedObject sobj2 = new UnityEditor.SerializedObject(obj2);
                sobj.ApplyModifiedProperties();
                sobj2.ApplyModifiedProperties();
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj);
                PrefabUtility.RecordPrefabInstancePropertyModifications(obj2);
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            // Tunsave the current scene if there's any change in the editor GUI
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }


    void SubdivideMaplocation()
    {
        if (Selection.GetFiltered<MapLocation>(SelectionMode.TopLevel).Length == 0)
        {
            Debug.Log("No map locations selected!");
            return;
        }

        IEnumerable<Object> targets = Selection.GetFiltered<MapLocation>(SelectionMode.TopLevel);

        foreach (var obj in targets)
        {
            if (!(obj is MapLocation platform))
                continue;

            foreach (var obj2 in targets)
            {
                if (obj == obj2)
                    continue;

                if (!(Selection.activeObject is MapLocation))
                    continue;
                MapLocation obj3 = (MapLocation)PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject);
                ((MapLocation)obj).GetConnectedLocations().Add(((MapLocation)obj3));
                ((MapLocation)obj3).GetConnectedLocations().Add(((MapLocation)obj));

                ((MapLocation)obj2).GetConnectedLocations().Add(((MapLocation)obj3));
                ((MapLocation)obj3).GetConnectedLocations().Add(((MapLocation)obj2));

                ((MapLocation)obj).GetConnectedLocations().Remove(((MapLocation)obj2));
                ((MapLocation)obj2).GetConnectedLocations().Remove(((MapLocation)obj));
            }
        }
    }
}