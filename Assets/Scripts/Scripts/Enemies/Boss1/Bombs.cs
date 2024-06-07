using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bombs : MonoBehaviour
{
    [SerializeField]
    private Text _bombCountdown;
    [SerializeField]
    private GameObject _bombExplosion;
    [SerializeField]
    private GameObject _explosion;
    private AudioSource _explosionAudio;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BombTimer());
        _explosionAudio = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                StopCoroutine(BombTimer());
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _explosionAudio.Play();
                Destroy(this.gameObject);
            }
        }
    }
    private IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(1);
        //2
        yield return new WaitForSeconds(1);
        //1
        yield return new WaitForSeconds(1);
        Instantiate(_bombExplosion, transform.position, Quaternion.identity);
        _explosionAudio.Play();
        Destroy(this.gameObject);
    }
}
