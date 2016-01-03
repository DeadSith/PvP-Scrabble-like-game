using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    //Make Direction Checks
    public Tile TilePrefab;
    public byte NumberOfRows = 15;
    public byte NumberOfColumns = 15;
    public LetterBox Player1;
    public LetterBox Player2;
    public byte CurrentPlayer = 1;
    public float DistanceBetweenTiles = 1.2f;
    public Tile[,] Field;
	void Start () {
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
                Field[i,j]= newTile;
                xOffset += DistanceBetweenTiles;
            }
            xOffset = 0;
            yOffset += DistanceBetweenTiles;
        }
    }
}
