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

        speed = Mathf.RoundToInt(targetRb.velocity.magnitude * 3.6f);
        speedLabel.text = speed + " km/h";
        tachometer.fillAmount = Mathf.Lerp(0, 1, speed / controller.maxSpeed);
    }
}
