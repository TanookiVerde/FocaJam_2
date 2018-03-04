using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	[SerializeField] private List<RectTransform> screens;
	[SerializeField] private List<Color> screensColors;

	[SerializeField] private List<GameObject> background;
	[SerializeField] private RectTransform bgBullet;

	[SerializeField] private GameData gameData;
	[SerializeField] private Dropdown dropdownMap;
	[SerializeField] private Dropdown dropdownRounds;

	int currentScreen = 0;
	[SerializeField] float duration = 0.5f;
	float initialPos;

	public void Start(){
		initialPos = screens[0].transform.localPosition.x;
		InitializeMenu();
	}
	public void OpenNewScene(string name){
		SceneManager.LoadSceneAsync(name);
	}
	public void ChangeScreen(int nextScreen){
		screens[currentScreen].transform.DOLocalMoveX(initialPos-2000,duration);
		Camera.main.DOColor(screensColors[nextScreen],duration);
		for(int i = 0; i < background.Count; i++){
			background[i].transform.DOScale(new Vector3(1,1,1)*(1+0.25f*nextScreen*(i+1)*0.25f),duration);
		}
		currentScreen = nextScreen;
		screens[currentScreen].transform.DOLocalMoveX(initialPos+2000,0);
		screens[currentScreen].transform.DOLocalMoveX(initialPos,duration);
	}
	public void InitializeMenu(){
		screens[currentScreen].transform.DOLocalMoveX(initialPos,0);
		for(int i = 1; i < screens.Count; i++){
			screens[i].transform.DOLocalMoveX(initialPos+2000,0);
		}
	}
	public void GameDataMapSave(){
		gameData.mapNumber = dropdownMap.value;
	}
	public void GameDataRoundsSave(){
		gameData.rounds = (RoundQuantity)dropdownRounds.value;
	}
}
