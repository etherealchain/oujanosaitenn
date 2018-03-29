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
	bool playEnd;
	bool moveFinish ;
	bool sceneChanged ;

	AudioSource[] audioSources;
	LangTable table;

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
		playEnd = false;
		moveFinish = false;
		sceneChanged = false;
		face = null;
	}
	void Start () {
		// button setting
		TextAsset json = Resources.Load("lang_table") as TextAsset;
		table = JsonUtility.FromJson<LangTable>(json.text);
		changeButtonText();
		
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
			if(!playEnd && !win)
				updateGame(textPlayer.TextIndex);
		}

		if(moveFinish ){
			if(red.GetComponent<Mover>().moveComplete &&
				green.GetComponent<Mover>().moveComplete &&
				blue.GetComponent<Mover>().moveComplete )
				{
					if(!sceneChanged){
						SceneLoader.Instance.GetComponent<SceneLoader>().ChangeScene("scene3", true);
						sceneChanged = true;
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
				moveFinish = true;
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
					textPlayer.loadTextFile("game1Fail1");
					playEnd = true;
					updateText();
				}
				else if(mover.destination == river.position){
					textPlayer.loadTextFile("game1Fail2");
					playEnd = true;
					updateText();				
				}
				else if(mover.destination == redHouse.position){
					textPlayer.loadTextFile("game1Fail3");
					playEnd = true;
					updateText();
				}
				else if(mover.destination == greenHouse.position){
					textPlayer.loadTextFile("game1Fail4");
					playEnd = true;
					updateText();
				}
				else{
					textPlayer.loadTextFile("game1End");
					updateText();
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

	void changeButtonText(){
		Transform redHouseBut = buttonView.FindDeepChild("redHouse");
		Transform greenHouseBut = buttonView.FindDeepChild("greenHouse");
		Transform labBut = buttonView.FindDeepChild("lab");
		Transform roadBut = buttonView.FindDeepChild("road");
		Transform riverBut = buttonView.FindDeepChild("river");

		redHouseBut.GetChild(0).GetComponent<Text>().font = SceneLoader.Instance.currentFont;
		greenHouseBut.GetChild(0).GetComponent<Text>().font = SceneLoader.Instance.currentFont;
		labBut.GetChild(0).GetComponent<Text>().font = SceneLoader.Instance.currentFont;
		roadBut.GetChild(0).GetComponent<Text>().font = SceneLoader.Instance.currentFont;
		riverBut.GetChild(0).GetComponent<Text>().font = SceneLoader.Instance.currentFont;

		switch(SceneLoader.Instance.currentLanguage){
			case SceneLoader.Language.japanese:
			redHouseBut.GetChild(0).GetComponent<Text>().text = table.japanese[0];
			greenHouseBut.GetChild(0).GetComponent<Text>().text = table.japanese[1];
			labBut.GetChild(0).GetComponent<Text>().text = table.japanese[2];
			roadBut.GetChild(0).GetComponent<Text>().text = table.japanese[3];
			riverBut.GetChild(0).GetComponent<Text>().text = table.japanese[4];
			break;
			case SceneLoader.Language.english:
			redHouseBut.GetChild(0).GetComponent<Text>().text = table.english[0];
			greenHouseBut.GetChild(0).GetComponent<Text>().text = table.english[1];
			labBut.GetChild(0).GetComponent<Text>().text = table.english[2];
			roadBut.GetChild(0).GetComponent<Text>().text = table.english[3];
			riverBut.GetChild(0).GetComponent<Text>().text = table.english[4];
			break;
			case SceneLoader.Language.chinese:
			redHouseBut.GetChild(0).GetComponent<Text>().text = table.chinese[0];
			greenHouseBut.GetChild(0).GetComponent<Text>().text = table.chinese[1];
			labBut.GetChild(0).GetComponent<Text>().text = table.chinese[2];
			roadBut.GetChild(0).GetComponent<Text>().text = table.chinese[3];
			riverBut.GetChild(0).GetComponent<Text>().text = table.chinese[4];
			break;
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
