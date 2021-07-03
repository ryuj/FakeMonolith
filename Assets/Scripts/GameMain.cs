using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum Color
    {
        White,
        Pink,
        Yellow
    }

    public class GameMain : MonoBehaviour
    {
        public struct BlockInfo
        {
            public GameObject block;
            public (int x, int y) pos;
            public Color color;
            public bool active;
        }

        private Camera mainCamera;
        private readonly List<GameObject> materials = new List<GameObject>();
        private const int MaxX = 5;
        private readonly TouchConductor conductor = new TouchConductor();

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
            var count = MaxX;
            var lenX = size.x;
            var lenY = size.y;
            var parent = GameObject.Find("Parent");
            var blockInfos = new List<BlockInfo>();
            for (var i = 0; i < count; ++i)
            {
                for (var j = 0; j < count; ++j)
                {
                    var randomIndex = Random.Range(0, materials.Count);
                    var ins = Instantiate(materials[randomIndex]) as GameObject;
                    // z==0 だと表示されない
                    ins.transform.position = new Vector3(lenX * i, -lenY * j, 10) + zeroPos + new Vector3(lenX / 2, -lenY / 2, 0);
                    ins.transform.parent = parent.transform;
                    var receiver = ins.GetComponent<TouchReceiver>();
                    receiver.conductor = conductor;
                    receiver.handlerId = GetIndexFromPos(i, j).ToString();

                    var info = new BlockInfo
                    {
                        block = ins,
                        pos = (i, j),
                        color = (Color)randomIndex,
                        active = true,
                    };
                    blockInfos.Add(info);
                }
            }
            conductor.blockInfos = blockInfos;
        }

        private int GetIndexFromPos(int x, int y)
        {
            return x * MaxX + y;
        }
    }
}
