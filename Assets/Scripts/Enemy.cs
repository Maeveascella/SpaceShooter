using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    [SerializeField]
    private Animator _anim;

    private AudioSource _ExplodeAudio;

    [SerializeField]
    private GameObject _EnemyLasers;
    private float _fireRate;
    private float _canFire = -1f;
    private bool _isEnemyDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _ExplodeAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if (_ExplodeAudio == null)
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            //float RandomX = Random.Range (-10.5f, 10.5f)
            transform.position = new Vector3(Random.Range(-10.5f, 10.5f), 8.5f, 0);
        }

        if (Time.time > _canFire && _isEnemyDead == false)
        {
            _fireRate = Random.Range(4f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_EnemyLasers, transform.position + new Vector3(-1.983f, -0.45f, 0), Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
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
            EnemyDeath();
            
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        _isEnemyDead = true;
        _anim.SetTrigger("OnEnemy Death");
        _speed = 0;
        _ExplodeAudio.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.3f);
    }
}
