using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public map mapTile;
	public float width;
	public Sprite mapSprite;
	public Player player;
	private Player thisPlayer;
	private map tile;

	void Start () {

		thisPlayer = Instantiate (player) as Player;
		thisPlayer.Move (new Vector2(3,3));
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				tile =  Instantiate (mapTile, new Vector3 (i*mapSprite.rect.width*0.01f, j*mapSprite.rect.height*0.01f, 0), Quaternion.identity) as map;
				tile.player = thisPlayer;
				tile.coords = new Vector2(i,j);
			}
		}

	}

	void Update () {
	
	}
}
