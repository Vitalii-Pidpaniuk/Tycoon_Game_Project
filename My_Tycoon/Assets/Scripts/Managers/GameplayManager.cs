using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
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
        [SerializeField] private List<Button> buildingButtons;
        private Button _activeButton;
        public GameMode gameMode = GameMode.Swiping;
        

        protected override void Start()
        {
            InitializeGame();
            
            swipingMode.onClick.AddListener(() => SetMode(GameMode.Swiping, swipingMode));
            buildingMode.onClick.AddListener(() => SetMode(GameMode.Building, buildingMode));
            replacingMode.onClick.AddListener(() => SetMode(GameMode.Replacing, replacingMode));
            adjustingMode.onClick.AddListener(() => SetMode(GameMode.Adjusting, adjustingMode));
            foreach (var button in buildingButtons)
            {
                button.onClick.AddListener(() => SetMode(GameMode.Building, buildingMode));
            }

            SaveLoadManager.Instance.LoadGame();
            
            SetMode(GameMode.Swiping, swipingMode);
        }

        private void InitializeGame()
        {
            Debug.Log("Game Initialized");
            BuildingManager.Instance.BuildManagerInit();
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
                    break;
                case GameMode.Building:
                    BuildingManager.Instance.buildingPlacer.SetBuildingMode(true);
                    swipingMode.interactable = true;
                    break;
                case GameMode.Replacing:
                    BuildingManager.Instance.buildingPlacer.SetReplacingMode(true);
                    break;
                case GameMode.Adjusting:
                    BuildingManager.Instance.buildingPlacer.SetAdjustingMode(true);
                    break;
            }
        }

        public void ExitToMainMenu()
        {
            SaveLoadManager.Instance.SaveGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
