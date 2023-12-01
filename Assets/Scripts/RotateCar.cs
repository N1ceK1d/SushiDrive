using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCar : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    private void Update()
    {
        // Определяем угловую скорость вращения
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // Вращаем цилиндр вокруг оси X
        transform.Rotate(0, rotationAngle, 0);
    }
}
