using System.Net;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    public Transform arrow = default!;
    public bool IsAlternative { get; set; }
    public GameTile NextTileOnPath => nextOnPath;
    public Vector3 ExitPoint { get; private set; }
    public bool HasPath => distance != int.MaxValue;

    public GameTile GrowPathNorth() => GrowPathTo(north, Direction.South);
    public GameTile GrowPathEast() => GrowPathTo(east, Direction.West);
    public GameTile GrowPathSouth() => GrowPathTo(south, Direction.North);
    public GameTile GrowPathWest() => GrowPathTo(west, Direction.East);

    public GameTileContent Content
    {
        get => content;
        set
        {
            Debug.Assert(value != null, "Null assigned to content!");
            if (content != null)
            {
                Debug.Log("Start Recycle");
                content.Recycle();
            }
            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }

    public Direction PathDirection { get; private set; }

    private int distance;
    
    private GameTileContent content;

    private GameTile north, east, south, west, nextOnPath;

    private static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        Debug.Assert(
            west.east == null && east.west == null, "Redefined neighbors!"
        );
        west.east = east;
        east.west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        Debug.Assert(
            south.north == null && north.south == null, "Redefined neighbors!"
        );
        south.north = north;
        north.south = south;
    }

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }

    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    public void ShowPath()
    {
        if (distance == 0 || content.Type == GameTileContentType.Wall)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        arrow.gameObject.SetActive(true);
        arrow.localRotation =
            nextOnPath == north ? northRotation :
            nextOnPath == east ? eastRotation :
            nextOnPath == south ? southRotation :
            westRotation;
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }

    private GameTile GrowPathTo(GameTile neighbor, Direction direction)
    {
        Debug.Assert(HasPath, "No path!");
        if (!HasPath || neighbor == null || neighbor.Content.BlocksPath || neighbor.HasPath)
        {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;

        neighbor.ExitPoint = neighbor.transform.localPosition + direction.GetHalfVector();

        neighbor.PathDirection = direction;                  
        return neighbor;
    }

}
