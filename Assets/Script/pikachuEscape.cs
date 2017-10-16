using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pikachuEscape : MonoBehaviour {

	public Transform[] pos;

	List<Transform> randomSelect;
	bool isRunning ;
	Mover mover;
	Vector3 currentPos;

	void Awake(){
		isRunning = false;
	}
	
	// Use this for initialization
	void Start () {
		mover = GetComponent<Mover>();
		randomSelect = new List<Transform>();
		currentPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(isRunning && mover.moveComplete){
			isRunning = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(!isRunning){
			startRunning();
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if(!isRunning){
			startRunning();
		}
	}

	void startRunning(){
		randomSelect.Clear();
		for(int i = 0; i < 4; i++){
			if(currentPos != pos[i].position){
				randomSelect.Add(pos[i]);
			}
		}
		currentPos = randomSelect[Random.Range(0,3)].position;
		mover.startPathFinding(currentPos);
		isRunning = true;
	}
}
