using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Game1Controller : MonoBehaviour {

	public Transform red;
	public Transform green;
	public Transform blue;
	public Transform pikachu;
	public Transform redHouse;
	public Transform greenHouse;
	public Transform lab;
	public Transform river;
	public Transform road;
	public Transform buttonView;
	public Transform gameOverView;

	TextPlayer textPlayer;
	string[] parse;
	bool isPlaying;
	Transform character;
	string face;
	int gameIndex;

	bool redHouseGuarded ;
	bool greenHouseGuarded ;
	bool labGuarded ;
	bool roadGuarded ;
	bool riverGuarded ;

	bool win ;
	bool finish ;
	bool changed ;

	AudioSource[] audioSources;

	void Awake(){
		character = null;
		isPlaying = false;
		gameIndex= 0;
		redHouseGuarded = false;
		greenHouseGuarded = false;
		labGuarded = false;
		roadGuarded = false;
		riverGuarded = false;
		win = false;
		finish = false;
		changed = false;
		face = null;
	}
	void Start () {
		audioSources = GetComponents<AudioSource>();
		buttonView.gameObject.SetActive(false);
		gameOverView.gameObject.SetActive(false);
		textPlayer = GetComponent<TextPlayer>();
		updateText();
	}
	
	
	// Update is called once per frame
	void Update () {
		if(!isPlaying && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))){
			textPlayer.showArrow = true;
			updateText();
			updateGame(textPlayer.TextIndex);
		}
		if(finish ){
			if(red.GetComponent<Mover>().moveComplete &&
				green.GetComponent<Mover>().moveComplete &&
				blue.GetComponent<Mover>().moveComplete )
				{
					if(!changed){
						SceneLoader.Instance.GetComponent<SceneLoader>().ChangeScene("scene3", true);
						changed = true;
					}
				}
		}
		else if(textPlayer.over){
			if(win)
			{
				red.GetComponent<Mover>().hideWhenFinish = true;
				red.GetComponent<Mover>().startPathFinding(lab.position);
				green.GetComponent<Mover>().hideWhenFinish = true;
				green.GetComponent<Mover>().startPathFinding(lab.position);
				blue.GetComponent<Mover>().hideWhenFinish = true;
				blue.GetComponent<Mover>().startPathFinding(lab.position);
				finish = true;
			}
			else{
				if(!gameOverView.gameObject.activeSelf){
					gameOverView.gameObject.SetActive(true);
					audioSources[0].Stop();
					audioSources[1].Play();
				}
			}
		}

		if(character != null && character.GetComponent<Mover>().moveComplete){
			if(character == pikachu){
				Mover mover = pikachu.GetComponent<Mover>();
				if(mover.destination == road.position){
					textPlayer.playText(
						"ブルー：　ああ...ピカチュウ逃げた...\n"+
						"グリーン：　レッドのせいだ、なぜ道路が守られない？\n"+
						"レッド：　何だと！\n"+
						"グリーン：　やる気か！");
				}
				else if(mover.destination == river.position){
					textPlayer.playText(
						"ブルー：　ピカチュウが水泳できるの？\n"+
						"グリーン：　珍しいね、驚いた\n"+
						"レッド：　あれが...なみのり？");					
				}
				else if(mover.destination == redHouse.position){
					textPlayer.playText(
						"ママ：　レッド！！どうしてネズミが家に入る！？\n"+
									"レッド：　しまった！！母ちゃん怒っていた！");
				}
				else if(mover.destination == greenHouse.position){
					textPlayer.playText(
						"ナナミ：　グリーン～ピカチュウを苛めるはだめよ～\n"+
						"グリーン：　レッド！おまえのせいだ！！\n"+
						"レッド：　オレは何もわからないわよ！\n");
				}
				else{
					textPlayer.playText(
						"グリーン：　オイ、ピカチュウが研究所の前に気を失った\n"+
						"ブルー：　あ！！ひどいの傷！！\n"+
						"レッド：　ピカチュウ！！！");
					win = true;
				}
				character = null;
				isPlaying = false;
				textPlayer.showArrow = true;
			}
			else if(face != null){
				character.GetComponent<Animator>().SetTrigger(face);
				character = null;
				isPlaying = false;
				face = null;
				gameIndex++;

				if(gameIndex == 3){
					isPlaying = true;
					pikachuRun();
				}
				else{
					textPlayer.activeArrow();
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

	void pikachuRun(){
		character = pikachu;
		Mover mover = character.GetComponent<Mover>();
		mover.hideWhenFinish = true;
		if(!roadGuarded){
			mover.startPathFinding(road.position);
		}
		else if(!riverGuarded){
			mover.startPathFinding(river.position);
		}
		else if(labGuarded){
			int i = UnityEngine.Random.Range(0,2);
			switch(i){
				default:
				case 0:
					mover.startPathFinding(redHouse.position);
				break;
				case 1:
					mover.startPathFinding(greenHouse.position);
				break;
			}
		}
		else{
			mover.startPathFinding(lab.position);
		}
	}

	void updateGame(int nextline){
		switch(nextline){
			case 3:
			isPlaying = true;
			character = blue;
			buttonView.gameObject.SetActive(true);
			textPlayer.showArrow = false;
			textPlayer.finishText();
			break;
			case 5:
			isPlaying = true;
			character = green;
			buttonView.gameObject.SetActive(true);
			textPlayer.showArrow = false;
			textPlayer.finishText();
			break;
			case 7:
			isPlaying = true;
			character = red;
			buttonView.gameObject.SetActive(true);
			textPlayer.showArrow = false;
			textPlayer.finishText();
			break;
			default:
			break;
		}
	}

	public void guardRedHouse(){
		updateText();
		character.GetComponent<Mover>().startPathFinding(redHouse.position);
		buttonView.GetChild(0).GetChild(0).GetComponent<Button>().interactable = false;
		face = StaticUtil.animeFaceDown;
		buttonView.gameObject.SetActive(false);
		redHouseGuarded = true;
	}
	public void guardGreenHouse(){
		updateText();
		character.GetComponent<Mover>().startPathFinding(greenHouse.position);
		buttonView.GetChild(0).GetChild(1).GetComponent<Button>().interactable = false;
		face = StaticUtil.animeFaceDown;
		buttonView.gameObject.SetActive(false);
		greenHouseGuarded = true;
	}
	public void guardLab(){
		updateText();
		character.GetComponent<Mover>().startPathFinding(lab.position);
		buttonView.GetChild(0).GetChild(2).GetComponent<Button>().interactable = false;
		face = StaticUtil.animeFaceDown;
		buttonView.gameObject.SetActive(false);
		labGuarded = true;
	}
	public void guardRoute(){
		updateText();
		character.GetComponent<Mover>().startPathFinding(road.position);
		buttonView.GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
		face = StaticUtil.animeFaceDown;
		buttonView.gameObject.SetActive(false);
		roadGuarded = true;
	}
	public void guardRiver(){
		updateText();
		character.GetComponent<Mover>().startPathFinding(river.position);
		buttonView.GetChild(0).GetChild(4).GetComponent<Button>().interactable = false;
		face = StaticUtil.animeFaceRight;
		buttonView.gameObject.SetActive(false);
		riverGuarded = true;
	}

	public void reTry(){
		SceneLoader.Instance.GetComponent<SceneLoader>().ChangeScene("game1", true);
	}
	public void quit(){
		Application.Quit();
	}

}
