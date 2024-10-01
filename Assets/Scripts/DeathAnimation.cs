using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
        public SpriteRenderer spriteRenderer;
        public Sprite deadSprite; // falls wir ein dead Sprite verwenden möchten 

        private void Reset(){
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable(){
            UpdateSprite();
            DisablePhysics();
            StartCoroutine(Animate());

        }

        private void UpdateSprite(){
            spriteRenderer.enabled = true;
            spriteRenderer.sortingOrder = 10;


            /* Sobald Sprites das sind aktivieren 
            if (deadSprite != null) {
                spriteRenderer.sprite = deadSprite;
            }
            */
        }

        private void DisablePhysics() {
            Collider2D[] colliders = GetComponents<Collider2D>();

            foreach(Collider2D collider in colliders) {
                collider.enabled = false;
            }

            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            EntityMovement entityMovement = GetComponent<EntityMovement>();

            if(playerMovement != null) {
                playerMovement.enabled = false;
            }

            if(entityMovement != null) {
                entityMovement.enabled = false; 
            }


        }

    private IEnumerator Animate() {
        float duration = 3f; // Or another condition for the end of the animation
        float rotationSpeed = 180f; // Rotation speed in degrees per second
        float shrinkSpeed = 0.5f;   // Shrink speed in units per second
        float scaleThreshold = 0.01f; // Threshold for the object's scale

            while (duration > 0f) {

                // Rotation only in one direction (counter-clockwise)
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                // Shrink
                transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, 0f) * Time.deltaTime;
                // Check if the object's scale is small enough to destroy it
                    if (transform.localScale.x < scaleThreshold && transform.localScale.y < scaleThreshold){
                        break;
                        }


                duration -= Time.deltaTime; // Reduce the remaining duration based on elapsed time
                yield return null;
            }

        // After the animation, you can destroy the GameObject
        Destroy(gameObject);
    }




}
