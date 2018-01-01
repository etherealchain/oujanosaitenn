using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {

	public Button start;
	public GameObject languageButton;
	public GameObject languageView;
	// Use this for initialization
	void Start () {
		languageView.SetActive(false);
		languageButton.SetActive(false);
		start.onClick.AddListener(() =>{SceneLoader.Instance.ChangeScene("start",true);});
		StartCoroutine(enableAfter());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator enableAfter(){
		yield return new WaitForSeconds(15);
		languageButton.SetActive(true);
	}
	public void closeLanguageView(){
		languageView.SetActive(false);
	}

	public void languageButtonHandler(){
		languageView.SetActive(true);
		Button jp = languageView.transform.FindDeepChild("Jp").GetComponent<Button>();
		Button en = languageView.transform.FindDeepChild("En").GetComponent<Button>();
		Button ch = languageView.transform.FindDeepChild("Ch").GetComponent<Button>();
		switch(SceneLoader.Instance.currentLanguage){
			case SceneLoader.language.japanese:
				jp.Select();
				break;
			case SceneLoader.language.english:
				en.Select();
				break;
			case SceneLoader.language.chinese:
				ch.Select();
				break;
		}
	}

	public void chooseJapanese(){
		SceneLoader.Instance.currentLanguage = SceneLoader.language.japanese;
	}
	public void chooseEnglish(){
		SceneLoader.Instance.currentLanguage = SceneLoader.language.english;
	}
	public void chooseChinese(){
		SceneLoader.Instance.currentLanguage = SceneLoader.language.chinese;
	}
}
