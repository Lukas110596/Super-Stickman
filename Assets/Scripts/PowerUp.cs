using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Rubber,
        Tipex,
        OpaqueWhite
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other){if (other.CompareTag("Player")) {Collect(other.gameObject);}}

    private async void Collect(GameObject _player)
    {
        Player player = _player.GetComponent<Player>();
        PlayerMovement movement = _player.GetComponent<PlayerMovement>();
        player.pickUpPowerUpSound.Play();

        switch (type)
        {
            case Type.Rubber:
                Destroy(gameObject);
                if (player.currentHealth<3) {
                    player.currentHealth++;
                    player.UpdateHeartsUI();
                }
                // set hasRubber true -> player can reactivate rubber
                // activate rubber effects on collect for 5 seconds
                player.rubberActive = true;
                player.JumpBoost();
                movement.maxJumpHeight = 7f;
                player.hasRubber = true;
                player.ObtainedPowerUps();
                await Task.Delay(5000);
                movement.maxJumpHeight = 5f;
                player.rubberActive = false;
                player.JumpBoost();
                break;
            case Type.Tipex:
                Destroy(gameObject);
                // set hasTipex true -> player can reactivate rubber
                // activate tipex effects on collect for 5 seconds
                player.tipexActive = true;
                player.SpeedBoost();
                movement.movementSpeed = 12f;
                player.hasTipex = true;
                player.ObtainedPowerUps();
                await Task.Delay(5000);
                movement.movementSpeed = 8f;
                player.tipexActive = false;
                player.SpeedBoost();
                // StartCoroutine(TipexTimer(movement));
                break;
            case Type.OpaqueWhite:
                Destroy(gameObject);
                player.UpdateInvicibleUI();
                player.Invincibilty();
                break;
        }

    }
}
