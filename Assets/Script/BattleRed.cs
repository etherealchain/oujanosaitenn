using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRed : BattleBase {

	bool tap;
	float spIncrease = 2.5f;
	protected override void Awake(){
		base.Awake();
		tap = false;
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(gameController.battleStart){
			if(sp > MinVlaue && !spControl)
				spRoutine = StartCoroutine(spReduce());

			if(sp < MaxValue){
				if(tap && Input.GetKeyDown(KeyCode.Z)){
					tap = false;
					sp+=spIncrease;
				}
				else if(!tap && Input.GetKeyDown(KeyCode.X)){
					tap = true;
					sp+=spIncrease;
				}
			}
			if(Input.GetKeyDown(KeyCode.Space)){
				if(sp > MaxValue*0.95f){
					gameController.Attack(true, pokemonPoint,3);
				}
				else if(sp > MaxValue*2/3.0f){
					gameController.Attack(true, pokemonPoint,2);
				}
				else if(sp > MaxValue/3.0f){					
					gameController.Attack(true, pokemonPoint,1);
				}
				sp = MinVlaue;
				StopCoroutine(spRoutine);
				spControl = false;
			}
		}
	}

	IEnumerator spReduce(){
		spControl = true;
		while(sp > MinVlaue){
			yield return new WaitForSeconds(0.01f);
			sp-=0.1f;
		}
		spControl = false;
	}
}
