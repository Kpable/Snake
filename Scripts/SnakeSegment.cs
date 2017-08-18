using UnityEngine;

// SnakeSegment Class
public class SnakeSegment : MonoBehaviour {

    // The head will move based on direction of input
    protected Direction currentDirection;
    protected Direction nextDirection;
    
    // The segments will move based on the movement of the segment in front of it
    protected Vector3 previousPosition;
    protected Vector3 currentPosition;

    private int movementDelay;              // The delay before moving (so it appears in back)

    // Move function for the head to override. 
    public virtual void Move() {
        Debug.Log("segment Move");
    }

    public void Move(SnakeSegment segmentToFollow)
    {
        // If there is no movement delay move
        if (movementDelay == 0)
        {
            // Set the previous position to the current position
            previousPosition = currentPosition;
            // Set the current position to the previous segment's previous position
            currentPosition = segmentToFollow.previousPosition;
            // Set the position to the current position
            transform.position = currentPosition;
        }
        else
            // Decrement movement delay
            movementDelay--;
    }

    public void SetDirection(Direction dir)
    {
        // Set the new direction
        nextDirection = dir;
    }

    public void SetMovementDelay(int delay)
    {
        // Set the movement delay
        movementDelay = delay;
    }
}
