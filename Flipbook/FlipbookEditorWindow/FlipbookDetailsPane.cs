using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

// Details pane for the focused flipbook, this was intended to show
// things like file size, name, sentence count etc. It also does some
// Asset Database manipulation to get a dropdown with all the Applicable
// assets in it, this was while I had no way to open the Flipbook editor
// window from a given assets context (right click) menu.
public class FlipbookDetailsPane : VisualElement
{
    public Action<Flipbook> onSelectionChanged;

    public FlipbookDetailsPane()
    {
        style.width = 450;
        style.height = 500;

        //Setup the dropdown for TargetClip selector
        DropdownField flipBookSelect = new();
        flipBookSelect.label = "Current Flipbook";
        flipBookSelect.style.alignSelf = Align.Center;
        flipBookSelect.style.width = 300;

        string[] assetGUIDs = AssetDatabase.FindAssets("t:" + nameof(Flipbook));

        List<Flipbook> allBooks = new();

        //Collate all Flipbook assets into a list using their GUIDs
        foreach (string GUID in assetGUIDs)
        {
            allBooks.Add(AssetDatabase.LoadAssetAtPath<Flipbook>(AssetDatabase.GUIDToAssetPath(GUID)));
        }

        //Add each one as a choice in the dropdown
        for (int i = 0; i < allBooks.Count; i++)
        {
            flipBookSelect.choices.Add(AssetDatabase.GUIDToAssetPath(assetGUIDs[i]));
        }

        //Finally set the deault value for the dropdown
        flipBookSelect.value = "None Selected";

        //Selection is linked here
        flipBookSelect.RegisterValueChangedCallback((name) =>
        {
            onSelectionChanged?.Invoke(allBooks[flipBookSelect.index]);
        });

        Add(flipBookSelect);
    }

}
