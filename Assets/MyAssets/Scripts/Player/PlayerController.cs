using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Scene_Manager _scene;
    private UIController _uIController;
    private AudioManager _audioManager;
    private float _health = 100;
    private Rigidbody _playerRigidbody;
    private bool _killed = false;
    private float _recharge;
    private static float _score = 0;
    public float score { get { return _score; } set { _score = value; } }
    private float _upgrades = 0;
    private Animator _playerAnim;
    private float _updating = 2f;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private GameObject _garlic;
    [SerializeField]
    private float _garlic2Speed = 1500;
    private float _playerSpeed = 15f;
    private float _mouseX;
    private float _mouseY=90;
    private float _moveFB;
    private float _moveLR;
    private float _sensitivity=5f;
    // Start is called before the first frame update
    void Start()
    {
        _scene = GameObject.FindObjectOfType<Scene_Manager>();
        _uIController = GameObject.FindObjectOfType<UIController>();
        
        _playerAnim = GetComponentInChildren<Animator>();
        _audioManager = GameObject.FindObjectOfType<AudioManager>();
        Cursor.lockState = CursorLockMode.Locked;
        _moveLR = 0;
        _moveFB = 0;
        _playerRigidbody = transform.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (_uIController == null)
        {
            return;
        }
        if (!_killed)
        {

            _mouseX = Input.GetAxis("Mouse X");
            _mouseY -= Input.GetAxis("Mouse Y")*_sensitivity;
            _mouseY = Mathf.Clamp(_mouseY, -45, 45);
            _camera.transform.localRotation = Quaternion.Euler(_mouseY, 0, 0);
            transform.Rotate(0, _mouseX * _sensitivity, 0);
            _moveFB = Input.GetAxis("Vertical");
            _moveLR = Input.GetAxis("Horizontal");
            MovePlayer();

            if (Input.GetMouseButton(0))
            {
                if (_recharge < 0)
                {
                    Fire();
                    _recharge = 0.4f;
                }
                
            }
            _recharge -= Time.deltaTime;
            _updating -= Time.deltaTime;
        }
    }

    private void UpdateLifeBar()
    {
        _uIController.UpdateLifeBar(_health);
    } 
    private void MovePlayer()
    {
        Vector3 movement = new Vector3(_moveLR, 0, _moveFB).normalized * _playerSpeed;
        movement = transform.rotation * movement;
        _playerRigidbody.velocity = movement;
        _moveFB = 0;
        _moveLR = 0;
    }

    public void UpdateScore(float value)
    {
        _score += value;
        _uIController.UpdateScore(_score);
    }

   
    private void Fire()
    {
        _playerAnim.SetTrigger("attack");
        _audioManager.PlayAudio("Garlic");
        GameObject newGarlic = Instantiate(_garlic, _target.transform.position, _target.transform.rotation) as GameObject;
        
        Rigidbody tmpGarlicRB = newGarlic.GetComponent<Rigidbody>();
        tmpGarlicRB.AddForce(tmpGarlicRB.transform.forward * _garlic2Speed);
        Destroy(newGarlic, 5f);
    }

    void HealthUpdate(float value)
    {
        _health += value;
        if (_health > 100)
        {
            _health = 100;
        }
        UpdateLifeBar();
        if (_health <= 0)
        {
            _killed = true;
            _scene.LoadGameOver();
        }
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBat")
        {
            _audioManager.PlayAudio("Impact");
            _audioManager.PlayAudio("Hurt");
            HealthUpdate(-2);
        }else if(other.tag == "Upgrade" && _updating<=0)
        {
            Destroy(other.gameObject,0f);
            _audioManager.PlayAudio("Upgrade");
            _uIController.ShowMessage("Vida incrementada");
             HealthUpdate(10);
            _updating = 2f;
        }
    }
}
