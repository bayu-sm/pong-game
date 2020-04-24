using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class NotificationHelper : MonoBehaviour
{
	private static NotificationHelper instance;

	private static GameObject notificationUI;
	private static Text notificationTxt;

	private static System.Action actions;

	void Start()
	{
		if (instance)
			return;
		instance = this;
		notificationUI = transform.GetChild (0).gameObject;
		notificationTxt = notificationUI.transform.GetChild (0).GetComponent<Text> ();
		gameObject.SetActive (false);
	}

	private readonly Vector3 defaultCustomPlayPanelScale = new Vector3 (0.1f, 0.1f, 1f);

	public static void ShowNotification(string message,System.Action action)
	{
		notificationTxt.text = message;
		actions = new System.Action (action);
		instance.gameObject.SetActive (true);
		notificationUI.SetActive (true);
		LeanTween.scale (notificationUI, Vector3.one, 0.5f);
	}

	public void OnClickYes()
	{
		LeanTween.scale (notificationUI, defaultCustomPlayPanelScale, 0.5f).setOnComplete (() => {
			instance.gameObject.SetActive (false);
			notificationUI.SetActive (false);
			actions.Invoke ();
		});
	}

	public void OnClickNo()
	{
		LeanTween.scale (notificationUI, defaultCustomPlayPanelScale, 0.5f).setOnComplete (() => {
			instance.gameObject.SetActive (false);
			notificationUI.SetActive (false);
		});
	}
}
