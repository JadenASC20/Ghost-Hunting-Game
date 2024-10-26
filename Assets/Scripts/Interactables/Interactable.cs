//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;

//public class Interactable : MonoBehaviour
//{
//    private XRGrabInteractable grabInteractable;
//    private Rigidbody rb;
//    public Script interactablesScript;

//    // References to the provided scripts

//    private void Awake()
//    {
//        grabInteractable = GetComponent<XRGrabInteractable>();
//        rb = GetComponent<Rigidbody>();

//        // Subscribe to events
//        grabInteractable.selectExited.AddListener(OnThrow);
//    }

//    private void OnDestroy()
//    {
//        // Clean up event subscriptions
//        grabInteractable.selectExited.RemoveListener(OnThrow);
//    }

//    private void OnThrow(SelectExitEventArgs arg)
//    {
//        // Optionally, you can handle logic right after the object is thrown
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if the object collided with the floor (you may need to adjust this check based on your floor's tag)
//        if (collision.gameObject.CompareTag("Floor"))
//        {
//            // Call methods from the provided scripts
//            Debug.Log("Object Thrown!");

//            // Optionally, you can destroy the object after throwing
//            // Destroy(gameObject);
//        }
//    }
//}
