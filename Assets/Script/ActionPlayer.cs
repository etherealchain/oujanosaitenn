using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionPlayer : MonoBehaviour {

	public TextAsset actionList;
	public Transform red;
	public Transform green;
	public Transform blue;
	public Transform oak;
	public Transform pikachu;
	public float routineWait = 1;

	[HideInInspector]
	public bool isPlaying;
	bool auto;
	bool waitTillStop;

	string[] actions;
	string[] parse;
	Transform character;
	string target;
	int index = 0;
	Coroutine routine;

	// Use this for initialization
	void Awake () {
		actions = actionList.text.Split(new string[]{"\r\n","\n"},StringSplitOptions.None);
		isPlaying = false;
		auto = false;
		waitTillStop = false;
	}

	void Update(){
		if(!auto && isPlaying){
			routine = StartCoroutine(routineUpdate());
			auto = true;
		}

		if(waitTillStop){
			if(character.GetComponent<Mover>().moveComplete){
				waitTillStop = false;
				isPlaying = false;
				auto = false;
			}
		}
	}
	
	public void updateAction(){
		if(index < actions.Length){
			parse = actions[index].Split(new string[]{" "},StringSplitOptions.None);
			if(isPlaying){
				parseCharacter(parse[0]);
				if(isPlaying && !waitTillStop){	// if is not stop or wait for stop
					parseMovement(parse[1]);
					if(parse.Length > 2){
						for(int i = 2; i < parse.Length; i++){
							parseLeft(parse[i]);
						}
					}
				}
			}
			else{
				for(int i = 0; i < parse.Length; i++){
					switch(i%2){
						default:
						case 0:
						parseCharacter(parse[i]);
						break;
						case 1:
						parseMovement(parse[i]);
						break;
					}
				}
			}
			index++;
		}
	}

	void parseCharacter(string CharacterName){
		if(CharacterName.Equals("red")){
			character = red;
		}
		else if(CharacterName.Equals("green")){
			character = green;
		}
		else if(CharacterName.Equals("blue")){
			character = blue;
		}
		else if(CharacterName.Equals("oak")){
			character = oak;
		}
		else if(CharacterName.Equals("pikachu")){
			character = pikachu;
		}
		else if(CharacterName.Equals("music")){
			character = transform;
			target = CharacterName;
		}
		else if(CharacterName.Equals("start")){
			isPlaying = true;
		}
		else if(CharacterName.Equals("stop")){
			isPlaying = false;
			auto = false;
			StopCoroutine(routine);
		}
		else if(CharacterName.Equals("waitTillStop")){
			waitTillStop = true;
			StopCoroutine(routine);
		}
		else{
			character = GameObject.Find(CharacterName).transform;
		}
		if(character != null)
			character.gameObject.SetActive(true);
	}
	void parseMovement(string movement){
		
		if(character.name.Equals("Manager")){
			if(target.Equals("music")){
				if(movement.Equals("off")){
					character.GetComponent<AudioSource>().Stop();
				}
			}
		}
		else{
			Animator animator = character.GetComponent<Animator>();
			Light light = character.GetComponent<Light>();
			if(animator != null){
				if(isPlaying){
					GameObject target = GameObject.Find(movement);
					if(target == null)
						animator.SetTrigger(movement);
					else
						character.GetComponent<Mover>().startPathFinding(target.transform.position);
				}
				else{
					animator.SetTrigger(movement);
				}
			}
			else if(light != null){
				if(movement.Equals("off")){
					light.intensity = 0;
				}
			}	
		}
	}
	void parseLeft(string left){
		if(left.Contains("face")){
			character.GetComponent<Mover>().finalFace = left;
		}
		else if(left.Equals("hide")){
			character.GetComponent<Mover>().hideWhenFinish = true;
		}
	}

	IEnumerator routineUpdate(){
		while(isPlaying){
			updateAction();
			yield return new WaitForSeconds(routineWait);
		}
	}
}
