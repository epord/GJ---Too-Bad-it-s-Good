using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] public GameObject focus;
    [SerializeField] public float movementSpeed = 8f;

    private float yFocusOffset = 0f;

    private Transform focusGoal;

    // Start is called before the first frame update
    void Start()
    {
        focusGoal = transform.Find("FocusGoal");
        if (focusGoal != null)
        {
            yFocusOffset = focusGoal.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(transform.position.x, focus.transform.position.y - yFocusOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSpeed);
    }
}
