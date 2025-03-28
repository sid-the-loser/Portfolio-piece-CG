using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RotatingGrass : MonoBehaviour
{
    [SerializeField] private float threshold = 1f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float recoverySpeed = 0.1f;
    
    private static Transform _playerTransform;
    private Quaternion _initialRotation;
    
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _initialRotation = transform.rotation;
    }

    void Update()
    {
        var sqDistance = Mathf.Pow(_playerTransform.position.x - transform.position.x, 2f) + 
                                Mathf.Pow(_playerTransform.position.z - transform.position.z, 2f);

        if (sqDistance >= threshold)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation, 
                                                Time.deltaTime * recoverySpeed);
            // transform.rotation = _initialRotation;
        }
        else
        {
            var angle = ((threshold - sqDistance) / threshold) * 90f;

            var displacement = new Vector2(_playerTransform.position.z - transform.position.z,
                            _playerTransform.position.x - transform.position.x);
            
            var direction = displacement * new Vector2(Q_rsqrt(Mathf.Pow(displacement.x, 2)), 
                                                                Q_rsqrt(Mathf.Pow(displacement.y, 2))) * -1;

            var finalAngle = Quaternion.Euler(direction.x * angle, 0, direction.y * angle * -1);

            transform.rotation = Quaternion.Slerp(transform.rotation, finalAngle, Time.deltaTime * rotationSpeed);
        }
    }
    
    private float Q_rsqrt(float number) // implementation of John Carmak's fast inverse sq root
        // referenced from https://en.m.wikipedia.org/wiki/Fast_inverse_square_root
    {
        int i;
        float x2, y;
        const float threehalfs = 1.5f;

        x2 = number * 0.5f;
        y = number;
        
        i = BitConverter.ToInt32(BitConverter.GetBytes(y), 0);
        i = 0x5f3759df - (i >> 1);
        y = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
        
        y = y * (threehalfs - (x2 * y * y));
        // y = y * (threehalfs - (x2 * y * y)); // Optional second iteration
        
        return y;
    }
}
