using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Destroy", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
