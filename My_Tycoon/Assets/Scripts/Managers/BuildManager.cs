using System.Collections;
using System.Collections.Generic;
using Map;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Managers
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        private Graph _graph;
        [SerializeField] private int gridSize;
        [SerializeField] private GameObject tilePrefab;
        public BuildingPlacer buildingPlacer;
        
        public void BuildManagerInit()
        {
            _graph = gameObject.AddComponent<Graph>();
            _graph.GenerateGridGraph(gridSize);
            _graph.PrintGraph();
            
            foreach (var node in _graph.nodes)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(node.X * 1, 0, node.Y * 1), quaternion.identity);
                
                tile.layer = LayerMask.NameToLayer("Terrain");
            }
        }
    }
}
