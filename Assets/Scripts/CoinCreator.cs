using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCreator : MonoBehaviour
{
    public int coins_count = 10;
    public Transform coin;

    public Vector3 minPosition;
    public Vector3 maxPosition;

    private Vector3 positionCoin;
    void Start()
    {

        for (int i = coins_count; i > 0; i--)
        {
            positionCoin = new Vector3(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z)
            );
            Instantiate(coin, positionCoin, Quaternion.identity);
        }
    }
}
