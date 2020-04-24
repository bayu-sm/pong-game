using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Paddle : MonoBehaviour
{
	[SerializeField]
	private float paddleSpeed = 10;

	[SerializeField]
	private Color _colorBall;

	public Color ColorBall {
		get { return _colorBall; }
	}

	private float _paddleHalfLength;

	public float PaddleHalfLength {
		get { return _paddleHalfLength; }
	}

	private float maximumPosY;

    // Start is called before the first frame update
    void Start()
	{
		name = GamePlayManager.paddleName;
		SetPaddleHalfLength ();
		GetMaximumPosY ();
	}

	void SetPaddleHalfLength()
	{
		BoxCollider2D boxCol = GetComponent<BoxCollider2D> ();
		_paddleHalfLength = boxCol.size.x * transform.localScale.x > boxCol.size.y * transform.localScale.y ? boxCol.size.x * transform.localScale.x / 2 : boxCol.size.y * transform.localScale.y / 2;
	}

	void GetMaximumPosY()
	{
		maximumPosY = Camera.main.ScreenToWorldPoint (new Vector2 (0, Screen.height)).y - PaddleHalfLength;
	}

    // Update is called once per frame
    void Update()
	{
		Move (); 
		CheckMaximumPosY ();
	}

	const string horizontalName = "Horizontal";
	const string verticalName = "Vertical";

	float prevYPos;

	void Move()
	{
		if (GameManager.isAndroid) {
			Touch touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				prevYPos = touch.position.y;
			} else if (touch.phase == TouchPhase.Moved) {
				transform.Translate (transform.up * (touch.position.y - prevYPos / Screen.height * maximumPosY * 2) * Time.deltaTime); 
				prevYPos = touch.position.y;
			}
		} else {
			if (Input.GetAxisRaw (horizontalName) != 0) {
				transform.Translate (transform.up * Input.GetAxisRaw (horizontalName) * paddleSpeed * Time.deltaTime);
			} else {
				transform.Translate (transform.up * Input.GetAxisRaw (verticalName) * paddleSpeed * Time.deltaTime);
			}
		}
	}

	void CheckMaximumPosY()
	{
		if (transform.position.y > maximumPosY) {
			transform.position = new Vector2 (transform.position.x, maximumPosY);
		} else if (transform.position.y < -maximumPosY) {
			transform.position = new Vector2 (transform.position.x, -maximumPosY);
		}
	}
}
