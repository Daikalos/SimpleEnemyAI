using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    #region Variables

    [SerializeField, Range(1, 30)]
    private int m_Health = 20;
    [SerializeField, Range(0.0f, 50.0f)]
    private float m_ViewRange = 10.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_ViewAngle = 0.6f;
    [SerializeField, Range(0.0f, 25.0f)]
    private float m_ApproachRange = 4.5f;
    [SerializeField, Range(0.0f, 25.0f)]
    private float m_AttackRange = 5.0f;
    [SerializeField, Range(0.0f, 5.0f)]
    private float m_AttackRate = 2.5f;
    [SerializeField, Range(0.0f, 8.0f), Tooltip("How fast this rotates towards target when attacking")]
    private float m_RotationSpeed = 4.5f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("the minimum health before the enemy attempts to flee")]
    private float m_FleeBoundary = 0.4f;

    [SerializeField]
    private GameObject m_Muzzle = null;
    [SerializeField]
    private GameObject m_Bullet = null;

    #endregion

    #region Properties

    public List<GameObject> Targets { get; } = new List<GameObject>();
    public NavMeshAgent Agent { get; private set; } = null;
    public Rigidbody RB { get; private set; } = null;
    public GameObject Target { get; private set; } = null;

    public int Health => m_Health;
    public float ViewRange => m_ViewRange;
    public float ViewAngle => m_ViewAngle;
    public float ApproachRange => m_ApproachRange;
    public float AttackRange => m_AttackRange;
    public float AttackRate => m_AttackRate;
    public float RotationSpeed => m_RotationSpeed;
    public float FleeBoundary => m_FleeBoundary;

    public bool ShouldFlee { get; private set; }
    public bool IsTargetFound { get; private set; }
    public bool IsTargetVisible { get; private set; }
    public bool IsWithinViewRange { get; private set; }
    public bool IsWithinViewAngle { get; private set; }
    public bool IsBehindWall { get; private set; }
    public bool IsWithinApproachRange { get; private set; }
    public bool IsWithinAttackRange { get; private set; }

    public int StartHealth { get; set; }
    public float StartSpeed { get; set; }

    #endregion

    protected virtual void Awake()
    {
        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(t =>
        {
            if (!t.Equals(this))
                Targets.Add(t);
        });
        Agent = GetComponent<NavMeshAgent>();
        RB = GetComponent<Rigidbody>();

        StartHealth = m_Health;
        StartSpeed = Agent.speed;
    }

    public void Perception()
    {
        IsTargetFound = (Target != null);

        if (!IsTargetFound)
            return;

        Vector3 dir = (Target.transform.position - transform.position);

        float distTo = (Target.transform.position - transform.position).magnitude;
        float withinAngle = Vector3.Dot(dir, transform.forward);

        ShouldFlee = (Health < (StartHealth * FleeBoundary));
        IsWithinViewRange = (distTo < ViewRange);
        IsWithinViewAngle = (withinAngle > ViewAngle);
        IsBehindWall = (Physics.Raycast(transform.position, dir, ViewRange, LayerMask.GetMask("Environment")));
        IsWithinApproachRange = (distTo < ApproachRange);
        IsWithinAttackRange = (distTo < AttackRange);
        IsTargetVisible = (IsWithinViewRange && IsWithinViewAngle && !IsBehindWall);
    }

    public void TakeDamage(GameObject bulletOwner)
    {
        SetTarget(bulletOwner); // Change target to the owner of the bullet

        m_Health -= 2;
        if (m_Health <= 0)
        {
            Destroy(gameObject);
            for (int i = Targets.Count - 1; i >= 0; --i) // Remove this enemy from all other enemy target lists
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

    public GameObject ClosestTarget()
    {
        GameObject result = null;
        float minDistance = float.MaxValue;

        foreach (GameObject target in VisibleTargets())
        {
            float distance = (target.transform.position - transform.position).magnitude;
            if (distance < minDistance)
            {
                result = target;
                minDistance = distance;
            }
        }
        return result;
    }
    public List<GameObject> VisibleTargets()
    {
        List<GameObject> visibleTargets = new List<GameObject>();
        foreach (GameObject target in Targets)
        {
            if (!TargetVisible(target))
                continue;

            visibleTargets.Add(target);
        }

        return visibleTargets;
    }

    public static Vector3 RandomPoint(Vector3 origin, float distance, int layermask)
    {
        // Returns a random point on navigation mesh

        Vector3 randDirection = (Random.insideUnitSphere * distance) + origin;
        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

        return navHit.position;
    }

    public void CreateBullet(GameObject target)
    {
        GameObject bullet = Instantiate(m_Bullet, m_Muzzle.transform.position,
            Quaternion.LookRotation(target.transform.position - m_Muzzle.transform.position));
        bullet.GetComponent<Bullet>().Owner = gameObject;
    }

    public bool TargetVisible(GameObject target)
    {
        return WithinViewRange(target) && WithinViewAngle(target) && !BehindWall(target);
    }
    public bool WithinViewRange(GameObject target)
    {
        float distanceTo = (target.transform.position - transform.position).magnitude;
        return (distanceTo < ViewRange);
    }
    public bool WithinViewAngle(GameObject target)
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float withinAngle = Vector3.Dot(dir, transform.forward); // 1 = Looking at, -1 = Opposite direction

        return (withinAngle > ViewAngle);
    }
    public bool BehindWall(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position);
        return Physics.Raycast(transform.position, direction, ViewRange, LayerMask.GetMask("Environment"));
    }
    public bool WithinApproachRange(GameObject target)
    {
        float distanceTo = (target.transform.position - transform.position).magnitude;
        return (distanceTo < ApproachRange);
    }
    public bool WithinAttackRange(GameObject target)
    {
        float distanceTo = (target.transform.position - transform.position).magnitude;
        return (distanceTo < AttackRange);
    }
}
