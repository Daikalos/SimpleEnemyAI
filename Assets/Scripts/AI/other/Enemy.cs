using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Range(1, 10)]
    private int m_Health = 5;

    public int Health => m_Health;

    public void TakeDamage()
    {
        --m_Health;
        if (m_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
