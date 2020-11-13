using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    protected int m_Health = 5;

    public int Health => m_Health;

    public abstract void TakeDamage();
}
