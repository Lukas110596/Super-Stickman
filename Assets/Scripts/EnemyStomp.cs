
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyStomp : MonoBehaviour {

    //public Sprite flatSprite; falls wir nen Todes sprite erschaffen wollen

    private void OnCollisionEnter2D (Collision2D collision){

        string enemy = "";
        if (gameObject.CompareTag("Pen")) {
            enemy="Pen";
        } else if (gameObject.CompareTag("Pencil")) {
            enemy="Pencil";
        } else {
            enemy = "Brush";
        }


        if(collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            //check if the player jumped on the head 
            if(collision.transform.DotTest(transform, Vector2.down)) {
                player.stompingSound.Play();
                player.score+=2;
                if (player.rubberActive && enemy=="Pencil") {
                    player.score+=10;
                } else if (player.tipexActive && enemy =="Pen") {
                    player.score+=10;
                } else if (player.invincible && enemy =="Brush") {
                    player.score+=10;
                }
                player.UpdateScoreUI();
                player.StartCoroutine(player.ScoreHighlights());
                Flatten();
        
            } else {
                player.Hit();
                player.score--;
                player.UpdateScoreUI();
            }

        }
    }

    private void Flatten(){
                GetComponent<Collider2D>().enabled = false;
                GetComponent<EntityMovement>().enabled = false;
                GetComponent<AnimationScript>().enabled = false;
                Destroy(gameObject, 0.5f);
    }



}
