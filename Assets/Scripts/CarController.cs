using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [Header("Engine parameters")]
    public float enginePower; //В Киловаттах
    public float maxRPM;
    public float maxSpeed;
    public float engineTorque;
    public float brakePower;

    [Header("Transmission params")]
    public float[] gearRatios;
    public float currentRatio;
    public int currentGear = 0;
    public float mainGearRatio;
    public TMP_Text currentGearText;

    [Header("Wheels colliders")]
    public WheelCollider FR;
    public WheelCollider FL;
    public WheelCollider RR;
    public WheelCollider RL;

    [Header("Steering param")]
    public float wheelBase;
    public float turnRadius;
    public float rearTrack;
    public float wheelCirle;
    // Диаметр колеса: Ширина шины, м х Профиль, % х 2 + Диаметр обода, дюймов х 2,54/100
    // Пример: шина 195/65R15: 0,195 х 0,65 х 2 + 15 х (2,54 / 100) = 0,63 м
    // Окружность колеса: Диаметр колеса х число Пи (3,14)
    // Пример: 0,63 м х 3,14 = 1,98 м

    private float ackermanAngleLeft;
    private float ackermanAngleRight;

    private float vertical;
    private float horizontal;

    private float downForce;
    private float currSpeed;
    public Rigidbody car;

    private int coins = 0;
    private bool isAlive = true; //Жива ли машина. Если да, то она будет двигаться
	private bool isKilled = false; //Эта переменная нужна, чтобы триггер сработал только один раз
    private float topSpeedDrag, idleDrag = 0.05f;
    private float reverseDrag = 0.6f;
	public  float runningDrag = 0.02f;

    private void Start() {
        car = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        MaxSpeed();
        LimitSpeed();
        CalculateEngineTorque();
        currSpeed = car.velocity.magnitude * 3.6f;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        PozitiveAckermanSteering();
        FR.steerAngle = ackermanAngleRight;
        FL.steerAngle = ackermanAngleLeft;

        RR.motorTorque = vertical * CalculateWheelTorque();
        RL.motorTorque = vertical * CalculateWheelTorque();

        addDownForce();
        adjustDrag();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            BoostGear();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            LowerGear();
        }  
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

     public float MaxSpeed()
    {
        maxSpeed = (wheelCirle / (mainGearRatio * currentRatio)) * (60f/1000f) * maxRPM;
        return maxSpeed;
    }

    public void LimitSpeed()
    {
        if(car.velocity.magnitude > maxSpeed / 3.6f)
        {
            car.velocity = Vector3.ClampMagnitude(car.velocity, maxSpeed / 3.6f);
        }
    }

    public float CalculateWheelTorque()
    {
        float torque = 0.0f;
        torque = (engineTorque * currentRatio * mainGearRatio) / 2f;
        Debug.Log(torque);
        return torque;
    }

    public float CalculateEngineTorque()
    {
        engineTorque = enginePower * 9550f / maxRPM; 
        return engineTorque;
    }

    public float BoostGear()
    {
        if(currentGear < gearRatios.Length - 1)
        {
            currentGear += 1;
            currentRatio = gearRatios[currentGear];
        }
        currentGearText.text = currentGear.ToString();
        return gearRatios[currentGear];
        
    }

    public float LowerGear()
    {
        if(currentGear >= 0)
        {
            currentGear -= 1;
            currentRatio = gearRatios[currentGear];
        }
        currentGearText.text = currentGear.ToString();
        return gearRatios[currentGear];
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins+=1;
        }
    }

    void adjustDrag()
	{
		if (currSpeed >= maxSpeed)
			car.drag = topSpeedDrag;
		else if (Input.GetAxis("Vertical") == 0)
			car.drag = idleDrag;
		else if (currSpeed >= maxSpeed && currentGear == -1 && RR.rpm <= 0)
			car.drag = reverseDrag;
		else {
			car.drag = runningDrag;
		}
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Car");
        if(other.tag == "Car" || other.tag == "Wall") //Если машина игрока столкнулась со стеной или другой машиной
        {
            Debug.Log("Car");
            if(car != null) //Если есть модель
			{
				if(!isKilled) //Если триггер ещё не сработал
				{
					//Destroy(car); //Удалить старую модель

					//Добавить новую модель
					// var broken = Instantiate(brokenPrefab, transform.position, Quaternion.Euler(new Vector3(0f, -270f, 0f)));
					// broken.transform.SetParent(modelHolder.transform);

					isKilled = true; //Указать, что триггер сработал
					StartCoroutine("Die"); //Запустить процесс умирания
				}
			}
        }
    
        if(other.tag == "Coin") //Если столкновение с монетой
        {
            if(car != null) //Если столкнулась машина игрока
            {
                coins += 100; //Добавить 100 очков

                other.GetComponent<SushiCoin>().Delete(); //Удалить монету
            }
        }
    }
    
    IEnumerator Die() //Процесс умирания
    {
        Debug.Log("End!");
        yield return new WaitForSeconds(2f); //Подождать 2 секунды
        //SceneManager.LoadScene("Menu"); //Перейти в меню
    }
}
