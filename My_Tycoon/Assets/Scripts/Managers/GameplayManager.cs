using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public enum GameMode
    {
        Swiping,
        Building,
        Replacing,
        Adjusting
    }
    
    public class GameplayManager : GameManager
    {
        [SerializeField] private Button swipingMode, buildingMode, replacingMode, adjustingMode;
        private Button _activeButton;
        public GameMode gameMode = GameMode.Swiping;

        protected override void Start()
        {
            base.InitializeGame();
            swipingMode.onClick.AddListener(() => SetMode(GameMode.Swiping, swipingMode));
            buildingMode.onClick.AddListener(() => SetMode(GameMode.Building, buildingMode));
            replacingMode.onClick.AddListener(() => SetMode(GameMode.Replacing, replacingMode));
            adjustingMode.onClick.AddListener(() => SetMode(GameMode.Adjusting, adjustingMode));
            
        }
        
        private void SetMode(GameMode mode, Button clickedButton)
        {
            gameMode = mode;
            
            SetModeBehaviour();
            
            if (_activeButton != null)
            {
                _activeButton.interactable = true;
            }

            _activeButton = clickedButton;
            _activeButton.interactable = false;
        }

        private void SetModeBehaviour()
        {
            switch (gameMode)
            {
                case GameMode.Swiping:
                    BuildingManager.Instance.buildingPlacer.SetBuildingMode(false);
                    BuildingManager.Instance.buildingPlacer.SetReplacingMode(false);
                    BuildingManager.Instance.buildingPlacer.SetAdjustingMode(false);
                    break;
                case GameMode.Building:
                    BuildingManager.Instance.buildingPlacer.SetBuildingMode(true);
                    //BuildingManager.Instance.buildingPlacer.SetReplacingMode(false);
                    //BuildingManager.Instance.buildingPlacer.SetAdjustingMode(false);
                    break;
                case GameMode.Replacing:
                    BuildingManager.Instance.buildingPlacer.SetReplacingMode(true);
                    //BuildingManager.Instance.buildingPlacer.SetBuildingMode(false);
                    //BuildingManager.Instance.buildingPlacer.SetAdjustingMode(false);
                    break;
                case GameMode.Adjusting:
                    BuildingManager.Instance.buildingPlacer.SetAdjustingMode(true);
                    //BuildingManager.Instance.buildingPlacer.SetReplacingMode(false);
                    //BuildingManager.Instance.buildingPlacer.SetBuildingMode(false);
                    break;
            }
        }
    }
}