using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game3Controller : MonoBehaviour {
	
	public Color hpFull;
	public Color hpHalf;
	public Color hpDying;
	public Color spFull;
	public Color sp2nd;
	public Color sp1st;
	public Color splow;
	public Transform red;
	public Transform green;
	public Transform squirtle;
	public Transform charmander;
	public Transform pikachu;
	public Transform eevee;
	public Transform redMask;
	public Transform greenMask;
	public Transform tutorial;
	[HideInInspector]
	public bool battleStart;

	bool isPlaying ;
	TextPlayer textPlayer;
	BattleRed battleRed;
	BattleGreen battleGreen;
	bool greenAttacked;

	void Awake(){
		battleStart = false;
		isPlaying = true;
	}
	// Use this for initialization
	void Start () {
		textPlayer = GetComponent<TextPlayer>();
		textPlayer.showArrow = true;
		updateText(0);
		updateGame(textPlayer.TextIndex);

		battleRed = GetComponent<BattleRed>();
		battleGreen = GetComponent<BattleGreen>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isPlaying && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))){
			updateText(textPlayer.TextIndex);
			updateGame(textPlayer.TextIndex);
		}
		if(battleStart)
		{
			tutorial.gameObject.SetActive(true);
		}
		else{
			tutorial.gameObject.SetActive(false);
		}
	}

	void updateText(int index){
		if(textPlayer.isPlaying){
			textPlayer.finishText();
			StartCoroutine(delayUpdate(2, index));
		}
		else{
			textPlayer.setTextIndex(index);
		}
	}

	void updateGame(int nextLine){
		switch(nextLine){
			case 1:
			StartCoroutine(delayToStopPlaying(2));
			break;
			case 2:
			isPlaying = true;
			green.GetComponent<Animator>().SetTrigger("moveOut");
			StartCoroutine(delayPokemonOut(1.5f, squirtle, greenMask,battleGreen.lifeBar));
			StartCoroutine(delayToStopPlaying(1.5f));
			break;
			case 3:
			isPlaying = true;
			textPlayer.showArrow = false;
			red.GetComponent<Animator>().SetTrigger("moveOut");
			StartCoroutine(delayPokemonOut(1, charmander, redMask,battleRed.lifeBar));
			StartCoroutine(delayBattleStart(2));
			break;
			case 8:
			isPlaying = true;
			textPlayer.showArrow = false;
			pokemonOut(pikachu, redMask, battleRed.lifeBar);
			StartCoroutine(delayBattleStart(2));
			break;
			case 18:
			isPlaying = true;
			textPlayer.showArrow = false;
			pokemonOut(eevee, greenMask, battleGreen.lifeBar);
			StartCoroutine(delayBattleStart(2));
			break;
			case 24:
			case 25:
			case 26:
			StartCoroutine(delayCheckHP(1.5f,greenAttacked));
			break;
			case 13:
			case 23:
			// change scene
			isPlaying = true;
			textPlayer.showArrow = false;
			StartCoroutine(delayChangeScene(2));
			break;
			default:
			break;
		}
	}

	public void Attack(bool isRed, int pokemon, int move){
		battleStart = false;
		tutorial.gameObject.SetActive(false);
		int nextLine;

		switch(move){
			default:
			case 1:
			if(isRed){
				if(pokemon == 2)
					textPlayer.setTextIndex(3);
				else
					textPlayer.setTextIndex(8);
				battleGreen.setSptoZero();
			}
			else{
				if(pokemon == 2)
					textPlayer.setTextIndex(13);
				else
					textPlayer.setTextIndex(18);
				battleRed.setSptoZero();
			}
			nextLine = 23;
			break;
			case 2:
			if(isRed){
				if(pokemon == 2)
					textPlayer.setTextIndex(4);
				else
					textPlayer.setTextIndex(9);
				battleGreen.minusHpSmall();
				if(battleGreen.pokemonPoint == 2)
					squirtle.GetComponent<Animator>().SetTrigger("hitted");
				else
					eevee.GetComponent<Animator>().SetTrigger("hitted");
			}
			else{
				if(pokemon == 2)
					textPlayer.setTextIndex(14);
				else
					textPlayer.setTextIndex(19);
				battleRed.minusHpSmall();
				if(battleRed.pokemonPoint == 2)
					charmander.GetComponent<Animator>().SetTrigger("hitted");
				else
					pikachu.GetComponent<Animator>().SetTrigger("hitted");
			}
			nextLine = 24;
			break;
			case 3:
			if(isRed){
				if(pokemon == 2)
					textPlayer.setTextIndex(5);
				else
					textPlayer.setTextIndex(10);
				battleGreen.minusHpBig();
				if(battleGreen.pokemonPoint == 2)
					squirtle.GetComponent<Animator>().SetTrigger("hitted");
				else
					eevee.GetComponent<Animator>().SetTrigger("hitted");
			}
			else{
				if(pokemon == 2)
					textPlayer.setTextIndex(15);
				else
					textPlayer.setTextIndex(20);
				battleRed.minusHpBig();
				if(battleRed.pokemonPoint == 2)
					charmander.GetComponent<Animator>().SetTrigger("hitted");
				else
					pikachu.GetComponent<Animator>().SetTrigger("hitted");
			}
			nextLine = 25;
			break;
		}
		greenAttacked = isRed;
		StartCoroutine(delayUpdate(2, nextLine));
	}
	
	void pokemonOut(Transform pokemon, Transform mask, Transform lifeBar){
		mask.GetComponent<Animator>().SetTrigger("maskOpen");
		pokemon.gameObject.SetActive(true);
		lifeBar.gameObject.SetActive(true);
	}
	IEnumerator delayToStopPlaying(float waitTime){
		yield return new WaitForSeconds(waitTime);
		isPlaying = false;
		textPlayer.showArrow = true;
	}
	IEnumerator delayBattleStart(float waitTime){
		yield return new WaitForSeconds(waitTime);
		battleStart = true;
	}
	IEnumerator delayPokemonOut(float waitTime, Transform pokemon, Transform mask, Transform lifeBar){
		yield return new WaitForSeconds(waitTime);
		pokemonOut(pokemon,mask,lifeBar);
	}
	IEnumerator delayCheckHP(float waitTime, bool green){
		yield return new WaitForSeconds(waitTime);
		BattleBase battle;
		if(green)
			battle = battleGreen;
		else
			battle = battleRed;

		if(battle.GetHp() == battle.MinVlaue){
			if(battle.pokemonPoint == 2){
				if(green){
					squirtle.GetComponent<Animator>().SetTrigger("faint");
					textPlayer.setTextIndex(16);
				}
				else{
					charmander.GetComponent<Animator>().SetTrigger("faint");
					textPlayer.setTextIndex(6);
				}
				battle.pokemonPoint--;
				battle.resetHp();
				battle.resetSp();
			}
			else{
				if(green){
					eevee.GetComponent<Animator>().SetTrigger("faint");
					textPlayer.setTextIndex(21);
				}
				else{
					pikachu.GetComponent<Animator>().SetTrigger("faint");
					textPlayer.setTextIndex(11);
				}
			}
			isPlaying = false;
			textPlayer.showArrow = true;
		}
		else{
			battleStart = true;
		}
	}
	IEnumerator delayUpdate(float waitTime, int index){
		yield return new WaitForSeconds(waitTime);
		updateText(index);
		updateGame(textPlayer.TextIndex);
	}
	IEnumerator delayChangeScene(float waitTime){
		yield return new WaitForSeconds(waitTime);
		SceneLoader.Instance.ChangeScene("scene7",true);
	}
	
}
