using System.Collections;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour
{
    [SerializeField]
    private Boundary boundary;
    [SerializeField]
    private float tilt;
    [SerializeField]
    private float dodge;
    [SerializeField]
    private float smoothing;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector2 startWait;
    [SerializeField]
    private Vector2 maneuverTime;
    [SerializeField]
    private Vector2 maneuverWait;

    private float _targetManeuver;
    private Rigidbody _rigidbody;
    private float _currentSpeed;

    private GameObject _player;
    private bool _isTracking;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentSpeed = _rigidbody.velocity.z;

        _player = GameObject.FindWithTag("Player");

        if (null != _player)
        {
            _isTracking = (Random.value < 0.5);
        }
        else
        {
            _isTracking = false;
        }

        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            _targetManeuver = Random.Range(1.0f, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            _targetManeuver = 0.0f;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(_rigidbody.velocity.x, _targetManeuver, Time.deltaTime * smoothing);

        float movement;

        if (null != _player && _isTracking)
        {
            movement = Mathf.MoveTowards(_rigidbody.position.x, _player.transform.position.x, Time.deltaTime * speed);
        }
        else
        {
            movement = _rigidbody.position.x;
            _rigidbody.velocity = new Vector3(newManeuver, 0.0f, _currentSpeed);
        }

        _rigidbody.position = new Vector3
        (
            Mathf.Clamp(movement, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(_rigidbody.position.z, boundary.zMin, boundary.zMax)
        );
        _rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, _rigidbody.velocity.x * -tilt);
    }
}
