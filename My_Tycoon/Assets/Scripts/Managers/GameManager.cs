using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public abstract class GameManager : Singleton<GameManager>
    {
        
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
        }

        public void SaveGame() {}
        public void LoadGame() {}
    }
}