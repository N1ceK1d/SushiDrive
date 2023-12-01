using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float enginePower;
    public float brakePower;
    public WheelCollider FR;
    public WheelCollider FL;
    public WheelCollider RR;
    public WheelCollider RL;

    public float wheelBase;
    public float turnRadius;
    public float rearTrack;

    private float ackermanAngleLeft;
    private float ackermanAngleRight;

    private float vertical;
    private float horizontal;

    private float downForce;
    private float currSpeed;
    public Rigidbody car;

    private int coins = 0;

    private void Start() {
        car = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        currSpeed = car.velocity.magnitude * 3.6f;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        PozitiveAckermanSteering();
        FR.steerAngle = ackermanAngleRight;
        FL.steerAngle = ackermanAngleLeft;

        RR.motorTorque = vertical * enginePower;
        RL.motorTorque = vertical * enginePower;

        addDownForce();
    }

    private void Update() {
        Handbrake();
    }

    public void PozitiveAckermanSteering()
    {
        if(horizontal > 0)
        {
            ackermanAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * horizontal;
            ackermanAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * horizontal;
        }
        else if(horizontal < 0)
        {
            ackermanAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * horizontal;
            ackermanAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * horizontal;
        }
        else
        {
            ackermanAngleLeft = 0;
            ackermanAngleRight = 0;
        }
    }

    public void Handbrake()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RR.brakeTorque = brakePower;
            RL.brakeTorque = brakePower;
            Debug.Log("Stope");
        }
        else 
        {
            RR.brakeTorque = 0;
            RL.brakeTorque = 0;
        }
    }

	void addDownForce()
    {
		downForce = currSpeed / 2;
		car.AddForce(-transform.up * downForce * car.velocity.magnitude);
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins+=1;
        }
    }
}
