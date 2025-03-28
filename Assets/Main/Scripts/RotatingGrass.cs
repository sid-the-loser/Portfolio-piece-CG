using Unity.VisualScripting;
using UnityEngine;

public class RotatingGrass : MonoBehaviour
{
    private float _threshold = 2f;
    
    private static Transform _playerTransform;
    
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        var sqDistance = Mathf.Pow(_playerTransform.position.x - transform.position.x, 2f) + 
                                Mathf.Pow(_playerTransform.position.z - transform.position.z, 2f);

        if (sqDistance >= _threshold)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            var angle = ((_threshold - sqDistance) / _threshold) * 90f;

            var direction = new Vector2(_playerTransform.position.x - transform.position.x,
                _playerTransform.position.z - transform.position.z);

            if (direction.x != 0) direction.x /= Mathf.Abs(direction.x);
            if (direction.y != 0) direction.y /= Mathf.Abs(direction.y);

            var finalAngle = Quaternion.Euler(direction.x * angle, 0, direction.y * angle);

            transform.rotation = Quaternion.Slerp(transform.rotation, finalAngle, Time.deltaTime * 10f);
        }
    }
}
