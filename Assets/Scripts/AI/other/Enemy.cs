using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField, Range(1, 20)]
    private int m_Health = 15;
    [SerializeField, Range(0.0f, 50.0f)]
    private float m_ViewRange = 10.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_ViewAngle = 0.6f;
    [SerializeField, Range(0.0f, 25.0f)]
    private float m_AttackRange = 5.0f;
    [SerializeField, Range(0.0f, 5.0f)]
    private float m_AttackRate = 2.5f;
    [SerializeField, Range(0.0f, 8.0f), Tooltip("How fast this rotates towards target when attacking")]
    private float m_RotationSpeed = 4.5f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("the limit on health before the enemy attempts to flee")]
    private float m_FleeBoundary = 0.4f;

    [SerializeField]
    private GameObject m_Muzzle = null;
    [SerializeField]
    private GameObject m_Bullet = null;

    #endregion

    #region Properties

    public List<GameObject> Targets { get; } = new List<GameObject>();
    public NavMeshAgent Agent { get; private set; } = null;
    public GameObject Target { get; private set; } = null;
    public GameObject Muzzle => m_Muzzle;
    public GameObject Bullet => m_Bullet;

    public float ViewRange => m_ViewRange;
    public float ViewAngle => m_ViewAngle;
    public float AttackRange => m_AttackRange;
    public float AttackRate => m_AttackRate;
    public float RotationSpeed => m_RotationSpeed;
    public float FleeBoundary => m_FleeBoundary;

    public int Health => m_Health;
    public int StartHealth { get; set; }

    #endregion

    protected virtual void Awake()
    {
        StartHealth = m_Health;

        Agent = GetComponent<NavMeshAgent>();
        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(t =>
        {
            if (!t.Equals(this))
                Targets.Add(t);
        });
    }

    public virtual void TakeDamage()
    {
        m_Health -= 2;
        if (m_Health <= 0)
        {
            Destroy(gameObject);
            for (int i = Targets.Count - 1; i >= 0; --i)
            {
                Enemy enemy = Targets[i].GetComponent<Enemy>();

                if (enemy == null)
                    continue;

                enemy.Targets.Remove(gameObject);
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        if (target == gameObject) // Cannot set self as target
            return;

        Target = target;
    }
}
