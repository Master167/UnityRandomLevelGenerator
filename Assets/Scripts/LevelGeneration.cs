using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public GameObject StartingRoom;

    public Direction StartingDirection;

    public List<GameObject> AvailableRooms;
    
    public int NumberOfRooms;

    // So I don't need to calculate the opposite every time.
    private Direction OppositeOfStartingDirection;

	// Use this for initialization
	void Start () {
        // Initialize the Random number generator
        Random.InitState(System.Environment.TickCount);

        OppositeOfStartingDirection = GetOppositeDirection(StartingDirection);

        if (NumberOfRooms < 1)
        {
            throw new System.Exception("Number of rooms for Level Generation is Less then 1");
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
        currentRoom = Instantiate(StartingRoom, transform);
        
        // determine direction
        currentDirection = StartingDirection;

        // Close extra doors
        currentRoom.GetComponent<BasicRoom>().OpenDoor(currentDirection);
        
        // set as previous room and direction
        previousRoom = currentRoom;
        previousDirection = currentDirection;
        roomsGenarated++;

        Debug.Log("First room generated, Moving to subsequent rooms");
        while (roomsGenarated < numberOfRooms)
        {
            //Debug.LogFormat("Generating room {0}", roomsGenarated);
            currentRoom = Instantiate(GetNextRoom(), transform);
            currentDirection = ChartNewCourse(previousDirection);

            currentRoom.GetComponent<BasicRoom>().CreatePath(currentDirection, GetOppositeDirection(previousDirection));

            translationVector = GetRoomTranslation(currentRoom, previousRoom, currentDirection, previousDirection);
            //Debug.Log("Translating room");
            //Debug.Log(translationVector);
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
        // Get a random value
        var value = Random.Range(0, AvailableRooms.Count);

        // Get a random room
        GameObject selectedRoom = AvailableRooms[value];

        return selectedRoom;
    }

    private Direction ChartNewCourse(Direction previousDirection)
    {
        Direction newDirection;
        // I didn't add left to this because going Left could cause problems with overlap.
        Direction[] possibleDirections = { Direction.Up, Direction.Down, Direction.Right };
        int value;
        do
        {
            value = Random.Range(0, possibleDirections.Length);
            //Debug.LogFormat("Random produced:{0} which means {1} direction", value, possibleDirections[value]);
            newDirection = possibleDirections[value];
        } while (newDirection == GetOppositeDirection(previousDirection) && newDirection != OppositeOfStartingDirection);
        

        return newDirection;
    }

    public static Direction GetOppositeDirection(Direction direction)
    {
        Direction returnValue;

        switch(direction)
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

    private Vector3 GetRoomTranslation(GameObject currentRoom, GameObject previousRoom, Direction currentDirection, Direction previousDirection)
    {
        //Debug.LogFormat("GetRoomTranslation was given currentDirection:{0} and previousDirection:{1}", currentDirection, previousDirection);
        Vector3 translationVector;
        Transform currentRoomDoorTransform = currentRoom.GetComponent<BasicRoom>().GetRoomTransform(GetOppositeDirection(previousDirection));
        Transform previousRoomDoorTransform = previousRoom.GetComponent<BasicRoom>().GetRoomTransform(previousDirection);

        //Debug.Log("Current Room Door Position");
        //Debug.Log(currentRoomDoorTransform.position);
        //Debug.Log("Current Room Door Local Position");
        //Debug.Log(currentRoomDoorTransform.localPosition);
        //Debug.Log("Previous Room Door Position");
        //Debug.Log(previousRoomDoorTransform.position);
        //Debug.Log("Previous Room Door Local Position");
        //Debug.Log(previousRoomDoorTransform.localPosition);

        if (previousDirection == Direction.Up)
        {
            translationVector = new Vector3(
                    System.Math.Abs(currentRoomDoorTransform.localPosition.x) + previousRoomDoorTransform.position.x,
                    System.Math.Abs(currentRoomDoorTransform.localPosition.y) + previousRoomDoorTransform.position.y,
                    System.Math.Abs(currentRoomDoorTransform.localPosition.z) + previousRoomDoorTransform.position.z
            );
        }
        else if (previousDirection == Direction.Down)
        {
            translationVector = new Vector3(
                    currentRoomDoorTransform.localPosition.x + previousRoomDoorTransform.position.x,
                    -currentRoomDoorTransform.localPosition.y + previousRoomDoorTransform.position.y,
                    currentRoomDoorTransform.localPosition.z + previousRoomDoorTransform.position.z
            );
        }
        else if (previousDirection == Direction.Left)
        {
            throw new System.NotImplementedException(string.Format("previousDirection {0} is not implemented", previousDirection));
            translationVector = new Vector3(
                    -currentRoomDoorTransform.localPosition.x + previousRoomDoorTransform.position.x,
                    currentRoomDoorTransform.localPosition.y + previousRoomDoorTransform.position.y,
                    currentRoomDoorTransform.localPosition.z + previousRoomDoorTransform.position.z
            );
        }
        else
        {
            // previousDirection == Direction.Right
            
            translationVector = new Vector3(
                    System.Math.Abs(currentRoomDoorTransform.localPosition.x) + previousRoomDoorTransform.position.x,
                    System.Math.Abs(currentRoomDoorTransform.localPosition.y) + previousRoomDoorTransform.position.y,
                    System.Math.Abs(currentRoomDoorTransform.localPosition.z) + previousRoomDoorTransform.position.z
            );
            //}
        }

        //Debug.Log("Resulting Translation Vector");
        //Debug.Log(translationVector);

        return translationVector;
    }
}



public enum Direction
{
    Up,
    Down,
    Left,
    Right
}