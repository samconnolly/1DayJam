using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public map mapTile;
	public float width;
	public Sprite mapSprite;
	public Player player;
	private Player thisPlayer;
	private map tile;
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

	void Start () {

		thisPlayer = Instantiate (player) as Player;
		
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

		// define types
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				tile =  Instantiate (mapTile, new Vector3 (i*mapSprite.rect.width*0.01f, j*mapSprite.rect.height*0.01f, 0), Quaternion.identity) as map;
				tile.player = thisPlayer;
				tile.coords = new Vector2(i,j);

				if (tile.coords == bossCoords)
				{
					tile.type = "boss";
				}
				else if (empty.Contains(tile.coords))
				{
					tile.type = "empty";
				}
				else if (combat.Contains(tile.coords))
				{
					tile.type = "combat";
				}
				else if (trap.Contains(tile.coords))
				{
					tile.type = "trap";
				}
				else if (social.Contains(tile.coords))
				{
					tile.type = "social";
				}
				else if (puzzle.Contains(tile.coords))
				{
					tile.type = "puzzle";
				}
				else if (tile.coords == shopCoords)
				{
					tile.type = "Shop";
				}

				if (path.Contains(tile.coords))
				{
					tile.type = tile.type + " - path";
				}
			}
		}

	}

	void Update () {
	}
}
