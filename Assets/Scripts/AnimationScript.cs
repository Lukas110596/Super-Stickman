using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationScript : MonoBehaviour {
    // Array of sprites so we can use more than one / again public so we can change it Unity itself 
    public Sprite[] sprites;
    // 6 frame per second 
    public float framrate = 1f/6f;

    private SpriteRenderer spriteRenderer;
    private int frame;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable(){
        InvokeRepeating(nameof(Animate), framrate, framrate);
    }

    private void OnDisable(){
        CancelInvoke();
    }

    // animation of different sprites and their sequence
    private void Animate() {
        frame++;
        
        if (frame >= sprites.Length){
            frame = 0;
        }
        
        if(frame >= 0 && frame < sprites.Length){
        spriteRenderer.sprite = sprites[frame];
        }

    }
}