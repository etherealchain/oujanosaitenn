using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour {

	public Transform buttons;
	// Use this for initialization
	void Start () {
		buttons.gameObject.SetActive(false);
		StartCoroutine(activeButtons());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator activeButtons(){
		yield return new WaitForSeconds(30);
		buttons.gameObject.SetActive(true);
	}

	public void replay(){
		SceneLoader.Instance.ChangeScene("start",true);
	}
	public void title(){
		SceneLoader.Instance.ChangeScene("title", true);
	}
	public void quit(){
		Application.Quit();
	}
}
