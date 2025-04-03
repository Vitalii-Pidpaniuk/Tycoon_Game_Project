using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public abstract class GameManager : Singleton<GameManager>
    {
        //[SerializeField] protected GameObject buildManager;
        
        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {
            InitializeGame();
        }

        protected void InitializeGame()
        {
            Debug.Log("Game Initialized");
            BuildingManager.Instance.BuildManagerInit();
        }

        public void SaveGame() { /* Логіка збереження */ }
        public void LoadGame() { /* Логіка завантаження */ }
    }
}