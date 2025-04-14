using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class BuildingItem : MonoBehaviour
    {
        [SerializeField] private GameObject buildButton;
        

        public void ShowBuildButton()
        {
            buildButton.SetActive(!buildButton.activeSelf);
        }
    }
}