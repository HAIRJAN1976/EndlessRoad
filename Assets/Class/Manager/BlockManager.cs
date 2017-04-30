using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static GameObject GetBlock(Transform transform, int x, int y, string type)
    {
        string[] types = type.Split(':');

        string path = string.Format("Block/naturePack_{0:D3}", int.Parse(types[0]));
        GameObject model = Resources.Load<GameObject>(path);

        GameObject gameObject = (GameObject)Instantiate(model, new Vector3(x * 3, 0, y * 3), Quaternion.identity, transform);
        gameObject.name = type.ToString();


        //string path = string.Format("Block/naturePack_001.obj")
        gameObject.AddComponent<MeshCollider>().sharedMesh = getFlatMesh();

        if (int.Parse(types[1]) > 5)
        {
            GameObject model2 = Resources.Load<GameObject>(string.Format("Block/naturePack_{0:D3}", int.Parse(types[1])));
            GameObject gameObject2 = (GameObject)Instantiate(model2, new Vector3(x * 3, 0, y * 3), Quaternion.identity, gameObject.transform);
        }

        gameObject.transform.tag = GameCode.GetTag(int.Parse(types[2]));
        
        return gameObject;
    }

    private static Mesh flatMesh;

    private static Mesh getFlatMesh()
    {
        if(flatMesh == null)
        {
            string path = "Block/naturePack_001";
            GameObject model = Resources.Load<GameObject>(path);
            flatMesh = model.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;

            // 出力先ファイル名
            //var filename = "Assets/Resources/Mesh/" + flatMesh.name + ".asset";

            //// Assetへ保存
            //UnityEditor.AssetDatabase.CreateAsset(flatMesh, filename);

            //flatMesh = (Mesh)UnityEditor.AssetDatabase.LoadAssetAtPath(filename, typeof(Mesh));
        }

        return flatMesh;
    }
}
