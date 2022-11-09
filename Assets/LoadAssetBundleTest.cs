using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundleTest : MonoBehaviour
{

    void Start()
    {
        string pathDir = System.IO.Path.Combine(Application.dataPath, "MyBundles");
    }

}
