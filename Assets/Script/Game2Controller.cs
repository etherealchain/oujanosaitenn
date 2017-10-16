using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2Controller : MonoBehaviour {

	public Transform red;
	public Transform pikachu;
	public Transform fireLight;
	TextPlayer textPlayer;
	Coroutine textRoutine;
	RedHandler redHandler;
	Walking walking;

	bool isPlaying ;
	bool complete;

	void Awake(){
		isPlaying = false;
		complete = false;
	}

	// Use this for initialization
	void Start () {
		textPlayer = GetComponent<TextPlayer>();
		redHandler = red.GetComponent<RedHandler>();
		walking = red.GetComponent<Walking>();
		walking.enabled = false;
		red.GetComponent<Animator>().SetTrigger("faceUp");
		updateText();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isPlaying && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))){
			textPlayer.showArrow = true;
			updateText();
			updateGame(textPlayer.TextIndex);
		}

		if(complete){
			if(pikachu.GetComponent<Mover>().moveComplete){
				pikachu.GetComponent<Mover>().startPathFinding(redHandler.food.position);
				walking.stopMoving();
				walking.enabled = false;
				isPlaying = false;
				complete = false;
				float disX = redHandler.food.position.x - red.position.x;
				float disY = redHandler.food.position.y - red.position.y;
				if(Mathf.Abs(disX) > Mathf.Abs(disY)){
					if(disX > 0){
						red.GetComponent<Animator>().SetTrigger("faceRight");
					}
					else{
						red.GetComponent<Animator>().SetTrigger("faceLeft");
					}
				}
				else{
					if(disY > 0){
						red.GetComponent<Animator>().SetTrigger("faceUp");
					}
					else{
						red.GetComponent<Animator>().SetTrigger("faceDown");
					}
				}
			}
		}
	}

	void updateText(){
		if(textPlayer.isPlaying){
			textPlayer.finishText();
		}
		else{
			textPlayer.updateText();
		}
	}

	void updateGame(int nextLine){
		switch(nextLine){
			case 3:
			isPlaying = true;
			walking.enabled = true;
			textPlayer.showArrow = false;
			textRoutine = StartCoroutine(showTextLater(nextLine,15));
			break;
			case 8:
			isPlaying = true;
			walking.enabled = true;
			textPlayer.showArrow = false;
			break;
			case 11:
			isPlaying = true;
			walking.enabled = true;
			textPlayer.showArrow = false;
			textRoutine = StartCoroutine(showTextLater(nextLine,15));
			break;
			case 14:
			isPlaying = true;
			textPlayer.showArrow = false;
			walking.enabled = true;
			break;
			case 17:
			isPlaying = true;
			walking.enabled = true;
			textPlayer.showArrow = false;
			redHandler.checkDistance = true;
			break;
			case 20:
			isPlaying = true;
			textPlayer.showArrow = false;
			textPlayer.finishText();
			red.GetComponent<Mover>().startPathFinding(pikachu.position);
			StartCoroutine(waitForFinish());
			break;
			default:
			break;
		}
	}
	IEnumerator waitForFinish(){
		yield return new WaitForSeconds(2);
		SceneLoader.Instance.ChangeScene("scene5", true);
	}

	IEnumerator showTextLater(int index, float times){
		yield return new WaitForSeconds(times);
		textPlayer.setTextIndex(index);
		if(index == 9 ){
			walking.stopMoving();
			walking.enabled = false;
			isPlaying = false;
		}
	}
	public void gameFinish(){
		StopCoroutine(textRoutine);
		complete = true;
	}
	public void forestBlock(){
		textPlayer.setTextIndex(4);
		StopCoroutine(textRoutine);
		textRoutine = StartCoroutine(showTextLater(3,5));
	}
	public void foodPlaced(){
		StopCoroutine(textRoutine);
		textPlayer.setTextIndex(14);
		textPlayer.showArrow = true;
		isPlaying = false;
		walking.stopMoving();
		walking.enabled = false;
	}
	public void foodFounded(){
		StopCoroutine(textRoutine);
		textPlayer.setTextIndex(12);
		textPlayer.showArrow = true;
		walking.stopMoving();
		walking.enabled = false;
		isPlaying = false;
	}
	public void fireFounded(){
		fireLight.GetComponent<Animator>().SetTrigger("lightUp");
		walking.stopMoving();
		walking.enabled = false;
		StartCoroutine(waitForLight());
	}
	IEnumerator waitForLight(){
		yield return new WaitForSeconds(1);
		StopCoroutine(textRoutine);
		textPlayer.setTextIndex(5);
		isPlaying = false;
	}
	public void pikachuFounded(){
		textPlayer.setTextIndex(8);
		StopCoroutine(textRoutine);
		textRoutine = StartCoroutine(showTextLater(9,10));
	}
}
