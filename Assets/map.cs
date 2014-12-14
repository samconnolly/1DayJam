using UnityEngine;
using System.Collections;

public class map : MonoBehaviour {

	public Sprite[] spritesStart;
	public Sprite[] spritesPuzzle;
	public Sprite[] spritesCombat;
	public Sprite[] spritesSocial;
	public Sprite[] spritesTrap;
	public Sprite[] spritesShop;
	public Sprite[] spritesBoss;
	private Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	public int index = 0;
	private Vector3 mousePosition;
	public Vector2 coords = new Vector2(0,0);
	private bool selected = false;
	public Player player;
	private bool adjacent = false;

	// Use this for initialization
	void Start () {
		spriteRenderer = renderer as SpriteRenderer;
		sprites = spritesStart;
	}
	
	// Update is called once per frame
	void Update () {

		// adjacent?
		if ((player.coords - coords).magnitude <= 1.0f) {
						adjacent = true;
				} else {
						adjacent = false;
				}

		// mouse highlighting/clicking
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (mousePosition.x > transform.position.x - sprites[0].rect.width*0.005f
		    && mousePosition.x < transform.position.x + sprites[0].rect.width*0.005f
		    && mousePosition.y > transform.position.y - sprites[0].rect.height*0.005f
		    && mousePosition.y < transform.position.y + sprites[0].rect.height*0.005f) {

			if (Input.GetMouseButtonDown(0) | selected == true){
				index = 2;
				selected = true;
				if (adjacent == true)
				{
					player.Move(coords);
				}
			}
			else{
				index = 1;
			}
		} else {
			index = 0;
			selected = false;
		}

		if (adjacent == true) {
						spriteRenderer.color = Color.white;
				}
		else{
			spriteRenderer.color = Color.gray;
		}

		spriteRenderer.sprite = sprites [index];
	
	}
}
