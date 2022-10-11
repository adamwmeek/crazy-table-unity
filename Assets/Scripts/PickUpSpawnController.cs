using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawnController : MonoBehaviour
{
    public int numberToGenerate;
    public int generationTimeout;
    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnerCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnerCoroutine() 
    {
        yield return new WaitForSeconds(generationTimeout);
        
        for(int i=0; i < numberToGenerate; i++)
        {
            float x = Random.Range(-9.5f, 9.5f);
            float z = Random.Range(-9.5f, 9.5f);
            Vector3 position = new Vector3(x, 0.5f, z);
            Quaternion rotation = new Quaternion(0.25f, 0.25f, 0.25f, 1.0f);
            GameObject obj = Instantiate(prefab, position, rotation);

            yield return new WaitForSeconds(3);
        }   
    }
}
