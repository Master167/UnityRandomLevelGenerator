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

        OppositeOfStartingDirection = Constants.GetOppositeDirection(StartingDirection);

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
        
        // Remember this will be relative to the LevelGeneration GameObject
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
            currentDirection = ChartNewCourse(previousDirection);

            currentRoom = Instantiate(GetNextRoom(currentDirection), transform);

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

    private GameObject GetNextRoom(Direction nextDirection)
    {
        GameObject selectedRoom;
        int count = 0;
        do
        {
            // Get a random value
            var value = Random.Range(0, AvailableRooms.Count);

            // Get a random room
            selectedRoom = AvailableRooms[value];

            count++;
            Debug.LogFormat("Selecting room {0}", count);
            // Check that room works for new direction
        } while (!selectedRoom.GetComponent<BasicRoom>().HasDirection(nextDirection));

        return selectedRoom;
    }

    private Direction ChartNewCourse(Direction previousDirection)
    {
        Direction oppositeOfPrevious = Constants.GetOppositeDirection(previousDirection);

        List<Direction> possibleDirections = new List<Direction> { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

        //// For a single direction, MAKE SURE ITS THE SAME OF THE STARTING DIRECTION
        //List<Direction> possibleDirections = new List<Direction> { Direction.Up };

        // To remove directions that we don't want to turn.
        possibleDirections.Remove(oppositeOfPrevious);
        possibleDirections.Remove(OppositeOfStartingDirection);

        int value = Random.Range(0, possibleDirections.Count);
        //Debug.LogFormat("Random produced:{0} which means {1} direction", value, possibleDirections[value]);
        Direction newDirection = possibleDirections[value];

        //Debug.LogFormat("ChartNewCourse Result:{0}", newDirection);

        return newDirection;
    }

    // This is not translating correctly for the StairRoom if Current = Up and Previous = Right
    // Also need to figure out a way to indicate what direction a room was designed for a player to go through.
    // Like From Top to Bottom or Left to Right.
    private Vector3 GetRoomTranslation(GameObject currentRoom, GameObject previousRoom, Direction currentDirection, Direction previousDirection)
    {
        Debug.LogFormat("GetRoomTranslation was given currentDirection:{0} and previousDirection:{1}", currentDirection, previousDirection);
        Vector3 translationVector;
        Transform currentRoomDoorTransform = currentRoom.GetComponent<BasicRoom>().GetRoomTransform(Constants.GetOppositeDirection(previousDirection));
        Transform previousRoomDoorTransform = previousRoom.GetComponent<BasicRoom>().GetRoomTransform(previousDirection);

        //Debug.LogFormat("Current Room Door Position: {0}", currentRoomDoorTransform.position);
        //Debug.LogFormat("Current Room Door Local Position: {0}", currentRoomDoorTransform.localPosition);
        //Debug.LogFormat("Previous Room Door Position: {0}", previousRoomDoorTransform.position);
        //Debug.LogFormat("Previous Room Door Local Position: {0}", previousRoomDoorTransform.localPosition);

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
            // Needs Help
            translationVector = new Vector3(
                    -System.Math.Abs(currentRoomDoorTransform.localPosition.x) + previousRoomDoorTransform.position.x,
                    System.Math.Abs(currentRoomDoorTransform.localPosition.y) + previousRoomDoorTransform.position.y,
                    System.Math.Abs(currentRoomDoorTransform.localPosition.z) + previousRoomDoorTransform.position.z
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
        }

        //Debug.LogFormat("Resulting Translation Vector: {0}", translationVector);

        return translationVector;
    }
}