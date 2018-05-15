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
        switch(doorToClose)
        {
            case Direction.Up:
                TopDoor.SetActive(true);
                break;
            case Direction.Down:
                BottomDoor.SetActive(true);
                break;
            case Direction.Left:
                LeftDoor.SetActive(true);
                break;
            case Direction.Right:
            default:
                RightDoor.SetActive(true);
                break;
        }

        return;
    }

    public void OpenDoor(Direction doorToOpen)
    {
        //Debug.LogFormat("Opening Door {0}", doorToOpen);
        // Open the door by deactivating the door object
        switch (doorToOpen)
        {
            case Direction.Up:
                TopDoor.SetActive(false);
                break;
            case Direction.Down:
                BottomDoor.SetActive(false);
                break;
            case Direction.Left:
                LeftDoor.SetActive(false);
                break;
            case Direction.Right:
            default:
                RightDoor.SetActive(false);
                break;
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
}
