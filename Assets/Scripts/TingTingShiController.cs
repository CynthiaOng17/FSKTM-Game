using UnityEngine;
using System.Collections;

public class TingTingShiController : MonoBehaviour
{
    public float throwInterval = 3f;
    public GameObject examPaperPrefab;
    public int health = 2;
    public float activationRange = 20f;
    private Transform player;
    private bool isActivated = false;
    private float throwTimer = 0f;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (!isActivated && player != null)
        {
            float distance = Vector2.Distance(
                transform.position, player.position);
            if (distance <= activationRange)
            {
                isActivated = true;
                Debug.Log("TingTingShi activated!");
            }
        }

        if (isActivated)
        {
            throwTimer += Time.deltaTime;
            if (throwTimer >= throwInterval)
            {
                throwTimer = 0f;
                ThrowDualPapers();
            }
        }
    }

    void ThrowDualPapers()
    {
        if (examPaperPrefab == null || player == null)
        {
            Debug.Log("Missing prefab or player!");
            return;
        }

        Vector2 dir = (player.position
            - transform.position).normalized;

        GameObject paperL = Instantiate(examPaperPrefab,
            transform.position + new Vector3(-0.5f, 0, 0),
            Quaternion.identity) as GameObject;

        GameObject paperR = Instantiate(examPaperPrefab,
            transform.position + new Vector3(0.5f, 0, 0),
            Quaternion.identity) as GameObject;

        ExamPaper epL = paperL.GetComponent<ExamPaper>();
        ExamPaper epR = paperR.GetComponent<ExamPaper>();

        if (epL != null) epL.Launch(dir);
        if (epR != null) epR.Launch(dir);

        Debug.Log("Dual papers thrown!");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("TingTingShi HP: " + health);
        if (health <= 0)
        {
            WinManager.Instance?.BossDefeated();
            Destroy(gameObject);
        }
    }
}