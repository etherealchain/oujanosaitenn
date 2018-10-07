using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBase : MonoBehaviour {

	public Transform lifeBar;
	[HideInInspector]
	public float MaxValue = 100;
	[HideInInspector]
	public float MinVlaue = 0;
	[HideInInspector]
	public int pokemonPoint;

	protected Game3Controller gameController;
	protected float sp;
	protected float hp;

	protected Transform hpSprite;
	protected Transform spSprite;

	protected Coroutine spRoutine;
	protected bool spControl;

	float smallDamage = 35;
	float bigDamage = 50;
	protected virtual void Awake(){
		pokemonPoint = 2;
		spRoutine = null;
		spControl = false;
		hp = MaxValue;
		sp = MinVlaue;
	}
	// Use this for initialization
	protected virtual void Start () {
		hpSprite = lifeBar.GetChild(0);
		spSprite = lifeBar.GetChild(1);
		gameController = GetComponent<Game3Controller>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		updateHp();
		updateSp();

		if(!gameController.battleStart){
			if(spRoutine != null)
				StopCoroutine(spRoutine);
			spControl = false;
		}
	}

	void updateHp(){
		float hpScale = 3.8f*(hp/100.0f);
		hpSprite.localScale = new Vector3(hpScale,hpSprite.localScale.y,1);

		if(hp<MaxValue*0.2f){
			hpSprite.GetComponent<SpriteRenderer>().color = gameController.hpDying;
		}
		else if(hp < MaxValue/2.0f){
			hpSprite.GetComponent<SpriteRenderer>().color = gameController.hpHalf;
		}
		else{
			hpSprite.GetComponent<SpriteRenderer>().color = gameController.hpFull;
		}
	}
	void updateSp(){

		float spScale = 2.4f*(sp/100.0f);
		spSprite.localScale = new Vector3(spScale, spSprite.localScale.y,1);

		if(sp<MaxValue/3){
			spSprite.GetComponent<SpriteRenderer>().color = gameController.splow;
		}
		else if(sp < MaxValue*2/3){
			spSprite.GetComponent<SpriteRenderer>().color = gameController.sp1st;
		}
		else if(sp < MaxValue*0.95f){
			spSprite.GetComponent<SpriteRenderer>().color = gameController.sp2nd;
		}
		else{
			spSprite.GetComponent<SpriteRenderer>().color = gameController.spFull;
		}
	}

	public float GetHp(){
		return hp;
	}
	public float getSp(){
		return sp;
	}
	public void resetHp(){
		hp = 100;
	}
	public void resetSp(){
		sp = 0;
	}

	public void setSptoZero(){
		StartCoroutine(spToZero());
	}
	public void minusHpSmall(){
		StartCoroutine(hpMinus(smallDamage));
	}
	public void minusHpBig(){
		StartCoroutine(hpMinus(bigDamage));
	}
	IEnumerator spToZero(){
		while(sp > MinVlaue){
			sp -= 0.5f;
			yield return new WaitForSeconds(0.01f);
		}
		if(sp < MinVlaue)
			sp = MinVlaue;
	}

	IEnumerator hpMinus(float value){
		float count = 0;
		float minus = 0.5f;
		while(count < value){
			hp -= minus;
			count += minus;
			if(hp <= MinVlaue){
				hp = MinVlaue;
				break;
			}
			yield return new WaitForSeconds(0.01f);
		}
	}
}
