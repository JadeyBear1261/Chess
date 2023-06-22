using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData   // This script contains te 2D array of Tile Data and has information about what and where is on the board/tile
{
    private Vector2 position = Vector2.zero;
    public Vector2 Position
    {
        get{ return position; }
    }

    private ChessPiece currentPiece = null;
    public ChessPiece CurrentPiece
    {
        get{ return currentPiece; }
        set{ currentPiece = value; }
    }

    public TileData(int x, int y)
    {
        position.x = x;
        position.y = y;

        if (y == 0 || y == 1 || y == 6 || y == 7)        
            currentPiece = GameObject.Find("[" + x.ToString() + "," + y.ToString() + "]").GetComponent<ChessPiece>();        
    }

    public void SwapFakePieces(ChessPiece newPiece)
    {
        currentPiece = newPiece;
    }
}
