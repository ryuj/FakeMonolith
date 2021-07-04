using Assets.Scripts;
using UnityEngine;

public class TouchConductor : ITouchConductor
{
    public BlockManager manager;
    public void OnClick(string id, GameObject gameObject)
    {
        Debug.Log($"touched: {id}");
        var index = int.Parse(id);
        (int x, int y) pos = manager.GetPosFromIndex(index);
        Debug.Log(pos);
        GameObject.Destroy(gameObject);

        Debug.Log("-- begin --");
        foreach (var block in manager.SearchNeighborBlock(pos.x, pos.y))
        {
            if (manager.blocks[index].color == block.color)
            {
                Debug.Log(block.pos);
                GameObject.Destroy(block.block);
            }
        }
        Debug.Log("-- end --");
    }
}
