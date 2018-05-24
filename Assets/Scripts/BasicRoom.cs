using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRoom : MonoBehaviour {

    public GameObject TopDoor;
    public GameObject BottomDoor;
    public GameObject LeftDoor;
    public GameObject RightDoor;

    // A set of variables that I can set in the Editor to tell the LevelGeneration script
    // what the direction the room is designed to be run through.
    public bool EnterFromTop;
    public bool EnterFromBottom;
    public bool EnterFromLeft;
    public bool EnterFromRight;

    public bool ExitToTop;
    public bool ExitToBottom;
    public bool ExitToLeft;
    public bool ExitToRight;

    // To be used in the get exit direction
    private List<Direction> exitDirections;

    // Use this for initialization
    void Awake()
    {
        // Setup the exitDirection list for this room.
        if (ExitToTop)
        {
            exitDirections.Add(Direction.Up);
        }
        if (ExitToBottom)
        {
            exitDirections.Add(Direction.Down);
        }
        if (ExitToLeft)
        {
            exitDirections.Add(Direction.Left);
        }
        if (ExitToRight)
        {
            exitDirections.Add(Direction.Down);
        }
    }

    //// Update is called once per frame
    //void Update () {

    //}

    /// <summary>
    /// Close the door in the direction desired
    /// </summary>
    /// <param name="doorToClose"></param>
    public void CloseDoor(Direction doorToClose)
    {
        //Debug.LogFormat("Closing Door {0}", doorToClose);
        // Close the door by activating the door object
        if (doorToClose == Direction.Up && TopDoor != null)
        {
            TopDoor.SetActive(true);
        }
        else if (doorToClose == Direction.Down && BottomDoor != null)
        {
            BottomDoor.SetActive(true);
        }
        else if (doorToClose == Direction.Left && LeftDoor != null)
        {
            LeftDoor.SetActive(true);
        }
        // doorToClose == Direction.Right
        else if (RightDoor != null)
        {
            RightDoor.SetActive(true);
        }

        return;
    }

    /// <summary>
    /// Open the door in the direction desired
    /// </summary>
    /// <param name="doorToOpen"></param>
    public void OpenDoor(Direction doorToOpen)
    {
        //Debug.LogFormat("Opening Door {0}", doorToOpen);
        // Open the door by deactivating the door object
        if (doorToOpen == Direction.Up && TopDoor != null)
        {
            TopDoor.SetActive(false);
        }
        else if (doorToOpen == Direction.Down && BottomDoor != null)
        {
            BottomDoor.SetActive(false);
        }
        else if (doorToOpen == Direction.Left && LeftDoor != null)
        {
            LeftDoor.SetActive(false);
        }
        // doorToOpen == Direction.Right
        else if (RightDoor != null)
        {
            RightDoor.SetActive(false);
        }

        return;
    }

    /* Commented out because I don't think I'll need this with the new direction.
    // <summary>
    /// Creates a path from the previousDirection to the newDirection by ensuring that 
    /// the doors open to travel through and extra directions are closed.
    /// </summary>
    /// <param name="newDirection"></param>
    /// <param name="previousDirection"></param>
    public void CreatePath(Direction newDirection, Direction previousDirection)
    {
        //Debug.LogFormat("Charting Course from {0} to {1}", previousDirection, newDirection);
        if (newDirection == previousDirection)
        {
            throw new System.Exception("newDirection and previousDirection are the same");
        }

        foreach (Direction direction in System.Enum.GetValues(typeof(Direction)))
        {
            if (direction != previousDirection && direction != newDirection)
            {
                CloseDoor(direction);
            }
            else
            {
                OpenDoor(direction);
            }
        }

        return;
    }
    */

    /// <summary>
    /// Returns the transform for the door of the direction passed in.
    /// Dev Note: This method will only work if there is one door in each direction.
    /// Re-work this if you implement multiple doors in each direction.
    /// </summary>
    /// <param name="doorDirection"></param>
    /// <returns></returns>
    public Transform GetRoomTransform(Direction doorDirection)
    {
        Transform returnValue;

        switch (doorDirection)
        {
            case Direction.Up:
                returnValue = TopDoor.transform;
                break;
            case Direction.Down:
                returnValue = BottomDoor.transform;
                break;
            case Direction.Left:
                returnValue = LeftDoor.transform;
                break;
            case Direction.Right:
            default:
                returnValue = RightDoor.transform;
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// Returns true if room has door for incoming direction
    /// Dev Note: Could just look at the public variable instead of calling a function.
    /// </summary>
    /// <param name="incomingDirection">Direction you'd like to go</param>
    /// <returns></returns>
    public bool HasDirection(Direction incomingDirection)
    {
        bool hasDirection = false;

        switch (incomingDirection)
        {
            case Direction.Up:
                hasDirection = EnterFromTop;
                break;
            case Direction.Down:
                hasDirection = EnterFromBottom;
                break;
            case Direction.Left:
                hasDirection = EnterFromLeft;
                break;
            case Direction.Right:
            default:
                hasDirection = EnterFromRight;
                break;
        }
        //Debug.LogFormat("Room {0} {1} {2}", gameObject.name, (hasDirection) ? "has" : "does not have", incomingDirection);

        return hasDirection;
    }

    /// <summary>
    /// Using provided incomingDirection, room will determine which direction to travel through.
    /// Returning the direction the player will be coming from.
    /// </summary>
    /// <param name="incomingDirection"></param>
    /// <returns></returns>
    public Direction GetExitDirection(Direction incomingDirection)
    {
        // Check the player has open door.
        OpenDoor(incomingDirection);

        // Determine which to exit
        var randomNumber = Random.Range(0, exitDirections.Count);

        // Return the direction of the door

        return exitDirections[randomNumber];
        /*
         * Returning the direction that the player is coming from to the Level generator
         * will allow it to find a room the is open to the new direction.
         * I can always use the GetDoorTransform() to determine where to put the room.
         */
    }
}
