using System;
using UnityEngine;
using System.Collections.Generic;
public class Grid : MonoBehaviour
{
    public enum Direction
    {
        Horizontal, Vertical, None
    }
    //Make Direction Checks
    public Tile TilePrefab;
    public Direction CurrentDirection= Direction.None;
    public bool isFirstGeneral = true;
    public int CurrentTurn=1;
    //public bool isFirstCurrentTurn = true;
    public byte NumberOfRows = 15;
    public byte NumberOfColumns = 15;
    public LetterBox Player1;
    public LetterBox Player2;
    public byte CurrentPlayer = 1;
    public float DistanceBetweenTiles = 1.2f;
    public Tile[,] Field;
    public List<Tile> CurrentTiles;
    
    
     
	void Start () {
     CurrentTiles = new List<Tile>();
	CreateField();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateField()
    {
        var xOffset = 0f;
        var yOffset = 0f;
        Field = new Tile[NumberOfRows, NumberOfColumns];
        for (var i = 0; i < NumberOfRows; i++)
        {
            for (var j = 0; j < NumberOfColumns; j++)
            {
                var newTile = Instantiate(TilePrefab,
                    new Vector2(transform.position.x + xOffset, transform.position.y+ yOffset),
                    transform.rotation) as Tile;
                newTile.transform.SetParent(gameObject.transform);
                newTile.Column = j;
                newTile.Row = i;
                Field[i,j] = newTile;
                xOffset += DistanceBetweenTiles;
            }
            xOffset = 0;
            yOffset += DistanceBetweenTiles;
        }
    }

    public void OnEndTurn()
    {
        CurrentTurn++;
        if (CurrentPlayer == 1)
        {
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(true);
            CurrentTiles = new List<Tile>();
            CurrentDirection = Direction.None;
            CurrentPlayer = 2;
        }
        else
        {
            Player1.gameObject.SetActive(true);
            Player2.gameObject.SetActive(false);
            CurrentPlayer = 1;
            CurrentDirection=Direction.None;
            CurrentTiles = new List<Tile>();
        }
        Debug.Log("Current player: "+CurrentPlayer);
    }
}
