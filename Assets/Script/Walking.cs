using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour {

	public enum Facing{up,down,right,left};
	Facing face;
	bool isWalking ;
	Animator animator;
	float forceScale ;
	Rigidbody2D rigid;

	public void stopMoving(){
		isWalking = false;
		rigid.velocity = Vector2.zero;
		switch(face){
			default:
			case Facing.up:
				animator.SetTrigger(StaticUtil.animeFaceUp);
			break;
			case Facing.down:
				animator.SetTrigger(StaticUtil.animeFaceDown);
			break;
			case Facing.right:
				animator.SetTrigger(StaticUtil.animeFaceRight);
			break;
			case Facing.left:
				animator.SetTrigger(StaticUtil.animeFaceLeft);
			break;
		}
	}
	void Awake(){
		forceScale = 80;
		isWalking = false;
	}

	// Use this for initialization
	void Start () {
		// size = (transform.localScale.x > transform.localScale.y)?transform.localScale.x:transform.localScale.y;
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isWalking){
			if(Input.GetAxis(StaticUtil.VerticalAxis) == 0 && Input.GetAxis(StaticUtil.HorizontalAxis) == 0){
				stopMoving();
			}
			else{
				// Vector3 temp;
				switch(face){
					default:
					case Facing.up:
						// transform.position = transform.position + new Vector3(0,speed,0)*Time.deltaTime;
						if(Input.GetAxis(StaticUtil.VerticalAxis) == 0 ){
							changeToHorizontalMovement();
						}
					break;
					case Facing.down:
						// transform.position = transform.position + new Vector3(0,-speed,0)*Time.deltaTime;
						if(Input.GetAxis(StaticUtil.VerticalAxis) == 0 ){
							changeToHorizontalMovement();
						}
					break;
					case Facing.right:
						// transform.position = transform.position + new Vector3(speed,0,0)*Time.deltaTime;
						if(Input.GetAxis(StaticUtil.HorizontalAxis) == 0 ){
							changeToVerticalMovement();
						}
					break;
					case Facing.left:
						// transform.position = transform.position + new Vector3(-speed,0,0)*Time.deltaTime;
						if(Input.GetAxis(StaticUtil.HorizontalAxis) == 0 ){
							changeToVerticalMovement();
						}
					break;
				}

				// if(!Physics.CheckSphere(temp, size/4, LayerMask.GetMask("unwalkable"))){
				// 	transform.position = temp;
				// }
			}
		}
		else{
			if(Input.GetAxis("Vertical") < 0){
				animator.SetTrigger(StaticUtil.animeWalkDown);
				isWalking = true;
				face = Facing.down;
				rigid.AddForce(Vector2.down*forceScale);
			}
			else if(Input.GetAxis("Vertical") > 0){
				animator.SetTrigger(StaticUtil.animeWalkUp);
				isWalking = true;
				face = Facing.up;
				rigid.AddForce(Vector2.up*forceScale);
			}
			else if(Input.GetAxis("Horizontal") > 0){
				animator.SetTrigger(StaticUtil.animeWalkRight);
				isWalking = true;
				face = Facing.right;
				rigid.AddForce(Vector2.right*forceScale);
			}
			else if(Input.GetAxis("Horizontal")  < 0){
				animator.SetTrigger(StaticUtil.animeWalkLeft);
				isWalking = true;
				face = Facing.left;
				rigid.AddForce(Vector2.left*forceScale);
			}
		}
	}

	void changeToVerticalMovement(){
		rigid.velocity = Vector2.zero;
		if(Input.GetAxis(StaticUtil.VerticalAxis)>0){
			face = Facing.up;
			animator.SetTrigger(StaticUtil.animeWalkUp);
			rigid.AddForce(Vector2.up*forceScale);
		}
		else{
			face = Facing.down;
			animator.SetTrigger(StaticUtil.animeWalkDown);
			rigid.AddForce(Vector2.down*forceScale);
		}
	}
	void changeToHorizontalMovement(){
		rigid.velocity = Vector2.zero;
		if(Input.GetAxis(StaticUtil.HorizontalAxis) > 0){
			face = Facing.right;
			animator.SetTrigger(StaticUtil.animeWalkRight);
			rigid.AddForce(Vector2.right*forceScale);
		}
		else{
			face = Facing.left;
			animator.SetTrigger(StaticUtil.animeWalkLeft);
			rigid.AddForce(Vector2.left*forceScale);
		}
	}
}
