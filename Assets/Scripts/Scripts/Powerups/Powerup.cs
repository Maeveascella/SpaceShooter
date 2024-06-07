using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _powerUpSpeed = 3f;
    [SerializeField]
    private int _powerUpID;
    //0 = TripleShot
    //1 = speed
    //2 = shield
    //3 = ReduceBoost
    //4 = HomingLaser
    //5 = BurstFire

    private AudioSource _powerupAudio;
    [SerializeField]
    private bool _playerMagnetActive = false;
    private Transform _playerPos;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _powerupAudio = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_powerupAudio == null)
        {
            Debug.LogError("Powerup Audio Not Found");
        }

        _playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
                switch (_powerUpID)
                {
                    case 0:
                        player.TripleShot();
                        break;
                    case 1:
                        player.SpeedPower();
                        break;
                    case 2:
                        player.ShieldActivated();
                        break;
                    case 3:
                        player.ReduceBoost();
                        break;
                    case 4:
                        player.HomingLaser();
                        break;
                    case 5:
                        player.BurstFire();
                        break;
                }
            _powerupAudio.Play();
            Destroy(this.gameObject);
        }
    }

    public void PlayerMagnet()
    {
        
        _playerMagnetActive = true;
    }

    void Movement()
    {
        if (_playerMagnetActive == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerPos.position, 8f * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        }

        if (transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
    }
}

