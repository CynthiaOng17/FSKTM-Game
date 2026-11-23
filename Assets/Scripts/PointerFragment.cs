using UnityEngine;

public class PointerFragment : MonoBehaviour
{
    public float cgpaValue = 0.2f;
    public bool isDeanListPiece = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            CGPAManager.Instance?.GainPoints(cgpaValue);
            WinManager.Instance?.FragmentCollected();
            if (isDeanListPiece)
                Debug.Log("Dean List fragment collected!");
            Destroy(gameObject);
        }
    }
}