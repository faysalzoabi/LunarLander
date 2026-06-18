using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    public void DestroySelf()
    {
        Debug.Log("destroyedddddd");
        Destroy(gameObject);
    }
}
