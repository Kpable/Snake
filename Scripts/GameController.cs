using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace Kpable.Snake
{
    public class GameController : MonoBehaviour
    {

        public GameObject foodPrefab;

        int w = 60;
        int h = 30;

        private GameObject currentFood;
        [SerializeField]
        private GameObject foodHolder;
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Text highScoreText;

        private int highScore;

        private void Awake()
        {
            if (PlayerPrefs.HasKey("Snake Game Initialized"))
                highScore = PlayerPrefs.GetInt("Snake High Score");
            else
            {
                PlayerPrefs.SetInt("Snake Game Initialized", 1);
                PlayerPrefs.SetInt("Snake High Score", 0);
                highScore = PlayerPrefs.GetInt("Snake High Score");
            }

            Time.timeScale = 1;
        }

        // Use this for initialization
        void Start()
        {
            SpawnFood();
            if (!foodHolder) foodHolder = GameObject.Find("Food");

            highScoreText.text = "High Score: " + highScore;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(w, 0, 0));
            Gizmos.DrawLine(new Vector3(0, h, 0), new Vector3(w, h, 0));
            Gizmos.DrawLine(new Vector3(w, 0, 0), new Vector3(w, h, 0));
            Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, h, 0));
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector3 GetCurrentFoodPosition()
        {
            return currentFood.transform.position;
        }

        public void AddScore()
        {
            int score = int.Parse(scoreText.text.Split(' ')[1]);
            score++;
            scoreText.text = "Score: " + score;
        }

        public void SpawnFood()
        {
            if (foodHolder.transform.childCount > 0)
                Destroy(foodHolder.transform.GetChild(0).gameObject);
            currentFood = Instantiate(foodPrefab, new Vector3(Random.Range(0, w), Random.Range(0, h), 0), Quaternion.identity) as GameObject;
            currentFood.transform.SetParent(foodHolder.transform);
        }

        public void EndGame()
        {
            Debug.Log("Game Ending");
            Time.timeScale = 0;
            CheckScore();
            StartCoroutine("RestartScene");

        }

        public void CheckScore()
        {
            int score = int.Parse(scoreText.text.Split(' ')[1]);

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("Snake High Score", highScore);
            }
        }

        public IEnumerator RestartScene()
        {
            yield return new WaitForSecondsRealtime(3f);
            SceneManager.LoadScene("Snake Game Over Screen");
        }
    }
}
