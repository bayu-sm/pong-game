using UnityEngine;


[CreateAssetMenu(menuName = "Ball Data",fileName = "New Ball Data")]
public class BallData : ScriptableObject
{
	public Sprite defaultBallSprite;
	public Color defaultBallColor;
	public float defaultBallSpeed;
	public TYPE_MOVEMENT defaultBallTypeMovement;
}
