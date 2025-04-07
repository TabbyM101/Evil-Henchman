using UnityEngine;

public class Trash : MonoBehaviour, IClickableObject
{
    [SerializeField] private GameObject particals;

    public void ClickableObject_Clicked(RaycastHit ray)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        PickupObject.Current.Pickup(gameObject);
    }

    public void Scored() 
    {
        PickupTrash.Current.Score();
        Instantiate(particals, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (gameObject.transform.position.y < .5f)
        {
            Debug.Log("Fallen");
            PickupTrash.Current.ReSpawn(gameObject);
            Destroy(gameObject);
        }
    }
}