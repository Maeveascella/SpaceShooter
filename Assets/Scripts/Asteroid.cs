using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _ExplosionPrefab;
    private SpawnManager _spawnmanager;

    private AudioSource _ExplosionAudio;

    // Start is called before the first frame update
    void Start()
    {
        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _ExplosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_spawnmanager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }

        if (_ExplosionAudio == null)
        {
            Debug.LogError("Explosion Audio Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 7.5f * Time.deltaTime);

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnmanager.StartSpawning();
            _ExplosionAudio.Play();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
