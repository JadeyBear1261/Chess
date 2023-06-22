using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData   // Move data move from, to and whether pieve is moved to killed
{
    public TileData firstPosition = null;
    public TileData secondPosition = null;
    public ChessPiece pieceMoved = null;
    public ChessPiece pieceKilled = null;
    public int score = int.MinValue;
}
