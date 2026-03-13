using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class EditorAutoRefresh {
    private static Object lastSelection = null;
    private static int lastSelectionID = 0;

    static EditorAutoRefresh() {
        EditorApplication.update += Update;
    }

    static void Update() {
        Object currentSelection = Selection.activeObject;

        // detect when selection is deleted 
        if (lastSelectionID != 0 && EditorUtility.InstanceIDToObject(lastSelectionID) == null) {
            EditorApplication.DirtyHierarchyWindowSorting();

            lastSelectionID = 0;
            lastSelection = null;
        }

        // save to update inspection window if selection changes
        if (currentSelection != lastSelection) {
            // force a save to trigger a refresh (there might be an alternative besides saving here?)
            if (currentSelection != false) {
                // save the current scene and all dirty assets
                if (!Application.isPlaying) {
                    AssetDatabase.SaveAssets();
                    EditorSceneManager.SaveOpenScenes();
                }
            }

            lastSelection = currentSelection;
            lastSelectionID = currentSelection ? currentSelection.GetInstanceID() : 0;
        }
    }
}
