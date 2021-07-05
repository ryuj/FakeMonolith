using System.Collections.Generic;
using System.Linq;
using Touch;
using UnityEngine;

public struct Config
{
    public int kindCount;
    public Vector3 zeroPos;
    public GameObject parent;
    public Vector3 tileSize;
    public TouchConductor conductor;
}

public struct BlockInfo
{
    public GameObject block;
    public (int x, int y) pos;
    public global::Color color;
    public bool active;
}

public class BlockManager
{
    private Config _config;
    public readonly List<BlockInfo> blocks = new List<BlockInfo>();

    private const int MaxX = 5;
    private const int MaxY = MaxX;

    public BlockManager SetConfig(Config config)
    {
        this._config = config;
        return this;
    }

    public void Create(int count)
    {
        var lenX = _config.tileSize.x;
        var lenY = _config.tileSize.y;
        var materials = GetMaterials().ToArray();
        for (var i = 0; i < count; ++i)
        {
            for (var j = 0; j < count; ++j)
            {
                var randomIndex = Random.Range(0, materials.Count());
                var ins = Object.Instantiate(materials[randomIndex], _config.parent.transform, true) as GameObject;
                // z==0 だと表示されない
                ins.transform.position = new Vector3(lenX * i, -lenY * j, 10) + _config.zeroPos + new Vector3(lenX / 2, -lenY / 2, 0);
                var receiver = ins.GetComponent<TouchReceiver>();
                receiver.conductor = _config.conductor;
                receiver.handlerId = GetIndexFromPos(i, j).ToString();

                var info = new BlockInfo
                {
                    block = ins,
                    pos = (i, j),
                    color = (global::Color)randomIndex,
                    active = true,
                };
                blocks.Add(info);
            }
        }
    }

    private IEnumerable<GameObject> GetMaterials()
    {
        return new List<GameObject>
        {
            Resources.Load("White") as GameObject,
            Resources.Load("Pink") as GameObject,
            Resources.Load("Yellow") as GameObject
        };
    }

    private static int GetIndexFromPos(int x, int y)
    {
        return x * MaxX + y;
    }

    public static (int, int) GetPosFromIndex(int index)
    {
        return (index / MaxX, index % MaxX);
    }

    public IEnumerable<BlockInfo> SearchNeighborBlock(int x, int y)
    {
        // top
        if (x % MaxX > 0)
            yield return blocks[GetIndexFromPos(x - 1, y)];
        // left
        if (y % MaxY > 0)
            yield return blocks[GetIndexFromPos(x, y - 1)];
        // right
        if (x % MaxX != MaxX - 1)
            yield return blocks[GetIndexFromPos(x + 1, y)];
        // bottom
        if (y % MaxY != MaxY - 1)
            yield return blocks[GetIndexFromPos(x, y + 1)];
    }
}