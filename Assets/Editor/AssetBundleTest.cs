using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetbundleTest
{

    [MenuItem("AssetBundleTest/PackALL")]
    public static void Pack()
    {
        BuildPipeline.BuildAssetBundles(Path.Combine(Application.dataPath, "MyBundles"), BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("AssetBundleTest/PackSelectAllFormat")]
    public static void PackSelectAllFormat()
    {
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle, "LZMA");
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, "LZ4");
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle, "Uncompress");
    }

    [MenuItem("AssetBundleTest/PackSelect(LZMA)")]
    public static void PackSelectLZMA()
    {
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle);
    }

    [MenuItem("AssetBundleTest/PackSelect(LZ4)")]
    public static void PackSelectLZ4()
    {
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression);
    }

    [MenuItem("AssetBundleTest/PackSelect(Uncompress)")]
    public static void PackSelectUncompress()
    {
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle);
    }

    private static void PackSelect(BuildAssetBundleOptions opt, string extName = null)
    {
        Object o = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(o);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer == null) return;

        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = string.Concat(importer.assetBundleName, "_", extName);
        build.assetNames = new string[] { path };

        BuildPipeline.BuildAssetBundles(Path.Combine(Application.dataPath, "MyBundles"), new AssetBundleBuild[] { build }, opt, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("AssetBundleTest/PackSelectALL")]
    public static void PackSelectALL()
    {
        Object[] o = Selection.GetFiltered<Object>(SelectionMode.Assets);
        AssetBundleBuild[] buildArray = new AssetBundleBuild[o.Length];
        AssetImporter importer;
        AssetBundleBuild build;
        for (int i = 0; i < buildArray.Length; i++)
        {
            importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(o[i]));
            build = new AssetBundleBuild();
            build.assetBundleName = importer.assetBundleName;
            build.assetNames = new string[] { importer.assetPath };
            buildArray[i] = build;
        }

        BuildPipeline.BuildAssetBundles(Path.Combine(Application.dataPath, "MyBundles"), buildArray, BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

}
