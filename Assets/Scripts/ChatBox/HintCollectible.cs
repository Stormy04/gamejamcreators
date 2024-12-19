using UnityEngine;

public class HintCollectible : MonoBehaviour
{
    public HintManager.HintName hintName;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            CollectHint();
        }
    }

    private void CollectHint()
    {
       
        HintManager.Instance.AddHint(hintName);
       Destroy(gameObject);
      

        
    }
}
