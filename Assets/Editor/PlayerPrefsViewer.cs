using UnityEditor;
using UnityEngine;

public class PlayerPrefsViewer : EditorWindow
{
    [MenuItem("Window/PlayerPrefs Viewer")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsViewer>("PlayerPrefs Viewer");
    }

    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs Viewer", EditorStyles.boldLabel);

        if (GUILayout.Button("Refresh"))
        {
            Repaint();
        }

        // Retrieve keys from PlayerPrefs
        string[] keys = PlayerPrefsUtility.GetKeys();

        // Display key-value pairs
        foreach (string key in keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(key, GUILayout.Width(150));
            GUILayout.Label(PlayerPrefs.GetString(key));
            GUILayout.EndHorizontal();
        }
    }
}

public static class PlayerPrefsUtility
{
    public static string[] GetKeys()
    {
        // Create a list to store keys
        var keys = new System.Collections.Generic.List<string>();

        // Iterate over all PlayerPrefs
        for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefs_Count", 0); i++)
        {
            // Retrieve key at index 'i'
            string key = PlayerPrefs.GetString("PlayerPrefs_" + i);
            keys.Add(key);
        }

        return keys.ToArray();
    }
}
