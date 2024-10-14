using UnityEngine;

public class GuestX : MonoBehaviour
{
    [SerializeField] private GameObject X;

    public void OnClickX()
    {
        X.SetActive(true);
        gameObject.SetActive(false);
    }
}
