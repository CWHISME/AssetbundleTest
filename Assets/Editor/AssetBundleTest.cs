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
        PackSelect(BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression);
    }

    private static void PackSelect(BuildAssetBundleOptions opt, string extName = null)
    {
        Object o = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(o);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer == null) return;

        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = string.IsNullOrEmpty(extName) ? importer.assetBundleName : string.Concat(importer.assetBundleName, "_", extName);
        build.assetNames = new string[] { path };

        BuildPipeline.BuildAssetBundles(Path.Combine(Application.dataPath, "MyBundles"), new AssetBundleBuild[] { build }, opt, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("AssetBundleTest/PackSelectALL")]
    public static void PackSelectALL()
    {
        Object[] o = Selection.GetFiltered<Object>(SelectionMode.Assets);
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>(o.Length);
        AssetImporter importer;
        AssetBundleBuild build;
        int index;
        for (int i = 0; i < o.Length; i++)
        {
            importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(o[i]));
            //判断是否存在重复
            index = assetBundleBuilds.FindIndex((x) => x.assetBundleName == importer.assetBundleName);
            if (index == -1)
            {
                build = new AssetBundleBuild();
                build.assetBundleName = importer.assetBundleName;
                build.assetNames = new string[] { importer.assetPath };
                assetBundleBuilds.Add(build);
            }
            else
            {
                //重复，则添加进去
                build = assetBundleBuilds[index];
                string[] newAssets = new string[build.assetNames.Length + 1];
                newAssets[0] = importer.assetPath;
                System.Array.Copy(build.assetNames, 0, newAssets, 1, build.assetNames.Length);
                build.assetNames = newAssets;
                assetBundleBuilds[index] = build;
            }
        }
        //System.Span<int> span = stackalloc int[2];
        //System.Runtime.InteropServices.Marshal.AllocHGlobal();
        BuildPipeline.BuildAssetBundles(Path.Combine(Application.dataPath, "MyBundles"), assetBundleBuilds.ToArray(), BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

}
