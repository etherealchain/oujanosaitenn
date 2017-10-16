using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedClick : MonoBehaviour {

	public Transform textview;
	int min = 0;
	int max = 100;
	int count = 0;
	bool tap = false;
	bool startSubtract = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(tap && Input.GetKeyDown(KeyCode.S)){
			tap = false;
			count++;
		}
		else if(!tap && Input.GetKeyDown(KeyCode.D)){
			tap = true;
			count++;
		}

		if(count > 0 && !startSubtract)
			StartCoroutine(subtract());
		textview.GetComponent<Text>().text = count.ToString();
	}

	IEnumerator subtract(){
		startSubtract = true;
		while(count > 0){
			yield return new WaitForSeconds(0.5f);
			count--;
		}
		startSubtract = false;
	}
}
