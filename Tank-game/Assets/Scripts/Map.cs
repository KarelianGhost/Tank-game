using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Map
{
    public Grid map;
    private int width = 30;
    private int height = 30;
    private int cellSize = 2;
    private Vector3 originPosition = Vector3.zero;
    private int playerSpawnPosX;
    private int playerSpawnPosY;

    public Map(int playerSpawnPosX, int playerSpawnPosY) {
        map = new Grid(width, height, cellSize, originPosition);
        this.playerSpawnPosX = playerSpawnPosX;
        this.playerSpawnPosY = playerSpawnPosY;
    }
}
