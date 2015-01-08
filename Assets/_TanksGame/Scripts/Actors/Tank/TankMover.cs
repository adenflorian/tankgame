﻿using UnityEngine;

public class TankMover : TankComponent {
	// Max values
	//[Range(0, 1)]
	public float maxForward = 5f;
	//[Range(0, 0.5f)]
	public float maxBackward = 2.5f;
	//[Range(0, 4)]
	public float maxTurnRate = 100f;

	// Acceleration values
	public float accelForward = 5f;
	public float accelBackward = 2.5f;
	public float accelTurn = 200f;

	// Friction Values
	public float frictionTurn = 300f;
	public float frictionforward = 4.5f;

	// Current values
	public float currentForwardSpeed = 0f;
	public float currentTurnRate = 0f;

	public float speedNormalized {
		get {
			return currentForwardSpeed / maxForward;
		}
		private set {
			speedNormalized = value;
		}
	}

	// Main gun movement (look)
	public float maxLookUp;

	public float maxLookDown;
	private float gunAngle = 0f;

	private void Update() {
		transform.position += transform.localToWorldMatrix.MultiplyVector(new Vector3(0f, 0f, currentForwardSpeed) * Time.deltaTime);

		transform.Rotate(0f, currentTurnRate * Time.deltaTime, 0f, Space.Self);
	}

	public void LookOrder(TankLook tankLook) {
		tank.tankTop.transform.Rotate(0f, tankLook.x, 0f, Space.Self);
		RotateGun(tankLook.y);
	}

	private void RotateGun(float y) {
		Transform gunTrans = tank.tankGun.transform;

		gunTrans.Rotate(y, 0f, 0f, Space.Self);
		gunAngle += y;

		if (gunAngle > maxLookUp) {
			float adjustX = gunAngle - maxLookUp;
			gunTrans.Rotate(-adjustX, 0f, 0f, Space.Self);
			gunAngle += -adjustX;
		} else if (gunAngle < maxLookDown) {
			float adjustX = maxLookDown - gunAngle;
			gunTrans.Rotate(adjustX, 0f, 0f, Space.Self);
			gunAngle += adjustX;
		}
	}

	public void MoveOrder(TankMove moveOrder) {
		if (moveOrder.forth) {
			currentForwardSpeed += accelForward * Time.deltaTime;
		} else if (moveOrder.back) {
			currentForwardSpeed -= accelBackward * Time.deltaTime;
		} else {
			if (currentForwardSpeed > 0) {
				currentForwardSpeed -= frictionforward * Time.deltaTime;
				currentForwardSpeed = Mathf.Clamp(currentForwardSpeed, 0, maxForward);
			} else if (currentForwardSpeed < 0) {
				currentForwardSpeed += frictionforward * Time.deltaTime;
				currentForwardSpeed = Mathf.Clamp(currentForwardSpeed, -maxBackward, 0);
			}
		}

		if (moveOrder.left) {
			currentTurnRate -= accelTurn * Time.deltaTime;
		} else if (moveOrder.right) {
			currentTurnRate += accelTurn * Time.deltaTime;
		} else {
			if (currentTurnRate > 0) {
				currentTurnRate -= frictionTurn * Time.deltaTime;
				currentTurnRate = Mathf.Clamp(currentTurnRate, 0, maxTurnRate);
			} else if (currentTurnRate < 0) {
				currentTurnRate += frictionTurn * Time.deltaTime;
				currentTurnRate = Mathf.Clamp(currentTurnRate, -maxTurnRate, 0);
			}
		}

		currentForwardSpeed = Mathf.Clamp(currentForwardSpeed, -maxBackward, maxForward);
		currentTurnRate = Mathf.Clamp(currentTurnRate, -maxTurnRate, maxTurnRate);
	}
}
