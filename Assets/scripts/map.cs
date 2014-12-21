using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class map : MonoBehaviour {

	public Sprite[] sprites;
	public int index = 0;
	public Vector3 mousePosition;


	public Font font;
	public Vector3 position;
	public string type = "Free";

	private bool open = false;

	private Player playerObject;
	private mapTile mapTile;
	private Sprite mapSprite;
	private textScreen gameTextScreen;

	private Player thisPlayer;
	private List<mapTile> tiles = new List<mapTile>{};
	
	private float width;
	private int dist;
	private List<Vector2> path = new List<Vector2>{};
	private List<Vector2> empty = new List<Vector2>{};
	private List<Vector2> combat = new List<Vector2>{};
	private List<Vector2> social = new List<Vector2>{};
	private List<Vector2> trap = new List<Vector2>{};
	private List<Vector2> puzzle = new List<Vector2>{};
	private Vector2 shopCoords;
	public int[,] distGrid = new int[5, 5];
	public Vector2 playerCoords;
	public Vector2 bossCoords;
	private List<Vector2> locs = new List<Vector2>{};
	private List<Vector2> eventLocs = new List<Vector2>{};

	// village/wood, town/field, city/moor, keep/forest, cottage/hills, church/mountain, camp/lake
	private float[] locationChances = new float[]{ 0.16f,0.16f,0.13f,0.15f,0.16f,0.13f,0.11f};
	private string[] settlementLocations = new string[]{"village", "town", "city", "keep", "cottage", "church", "camp"};
	private string[] naturalLocations = new string[]{"wood", "field", "moor", "forest", "hills", "mountain", "lake"};

	private mapTile selectedTile;

	// Use this for initialization
	void Start () {

		playerObject = objectHelper.playerObject;
		thisPlayer = Instantiate (playerObject) as Player;
		objectHelper.player = thisPlayer;

		int dir = Random.Range (0, 2);
		if (dir == 0) {
			playerCoords = new Vector2 (Random.Range (0, 5), 0);
		} else {
			playerCoords = new Vector2 (0,Random.Range (0, 5));
		}
		thisPlayer.coords = playerCoords;
		
		bossCoords = thisPlayer.coords;
		while ((bossCoords - thisPlayer.coords).magnitude <= 4) {
			bossCoords = new Vector2 (Random.Range (0, 5), Random.Range (0, 5));
		}
		
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				dist = (int)((new Vector2(i,j) - bossCoords).magnitude*3);
				distGrid[i,j] = dist;
			}
		}
		
		path.Add(thisPlayer.coords);
		List<Vector2> moves;
		int k = 0;
		while (path[path.Count-1] != bossCoords && k < 100000){
			
			k += 1;
			moves = new List<Vector2> {new Vector2(1,0),
				new Vector2(-1,0),
				new Vector2(0,1),
				new Vector2(0,-1)};
			while (moves.Count > 0){	
				Vector2 move= moves[Random.Range(0,moves.Count)];
				Vector2 pos = path[path.Count-1] + move;
				
				if (pos.x >= 0 && pos.y >= 0 && pos.x < 5 && pos.y < 5)
				{
					if (path.Contains(pos) == false)
					{
						if (distGrid[(int)pos.x,(int)pos.y] < distGrid[(int)path[path.Count-1].x,(int)path[path.Count-1].y])
						{
							path.Add(pos);
						}
					}
				}
				moves.Remove(move);
			}
			moves = new List<Vector2> {new Vector2(1,0),
				new Vector2(-1,0),
				new Vector2(0,1),
				new Vector2(0,-1)};
			while (moves.Count > 0){	
				Vector2 move= moves[Random.Range(0,moves.Count)];
				Vector2 pos = path[path.Count-1] + move;
				
				if (pos.x >= 0 && pos.y >= 0 && pos.x < 5 && pos.y < 5)
				{
					if (path.Contains(pos) == false)
					{
						if (distGrid[(int)pos.x,(int)pos.y] <= distGrid[(int)path[path.Count-1].x,(int)path[path.Count-1].y])
						{
							path.Add(pos);
						}
					}
				}
				moves.Remove(move);
			}
		}
		
		// define all positions
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				locs.Add(new Vector2(i,j));
			}
		}
		
		// remove start position
		locs.Remove (thisPlayer.coords);
		
		// missing squares
		int missing = Random.Range (2, 4);
		int count = 0;
		bool edge = false;
		while (count < missing | edge == false)
		{
			
			Vector2 pos = new Vector2(Random.Range (0, 5),Random.Range (0, 5));
			if (path.Contains( pos) == false && bossCoords != pos)
			{
				if ((pos.x == 0 | pos.y == 0 | pos.x == 4 | pos.y == 4 ) && edge == false)
				{
					empty.Add(pos);
					edge = true;
					locs.Remove(pos);
				}
				else if (pos.x != 0 && pos.y != 0 && pos.x != 4 && pos.y != 4 )
				{
					empty.Add(pos);
					count += 1;
					locs.Remove(pos);
				}
			}
		}
		
		// shop
		shopCoords = locs [Random.Range (0, locs.Count)];
		locs.Remove (shopCoords);
		
		// combat
		for (int i = 0; i < 4; i++) {
			combat.Add(locs [Random.Range (0, locs.Count)]);
			locs.Remove (combat[combat.Count-1]);
		}
		// social
		for (int i = 0; i < 4; i++) {
			social.Add(locs [Random.Range (0, locs.Count)]);
			locs.Remove (social[social.Count-1]);
		}
		// trap
		for (int i = 0; i < 4; i++) {
			trap.Add(locs [Random.Range (0, locs.Count)]);
			locs.Remove (trap[trap.Count-1]);
		}
		// puzzle
		for (int i = 0; i < 4; i++) {
			puzzle.Add(locs [Random.Range (0, locs.Count)]);
			locs.Remove (puzzle[puzzle.Count-1]);
		}
		
		// define types, create tiles
		mapTile = objectHelper.mapTileObject;
		mapSprite = objectHelper.mapSprite;

		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				tiles.Add(Instantiate (mapTile, new Vector3 (i*mapSprite.rect.width*0.01f, j*mapSprite.rect.height*0.01f, 0), Quaternion.identity) as mapTile);
				tiles[tiles.Count-1].player = thisPlayer;
				tiles[tiles.Count-1].coords = new Vector2(i,j);
				tiles[tiles.Count-1].gameMap = this;
				
				if ((thisPlayer.coords - tiles[tiles.Count-1].coords).magnitude <= 1.0f) {
					tiles[tiles.Count-1].adjacent = true;
				} else {
					tiles[tiles.Count-1].adjacent = false;
				}

				//tiles[tiles.Count-1].type = i.ToString() + j.ToString();

				if (tiles[tiles.Count-1].coords == bossCoords)
				{
					tiles[tiles.Count-1].type = "boss";
				}
				else if (tiles[tiles.Count-1].coords == thisPlayer.coords)
				{
					tiles[tiles.Count-1].type = "Start";
				}
				else if (empty.Contains(tiles[tiles.Count-1].coords))
				{
					tiles[tiles.Count-1].type = "empty";
				}
				else if (combat.Contains(tiles[tiles.Count-1].coords))
				{
					tiles[tiles.Count-1].type = "combat";
					tiles[tiles.Count-1].location = Roll_Location();
				}
				else if (trap.Contains(tiles[tiles.Count-1].coords))
				{
					tiles[tiles.Count-1].type = "trap";
					tiles[tiles.Count-1].location = Roll_Location();
				}
				else if (social.Contains(tiles[tiles.Count-1].coords))
				{
					tiles[tiles.Count-1].type = "social";
					tiles[tiles.Count-1].location = Roll_Location();
				}
				else if (puzzle.Contains(tiles[tiles.Count-1].coords))
				{
					tiles[tiles.Count-1].type = "puzzle";
					tiles[tiles.Count-1].location = Roll_Location();
				}
				else if (tiles[tiles.Count-1].coords == shopCoords)
				{
					tiles[tiles.Count-1].type = "Shop";
				}
				else
				{					
					tiles[tiles.Count-1].location = Roll_Location();
				}
				
				if (path.Contains(tiles[tiles.Count-1].coords))
				{
					tiles[tiles.Count-1].type = tiles[tiles.Count-1].type + " - path";
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	private string Roll_Location()
	{
		bool natural = false;
		if (Random.Range (0, 2) == 0) {
						natural = true;
				}

		string location = "none";
		
		float roll = Random.value;
		float chance = 0;

		for (int i=0;i<locationChances.Length;i++)
		{

			chance += locationChances[i];


			if (roll < chance)
			{
				if (natural)
				{
					location = naturalLocations[i];
				}
				else
				{
					location = settlementLocations[i];
				}
				break;
			}
		}

		return location;
	}


	public void Close_Map()
	{		
		open = false;

		foreach (mapTile tile in tiles) {

			if (tile.selected == true)
			{
				selectedTile = tile;
			}
			tile.Close_Tile();
				}
		thisPlayer.Hide ();

		gameTextScreen = Instantiate (objectHelper.textScreenObject) as textScreen;
		gameTextScreen.Setup (selectedTile);
		
		print ("close map");
	}

	public void Open_Map()
	{
		open = true;
		
		foreach (mapTile tile in tiles) {

			tile.Open_Tile();
			
			if ((thisPlayer.coords - tile.coords).magnitude <= 1.0f) {
				tile.adjacent = true;
			} else {
				tile.adjacent = false;
			}
		}

		
		thisPlayer.Show();		
		print ("open map");

	}
}
