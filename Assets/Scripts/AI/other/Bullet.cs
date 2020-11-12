﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Force applied on bullet when fired"), Range(0.0f, 2000.0f)]
    private float m_Speed = 800.0f;

    private Rigidbody m_Rigidbody;
    private TrailRenderer m_Trail;

    /// <summary>
    /// If bullet is going to collide with an object
    /// </summary>
    public bool ObjectCollision { get; private set; } = false;

    private void Awake()
    {
        Destroy(this, 10.0f);
    }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Trail = GetComponentInChildren<TrailRenderer>();

        m_Rigidbody.AddForce(transform.forward * m_Speed);
    }

    private void FixedUpdate()
    {
        //Used to prevent bullet colliding with target when behind a object
        ObjectCollision = (Physics.Raycast(transform.position, transform.forward, out RaycastHit objectHit, m_Rigidbody.velocity.magnitude * Time.deltaTime));
        
        if (ObjectCollision && objectHit.collider.CompareTag("Enemy"))
            ObjectCollision = false;

        if (ObjectCollision)
        {
            //Set position to collision point to provide better visual feedback on where bullet collided with object
            transform.position = objectHit.point;

            DestroyBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ObjectCollision)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().TakeDamage();
                DestroyBullet();
            }
        }
    }

    private void DestroyBullet()
    {
        //Seperate trail from parent to prevent it from being destroyed when bullet is destroyed
        m_Trail.transform.parent = null;
        m_Trail.autodestruct = true;
        m_Trail.time = 0.15f;

        Destroy(gameObject);
    }
}
