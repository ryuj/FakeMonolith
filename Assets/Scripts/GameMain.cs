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
            var parent = GameObject.Find("Parent");

            var manager = new BlockManager();
            manager.SetConfig(new Config {
                kindCount = 3,
                parent = parent,
                zeroPos = zeroPos,
                tileSize = size,
                conductor = conductor
            }).Create(count);

            conductor.manager = manager;
        }
    }
}
