using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        // physics engine is not controlling this object
        if(rigidbody.isKinematic) {
            return false;
        }

        float radius = 0.25f;
        float distance = 0.5f;
        /* set distance based on jumping or grounded (and movement)
         1.1f for jumping, 0.5f for grounded movement -> come as close as possible to a block that you collided with */
        if (Input.GetButton("Horizontal") && Input.GetButtonDown("Jump")) {
            distance = 1.1f;
        } else if (Input.GetButtonDown("Horizontal") && !Input.GetButtonDown("Jump")){
            distance = 0.5f;
        } else if (!Input.GetButtonDown("Horizontal") && Input.GetButtonDown("Jump")) {
            distance = 1.1f;
        } 

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        // return true when we hit something and its not our own body
        return hit.collider != null && hit.rigidbody != rigidbody;
    } 

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        // if 1 -> exactly same
        // if -1 -> exact opposite of each other
        // if 0 -> exactly perpendicular (rechtwinkelig)
        return Vector2.Dot(direction.normalized,testDirection) > 0.25f;
    }

}
