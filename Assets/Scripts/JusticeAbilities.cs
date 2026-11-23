using UnityEngine;
using UnityEngine.SceneManagement;

public class JusticeAbilities : MonoBehaviour
{
    private ExamPaper caughtPaper = null;
    public float catchRange = 5f;

    void Update()
    {
        // Press F to catch paper
        if (Input.GetKeyDown(KeyCode.F))
            TryCatchPaper();

        // Press G to throw back
        if (Input.GetKeyDown(KeyCode.G)
            && caughtPaper != null)
            ThrowPaperBack();
    }

    void TryCatchPaper()
    {
        GameObject[] papers = GameObject
            .FindGameObjectsWithTag("ExamPaper");

        if (papers.Length == 0)
        {
            Debug.Log("No papers nearby!");
            return;
        }

        float nearest = catchRange;
        ExamPaper nearestPaper = null;

        foreach (GameObject p in papers)
        {
            float dist = Vector2.Distance(
                transform.position,
                p.transform.position);
            if (dist < nearest)
            {
                nearest = dist;
                nearestPaper = p
                    .GetComponent<ExamPaper>();
            }
        }

        if (nearestPaper != null)
        {
            caughtPaper = nearestPaper;
            caughtPaper.gameObject.SetActive(false);
            Debug.Log("Paper caught! Press G to throw back!");
        }
        else
        {
            Debug.Log("No paper in range!");
        }
    }

    void ThrowPaperBack()
    {
        if (caughtPaper == null) return;

        #region FIX
        if(SceneManager.GetActiveScene().name == "FSKTM Special Level_Zone2")
        {
            caughtPaper.isThrownBack = true;

                    caughtPaper.gameObject.SetActive(true);
        caughtPaper.transform.position = transform.position;

        GameObject tts = GameObject.FindWithTag("Enemy");
        if (tts != null)
        {
            Vector2 dir = (tts.transform.position
                - transform.position).normalized;
            caughtPaper.ThrowBack(dir);
            Debug.Log("Paper thrown back at TingTingShi!");
        }
        else
        {
            Debug.Log("TingTingShi not found!");
            Destroy(caughtPaper.gameObject);
        }
        return;
        }
        #endregion

        caughtPaper.gameObject.SetActive(true);
        caughtPaper.transform.position = transform.position;

        GameObject boss = GameObject.Find("TingTingShi");
        if (boss != null)
        {
            Vector2 dir = (boss.transform.position
                - transform.position).normalized;
            caughtPaper.ThrowBack(dir);
            Debug.Log("Paper thrown back at TingTingShi!");
        }
        else
        {
            Debug.Log("TingTingShi not found!");
        }
        caughtPaper = null;
    }
}