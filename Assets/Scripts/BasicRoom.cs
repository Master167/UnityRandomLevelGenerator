using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRoom : MonoBehaviour {

    public GameObject TopDoor;
    public GameObject BottomDoor;
    public GameObject LeftDoor;
    public GameObject RightDoor;
    
	// Use this for initialization
	void Awake () {
        
	}

    //// Update is called once per frame
    //void Update () {

    //}

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
    
    public void CreatePath(Direction newDirection, Direction previousDirection)
    {
        //Debug.LogFormat("Charting Course from {0} to {1}", previousDirection, newDirection);
        if (newDirection == previousDirection)
        {
            throw new System.Exception("newDirection and previousDirection are the same");
        }
        
        foreach(Direction direction in System.Enum.GetValues(typeof(Direction)))
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

    // This method will only work if there is one door in each direction.
    // Re-work this if you implement multiple doors in each direction
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
    /// </summary>
    /// <param name="incomingDirection">Direction you'd like to go</param>
    /// <returns></returns>
    public bool HasDirection(Direction incomingDirection)
    {
        bool hasDirection = false;

        switch (incomingDirection)
        {
            case Direction.Up:
                hasDirection = (TopDoor != null);
                break;
            case Direction.Down:
                hasDirection = (BottomDoor != null);
                break;
            case Direction.Left:
                hasDirection = (LeftDoor != null);
                break;
            case Direction.Right:
            default:
                hasDirection = (RightDoor != null);
                break;
        }
        Debug.LogFormat("Room {0} {1} {2}", gameObject.name, (hasDirection) ? "has" : "does not have", incomingDirection);

        return hasDirection;
    }
}
