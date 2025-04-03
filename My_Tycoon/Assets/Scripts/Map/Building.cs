using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public enum BuildingType
    {
        House,
        Resource
    }
    
    public class Building : MapObject
    {
        //resource to build
        public BuildingType buildingType;

        public void Replace()
        {
            //Prefab = terrainPrefab
        }

        public void Build()
        {
        }
    }
}