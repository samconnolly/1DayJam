using UnityEngine;
using System.Collections;

public class inputHelper : MonoBehaviour {

	private static bool previous_state = false;
	private static bool state = false;
	public static bool clicked = false;

	void Update () {
			previous_state = state;
			state = Input.GetMouseButtonDown(0);
			clicked = false;
		}

	public static bool GetMouseClick()
	{
		if (previous_state == true && state == false && clicked == false)
		{
			//clicked = true;
			//print ("click");
			return true;
		}
		else{
			return false;
		}

	}

}
