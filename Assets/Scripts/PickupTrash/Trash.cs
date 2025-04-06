using UnityEngine;

public class Trash : MonoBehaviour, IClickableObject
{
    public void ClickableObject_Clicked(RaycastHit ray)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        PickupObject.Current.Pickup(gameObject);
    }

    private void Update()
    {
        if (gameObject.transform.position.y < .5f)
        {
            Debug.Log("Fallen");
            Destroy(gameObject);
        }
    }
}