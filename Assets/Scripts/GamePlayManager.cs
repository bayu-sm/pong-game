using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class GamePlayManager : MonoBehaviour
{
	private const string dataPathDefaultBall = "Prefabs/Ball";
	private const string dataPathBallData = "Prefabs/Ball Data";

	public static readonly string paddleName = "Paddle";

	//public static GamePlayManager instance;

	private BallData ballData;
	private GameObject ballObjPrefabs;

	void Awake()
	{
		//instance = this;
		SetBallData (Instantiate (Resources.Load<GameObject> (dataPathDefaultBall)) as GameObject);
	}

	void SetBallData(GameObject ballObj)
	{
		switch (GameManager.gamePlayMode) {
		case GameManager.GAMEPLAY_MODE.FREE_PLAY:
			BallData[] allBD = Resources.LoadAll<BallData> (dataPathBallData);
			ballData = allBD [UnityEngine.Random.Range (0, allBD.Length)];
			ballObj.GetComponent<SpriteRenderer> ().sprite = ballData.defaultBallSprite;
			ballObj.GetComponent<SpriteRenderer> ().color = ballData.defaultBallColor;
			ballObj.GetComponent<Ball> ().SetSpeed = ballData.defaultBallSpeed;
			ballObj.GetComponent<Ball> ().typeMovement = ballData.defaultBallTypeMovement;
			break;
		case GameManager.GAMEPLAY_MODE.CUSTOM_PLAY:
			ballObj.GetComponent<SpriteRenderer> ().sprite = GameManager.customSprite;
			ballObj.GetComponent<SpriteRenderer> ().color = GameManager.customColor;
			ballObj.GetComponent<Ball> ().SetSpeed = GameManager.customSpeed;
			ballObj.GetComponent<Ball> ().typeMovement = GameManager.customTypeMovement;
			break;
		case GameManager.GAMEPLAY_MODE.RANDOM_PLAY:
			ballObj.GetComponent<SpriteRenderer> ().sprite = GameManager.customSprite;
			ballObj.GetComponent<SpriteRenderer> ().color = GameManager.customColor;
			ballObj.GetComponent<Ball> ().SetSpeed = GameManager.customSpeed;
			ballObj.GetComponent<Ball> ().typeMovement = GameManager.customTypeMovement;
			break;
		}
	}

	[Header("UI Object")]
	private static Text timeTxt;
	private static GameObject gameOverPanel;
	private static Text gameOverTxt;

	private static int prevTime;
	private static float timer;

	void Start()
	{
		timeTxt = transform.GetChild (0).GetComponent<Text> ();
		gameOverPanel = transform.GetChild (1).gameObject;
		gameOverTxt = gameOverPanel.transform.GetChild (0).GetComponent<Text> ();
		timeTxt.text = "00:00";

		prevTime = 0;
		timer = 0;
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer > prevTime) {
			prevTime = Mathf.RoundToInt (timer);
			timeTxt.text = Mathf.FloorToInt (timer / 60).ToString ("00") + ":"
			+ (prevTime % 60).ToString ("00");
		}
	}

	public static void GameOver(float accRate)
	{
		int score = Mathf.RoundToInt (timer * (accRate / 50));
		if (score > PlayerPrefs.GetInt (GameManager.HighScoreSaveName)) {
			PlayerPrefs.SetInt (GameManager.HighScoreSaveName, score);
		}
		//ShowNotif
		gameOverTxt.text = "Time : " + timeTxt.text + "\nAccurancy Rate : " + Mathf.Round (accRate * 100).ToString () + "\nYour score : " + score.ToString ();
		gameOverPanel.SetActive (true);
		LeanTween.scale (gameOverPanel, Vector3.one, 0.5f);
	}

	public void OnClickRestart()
	{
		NotificationHelper.ShowNotification ("Are you sure want to restart?", () => SceneLoader.LoadScene (GameManager.GamePlaySceneName));
	}

	public void OnClickBack()
	{
		NotificationHelper.ShowNotification ("Are you sure want to back to main menu?", () => SceneLoader.LoadScene (GameManager.MainMenuSceneName));
	}
}
