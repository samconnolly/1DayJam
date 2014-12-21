using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class textScreen : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Vector2 mousePosition;

	private GUIText mainText = new GUIText();
	private List<GUIText> optionText = new List<GUIText>{};

	private map gameMap;

	// Use this for initialization
	void Start () {
		gameMap = objectHelper.map;
	
	}
	
	// Update is called once per frame
	void Update () {

		mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (mousePosition.x > 5
		    && mousePosition.x < 10) {

			for (int i=0; i<optionText.Count; i++)
			{
				if (mousePosition.y > 4.4f - i*0.8
				    && mousePosition.y < 5.2  - i*0.8){
					optionText[i].color = Color.red;
					if (inputHelper.GetMouseClick() == true) {	
						gameMap.Open_Map();
						Kill();
					}
				}
				else
				{
					optionText[i].color = Color.black;
				}
			}
		}
	}

	public void Setup(mapTile tile){

		// setup situation
		string loc = tile.location;
		location location;
		if (loc == "Village") {
						location = statsHelper.locations [0];
				}
		else{
			location = statsHelper.locations [0];
		}
		string text = "You are at a " + location.name + ". ";
		gameEvent thisEvent = location.events[Random.Range(0,3)];
		text = text + "There is a " + thisEvent.subject;
		int n = thisEvent.n;
		string keyCharacter = thisEvent.character;
		string reward = thisEvent.reward;
		string punishment = thisEvent.punishment;

		string[] options = new string[]{ "Reward option - " + reward, "Punishment option - " + punishment};

		Sprite sceneSprite = statsHelper.scenes [0];

		// setup text
		mainText = Instantiate (objectHelper.textObject) as GUIText;
		mainText.color = Color.black;
		mainText.text = text;
		mainText.fontSize = 24;
		
		spriteRenderer = renderer as SpriteRenderer;

		for (int i=0; i<options.Length; i++) {
			optionText.Add (Instantiate (objectHelper.textObject) as GUIText);
			optionText[optionText.Count -1].text = "- " + options[i];
			optionText[optionText.Count -1].fontSize = 24;
			optionText[optionText.Count -1].transform.position = new Vector3 (0.5f, 0.8f - i*0.1f, 0);
			optionText[optionText.Count -1].color = Color.black;
		}

		spriteRenderer.sprite = sceneSprite;

		transform.position = new Vector3 (5.0f, 2.95f, 0);//Camera.main.WorldToViewportPoint (transform.position);
		transform.localScale = new Vector3 (0.766f, 0.766f, 1);
		mainText.transform.position = new Vector3 (0.5f, 0.9f, 0);

	}

	public void Setup(string text, string[] options, Sprite sceneSprite){
		

		// setup text
		mainText = Instantiate (objectHelper.textObject) as GUIText;
		mainText.color = Color.black;
		mainText.text = text;
		mainText.fontSize = 24;
		
		spriteRenderer = renderer as SpriteRenderer;
		
		for (int i=0; i<options.Length; i++) {
			optionText.Add (Instantiate (objectHelper.textObject) as GUIText);
			optionText[optionText.Count -1].text = "- " + options[i];
			optionText[optionText.Count -1].fontSize = 24;
			optionText[optionText.Count -1].transform.position = new Vector3 (0.5f, 0.8f - i*0.1f, 0);
			optionText[optionText.Count -1].color = Color.black;
		}
		
		spriteRenderer.sprite = sceneSprite;
		
		transform.position = new Vector3 (5.0f, 2.95f, 0);//Camera.main.WorldToViewportPoint (transform.position);
		transform.localScale = new Vector3 (0.766f, 0.766f, 1);
		mainText.transform.position = new Vector3 (0.5f, 0.9f, 0);
		
	}

	public void Kill()
	{
		
		Destroy (gameObject);
		Destroy (mainText);
		foreach (GUIText t in optionText) {
						Destroy (t);
				}

		}
}
