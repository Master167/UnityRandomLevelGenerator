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
        GameObject newRoom;
        Direction newDirection;
        Vector3 translationVector;

        // Initialize Level with first room
        // Put room in scene
        //Debug.Log("Starting to generate rooms");
        
        // Remember this will be relative to the LevelGeneration GameObject
        newRoom = Instantiate(StartingRoom, transform);
        
        // determine direction
        newDirection = StartingDirection;

        // Open the outgoing door
        newRoom.GetComponent<BasicRoom>().OpenDoor(newDirection);
        
        // set as previous room and direction
        previousRoom = newRoom;
        previousDirection = newDirection;
        roomsGenarated++;

        //Debug.Log("First room generated, Moving to subsequent rooms");
        while (roomsGenarated < numberOfRooms)
        {
            Debug.LogFormat("Generating room {0}", roomsGenarated);
            //newDirection = previousRoom.GetComponent<BasicRoom>().GetExitDirection(previousDirection);

            newRoom = GetNextRoom(Constants.GetOppositeDirection(previousDirection));

            // Using the Level Generator game object's transform, because we want all the rooms to be children of the level generator
            newRoom = Instantiate(newRoom, transform);

            // Open the newRoom's door
            newRoom.GetComponent<BasicRoom>().OpenDoor(Constants.GetOppositeDirection(previousDirection));

            // Move the new room
            translationVector = GetRoomTranslation(previousDirection, newRoom, previousRoom);
            //Debug.Log("Translating room");
            //Debug.Log(translationVector);
            newRoom.transform.Translate(translationVector);

            // Set values for next iteration
            newDirection = previousRoom.GetComponent<BasicRoom>().GetExitDirection(previousDirection);

            //Debug.LogFormat("PreviousDirection: {0} NewDirection: {1}", previousDirection, newDirection);

            previousDirection = newDirection;
            previousRoom = newRoom;
            roomsGenarated++;
        }

        return;
    }

    private GameObject GetNextRoom(Direction nextDirection)
    {
        GameObject selectedRoom;
        int count = 0;

        // Remove this once you're done working on this
        int errorCount = AvailableRooms.Count * 2;

        do
        {
            // Get a random value
            var value = Random.Range(0, AvailableRooms.Count);

            // Get a random room
            selectedRoom = AvailableRooms[value];

            count++;
            //Debug.LogFormat("Selecting room {0}", count);

            if (count > errorCount)
            {
                throw new System.Exception("GetNextRoom selection unable to find room for direction: " + nextDirection);
            }

            // Check that room works for new direction
        } while (!selectedRoom.GetComponent<BasicRoom>().HasDirection(Constants.GetOppositeDirection(nextDirection)));

        return selectedRoom;
    }

    /* Commented out for new direction
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
    */

    // This is not translating correctly for the StairRoom if New = Up and Previous = Right
    // Also need to figure out a way to indicate what direction a room was designed for a player to go through.
    // Like From Top to Bottom or Left to Right.
    /// <summary>
    /// Give the previous direction, newRoom, and previousRoom.
    /// Build the resulting vector to move the newRoom to line up
    /// with the previousRoom
    /// </summary>
    /// <param name="previousDirection"></param>
    /// <param name="newRoom"></param>
    /// <param name="previousRoom"></param>
    /// <returns></returns>
    private Vector3 GetRoomTranslation(Direction previousDirection, GameObject newRoom, GameObject previousRoom)
    {
        var newDirection = Constants.GetOppositeDirection(previousDirection);
        Vector3 translationVector = new Vector3();
        
        if (previousDirection == Direction.Up)
        {
            //translationVector = new Vector3(
            //        System.Math.Abs(currentRoomDoorTransform.localPosition.x) + previousRoomDoorTransform.position.x,
            //        System.Math.Abs(currentRoomDoorTransform.localPosition.y) + previousRoomDoorTransform.position.y,
            //        System.Math.Abs(currentRoomDoorTransform.localPosition.z) + previousRoomDoorTransform.position.z
            //);
        }
        else if (previousDirection == Direction.Down)
        {
            //translationVector = new Vector3(
            //        currentRoomDoorTransform.localPosition.x + previousRoomDoorTransform.position.x,
            //        -currentRoomDoorTransform.localPosition.y + previousRoomDoorTransform.position.y,
            //        currentRoomDoorTransform.localPosition.z + previousRoomDoorTransform.position.z
            //);
        }
        else if (previousDirection == Direction.Left)
        {
            //// Needs Help 
            //translationVector = new Vector3(
            //        -System.Math.Abs(currentRoomDoorTransform.localPosition.x) + previousRoomDoorTransform.position.x,
            //        System.Math.Abs(currentRoomDoorTransform.localPosition.y) + previousRoomDoorTransform.position.y,
            //        System.Math.Abs(currentRoomDoorTransform.localPosition.z) + previousRoomDoorTransform.position.z
            //);
        }
        else
        {
            //previousDirection == Direction.Right
            Debug.LogFormat("Moving Right");

            // Move the door to the previousRoom position
            var previousRoomPositionVector = previousRoom.transform.position;

            // Calculate a vector for how the doors need to align
            var previousDoorTransform = previousRoom.GetComponent<BasicRoom>().GetRoomTransform(previousDirection);
            var newDoorTransform = newRoom.GetComponent<BasicRoom>().GetRoomTransform(newDirection);

            Debug.LogFormat("PreviousDoor Position: {0} NewDoor Position: {1}", previousDoorTransform.position, newDoorTransform.position);
            Debug.LogFormat("PreviousDoor Local Position: {0} NewDoor Local Position: {1}", previousDoorTransform.localPosition, newDoorTransform.localPosition);

            var doorTranslationVector = new Vector3(
                previousDoorTransform.localPosition.x + System.Math.Abs(newDoorTransform.localPosition.x),
                previousDoorTransform.localPosition.y + System.Math.Abs(newDoorTransform.localPosition.y) // Not sure about the abs here
            );

            Debug.LogFormat("previousRoomPositionVector: {0} doorTranslationVector: {1}", previousRoomPositionVector, doorTranslationVector);

            translationVector = previousRoomPositionVector + doorTranslationVector;
            
        }

        Debug.LogFormat("Resulting Translation Vector: {0}", translationVector);

        return translationVector;
    }
}