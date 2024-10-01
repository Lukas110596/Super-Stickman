using System;
using System.Collections;
using UnityEngine;

public class Endpoint : MonoBehaviour
{

    public Transform bottom;
    public Transform door;
    public float speed = 6f;
    private int playerscore;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.endSound.Play();
            if (player.invincible) {
                player.score+=20;
            }
            if (player.currentHealth == 3) {
                player.score+=20;
            }
            player.score +=5;
            player.StartCoroutine(player.ScoreHighlights());
            player.UpdateScoreUI();
           
            //player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(0, -3, 0), Time.deltaTime*speed);
            StartCoroutine(LevelCompleteSequence(other.transform));
            playerscore = player.score;
        }
    }

    private  IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        MoveTo(player.transform, Vector3.down);

        yield return MoveTo(player, door.position);
        player.gameObject.SetActive(false);
        GameManager.instance.EndMenu(playerscore);
    }

    private IEnumerator MoveTo(Transform subject, Vector3 destination)
    {
        while (Vector3.Distance(subject.position, destination) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return null;
        }
        subject.position = destination;
    }

}
