using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{

    private float _speed = 3f;

    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private float _canFire;
    private float _fireRate = 2.5f;

    [SerializeField]
    private Transform _laserPoint;
    [SerializeField]
    private GameObject _explosionPrefab;


    private Transform _playerPos;
    private Player _player;
    private AudioSource _explosionAudio;
    private SpawnManager _spawnManager;

    private bool _isEnemyDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
        _laserPoint = GameObject.Find("GunPoint").GetComponent<Transform>();
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FireLaser();
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        Vector3 dir = transform.position - _playerPos.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);

        if (transform.position.y <= -6.5f)
        {
            //float RandomX = Random.Range (-10.5f, 10.5f)
            transform.position = new Vector3(Random.Range(-10.5f, 10.5f), 8.5f, 0);
        }
    }

    private void FireLaser()
    {
        if (Time.time > _canFire && _isEnemyDead == false)
        {
            _canFire = Time.time + _fireRate;           

            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, _laserPoint.position, transform.rotation);
            Laser laser = enemyLaser.GetComponent<Laser>();
            laser.AssignEnemyLaser();
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
