using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static readonly string HighScoreSaveName = "highscore";

	public static readonly string MainMenuSceneName = "MainMenuScene";
	public static readonly string GamePlaySceneName = "GamePlayScene";

	public static GameManager instance;
	public static bool isAndroid;

	public enum GAMEPLAY_MODE
	{
		FREE_PLAY,
		CUSTOM_PLAY,
		RANDOM_PLAY
	}

	public static GAMEPLAY_MODE gamePlayMode = GAMEPLAY_MODE.FREE_PLAY;

	[Header("Ball Data")]
	public static Sprite customSprite;
	public static Color customColor;
	public static float customSpeed;
	public static TYPE_MOVEMENT customTypeMovement;

    // Start is called before the first frame update
    void Awake()
	{
		if (instance) {
			Destroy (gameObject);
			return;
		}
		instance = this;
		#if UNITY_ANDROID
		isAndroid = true;
		#else
		isAndroid = false;
		#endif
	}

    // Update is called once per frame
	void Update()
	{
		DontDestroyOnLoad (gameObject);
	}
}
