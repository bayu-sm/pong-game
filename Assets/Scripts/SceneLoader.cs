using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DentedPixel;

public class SceneLoader : MonoBehaviour
{
	private static GameObject canvasObj;
	private static RectTransform fadeRectTr;
	private static RectTransform loadingRectTr;

	private static SceneLoader instance;

	private static readonly Vector2 defaultLoadingRectPos = new Vector2 (-1920, 0);

	private static readonly Color whiteColor = new Color (235 / 255, 235 / 255, 235 / 255, 1);
	private static readonly Color clearColor = new Color (15 / 255, 15 / 255, 15 / 255, 0);

	void Awake()
	{
		if (instance) {
			Destroy (gameObject);
			return;
		}
		instance = this;
		canvasObj = gameObject;

		fadeRectTr = new GameObject ("Fade", typeof(RectTransform)).GetComponent<RectTransform> ();
		fadeRectTr.SetParent (canvasObj.transform);
		fadeRectTr.anchorMin = Vector2.zero;
		fadeRectTr.anchorMax = Vector2.one;
		fadeRectTr.offsetMin = Vector2.zero;
		fadeRectTr.offsetMax = Vector2.zero;
		fadeRectTr.gameObject.AddComponent<Image> ().color = clearColor;
		fadeRectTr.gameObject.SetActive (false);

		loadingRectTr = new GameObject ("Loading", typeof(RectTransform)).GetComponent<RectTransform> ();
		loadingRectTr.SetParent (canvasObj.transform);
		loadingRectTr.anchorMin = Vector2.zero;
		loadingRectTr.anchorMax = Vector2.one;
		loadingRectTr.offsetMin = Vector2.zero;
		loadingRectTr.offsetMax = defaultLoadingRectPos;
		loadingRectTr.gameObject.AddComponent<Image> ().color = whiteColor;
		loadingRectTr.gameObject.SetActive (false);
	}

	void Update()
	{
		DontDestroyOnLoad (gameObject);
	}

	public static void LoadScene(string sceneName)
	{
		canvasObj.SetActive (true);
		fadeRectTr.gameObject.SetActive (true);
		loadingRectTr.gameObject.SetActive (true);
		instance.StartCoroutine (LoadSceneIE (sceneName));
	}

	static IEnumerator LoadSceneIE(string sceneName)
	{
		LeanTween.alpha (fadeRectTr, 1, 1f);
		yield return new WaitForSeconds (1f);
		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneName);
		while (!operation.isDone) {
			loadingRectTr.offsetMax = new Vector2 ((1 - operation.progress) * -1920, 0);
			yield return null;
		}
		canvasObj.SetActive (false);
		loadingRectTr.offsetMax = defaultLoadingRectPos;
		fadeRectTr.GetComponent<Image> ().color = clearColor;
		fadeRectTr.gameObject.SetActive (false);
		loadingRectTr.gameObject.SetActive (false);
	}
}
