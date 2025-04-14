using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace Managers
{
    public class UIGameManager : GameManager
    {
        [SerializeField] private UISceneManager uiManager;
        //[SerializeField] private SaveLoadManager saveLoadManager;
        
        protected override void Start()
        {
            base.Start();
            InitializeGame();
        }

        private void InitializeGame()
        {
            Instantiate(uiManager);
        }

        public void StartNewGame()
        {
            SaveLoadManager.Instance.ClearSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        public void ContinueGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}