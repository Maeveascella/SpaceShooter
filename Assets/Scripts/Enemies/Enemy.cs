using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float _randomX;

    private Player _player;

    [SerializeField]
    private Animator _anim;

    private AudioSource _explodeAudio;

    [SerializeField]
    private GameObject _enemyLasers;
    [SerializeField]
    private GameObject _enemyShield;
    private float _fireRate;
    private float _canFire = -1f;
    private bool _isEnemyDead = false;
    private SpawnManager _spawnManager;

    private bool _willReflect;
    private bool _hasShield;

    // Start is called before the first frame update
    void Start()
    {
        _explodeAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if (_explodeAudio == null)
        {
            Debug.LogError("Explosion Audio Not Found");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player not Found");
        }

        _anim = GetComponent<Animator>();
        if ( _anim == null)
        {
            Debug.LogError("Animator not found");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Not found");
        }

        _randomX = Random.Range(-.6f, .6f);

        _hasShield = Random.value < .3;
        if (_hasShield == true)
        {
            _enemyShield.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        EnemyFire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            if (_hasShield == true)
            {
                _hasShield = false;
                _enemyShield.SetActive(false);
            }
            else
            {
                EnemyDeath();
            }
            
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            if (_hasShield == true)
            {
                _hasShield = false;
                _enemyShield.SetActive(false);
            }
            else
            {
                EnemyDeath();
            }
        }
    }

    public void EnemyDeath()
    {
        _isEnemyDead = true;
        _anim.SetTrigger("OnEnemy Death");
        _speed = 0;
        _explodeAudio.Play();
        Destroy(GetComponent<Collider2D>());
        _spawnManager.EnemyKilled();
        Destroy(this.gameObject, 2.3f);
    }

    private void EnemyMovement()
    {

        transform.Translate(new Vector3 ((_randomX), -1, 0) * _speed * Time.deltaTime);
        _willReflect = Random.value < .4 * Time.deltaTime;
        if (_willReflect == true)
        {
            _randomX = _randomX * -1;
        }
 
        if (transform.position.y <= -6.5f)
        {
            //float RandomX = Random.Range (-10.5f, 10.5f)
            transform.position = new Vector3(Random.Range(-10.5f, 10.5f), 8.5f, 0);
        }

        if (transform.position.x <= -11.1f)
        {
            transform.position = new Vector3(11.1f, transform.position.y, 0);
        }
        else if (transform.position.x > 11.1f)
        {
            transform.position = new Vector3(-11.1f, transform.position.y, 0);
        }

    }

    private void EnemyFire()
    {

        if (Time.time > _canFire && _isEnemyDead == false)
        {
            _fireRate = Random.Range(4f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLasers, transform.position + new Vector3(-1.983f, -0.45f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

}
