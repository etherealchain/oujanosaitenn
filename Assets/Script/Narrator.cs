using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Narrator : MonoBehaviour {

	public TextAsset Main;
	public string next;
	public bool fadeInWhite = true;

	ActionPlayer actionPlayer;
	TextPlayer textPlayer;
	string[] main;
	int index;

	void Awake(){
		index = 0;
	}
	// Use this for initialization
	void Start () {
		main = Main.text.Split(new string[]{"\r\n","\n"},StringSplitOptions.None);
		actionPlayer = GetComponent<ActionPlayer>();
		textPlayer = GetComponent<TextPlayer>();
		checkIfGotoNext();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)){
			checkIfGotoNext();
		}
	}

	void checkIfGotoNext(){
		if(index < main.Length){
			if(!actionPlayer.isPlaying && !textPlayer.isPlaying){
				if(main[index][0].Equals('1')){
					actionPlayer.updateAction();
				}
				if(main[index][1].Equals('1')){
					textPlayer.updateText();
				}
				index++;
			}
			else if(!actionPlayer.isPlaying && textPlayer.isPlaying){
				textPlayer.finishText();
			}
		}
		else if(actionPlayer.isPlaying){
			// do nothing
		}
		else if(textPlayer.isPlaying){
			textPlayer.finishText();
		}
		else{
			SceneLoader.Instance.GetComponent<SceneLoader>().ChangeScene(next,fadeInWhite);
		}
	}
}
