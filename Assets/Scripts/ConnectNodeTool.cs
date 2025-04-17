using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
[Overlay(typeof(SceneView), "Location Connector", true)]
public class LocationToolOverlay : Overlay
{
    public override VisualElement CreatePanelContent()
    {
        var root = new VisualElement() { name = "Location Connector Tool Root" };
        root.Add(new Label() { text = "Location Connector Tool" });
        Button button = new Button() { text = "Connect selected MapLocations" };
        button.clicked += OnButtonClick;
        root.Add(button);
        return root;
    }
    void OnButtonClick()
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
                Debug.Log("Connecting nodes " + obj.name + " " + obj2.name);
                ((MapLocation)obj).GetConnectedLocations().Add(((MapLocation)obj2));
                ((MapLocation)obj2).GetConnectedLocations().Add(((MapLocation)obj));
            }
        }
    }

}