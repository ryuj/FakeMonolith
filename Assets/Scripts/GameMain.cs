using System.Collections.Generic;
using Touch;
using UnityEngine;

public enum Color
{
    White,
    Pink,
    Yellow
}

public class GameMain : MonoBehaviour
{
    private Camera _mainCamera;
    private readonly List<GameObject> _materials = new List<GameObject>();
    private const int MaxX = 5;
    private readonly TouchConductor _conductor = new TouchConductor();

    // Start is called before the first frame update
    private void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var zeroPos = _mainCamera.ScreenToWorldPoint(Vector3.zero);
        zeroPos.Scale(new Vector3(1, -1, 1));

        _materials.Add(Resources.Load("White") as GameObject);
        _materials.Add(Resources.Load("Pink") as GameObject);
        _materials.Add(Resources.Load("Yellow") as GameObject);

        // サイズ取るだけなのでどれでもいい
        var renderer = _materials[0].GetComponent<SpriteRenderer>();
        var size = renderer.bounds.size;
        var parent = GameObject.Find("Parent");

        var manager = new BlockManager();
        manager.SetConfig(new Config {
            kindCount = 3,
            parent = parent,
            zeroPos = zeroPos,
            tileSize = size,
            conductor = _conductor
        }).Create(MaxX);

        _conductor.manager = manager;
    }
}