using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform menuPos;
    public Transform customPos;

    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    public void GoToMenu()
    {
        targetPosition = menuPos.position;
        isMoving = true;
    }

    public void GoToCustom()
    {
        targetPosition = customPos.position;
        isMoving = true;
    }
}