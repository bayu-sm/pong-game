using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public enum TYPE_MOVEMENT{
	RIGIDBODY,
	TRANSFORM
}

[RequireComponent(typeof(Rigidbody2D),typeof(SpriteRenderer),typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
	//Geraknya bisa menggunakan rigidbody atau transform

	public TYPE_MOVEMENT typeMovement;

	private Rigidbody2D myRb;
	private SpriteRenderer mySprRender;

	private Vector2 direction;
	private float speed;

	public float SetSpeed {
		set { speed = value; }
	}

	private float maximumPosY;
	private float maximumPosX;

	private Color myDefaultColor;

    // Start is called before the first frame update
    void Start()
    {
		myRb = GetComponent<Rigidbody2D> ();
		mySprRender = GetComponent<SpriteRenderer> ();
		myDefaultColor = mySprRender.color;
		GenerateRandomDirection ();
		GetMaximumPos ();
    }

	void GetMaximumPos()
	{
		maximumPosY = Camera.main.ScreenToWorldPoint (new Vector2 (0, Screen.height)).y - (GetComponent<CircleCollider2D> ().radius * transform.localScale.y / 2);
		maximumPosX = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, 0)).x - (GetComponent<CircleCollider2D> ().radius * transform.localScale.x / 2);
	}

	//kemiringan arah vektor tidak boleh melebihi 45 derajat
	//Menggunakan trigonometri untuk menentukannya
	void GenerateRandomDirection()
	{
		//Random besaran kemiringannya antara -45 sampai 45 derajat
		int randomAngleDirection = UnityEngine.Random.Range (-45, 45);
		//Random apakah arahnya ke kanan atau kiri,jika kekiri tambahkan 180 derajat
		randomAngleDirection = UnityEngine.Random.Range (0, 2) == 1 ? 
			randomAngleDirection += 180 
			: randomAngleDirection;
		//Normalkan derajat sesuai unity circle dengan dikurangi 90 derajat lalu direpeat supaya nilainya antara 0-360
		//randomAngleDirection = (int)Mathf.Repeat (90 - randomAngelDirection, 360);
		randomAngleDirection = (int)Mathf.Repeat (randomAngleDirection, 360);
		//konversikan ke vektor menggunakan trigonometri
		//direction = new Vector2 (
		//	Mathf.Sin (randomAngleDirection * Mathf.Deg2Rad), 
		//	Mathf.Cos (randomAngleDirection * Mathf.Deg2Rad)
		//).normalized;
		direction = new Vector2 (
			Mathf.Cos (randomAngleDirection * Mathf.Deg2Rad), 
			Mathf.Sin (randomAngleDirection * Mathf.Deg2Rad)
		).normalized;
	}

    // Update is called once per frame
    void Update()
    {
		switch (typeMovement) {
		//case TYPE_MOVEMENT.RIGIDBODY:
		//	MoveWithRigidbody ();
		//	break;
		case TYPE_MOVEMENT.TRANSFORM:
			MoveWithTransform ();
			break;
		}
		CheckMaximumPos ();
    }

	void FixedUpdate()
	{
		switch (typeMovement) {
		case TYPE_MOVEMENT.RIGIDBODY:
			MoveWithRigidbody ();
			break;
		//case TYPE_MOVEMENT.TRANSFORM:
		//	MoveWithTransform ();
		//	break;
		}
	}

	void MoveWithRigidbody()
	{
		myRb.velocity = direction * speed;
	}

	void MoveWithTransform()
	{
		transform.Translate (direction * speed * Time.deltaTime);
	}

	bool mustCheckMaximumPosUp = true;
	bool mustCheckMaximumPosDown = true;

	void CheckMaximumPos()
	{
		if (transform.position.y > maximumPosY && mustCheckMaximumPosUp) {
			direction.y *= -1;
			mustCheckMaximumPosUp = false;
			mustCheckMaximumPosDown = true;
		} else if (transform.position.y < -maximumPosY && mustCheckMaximumPosDown) {
			direction.y *= -1;
			mustCheckMaximumPosDown = false;
			mustCheckMaximumPosUp = true;
		}
		if (transform.position.x > maximumPosX || transform.position.x < -maximumPosX) {
			//Debug.Log ("Ball is out!");
			GamePlayManager.GameOver (accurancyRate);
			gameObject.SetActive (false);
		}
	}

	bool checkTrigger = true;

	void OnTriggerEnter2D(Collider2D col)
	{
		//Cek jika objeck yang disentuh nama paddle
		if (checkTrigger && col.transform.name.Equals (GamePlayManager.paddleName)) {
			mustCheckMaximumPosUp = true;
			mustCheckMaximumPosDown = true;
			checkTrigger = false;
			Paddle paddle = col.transform.GetComponent<Paddle> ();
			//ubah arah vektornya
			float acc;
			ChangeDirectionAfterTouchPaddle (paddle.PaddleHalfLength, col.transform.position, out acc);
			accurancyRate = (accurancyRate * hitCount + acc) / (hitCount += 1);
			//ubah warnanya
			LeanTween.color (gameObject, paddle.ColorBall, 0.5f).setOnComplete
			(() => LeanTween.color (gameObject, myDefaultColor, 0.5f));
			StartCoroutine (WaitForFewSeconds ());
		}
	}

	IEnumerator WaitForFewSeconds()
	{
		yield return new WaitForSeconds (0.3f);
		checkTrigger = true;
	}

	private float accurancyRate = 0;
	private int hitCount = 0;

	//Dapatkan arah vektor setelah menyentuh pemukul(paddle)
	void ChangeDirectionAfterTouchPaddle(float paddleHalfLength,Vector2 pos,out float acc)
	{
		float differencePosY = 0;
		//float angleAdd = 0;
		if (transform.position.x < pos.x) {
			differencePosY = transform.position.y - pos.y;
		} else {
			differencePosY = pos.y - transform.position.y;
			//	angleAdd = 45;
		}
		differencePosY = Mathf.Abs (differencePosY) > paddleHalfLength ? 
			Mathf.Sign (differencePosY) 
			: differencePosY / paddleHalfLength;
		acc = 50 + (1 - Mathf.Abs (differencePosY)) * 50;
		float angleChange = 40f * differencePosY;
		float angleDirection = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg) + angleChange;
		angleDirection = Mathf.Repeat (angleDirection, 360);
		if (angleDirection >= 90 && angleDirection <= 270)
			angleDirection = Mathf.Clamp (angleDirection, 130, 230);
		else if (angleDirection < 90)
			angleDirection = Mathf.Clamp (angleDirection, 0, 50);
		else
			angleDirection = Mathf.Clamp (angleDirection, 310, 360);
		direction = new Vector2 (
			-Mathf.Cos (angleDirection * Mathf.Deg2Rad),
			Mathf.Sin (angleDirection * Mathf.Deg2Rad)
		);
	}
}
