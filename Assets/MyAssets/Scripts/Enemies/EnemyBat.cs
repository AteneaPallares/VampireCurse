using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBat: MonoBehaviour
{
    private AudioManager _audioManager;

    private float _scoreByKill = 1f;
    private GameObject _player;
    private GameObject _bat;
    private PlayerController _playerController;
    private NavMeshAgent _batNav;
    private float _lifetime = 10f;
    private float _wingtime = .1f;
    private bool _goup = true;
    private bool _killed = false;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _audioManager = GameObject.FindObjectOfType<AudioManager>();
            _playerController = _player.GetComponent<PlayerController>();
            _batNav = transform.GetComponent<NavMeshAgent>();
            _bat = transform.GetChild(0).gameObject;

        }
        else
        {
            _killed = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (_player != null && !_killed)
        {
            if (_lifetime <= 0)
            {
                _batNav.isStopped = true;
                Destroy(gameObject, 0f);
            }
            else
            {

                if (_wingtime <= 0)
                {
                
                    _wingtime = .1f;
                    var auxposition = _bat.transform.position;
                    if (_goup)
                    {
                        auxposition.y += .25f;
                        _goup = false;
                    }
                    else
                    {
                        auxposition.y -= .25f;
                        _goup = true;
                    }
                    _bat.transform.position = auxposition;
                }
                _batNav.SetDestination(_player.transform.position);
            }
           
        }
        _lifetime -= Time.deltaTime;
        _wingtime -= Time.deltaTime;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Garlic")
        {
            _killed = true;
            Destroy(gameObject, 0f);
            Destroy(other.gameObject);
            _playerController.UpdateScore(_scoreByKill);
        }
        
    }
}
