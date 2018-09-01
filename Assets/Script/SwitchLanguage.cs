using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchLanguage : MonoBehaviour {

	Transform canvas;
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas").transform;
		switch(SceneManager.GetActiveScene().name){
			case "transition1":
			setText(canvas.GetChild(0).GetComponent<Text>(),5);
			break;
			case "transition2":
			setText(canvas.GetChild(0).GetComponent<Text>(),6);
			setText(canvas.GetChild(1).GetComponent<Text>(),7);
			break;
			case "game3":
			setText(canvas.Find("tutorial").GetChild(0).GetComponent<Text>(),8);
			break;
			case "end":
			setText(canvas.GetChild(0).GetComponent<Text>(),9);
			setText(canvas.GetChild(2).GetChild(0).GetComponent<Text>(),10);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setText(Text showText, int index){
		showText.font = SceneLoader.Instance.currentFont;
		switch(SceneLoader.Instance.currentLanguage){
			case SceneLoader.Language.japanese:
			showText.text = SceneLoader.Instance.table.japanese[index];
			break;
			case SceneLoader.Language.english:
			showText.text = SceneLoader.Instance.table.english[index];
			break;
			case SceneLoader.Language.chinese:
			showText.text = SceneLoader.Instance.table.chinese[index];
			break;
		}
	}
}
