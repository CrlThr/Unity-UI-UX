using UnityEngine;

//manage the physical interraction between the player and the object to be pushed aside
public class PushObject : MonoBehaviour
{
    public float pushForce = 20f;
    private Rigidbody rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
            Debug.Log("None RigidBody found on" + gameObject.name);
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        // check if the other object have a rigidBody
        Rigidbody OtherigidBody = collision.rigidbody;
        if (rigidBody == null || OtherigidBody == null) return; 

        // calculate the direction of the collision
        Vector3 pushDirection = (rigidBody.position - OtherigidBody.position).normalized;

        rigidBody.AddForce(pushDirection * pushForce, ForceMode.Impulse); 

    }

}
