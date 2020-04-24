using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creator : EditorWindow
{
	//Membuat recources

	static Creator creator;

	private const string dataPathDefaultBall = "Assets/Resources/Prefabs/Ball.prefab";
	private const string dataPathBallData = "Assets/Resources/Prefabs/Ball Data/BallData";

	[MenuItem("Creator/Create Default Ball Prefab")]
	static void CreateDefaultBall()
	{
		CheckFolder ();
		GameObject defaultBall = new GameObject ("Ball", new System.Type[] {
			typeof(SpriteRenderer),
			typeof(Rigidbody2D),
			typeof(CircleCollider2D),
			typeof(Ball)
		});
		defaultBall.GetComponent<Rigidbody2D> ().gravityScale = 0;
		defaultBall.GetComponent<CircleCollider2D> ().isTrigger = true;
		PrefabUtility.SaveAsPrefabAsset (defaultBall, dataPathDefaultBall);
		DestroyImmediate (defaultBall);
	}

	[MenuItem("Creator/Create Ball Data")]
	static void OpenCreateBallDataWindow()
	{
		CheckFolder ();
		creator = (Creator)GetWindow<Creator> ("Create Ball Data");
		creator.minSize = new Vector2 (200, 100);
		creator.Show ();
	}

	const string folderPath0 = "Assets/Resources";
	const string folderPath1 = "Assets/Resources/Prefabs";
	const string folderPath2 = "Assets/Resources/Prefabs/Ball Data";

	static void CheckFolder()
	{
		if (!AssetDatabase.IsValidFolder (folderPath0)) {
			AssetDatabase.CreateFolder ("Assets", "Resources");
		}
		if (!AssetDatabase.IsValidFolder (folderPath1)) {
			AssetDatabase.CreateFolder ("Assets/Resources", "Prefabs");
		}
		if (!AssetDatabase.IsValidFolder (folderPath2)) {
			AssetDatabase.CreateFolder ("Assets/Resources/Prefabs", "Ball Data");
		}
	}

	static Sprite desireSprite;
	static Color desireColor = Color.white;
	static float desireSpeed;
	static TYPE_MOVEMENT desireTypeMovement = TYPE_MOVEMENT.TRANSFORM;
	Vector2 scrollPos;

	void OnGUI()
	{
		scrollPos = (Vector2)GUILayout.BeginScrollView (scrollPos);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Sprite Desire");
		desireSprite = (Sprite)EditorGUILayout.ObjectField (desireSprite, typeof(Sprite), false, null);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Color Desire");
		desireColor = EditorGUILayout.ColorField (desireColor);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Speed Desire");
		desireSpeed = EditorGUILayout.FloatField (desireSpeed);
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Type Movement Desire");
		desireTypeMovement = (TYPE_MOVEMENT)EditorGUILayout.EnumPopup (desireTypeMovement);
		GUILayout.EndHorizontal ();

		if (GUILayout.Button ("Create!")) {
			BallData bd = ScriptableObject.CreateInstance<BallData> ();
			bd.defaultBallSprite = desireSprite;
			bd.defaultBallColor = desireColor;
			bd.defaultBallSpeed = desireSpeed;
			bd.defaultBallTypeMovement = desireTypeMovement;
			int i = 0;
			while (AssetDatabase.IsMainAssetAtPathLoaded (dataPathBallData + i.ToString () + ".asset")) {
				i++;
			}
			AssetDatabase.CreateAsset (bd, dataPathBallData + i.ToString () + ".asset");
		}
		GUILayout.EndScrollView ();
	}
}
