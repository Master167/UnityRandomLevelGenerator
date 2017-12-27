using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public GameObject BasicRoom;

    public int NumberOfRooms;

	// Use this for initialization
	void Start () {
        ////When done, use this to start
        //Random.InitState(System.Environment.TickCount);
        Random.InitState(1);

        if (NumberOfRooms < 1)
        {
            Debug.Log("Number of rooms for Level Generation is Less then 1");
        }

        GenerateLevel(NumberOfRooms);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateLevel(int numberOfRooms)
    {
        int roomsGenarated = 0;
        GameObject previousRoom;
        Direction previousDirection;
        GameObject currentRoom;
        Direction currentDirection;
        Vector3 translationVector;
        
        // Initialize Level with first room
        // Put room in scene
        Debug.Log("Starting to generate rooms");
        currentRoom = Instantiate(BasicRoom, transform);
        
        // determine direction
        currentDirection = Direction.Right;

        // Close extra doors
        currentRoom.GetComponent<BasicRoom>().OpenDoor(currentDirection);

        // Set translation vector for next room,
        // You know where the door you are coming from it located.
        // You also know the door you are going to is located.
        // So set the translation to be how far those doors are offset from their parents' transform locations.
        throw new System.Exception("Do the things");

        // set as previous room and direction
        previousRoom = currentRoom;
        previousDirection = currentDirection;
        roomsGenarated++;

        Debug.Log("First room generated, Moving to subsequent rooms");
        while (roomsGenarated < numberOfRooms)
        {
            Debug.LogFormat("Generating room {0}", roomsGenarated);
            currentRoom = Instantiate(GetNextRoom(), transform);
            currentDirection = ChartNewCourse(previousDirection);

            currentRoom.GetComponent<BasicRoom>().CreatePath(currentDirection, GetOppositeDirection(previousDirection));

            //Position the room based on direction
            switch(currentDirection)
            {
                case Direction.Top:
                    translationVector = new Vector3(0, (currentRoom.GetComponent<BasicRoom>().Height / 2) + (previousRoom.GetComponent<BasicRoom>().Height / 2) + previousRoom.transform.position.y);
                    break;
                case Direction.Bottom:
                    translationVector = new Vector3(0, 0);
                    break;
                case Direction.Left:
                    translationVector = new Vector3(0, 0);
                    break;
                case Direction.Right:
                default:
                    translationVector = new Vector3((currentRoom.GetComponent<BasicRoom>().Width / 2) + (previousRoom.GetComponent<BasicRoom>().Width / 2) + previousRoom.transform.position.x, 0);
                    break;
            }
            Debug.Log("Translating room");
            Debug.Log(translationVector);
            currentRoom.transform.Translate(translationVector);

            // Set values for next iteration
            previousDirection = currentDirection;
            previousRoom = currentRoom;
            roomsGenarated++;
        }

        return;
    }

    private GameObject GetNextRoom()
    {
        // Once you have more rooms, Make this random
        return BasicRoom;
    }

    private Direction ChartNewCourse(Direction previousDirection)
    {
        // Make this a little more random

        return Direction.Right;
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        Direction returnValue;

        switch(direction)
        {
            case Direction.Top:
                returnValue = Direction.Bottom;
                break;
            case Direction.Bottom:
                returnValue = Direction.Top;
                break;
            case Direction.Left:
                returnValue = Direction.Right;
                break;
            case Direction.Right:
            default:
                returnValue = Direction.Left;
                break;
        }

        Debug.LogFormat("GetOppositeDirection was given {0} and returned {1}", direction, returnValue);

        return returnValue;
    }
}



public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}