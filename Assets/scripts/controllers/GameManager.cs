using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public  map mapObject;
	public  mapTile mapTileObject;
	public  Sprite mapSprite;
	public  Player playerObject;
	public GUIText textObject;
	public textScreen textScreenObject;

	public List<Sprite> scenes;
	private string titleText;
	private List<string[]> optionText;
	private string missionText;

	private map gameMap;
	private textScreen startScreen;

	void Start () {

		objectHelper.mapObject = mapObject;
		objectHelper.mapTileObject = mapTileObject;
		objectHelper.mapSprite = mapSprite;
		objectHelper.playerObject = playerObject;
		objectHelper.textObject = textObject;
		objectHelper.textScreenObject = textScreenObject;

		titleText = "Title 1";
		optionText = new List<string[]>{ new string[] {"Punishment choice","Reward choice"}};
		missionText = "This is the mission text";

		statsHelper.scenes = scenes;
		statsHelper.titleText = titleText;
		statsHelper.optionText = optionText;
		statsHelper.missionText = missionText;

		statsHelper.LoadXML();

		gameMap = Instantiate (mapObject) as map;
		objectHelper.map = gameMap;

		// start screen
		
		startScreen = Instantiate (objectHelper.textScreenObject) as textScreen;
		startScreen.Setup (statsHelper.missionText, 
		                   new string[]{"Begin"}, 
		                      statsHelper.scenes[0]);

	}

	void Update () {



	}
}
