using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {

	public Button start;
	public GameObject languageButton;
	public GameObject languageView;

	SceneLoader.Language tempLang;
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
		SceneLoader.Instance.setCurrentLanguage( tempLang);
		languageView.SetActive(false);
	}

	public void languageButtonHandler(){
		languageView.SetActive(true);
		Button jp = languageView.transform.FindDeepChild("Jp").GetComponent<Button>();
		Button en = languageView.transform.FindDeepChild("En").GetComponent<Button>();
		Button ch = languageView.transform.FindDeepChild("Ch").GetComponent<Button>();
		switch(SceneLoader.Instance.currentLanguage){
			case SceneLoader.Language.japanese:
				jp.Select();
				break;
			case SceneLoader.Language.english:
				en.Select();
				break;
			case SceneLoader.Language.chinese:
				ch.Select();
				break;
		}
	}

	public void chooseJapanese(){
		tempLang = SceneLoader.Language.japanese;
	}
	public void chooseEnglish(){
		tempLang = SceneLoader.Language.english;
	}
	public void chooseChinese(){
		tempLang = SceneLoader.Language.chinese;
	}
}
