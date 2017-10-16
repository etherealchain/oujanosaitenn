using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour {

	public string next;
	public int waitSeconds;
	// Use this for initialization
	void Start () {
		StartCoroutine(waitForEnd(waitSeconds));
	}

	IEnumerator waitForEnd(int wait){
		yield return new WaitForSeconds(wait);
		SceneLoader.Instance.ChangeScene(next, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
