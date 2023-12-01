using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoad : MonoBehaviour
{
    public List<GameObject> blocks; //Коллекция всех дорожных блоков
    public GameObject player; //Игрок
    public GameObject roadPrefab; //Префаб дорожного блока
    public GameObject carPrefab; //Префаб машины NPC
    public GameObject coinPrefab; //Префаб монеты
    
    private System.Random rand = new System.Random(); //Генератор случайных чисел
    
    void Update()
    {
        float x = player.GetComponent<CarController>().car.position.x; //Получение положения игрока
    
        var last = blocks[blocks.Count - 1]; //Номер дорожного блока, который дальше всех от игрока
    
        if(x > last.transform.position.x - 24.69 * 10f) //Если игрок подъехал к последнему блоку ближе, чем на 10 блоков
        {
            //Инстанцирование нового блока
            var block = Instantiate(roadPrefab, new Vector3(last.transform.position.x + 24.69f, last.transform.position.y, last.transform.position.z), Quaternion.identity); 
            block.transform.SetParent(gameObject.transform); //Перемещение блока в объект Road
            blocks.Add(block); //Добавление блока в коллекцию

            float side = rand.Next(1, 3) == 1 ? -1f : 1f; //Случайное определение стороны появления машины
 
            ////Добавление машины на сцену
            var car = Instantiate(carPrefab, new Vector3(last.transform.position.x + 24.69f, last.transform.position.y + 0.20f, last.transform.position.z + 1.30f * side), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            car.transform.SetParent(gameObject.transform); //Добавление машины в объект Road
        }
    
        foreach (GameObject block in blocks) 
        {
            bool fetched = block.GetComponent<RoadBlock>().Fetch(x); //Проверка, проехал ли игрок этот блок
    
            if(fetched) //Если проехал
            {
                blocks.Remove(block); //Удаление блока из коллекции
                block.GetComponent<RoadBlock>().Delete(); //Удаление блока со сцены
            }
        }
    }
}
