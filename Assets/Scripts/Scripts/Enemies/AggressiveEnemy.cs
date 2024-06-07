using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    private Player _player;
    private Transform _playerPos;
    private float _speed = 4f;
    private SpawnManager _spawnManager;
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player Not Found");
        }
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not Found");
        }
        if (_explosionAudio == null)
        {
            Debug.LogError("ExplosionAudio Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (transform.position.y >= _playerPos.position.y)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            Vector3 dir = transform.position - _playerPos.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg * .5f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
        else if (transform.position.y <= _playerPos.position.y)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y <= -6.5f)
        {
            //float RandomX = Random.Range (-10.5f, 10.5f)
            transform.position = new Vector3(Random.Range(-10.5f, 10.5f), 8.5f, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
            EnemyDeath();
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(15);
            Destroy(other.gameObject);
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        _spawnManager.EnemyKilled();
        _explosionAudio.Play();
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, .1f);
    }

}
