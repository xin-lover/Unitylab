using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AREnvMenu
{

    private const string s_prefabPath = "Assets/KunLun AR/";

    [MenuItem("Tool/AR/Create ARCore Base Required")]
    private static void CreateARCoreBaseRequired()
    {
        Debug.Log("Not yet supported ARCore...");
    }

    [MenuItem("Tool/AR/Create ARKit Base Required")]
    private static void CreateARKitBaseRequired()
    {
        string pfbname = "ARKitRoot";
        LoadCreateOnceObject(s_prefabPath + "Prefabs/ARKitRoot.prefab", pfbname);
    }

    [MenuItem("Tool/AR/Create Cloud Anchor Required")]
    private static void CreateARCoreCloudAnchorRequired()
    {
        string pfbname = "CloudAnchorRequired";
        LoadCreateOnceObject(s_prefabPath + "Prefabs/CloudAnchorRequired.prefab", pfbname);
    }

    private static void LoadCreateOnceObject(string path, string name)
    {
        string pfbname = name;
        if (GameObject.Find(pfbname) != null)
        {
            Debug.Log("There Already Created ARKitRoot..");
            return;
        }
        GameObject ARKitRootPfb = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        Debug.Assert(ARKitRootPfb != null);
        GameObject instance = GameObject.Instantiate<GameObject>(ARKitRootPfb);
        instance.name = pfbname;
    }
}
