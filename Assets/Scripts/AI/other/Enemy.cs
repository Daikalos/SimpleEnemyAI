﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    protected int m_Health = 15;

    public int Health => m_Health;
    public int StartHealth { get; set; }

    protected virtual void Awake()
    {
        StartHealth = m_Health;
    }

    public virtual void TakeDamage()
    {
        m_Health -= 2;
        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
