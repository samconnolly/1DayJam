using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public Vector2 coords;
	public Sprite maptile;
	public bool map = true;
	private int gold = 0;
	private int food = 20;
	private List<int> hp = new List<int>{10,10,10,10};
	private List<string> disease = new List<string>{"none","none","none","none"};
	private List<int> diseaseChance = new List<int>{0,0,0,0};
	public GUIText baseText;
	private GUIText goldText;
	private GUIText foodText;
	private GUIText hpText;
	private GUIText diseaseText;	
	public Font font;

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		
		goldText = Instantiate (baseText) as GUIText;
		goldText.transform.position = new Vector3 (0.05f, 0.9f, 0);
		goldText.font = font;
		foodText = Instantiate (baseText) as GUIText;
		foodText.transform.position = new Vector3 (0.05f, 0.85f, 0);
		foodText.font = font;
		hpText = Instantiate (baseText) as GUIText;
		hpText.transform.position = new Vector3 (0.05f, 0.8f, 0);
		hpText.font = font;
		diseaseText = Instantiate (baseText) as GUIText;
		diseaseText.transform.position = new Vector3 (0.05f, 0.75f, 0);
		diseaseText.font = font;
		
		spriteRenderer = renderer as SpriteRenderer;
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		goldText.text = "Gold: " + gold.ToString ();
		foodText.text = "Food: " + food.ToString();
		hpText.text = "HP: ";
		foreach (int h in hp)
		{
			hpText.text = hpText.text + " " + h.ToString ();
		}

		diseaseText.text = "Disease: ";
		for (int d=0; d < disease.Count; d ++)
		{
			diseaseText.text = diseaseText.text +   " " + disease[d].ToString ();
			diseaseText.text = diseaseText.text + " " + diseaseChance[d].ToString ();
		}

		transform.position = new Vector3 ( coords.x* maptile.rect.width * 0.01f, coords.y * maptile.rect.height * 0.01f, 0);
	}

	public void Move (Vector2 newPosition) {
		coords = newPosition;
	}
	public void Hide(){
				spriteRenderer.enabled = false;
		}
	
	public void Show(){
		spriteRenderer.enabled = true;
	}
}
