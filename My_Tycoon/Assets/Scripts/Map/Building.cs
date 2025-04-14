using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public enum BuildingType
    {
        House,
        Resource,
        TradingPoint
    }
    
    public class Building : MapObject
    {
        //resource to build
        public BuildingType buildingType;
    }
}