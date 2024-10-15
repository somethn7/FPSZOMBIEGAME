using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    public MouseMovement mouseMovement;
    public GameObject screenFader;

    public int HP = 100;
    public GameObject bloodyScreen;
    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    private bool isDead = false;

    private void Start()
    {
        playerHealthUI.text = $"Health: {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HP -= damageAmount;
        Debug.Log("Remaining HP: " + HP);

        if (HP <= 0)
        {
            Debug.Log("Player Dead");
            PlayerDead();
        }
        else
        {
            Debug.Log("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        if (isDead) return; 

        isDead = true;

        
        SoundManager.Instance.playerChannel.Stop();

       
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);

       
        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.playerChannel.Play();

      
        playerMovement.enabled = false;
        animator.enabled = true;
        playerHealthUI.gameObject.SetActive(false);

        screenFader.SetActive(true);

        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.SetActive(true);

        int waveSurvived = GlobalReferences.Instance.waveNumber;

        if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        GetComponent<MouseMovement>().enabled = true;
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            ZombieHand zombieHand = other.GetComponent<ZombieHand>();
            if (zombieHand != null)
            {
                Zombie zombie = zombieHand.GetComponentInParent<Zombie>();
                if (zombie != null && !zombie.isDead && !isDead)
                {
                    TakeDamage(zombieHand.damage);
                }
            }
        }
    }
}
