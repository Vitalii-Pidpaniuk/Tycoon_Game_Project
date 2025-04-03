using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public enum MapObjectType
    {
        Terrain,
        Road,
        Building
    }
    
    public abstract class MapObject : MonoBehaviour
    {
         public MapObjectType objectType;
         protected GameObject Prefab;
    }
}