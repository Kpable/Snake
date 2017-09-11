namespace Kpable.Snake
{
    // HeadSegment class
    public class HeadSegment : Segment
    {

        private void Awake()
        {
            // Set a default direction
            currentDirection = nextDirection = Direction.Right;
            // Store the current Position. 
            currentPosition = transform.position;
        }

        public override void Move()
        {
            //Debug.Log("head Move");

            // Set the previous position for the subsequent segment to follow
            previousPosition = currentPosition;

            // Move the head in the direction set
            // If the direction changed move and store the direction
            if (nextDirection != currentDirection)
            {
                // Move towards next direction
                transform.Translate(nextDirection.Vec());
                // Store that direction as the new current direction
                currentDirection = nextDirection;
            }
            else
                // Just move in current direction
                transform.Translate(currentDirection.Vec());

            // Now that we moved, store the position for subsequent segments
            currentPosition = transform.position;
        }

    }
}