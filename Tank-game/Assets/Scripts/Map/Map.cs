using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapLocation
{
    public int x;
    public int y;

    public MapLocation(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && y == ((MapLocation)obj).y;
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }

}

public class MapObject
{
    public MapLocation pos;
    public enum Direction
    {
        top,
        left,
        down,
        right
    };
    public Direction dir;

    public MapObject(MapLocation pos, Direction dir)
    {
        this.pos = pos;
        this.dir = dir;
    }

    public MapObject()
    {

    }
}

public class Map
{
    private MapLocation headquartersPosition;
    private int width;
    private int height;
    private float cellSize = 2;
    private Vector3 originPosition = Vector3.zero;

    private int[,] gridArray;

    public Map(int width, int height, MapLocation headquartersPosition)
    {
        this.width = width;
        this.height = height;
        this.headquartersPosition = headquartersPosition;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(new MapLocation(x, y)), GetWorldPosition(new MapLocation(x, y + 1)), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(new MapLocation(x, y)), GetWorldPosition(new MapLocation(x + 1, y)), Color.white, 100f);
            }
        }
    }

    private Vector3 GetWorldPosition(MapLocation pos)
    {
        return new Vector3(pos.x, 0.0f, pos.y) * cellSize + originPosition;
    }
    public Vector3 GetCellCenterPosition(MapLocation pos)
    {
        return GetWorldPosition(pos) + new Vector3(cellSize / 2, 0, cellSize / 2);
    }

    private void GetXY(Vector3 worldPosition, out MapLocation pos)
    {
        pos = new MapLocation(Mathf.FloorToInt((worldPosition - originPosition).x / cellSize),
            Mathf.FloorToInt((worldPosition - originPosition).z / cellSize));
    }

    public void SetValue(MapLocation pos, int value)
    {
        if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
        {
            gridArray[pos.x, pos.y] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        MapLocation pos;
        GetXY(worldPosition, out pos);
        SetValue(pos, value);
    }

    public int GetValue(MapLocation pos)
    {
        if (pos.x >= 0 && pos.y >= 0 && pos.x < width && pos.y < height)
        {
            return gridArray[pos.x, pos.y];
        }
        else
        {
            return -1;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        MapLocation pos;
        GetXY(worldPosition, out pos);
        return GetValue(pos);
    }

    public MapLocation GetHeadquartersPosition()
    {
        return headquartersPosition;
    }
}
