using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    // Public Variables
    public float pickupRange = 5f;
    public float baseThrowForce = 4f; 
    public float maxThrowMultiplier = 2f; 
    public Camera playerCamera; // Reference to the player's camera
    public Transform holdPosition;
    public  GameObject heldObject;
    public ChomperPlayFetch chomper; // Reference to the Chomper_PlayFetch script

    // Private Variables
    private Rigidbody heldObjectRb;
    private CharacterController playerController;



    void Start()
    {
        // Find the CharacterController in the Player hierarchy
        playerController = GetComponent<CharacterController>();
        chomper = GetComponent<ChomperPlayFetch>();
    }

    void Update()
    {
        // Toggle pickup and throw with the "E" key
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                ThrowObject();
            }
        }
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name); // Debugging raycast hit

            if (hit.collider.CompareTag("PickUp"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.isKinematic = true; // Disable physics while holding
                    heldObject.transform.SetParent(holdPosition);
                    heldObject.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

 
 void ThrowObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            heldObjectRb.isKinematic = false;

            // Calculate throw strength
            float playerSpeed = playerController.velocity.magnitude;
            float throwStrength = baseThrowForce + (playerSpeed * maxThrowMultiplier);

            // Apply force
            heldObjectRb.AddForce(playerCamera.transform.forward * throwStrength, ForceMode.Impulse);

            // Notify Chomper if this is the ball
            ChomperPlayFetch chomper = FindFirstObjectByType<ChomperPlayFetch>();
            if (chomper != null && heldObject == GameObject.Find("Ball")) // Ensure correct object
            {
                chomper.StartFetch(heldObject);
                Debug.Log("Chomper notified of thrown ball: " + heldObject.name);
            }

            heldObject = null;
        }
    }

}
