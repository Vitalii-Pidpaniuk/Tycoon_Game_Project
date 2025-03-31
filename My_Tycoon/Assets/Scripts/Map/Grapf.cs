using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Graph : MonoBehaviour
    {
        public int gridSize;
        public List<Node> nodes = new List<Node>();
        
        private void Awake()
        {
            //GenerateGridGraph(gridSize);
            //PrintGraph();
        }

        public void GenerateGridGraph(int size)
        {
            Node[,] grid = new Node[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Node node = new Node(x, y);
                    grid[x, y] = node;
                    nodes.Add(node);
                }
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Node node = grid[x, y];
                    if (x > 0) node.AddNeighbor(grid[x - 1, y]); // Лівий сусід
                    if (x < size - 1) node.AddNeighbor(grid[x + 1, y]); // Правий сусід
                    if (y > 0) node.AddNeighbor(grid[x, y - 1]); // Нижній сусід
                    if (y < size - 1) node.AddNeighbor(grid[x, y + 1]); // Верхній сусід
                }
            }
        }

        public void PrintGraph()
        {
            foreach (Node node in nodes)
            {
                string neighbors = string.Join(", ", node.Neighbors.ConvertAll(n => $"({n.X}, {n.Y})"));
                Debug.Log($"Вершина ({node.X}, {node.Y}) з'єднана з: {neighbors}");
            }
        }
    }

    public class Node
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<Node> Neighbors { get; private set; } = new List<Node>();

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void AddNeighbor(Node neighbor)
        {
            if (!Neighbors.Contains(neighbor))
            {
                Neighbors.Add(neighbor);
                neighbor.Neighbors.Add(this);
            }
        }
    }
}
