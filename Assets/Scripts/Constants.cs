
/// <summary>
/// This class contains constants for the life of the program
/// </summary>
public static class Constants {
    public static Direction GetOppositeDirection(Direction direction)
    {
        Direction returnValue;

        switch (direction)
        {
            case Direction.Up:
                returnValue = Direction.Down;
                break;
            case Direction.Down:
                returnValue = Direction.Up;
                break;
            case Direction.Left:
                returnValue = Direction.Right;
                break;
            case Direction.Right:
            default:
                returnValue = Direction.Left;
                break;
        }

        //Debug.LogFormat("GetOppositeDirection was given {0} and returned {1}", direction, returnValue);

        return returnValue;
    }
}

/// <summary>
/// Cardinal Directions
/// </summary>
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
