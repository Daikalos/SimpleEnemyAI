using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HP_Text : MonoBehaviour
{
    [SerializeField]
    private Enemy m_Enemy = null;

    private Text m_Text;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    void Update()
    {
        if (m_Enemy != null)
            m_Text.text = m_Enemy.Health.ToString();
        else
            Destroy(this);
    }
}
