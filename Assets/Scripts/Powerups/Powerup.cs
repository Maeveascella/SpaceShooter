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

    private AudioSource _powerupAudio;

    // Start is called before the first frame update
    void Start()
    {
        _powerupAudio = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();

        if (_powerupAudio == null)
        {
            Debug.LogError("Powerup Audio Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            Destroy(this.gameObject);
        }
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
                        player.BurstFire();
                        break;
                }
            _powerupAudio.Play();
            Destroy(this.gameObject);
        }
    }
}

