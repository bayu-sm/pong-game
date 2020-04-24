using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class MainMenuManager : MonoBehaviour
{
	public Text highScoreTxt;

    // Start is called before the first frame update
    void Start()
	{
		highScoreTxt.text = "Your highscore : " + PlayerPrefs.GetInt (GameManager.HighScoreSaveName).ToString ();
		TitleAnim ();
	}

	public void OnClickFreePlay()
	{
		NotificationHelper.ShowNotification ("Are you sure want to start free play mode?", () => {
			GameManager.gamePlayMode = GameManager.GAMEPLAY_MODE.FREE_PLAY;
			SceneLoader.LoadScene (GameManager.GamePlaySceneName);
		});
	}

	//Custom Mode

	[Header("Custom Play Data")]
	public RectTransform customPlayPanelRectTr;
	public Sprite[] allCharacterSprite;
	public Color[] allCharacterColor = new Color[] {
		Color.black,
		Color.blue,
		Color.cyan,
		Color.gray,
		Color.green,
		Color.magenta,
		Color.red,
		Color.white,
		Color.yellow
	};

	public Image playerSpriteMenu;
	public Slider playerSpeedMenu;
	public Text playerSpeedTxt;
	public Image playerColorMenu;

	private int playerSprite;//index
	private int playerColor;//index

	public void OnClickCustomPlay()
	{
		playerSpriteMenu.sprite = allCharacterSprite [playerSprite];
		playerSpeedTxt.text = playerSpeedMenu.value.ToString ();
		playerColorMenu.color = allCharacterColor [playerColor];
		customPlayPanelRectTr.gameObject.SetActive (true);
		LeanTween.scale (customPlayPanelRectTr, Vector3.one, 0.5f);
	}

	private readonly Vector3 defaultCustomPlayPanelScale = new Vector3 (0.1f, 0.1f, 1f);

	public void OnClickCloseCustomPlayPanel()
	{
		LeanTween.scale (customPlayPanelRectTr, defaultCustomPlayPanelScale,0.5f).setOnComplete (() => customPlayPanelRectTr.gameObject.SetActive (false));
	}

	public void OnClickChangePlayerSprite(bool isRight)
	{
		if (isRight) {
			playerSprite++;
			if (playerSprite >= allCharacterSprite.Length) {
				playerSprite = 0;
			}
		} else {
			playerSprite--;
			if (playerSprite < 0) {
				playerSprite = allCharacterSprite.Length - 1;
			}
		}
		playerSpriteMenu.sprite = allCharacterSprite [playerSprite];
	}

	public void OnClickChangePlayerSpeed()
	{
		//playerSpeedMenu.value = Mathf.RoundToInt (playerSpeedMenu.value);
		playerSpeedTxt.text = playerSpeedMenu.value.ToString ();
	}

	public void OnClickChangePlayerColor(bool isRight)
	{
		if (isRight) {
			playerColor++;
			if (playerColor >= allCharacterColor.Length) {
				playerColor = 0;
			}
		} else {
			playerColor--;
			if (playerColor < 0) {
				playerColor = allCharacterColor.Length - 1;
			}
		}
		playerColorMenu.color = allCharacterColor [playerColor];
	}

	public void OnClickPlayCustomPlay()
	{
		NotificationHelper.ShowNotification ("Are you sure want to start custom play mode?", () => {
			GameManager.gamePlayMode = GameManager.GAMEPLAY_MODE.CUSTOM_PLAY;
			GameManager.customSprite = allCharacterSprite[playerSprite];
			GameManager.customSpeed = playerSpeedMenu.value;
			GameManager.customColor = allCharacterColor[playerColor];
			GameManager.customTypeMovement = TYPE_MOVEMENT.TRANSFORM;
			SceneLoader.LoadScene (GameManager.GamePlaySceneName);
		});
	}

	public void OnClickRandomPlay()
	{
		NotificationHelper.ShowNotification ("Are you sure want to start custom play mode?", () => {
			GameManager.gamePlayMode = GameManager.GAMEPLAY_MODE.RANDOM_PLAY;
			GameManager.customSprite = allCharacterSprite [UnityEngine.Random.Range (0, allCharacterSprite.Length)];
			GameManager.customSpeed = UnityEngine.Random.Range (5, 21);
			GameManager.customColor = allCharacterColor [UnityEngine.Random.Range (0, allCharacterColor.Length)];
			GameManager.customTypeMovement = UnityEngine.Random.Range (0, 2) == 1 ? TYPE_MOVEMENT.TRANSFORM : TYPE_MOVEMENT.RIGIDBODY;
			SceneLoader.LoadScene (GameManager.GamePlaySceneName);
		});
	}

	#region Animation

	public RectTransform titleRectTr;

	float toAlpha = 1;

	void TitleAnim()
	{
		toAlpha = toAlpha == 1 ? 0.2f : 1;
		LeanTween.alpha (titleRectTr, toAlpha, 2).setOnComplete (() => TitleAnim ());
	}

	#endregion
}
