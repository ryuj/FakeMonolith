using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.GameMain;

public class TouchConductor : ITouchConductor
{
    private const int MaxX = 5;
    private const int MaxY = MaxX;

    public List<BlockInfo> blockInfos = new List<BlockInfo>();

    public void OnClick(string id, GameObject gameObject)
    {
        Debug.Log(id);
        var index = int.Parse(id);
        (int x, int y) pos = GetPosFromIndex(index);
        Debug.Log(pos);
        GameObject.Destroy(gameObject);

        Debug.Log("-- begin --");
        foreach (var block in SearchNeighborBlock(pos.x, pos.y))
        {
            if (blockInfos[index].color == block.color)
            {
                Debug.Log(block.pos);
                GameObject.Destroy(block.block);
            }
        }
        Debug.Log("-- end --");
    }

    private (int, int) GetPosFromIndex(int index)
    {
        return (index / MaxX, index % MaxX);
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
}
