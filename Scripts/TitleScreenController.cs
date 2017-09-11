using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Kpable.Snake
{
    public class TitleScreenController : MonoBehaviour
    {

        [SerializeField]
        private Text highScore;

        public void Start()
        {
            if (highScore) highScore.text = "High Score \n" + PlayerPrefs.GetInt("Snake High Score");
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Snake Gameplay");
        }

        public void ReturnToTitleScreen()
        {
            SceneManager.LoadScene("Snake Title Screen");
        }
    }
}
