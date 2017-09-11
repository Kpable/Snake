using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Kpable.Snake
{
    public enum Direction { Up, Left, Down, Right, Forward, Back }

    #region Enum Utils
    public static class EnumUtils
    {
        public static Vector3 Vec(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Vector3.up;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Right:
                    return Vector3.right;
                default:
                    return Vector3.up;
            }
        }

        //Extension method to get the Vector3 based on the Transform's direction. 
        public static Vector3 Trans(this Direction dir, Transform t)
        {
            switch (dir)
            {
                case Direction.Up:
                    return t.up;
                case Direction.Left:
                    return t.right * -1;
                case Direction.Down:
                    return t.up * -1;
                case Direction.Right:
                    return t.right;
                default:
                    return t.up;
            }
        }
    }

    #endregion Enum Direction End

    // Snake Class
    public class Snake : MonoBehaviour
    {

        public float snakeMoveRate = 0.1f;
        public GameObject snakeSegmentPrefab;
        private List<Segment> snake = new List<Segment>();
        private GameController gameController;

        private int collisionDelay = 0;

        void Awake()
        {
            gameController = GameObject.Find("Gameplay Controller").GetComponent<GameController>();
            if (gameController == null) Debug.LogWarning("Controller not found");

            // Get the current snake
            GetSnake();
        }

        void Start()
        {
            InvokeRepeating("MoveSnake", snakeMoveRate, snakeMoveRate);
        }

        void Update()
        {

            // Capture Input 
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            //Change Head direction
            if (h > 0) snake[0].SetDirection(Direction.Right);
            else if (h < 0) snake[0].SetDirection(Direction.Left);

            if (v > 0) snake[0].SetDirection(Direction.Up);
            else if (v < 0) snake[0].SetDirection(Direction.Down);

        }

        /// <summary>
        /// This function will iterate through all the children of the transform
        /// and add movement delay to each segment if necessary. 
        /// </summary>
        void GetSnake()
        {
            // If there are no childre, bad.. and dont do anything
            if (transform.childCount <= 0)
            {
                Debug.LogError("need to add a head");
                return;
            }

            // For each child 
            for (int i = 0; i < transform.childCount; i++)
            {
                // Grab the SnakeSegment component
                Segment segment = transform.GetChild(i).GetComponent<Segment>();
                // Add it to the snake
                snake.Add(segment);
                // Add delay equal to the number of segments except this one
                segment.SetMovementDelay(snake.Count - 1);
            }
        }

        void MoveSnake()
        {
            // For each segment
            foreach (Segment s in snake)
            {
                // If its the head, move by direction
                if (snake.IndexOf(s) == 0) s.Move();
                // Else move by the position of the previous segment
                else s.Move(snake[snake.IndexOf(s) - 1]);
            }

            CheckCollisions();
        }


        void CheckCollisions()
        {
            CheckFoodCollisions();
            CheckSegmentCollisions();
            CheckWallCollisions();
        }

        private void CheckFoodCollisions()
        {
            // If the head reached the position of a food object
            if (gameController.GetCurrentFoodPosition() == snake[0].transform.position)
            {
                Debug.Log("Eating food");

                // Eat the food
                EatFood();
            }
        }

        private void CheckSegmentCollisions()
        {
            bool colidedWithSelf = false;

            foreach (Segment s in snake)
            {
                if (snake.IndexOf(s) == 0) continue;
                if (snake.IndexOf(s) == snake.Count - 1)
                {
                    if (collisionDelay != 0)
                    {
                        Debug.Log("colision: " + collisionDelay);
                        collisionDelay--;
                        continue;
                    }
                    // else we crashed                   
                }
                if (snake[0].transform.position == s.transform.position)
                {
                    colidedWithSelf = true;
                    break;
                }
            }

            // Will collide with self when food is instatiated
            if (colidedWithSelf)
            {
                //Debug.Log("Collided with Self");
                gameController.EndGame();

                //Time.timeScale = 0;
            }
        }

        private void CheckWallCollisions()
        {
            bool collidedWithWall = false;

            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            //Debug.Log(obstacles[0].GetComponent<Collider2D>().bounds.ToString());
            foreach (GameObject obstacle in obstacles)
            {
                if (obstacle.GetComponent<Collider2D>().bounds.Contains(snake[0].transform.position))
                {
                    collidedWithWall = true;
                }
            }

            if (collidedWithWall)
            {
                Debug.Log("Collided with Obstacle");
                gameController.EndGame();
                //Time.timeScale = 0;
            }
        }
        private void CheckWrapCollisions()
        {
            bool collidedWithWrap = false;

            GameObject[] wraps = GameObject.FindGameObjectsWithTag("Obstacle");
            //Debug.Log(obstacles[0].GetComponent<Collider2D>().bounds.ToString());
            foreach (GameObject obstacle in wraps)
            {
                if (obstacle.GetComponent<Collider2D>().bounds.Contains(snake[0].transform.position))
                {
                    collidedWithWrap = true;
                }
            }

            if (collidedWithWrap)
            {
                Debug.Log("Collided with Obstacle");
                gameController.EndGame();
                //Time.timeScale = 0;
            }
        }

        void EatFood()
        {
            // Create a segment 
            GameObject segment = Instantiate(snakeSegmentPrefab) as GameObject;
            // Set its parent this transform
            segment.transform.SetParent(transform);
            // Set the position under the head 
            segment.transform.position = snake[0].transform.position;
            // Do not move until all segments have passed by delaying its movement 
            segment.GetComponent<Segment>().SetMovementDelay(snake.Count);
            // Delay collisions with new segment
            collisionDelay = snake.Count;
            // Add the segment to the snake
            snake.Add(segment.GetComponent<Segment>());
            // Add to score
            gameController.AddScore();
            //More Food!!
            gameController.SpawnFood();
        }

        void OnCollisionEnter2D(Collision2D target)
        {
            Debug.Log(target.gameObject.name);
        }

        void OnTriggerEnter2D(Collider2D target)
        {
            Debug.Log(target.name);
        }

    }
}