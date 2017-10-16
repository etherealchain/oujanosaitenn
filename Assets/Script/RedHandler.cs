using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHandler : MonoBehaviour {

	public Game2Controller gameController;
	public Transform food;
	public Transform forest;
	[HideInInspector]
	public bool checkDistance;
	bool getFire;
	bool getFood;
	bool foodPlaced;
	bool pikachuFirstTime;

	void Awake(){
		getFire = false;
		getFood = false;
		foodPlaced = false;
		pikachuFirstTime = true;
		checkDistance = false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(checkDistance){
			if( Vector3.Distance(food.position, transform.position) > 2.5f){
				checkDistance = false;
				gameController.gameFinish();
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if(other.name.Equals("fire1") || other.name.Equals("fire2")){
			if(!getFire){
				getFire = true;
				gameController.fireFounded();
			}
		}
		else if(other.name.Equals("forest")){
			if(getFire){
				Destroy(forest.gameObject);
			}
			else{
				gameController.forestBlock();
			}
		}
		else if(other.name.Equals("pikachuArea")){
			if(pikachuFirstTime){
				gameController.pikachuFounded();
				pikachuFirstTime = false;
			}
		}
		else if(other.name.Equals("Berry")){
			if(!getFood){
				food.gameObject.SetActive(false);
				getFood = true;
				gameController.foodFounded();
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.name.Equals("pikachuArea") && getFood){
			if(!foodPlaced && Input.GetKeyDown(KeyCode.Space)){
				foodPlaced = true;
				food.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
				food.gameObject.SetActive(true);
				gameController.foodPlaced();
			}
		}
	}
}
