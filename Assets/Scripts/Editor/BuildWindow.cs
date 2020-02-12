using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildWindow : EditorWindow
{
    List<SceneAsset> m_SceneAssets = new List<SceneAsset>();

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Build Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(BuildWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Scenes to include in build:", EditorStyles.boldLabel);
        for (int i = 0; i < m_SceneAssets.Count; ++i)
        {
            m_SceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(m_SceneAssets[i], typeof(SceneAsset), false);
        }
        if (GUILayout.Button("Add"))
        {
            m_SceneAssets.Add(null);
        }
        if (GUILayout.Button("Remove"))
        {
            m_SceneAssets.RemoveAt(m_SceneAssets.Count-1);
        }

        GUILayout.Space(8);

        GUILayout.Label("Build", EditorStyles.boldLabel);

        if (GUILayout.Button("Build Client"))
        {
            BuildClient();
        }

        if (GUILayout.Button("Build Server"))
        {
            BuildServer();
        }

        GUILayout.Space(2);


        if (GUILayout.Button("Build All"))
        {
            BuildAll();
        }
    }

    public void BuildAll()
    {
        BuildServer();
        BuildClient();
    }

    public void BuildServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new string[m_SceneAssets.Count];
        for(int i = 0; i < m_SceneAssets.Count; i++)
        {
            buildPlayerOptions.scenes[i] = AssetDatabase.GetAssetPath(m_SceneAssets[i]);
        }
        buildPlayerOptions.locationPathName = "Builds/Server/Server.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    public void BuildClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new string[m_SceneAssets.Count];
        for (int i = 0; i < m_SceneAssets.Count; i++)
        {
            buildPlayerOptions.scenes[i] = AssetDatabase.GetAssetPath(m_SceneAssets[i]);
        }
        buildPlayerOptions.locationPathName = "Builds/Client/Client.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}