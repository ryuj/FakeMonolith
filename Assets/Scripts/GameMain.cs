using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private struct BlockInfo
    {
        public GameObject block;
        public Vector2 pos;
    }

    private Camera mainCamera;
    private readonly List<GameObject> materials = new List<GameObject>();
    private readonly List<BlockInfo> blockInfos = new List<BlockInfo>();
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var zeroPos = mainCamera.ScreenToWorldPoint(Vector3.zero);
        zeroPos.Scale(new Vector3(1, -1, 1));

        materials.Add(Resources.Load("White") as GameObject);
        materials.Add(Resources.Load("Pink") as GameObject);
        materials.Add(Resources.Load("Yellow") as GameObject);

        // サイズ取るだけなのでどれでもいい
        var renderer = materials[0].GetComponent<SpriteRenderer>();
        var size = renderer.bounds.size;
        var count = 5;
        var lenX = size.x;
        var lenY = size.y;
        var parent = GameObject.Find("Parent");
        for (var i = 0; i < count; ++i)
        {
            for (var j = 0; j < count; ++j)
            {
                var ins = Instantiate(GetRandomObject()) as GameObject;
                // z==0 だと表示されない
                ins.transform.position = new Vector3(lenX * i, -lenY * j, 10) + zeroPos + new Vector3(lenX / 2, -lenY / 2, 0);
                ins.transform.parent = parent.transform;

                var info = new BlockInfo
                {
                    block = ins,
                    pos = new Vector2(i, j)
                };
                blockInfos.Add(info);
            }
        }
    }

    private GameObject GetRandomObject()
    {
        return materials[Random.Range(0, materials.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
