using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Vector2 coords;
	public Sprite maptile;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 ( coords.x* maptile.rect.width * 0.01f, coords.y * maptile.rect.height * 0.01f, 0);
	}

	public void Move (Vector2 newPosition) {
		coords = newPosition;
	}
}
