using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class statsHelper : MonoBehaviour {

	
	public static List<Sprite> scenes;
	public static string titleText;	
	public static string missionText;
	public static List<string[]> optionText;
	public static List<location> locations = new List<location>{};


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void LoadXML(){
		// load
		TextAsset textXML = (TextAsset)Resources.Load("stats", typeof(TextAsset));
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (textXML.text);

		// read
		XmlNodeList nodeList = doc.SelectNodes ("/occurences/combat/location");
		foreach (XmlNode node in nodeList){
			locations.Add((location)ScriptableObject.CreateInstance(typeof(location)));
			locations[locations.Count-1].name = node["name"].InnerText;

			XmlNodeList events = node.SelectNodes("outcome");
			foreach( XmlNode e in events)
			{
				locations[locations.Count-1].events.Add((gameEvent)ScriptableObject.CreateInstance(typeof(gameEvent)));
				locations[locations.Count-1].events[locations[locations.Count-1].events.Count-1].n = int.Parse(e["n"].InnerText);
				locations[locations.Count-1].events[locations[locations.Count-1].events.Count-1].subject = e["enemy"].InnerText;
				locations[locations.Count-1].events[locations[locations.Count-1].events.Count-1].reward = e["reward"].InnerText;
				locations[locations.Count-1].events[locations[locations.Count-1].events.Count-1].punishment = e["punishment"].InnerText;
				locations[locations.Count-1].events[locations[locations.Count-1].events.Count-1].character = e["character"].InnerText;
			}
		}

	}
}
