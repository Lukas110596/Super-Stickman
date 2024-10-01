using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer activeRenderer;
    private DeathAnimation deathAnimation;
    public int maxHealth = 3;
    public int currentHealth;
    public int score = 0;
    public bool hasRubber = false;
    public bool hasTipex = false;
    public bool rubberActive = false;
    public bool tipexActive = false;
    public Image[] hearts; 
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private bool hasTouchedWater = false;
    public bool invincible {get; private set;}
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI invicibleText;
    public TextMeshProUGUI highlightText;
    public TextMeshProUGUI rubberText;
    public TextMeshProUGUI tipexText;
    public TextMeshProUGUI obtainedPowerUps;
    [SerializeField] public AudioSource stompingSound;
    [SerializeField] public AudioSource hurtSound;
    [SerializeField] public AudioSource pickUpPowerUpSound;
    [SerializeField] public AudioSource endSound;
    [SerializeField] public AudioSource deathSound;
    [SerializeField] public AudioSource startingSound;

    void Start()
    {
        scoreText.text = "";
        invicibleText.text = "";
        highlightText.text = "";
        rubberText.text = "";
        tipexText.text = "";
        obtainedPowerUps.text = "";
        currentHealth = maxHealth;
        UpdateHeartsUI();
        UpdateScoreUI();
        StartCoroutine(UpdateRubberUI());
        StartCoroutine(UpdateTipexUI());
        StartCoroutine(UpdateInvicibleUI());
        StartCoroutine(ScoreHighlights());
        ObtainedPowerUps();
        startingSound.Play();
    }

    void Update()
    {
        Reactivate();

    }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }

    public void Hit()
    {
        if (!invincible)
        {
            if (currentHealth > 0)
            {
            currentHealth -= 1;
            hurtSound.Play();
            UpdateHeartsUI(); // Update the UI when the player is hit

            if (currentHealth == 0)
            {
                
                Death();
                
            }
            }
        }
        
    }

public void Death()
{
    deathAnimation.enabled = true;
    // Set all hearts to empty when the player dies
    currentHealth = 0;
    UpdateHeartsUI();
    GameManager.instance.RestartLevel();
    deathSound.Play();
    
    

}

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Water"))
    {
        // Set flag indicating that the player touched water
        hasTouchedWater = true;
        Death();
    }
}

// Update the UI to reflect the current health
public void UpdateHeartsUI()
{
    for (int i = 0; i < hearts.Length; i++)
    {
        if (i < currentHealth && hasTouchedWater)
        {
            // Display empty hearts for remaining health if player has touched water
            hearts[i].sprite = emptyHeart;
            hearts[i].enabled = true; // Ensure the image component is enabled
        }
        else if (i < currentHealth && !hasTouchedWater)
        {
            // Display full hearts for remaining health if not touched water
            hearts[i].sprite = fullHeart;
            hearts[i].enabled = true; // Ensure the image component is enabled
        }
        else
        {
            // Display empty hearts for lost health or if touched water
            hearts[i].sprite = emptyHeart;
            hearts[i].enabled = true; // Ensure the image component is enabled
        }
    }
}

    // function to call when invincibility active
    public void Invincibilty(float duration = 5f)
    {
        StartCoroutine(InvincibleAnimation(duration));

    }

    // invincible timer
    private IEnumerator InvincibleAnimation(float duration)
    {
        invincible = true;
        StartCoroutine(UpdateInvicibleUI());
        float elapsed = 0f;
        
        while (elapsed < duration) {
            elapsed+=Time.deltaTime;
            yield return null;
        }
        invincible = false;
        StartCoroutine(UpdateInvicibleUI());
    }

    // update score ui, called after killing enemy and reaching end of level
    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // update invincible text (countdown from 5)
    public IEnumerator UpdateInvicibleUI()
    {
        if (invincible) {
            float elapsed=0f;
            float time = 5f;
            while (elapsed<time) {
                invicibleText.text = "INVINCIBLE(" + Mathf.Ceil(time-elapsed) + "s)";
                elapsed+=Time.deltaTime;
                yield return null;
            } 
        } else {
            invicibleText.text = " ";
        }
    }

    // timer for displaying highlight text
    public IEnumerator ScoreHighlights()
    {
    float elapsedTime = 0f;
    float displayTime = 3f;
    highlightText.text = "";
    while (elapsedTime < displayTime)
    {
        UpdateHighlightText();
        elapsedTime += Time.deltaTime;
        yield return null;
    }
    highlightText.text = "";
    }

    // highlight text 
    private void UpdateHighlightText()
    {
        switch (score)
        {
            case 5:
                highlightText.text = "DOMINANT";
                break;
            case 10:
                highlightText.text = "MONSTER";
                break;
            case 20:
                highlightText.text = "DEMOOOON";
                break;
        }
    }

    // jump boost text
    public void JumpBoost() {
        StartCoroutine(UpdateRubberUI());
    }
    public IEnumerator UpdateRubberUI()
    {
        if (rubberActive) {
            float elapsed=0f;
            float time = 5f;
            while (elapsed<time) {
                rubberText.text = "JUMP BOOST(" + Mathf.Ceil(time-elapsed) + "s)";
                elapsed+=Time.deltaTime;
                yield return null;
            } 
        } else {

            rubberText.text = " ";
        }
    }

    // speed boost text
    public void SpeedBoost() {
        StartCoroutine(UpdateTipexUI());
    }

    public IEnumerator UpdateTipexUI()
    {
        if (tipexActive) {
            float elapsed=0f;
            float time = 5f;
            while (elapsed<time) {
                tipexText.text = "SPEED BOOST(" + Mathf.Ceil(time-elapsed) + "s)";
                elapsed+=Time.deltaTime;
                yield return null;
            } 
        } else {
            tipexText.text = " ";
        }
    }


    // function to reactive obtained power ups
    public async void Reactivate() {
        PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
        if (hasRubber && Input.GetButtonDown("Fire1") && !rubberActive) {
            pickUpPowerUpSound.Play();
            rubberActive = true;
            JumpBoost();
            movement.maxJumpHeight = 7f;
            await Task.Delay(5000);
            movement.maxJumpHeight = 5f;
            rubberActive = false;
            JumpBoost();
        }
        if (hasTipex && Input.GetButtonDown("Fire2") && !tipexActive) {
            pickUpPowerUpSound.Play();
            tipexActive = true;
            SpeedBoost();
            movement.movementSpeed = 12f;
            await Task.Delay(5000);
            movement.movementSpeed = 8f;
            tipexActive = false;
            SpeedBoost();
        }
    }

    public void ObtainedPowerUps() {
        string s ="";
        if (hasRubber) {
            s+="Rubber: available (R)"+"\n";
        } 
        if (hasTipex) {
            s+="Tipex: available (F)"+"\n";
        } 
        obtainedPowerUps.text = s;
    }


}
 