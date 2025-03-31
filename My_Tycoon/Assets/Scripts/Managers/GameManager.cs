using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Debug.Log("Game Initialized");
            BuildingManager.Instance.BuildManagerInit();
        }

        public void SaveGame() { /* Логіка збереження */ }
        public void LoadGame() { /* Логіка завантаження */ }
    }
}