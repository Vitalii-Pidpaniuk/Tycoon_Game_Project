using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UISceneManager : UIManager
    {
        [SerializeField] private Canvas canvas;
        public Button newGameButton, continueGameButton, settingsButton, quitButton;
        
        private void Start()
        {
            //Instantiate(canvas);
        }
    }
   
}