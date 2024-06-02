using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnmanager;

    private AudioSource _explosionAudio
        ;

    // Start is called before the first frame update
    void Start()
    {
        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_spawnmanager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }

        if (_explosionAudio == null)
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
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnmanager.StartSpawning();
            _explosionAudio.Play();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
