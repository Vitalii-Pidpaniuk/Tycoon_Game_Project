using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// namespace Managers
// {
//     public enum ItemType
//     {
//         Resource,
//         Building
//     }
//     
//     public class InventoryItem : MonoBehaviour
//     {
//         public ItemType itemType;
//         
//         [SerializeField] private GameObject buildButton;
//
//         private void Start()
//         {
//             switch (itemType)
//             {
//                 case ItemType.Resource:
//                     break;
//                 case ItemType.Building:
//                     break;
//             }
//         }
//
//         public void ShowBuildButton()
//         {
//             if (itemType == ItemType.Building && buildButton.activeSelf == false)
//             {
//                 buildButton.SetActive(true);
//             }
//             else
//             {
//                 buildButton.SetActive(false);
//             }
//         }
//     }
// }