using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Spawner : MonoBehaviour
{

    public GameObject[] prefabs;
    public int[] counts;
    public float duration=5;

    public string[] categories;



    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    private float startTime = 0;
    // Update is called once per frame
    void Update()
    {
        startTime += Time.deltaTime;
        if (startTime > duration)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Spawn();
            startTime = 0;
        }
    }

    public void Spawn(){
        var collider = GetComponent<PolygonCollider2D>();
        for (int j =0;j<counts.Length;j++)
        {
            for (int i = 0; i < counts[j]; i++)
            {
                var x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
                var y = Random.Range(collider.bounds.min.y, collider.bounds.max.y);
                var position = new Vector3(x, y, transform.position.z - 4);
                if (collider.OverlapPoint(position))
                {
                    var g = Instantiate(prefabs[j], transform);
                    g.transform.position = position;
                }
            }
        }
    }
}
