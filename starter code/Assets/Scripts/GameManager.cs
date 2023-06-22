using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeam  // What colour team is playing
{
    NONE = -1,
    WHITE,
    BLACK,
};

public class GameManager : MonoBehaviour    // This is the main script to run the game
{
    Minimax minimax;

    BoardManager board;
    public PlayerTeam playerTurn;   // Whose turn it is
    bool kingDead = false;      // If the king is dead, game over
    public GameObject fromHighlight;    // Highlight on board to show moves
    public GameObject toHighlight;

    private static GameManager instance;    
    public static GameManager Instance
    {
        get { return instance; }
    }
    private bool isCoroutineExecuting = false;

    private void Awake()
    {
        if (instance == null)        
            instance = this;        
        else if (instance != this)        
            Destroy(this);    
    }    

    void Start()
    {
        minimax = Minimax.Instance;
        board = BoardManager.Instance;        
        board.SetupBoard();
    }

    private void Update()
    {
        StartCoroutine(DoAIMove()); // Runs the game itself
    }

    IEnumerator DoAIMove()
    {       
        if(isCoroutineExecuting)    // Checks if coroutine is already running
            yield break;

        isCoroutineExecuting = true;

        if (kingDead)                    // Is the game over? If so console message 
            Debug.Log(playerTurn + " wins!");        
        else if (!kingDead)     // If not dead, will make move for current playerTurn
        {                     
            MoveData move = minimax.GetMove();
        
            RemoveObject("Highlight");  // Removes previous turn highlight from board and instantiates new for current
            ShowMove(move);

            yield return new WaitForSeconds(1);     // Delay in coroutine
            
            SwapPieces(move);  // Makes the actual move
            if(!kingDead)                // Is the game over after this move? If not change player turn to opponent
                UpdateTurn();     

            isCoroutineExecuting = false;                                                                                                         
        }
    }

    public void SwapPieces(MoveData move)
    {
        TileData firstTile = move.firstPosition;
        TileData secondTile = move.secondPosition;        

        firstTile.CurrentPiece.MovePiece(new Vector2(secondTile.Position.x, secondTile.Position.y));

        CheckDeath(secondTile);
                        
        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.chessPosition = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;            
    }   

    private void UpdateTurn()
    {     
        playerTurn = playerTurn == PlayerTeam.WHITE ? PlayerTeam.BLACK : PlayerTeam.WHITE;        
    }

    void CheckDeath(TileData _secondTile)
    {
        if (_secondTile.CurrentPiece != null)        
            if (_secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)           
                kingDead = true;                           
            else
                Destroy(_secondTile.CurrentPiece.gameObject);        
    }

    void ShowMove(MoveData move)
    {
        GameObject GOfrom = Instantiate(fromHighlight);
        GOfrom.transform.position = new Vector2(move.firstPosition.Position.x, move.firstPosition.Position.y);
        GOfrom.transform.parent = transform;

        GameObject GOto = Instantiate(toHighlight);
        GOto.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
        GOto.transform.parent = transform;
    }

    public void RemoveObject(string text)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(text);
        foreach (GameObject GO in objects)
            Destroy(GO);        
    }
}
