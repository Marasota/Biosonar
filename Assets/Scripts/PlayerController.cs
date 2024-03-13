using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    CharacterController _cC;
    [SerializeField] private float _movementSpeed = 8f;
    [SerializeField] private float _originalSpeedMultiplier = 1.5f;
    private float _currentSpeedMultiplier = 1;
    [SerializeField] float _sonarCoolDown = 3f;
    bool _isSonarEnable = true;

    [SerializeField] ParticleSystem _particleSystem;

    private List<AppearableEntity> _triggeredObjects = new List<AppearableEntity>();


    Vector3 _direction = Vector3.zero;
    float moveHorizontal = 0;
    float moveVertical = 0;

    private int _layerMask = 0;

    private void Awake()
    {
        Instance = this; 
        _cC = GetComponent<CharacterController>();
        _layerMask = 1 << LayerMask.NameToLayer("Appearing");
        _layerMask = ~_layerMask;
    }

    private void Update()
    {
        Sonar();
        Move();
    }

    #region Movement
    void CalculateDirection()
    {
        moveHorizontal = 0;
        moveVertical = 0;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            moveVertical = 1;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            moveVertical = -1;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            moveHorizontal = -1;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            moveHorizontal = 1;

        _direction = new Vector3(moveHorizontal, moveVertical, 0f);
    }

    private void Move()
    {
        CalculateDirection();
        Sprint();
        if (_direction != Vector3.zero)
        {
            _cC.Move(_direction.normalized * _movementSpeed * _currentSpeedMultiplier  * Time.deltaTime);
        }
    }


    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeedMultiplier = _originalSpeedMultiplier;
        }
        else {
            _currentSpeedMultiplier = 1;
        }
    }
    #endregion

    #region Sonar
    void Sonar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isSonarEnable == true)
        {
            _particleSystem.Play();
            AppearSomething();
            _isSonarEnable = false;
            Invoke("ReloadSonar", _sonarCoolDown);
        }
    }

    void ReloadSonar()
    {
        _isSonarEnable = true;
    }

    void AppearSomething()
    {
        for (int i = _triggeredObjects.Count - 1; i >= 0; i--)
        {
            int originalLayer = _triggeredObjects[i].gameObject.layer;
            _triggeredObjects[i].gameObject.layer = LayerMask.NameToLayer("Default");

            RaycastHit hit;
            Vector3 direction = _triggeredObjects[i].transform.position - transform.position;

            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, _layerMask))
            {
                _triggeredObjects[i].gameObject.layer = originalLayer;
                if (hit.transform == _triggeredObjects[i].transform)
                {
                    _triggeredObjects[i].Appear();
                    _triggeredObjects.RemoveAt(i);
                }
            }
        }
    }

    public void AddAppearableEntity(AppearableEntity tmp) {
        _triggeredObjects.Add(tmp);
    }

    public void RemoveAppearableEntity(AppearableEntity tmp)
    {
        _triggeredObjects.Remove(tmp);
    }

    #endregion 
}
