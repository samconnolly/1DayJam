using UnityEngine;
using System.Collections;

public class mapTile : MonoBehaviour {

	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;
	public int index = 0;
	public Vector3 mousePosition;
	public Vector2 coords = new Vector2(0,0);
	public bool selected = false;
	public Player player;
	public bool adjacent = false;
	public GUIText baseText;
	private GUIText text;
	public Font font;
	public Vector3 position;
	public string type = "Free";
	public string location = "None";
	public map gameMap;
	private bool visible = false;

	// Use this for initialization
	void Start () {
		spriteRenderer = renderer as SpriteRenderer;
		spriteRenderer.enabled = false;
		baseText = objectHelper.textObject;
		text = Instantiate (baseText) as GUIText;
		position = Camera.main.WorldToViewportPoint (transform.position);
		text.transform.position = Camera.main.WorldToViewportPoint(transform.position);
		text.text = "";
		text.font = font;
		text.transform.position += new Vector3 (-0.03f, 0,0);
	}
	
	// Update is called once per frame
	void Update () {
		if (visible) {
						mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

						// mouse highlighting/clicking
		
						if (mousePosition.x > transform.position.x - sprites [0].rect.width * 0.005f
								&& mousePosition.x < transform.position.x + sprites [0].rect.width * 0.005f
								&& mousePosition.y > transform.position.y - sprites [0].rect.height * 0.005f
								&& mousePosition.y < transform.position.y + sprites [0].rect.height * 0.005f) {
			
								if (inputHelper.GetMouseClick ()) {
										index = 2;
										selected = true;
										if (adjacent == true && type != "empty") {
												player.Move (coords);
												gameMap.Close_Map ();
										}
								} else if (selected == true) {
										index = 2;
				
								} else {
										index = 1;
								}
						} else {
								index = 0;
								selected = false;
						}
		
						if (adjacent == true) {
								spriteRenderer.color = Color.white;
						} else {
								spriteRenderer.color = Color.gray;
						}
		
						spriteRenderer.sprite = sprites [index];
				}
	
	}

	public void Close_Tile()
	{
		spriteRenderer.enabled = false;
		text.text = "";
		visible = false;
		}
	public void Open_Tile()
	{
		spriteRenderer.enabled = true;
		text.text = type + " : " + location;
		visible = true;
	}
}
