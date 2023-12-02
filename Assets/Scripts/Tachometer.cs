using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    public GameObject target;
    public TMP_Text speedLabel;
    public Image tachometer;

   
    private float speed;
    private Rigidbody targetRb;
    private CarController controller;
    
    private void Start() {
        targetRb = target.GetComponent<Rigidbody>();
        controller = target.GetComponent<CarController>();
    }

    void FixedUpdate()
    {
        tachometer.fillAmount = Mathf.Lerp(0, 1, speed / controller.maxSpeed);
        if((int)speed >= (int)controller.maxSpeed)
        {
            tachometer.fillAmount -= Random.Range(.0f, .1f);
        }
        speed = Mathf.RoundToInt(targetRb.velocity.magnitude * 3.6f);
        speedLabel.text = speed + " km/h";
        tachometer.color = (int)speed >= (int)controller.maxSpeed ? new Color(1f, 0.1803f, 0.3882f) : new Color(0.1960f, 0.7254f, 0.6326f);
    }
}
