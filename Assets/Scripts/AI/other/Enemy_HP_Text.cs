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
        if (m_Enemy == null)
        {  
            m_Text.text = "0";
            Destroy(this);
        }
        else
            m_Text.text = m_Enemy.Health.ToString();
    }
}
