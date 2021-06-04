using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGreen : BattleBase {

	BattleRed red;
	Coroutine battleRoutine;
	bool battleRoutineStarted;
	protected override void Awake(){
		base.Awake();
		battleRoutineStarted = false;
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();
		red = GetComponent<BattleRed>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(gameController.battleStart){
			if(sp < MaxValue && !spControl)
				spRoutine = StartCoroutine(spIncrease());

			if(!battleRoutineStarted)
				battleRoutine = StartCoroutine(battle());
		}
		else{
			if(battleRoutine != null)
				StopCoroutine(battleRoutine);
			battleRoutineStarted = false;
		}
	}

	void checkAttack(){
		if(sp > MaxValue*0.95f){
			gameController.Attack(false,pokemonPoint,3);
			sp = MinVlaue;
		}
		else if(sp > MaxValue*2/3 ){
			float redHp = red.GetHp();
			
			if(redHp <= 20){
				gameController.Attack(false,pokemonPoint,2);
				sp = MinVlaue;
			}
			else{
				if(Random.value < 0.33f){
					gameController.Attack(false,pokemonPoint,2);
					sp = MinVlaue;
				}
			}
		}
		else if(sp > MaxValue/3 ){
			float redSp = red.getSp();
			
			if(redSp > MaxValue*2/3){
				if(Random.value < 0.05f){
					gameController.Attack(false,pokemonPoint,1);
					sp = MinVlaue;
				}
			}
			else if(redSp > MaxValue/3){
				if(Random.value < 0.01f){
					gameController.Attack(false,pokemonPoint,1);
					sp = MinVlaue;
				}
			}
		}
	}	
	IEnumerator battle(){
		battleRoutineStarted = true;
		while(battleRoutineStarted){
			yield return new WaitForSeconds(0.6f);
			checkAttack();
		}
	}

	IEnumerator spIncrease(){
		spControl = true;
		while(sp < MaxValue){
			sp += 0.1f;
			yield return new WaitForSeconds(0.01f);
		}
		spControl = false;
	}
}
