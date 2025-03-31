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
        
        public void BuildManagerInit()
        {
            _graph = gameObject.AddComponent<Graph>();
            _graph.GenerateGridGraph(gridSize);
            _graph.PrintGraph();
            
            foreach (var node in _graph.nodes)
            {
                Instantiate(tilePrefab, new Vector3(node.X * 2, 0, node.Y * 2), quaternion.identity);
            }
        }
    }
}
