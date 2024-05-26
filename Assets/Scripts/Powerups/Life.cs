using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player.AddLife();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        Destroy(this.gameObject);
    }
}
