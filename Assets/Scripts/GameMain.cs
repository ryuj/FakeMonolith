using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ITouchParent
    {
        public void OnClick(string id, GameObject gameObject);
    }

    public enum Color
    {
        White,
        Pink,
        Yellow
    }

    public class GameMain : MonoBehaviour, ITouchParent
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
        private readonly List<BlockInfo> blockInfos = new List<BlockInfo>();
        private const int MaxX = 5;
        private const int MaxY = MaxX;
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
            for (var i = 0; i < count; ++i)
            {
                for (var j = 0; j < count; ++j)
                {
                    var randomIndex = Random.Range(0, materials.Count);
                    var ins = Instantiate(materials[randomIndex]) as GameObject;
                    // z==0 だと表示されない
                    ins.transform.position = new Vector3(lenX * i, -lenY * j, 10) + zeroPos + new Vector3(lenX / 2, -lenY / 2, 0);
                    ins.transform.parent = parent.transform;
                    var handler = ins.GetComponent<TouchHandler>();
                    handler.parent = this;
                    handler.handlerId = GetIndexFromPos(i, j).ToString();

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
        }

        private IEnumerable<BlockInfo> SearchNeighborBlock(int x, int y)
        {
            // top
            if (x % MaxX > 0)
                yield return blockInfos[GetIndexFromPos(x - 1, y)];
            // left
            if (y % MaxY > 0)
                yield return blockInfos[GetIndexFromPos(x, y - 1)];
            // right
            if (x % MaxX != MaxX - 1)
                yield return blockInfos[GetIndexFromPos(x + 1, y)];
            // bottom
            if (y % MaxY != MaxY - 1)
                yield return blockInfos[GetIndexFromPos(x, y + 1)];
        }

        private int GetIndexFromPos(int x, int y)
        {
            return x * MaxX + y;
        }

        private (int, int) GetPosFromIndex(int index)
        {
            return (index / MaxX, index % MaxX);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick(string id, GameObject gameObject)
        {
            Debug.Log(id);
            var index = int.Parse(id);
            (int x, int y) pos = GetPosFromIndex(index);
            Debug.Log(pos);
            Destroy(gameObject);

            Debug.Log("-- begin --");
            foreach (var block in SearchNeighborBlock(pos.x, pos.y))
            {
                if (blockInfos[index].color == block.color)
                {
                    Debug.Log(block.pos);
                    Destroy(block.block);
                }
            }
            Debug.Log("-- end --");
        }

        public void SearchAndDestroy(int x, int y, BlockInfo blockInfo)
        {
            foreach (var block in SearchNeighborBlock(x, y))
            {
                if (blockInfo.color == block.color)
                {
                    // TODO: チェック済みの判定
                    SearchAndDestroy(block.pos.x, block.pos.y, blockInfo);
                    Debug.Log(block.pos);
                    Destroy(block.block);
                }
            }
        }
    }
}
