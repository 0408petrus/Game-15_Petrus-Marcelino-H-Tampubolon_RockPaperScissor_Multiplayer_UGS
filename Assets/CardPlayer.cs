using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CardPlayer : MonoBehaviour
{
    public Transform atkPosRef;
    public Card choosenCard;
    [SerializeField] private TMP_Text nameText;
    public TMP_Text healthText;
    public HealthBar healthBar;
    public float Health;
    public PlayerStats stats = new PlayerStats
    {
        MaxHealth = 100,
        RestoreValue = 5,
        DamageValue = 10
    };
    public AudioSource audioSource;
    public AudioClip damageClip;
    private Tweener animationTweener;

    public TMP_Text NickName { get => nameText; }

    public bool IsReady = false;

    private void Start()
    {
        Health = stats.MaxHealth;
    }

    public void SetStats(PlayerStats newStats,bool restoreFullHealth = false)
    {
        this.stats = newStats;
        if(restoreFullHealth)
            Health = stats.MaxHealth;

        UpdateHealthBar();
    }

    public Attack? AttackValue
    {
        //get => choosenCard == null ? null : choosenCard.AttackValue;
        get
        {
            if (choosenCard == null)
                return null;
            else
                return choosenCard.AttackValue;
        }
    }

    // Start is called before the first frame update


    public void Reset()
    {
        if (choosenCard != null)
        {
            choosenCard.Reset();
        }

        choosenCard = null;
    }
    public void SetChoosenCard(Card newCard)
    {
        if (choosenCard != null)
        {
            choosenCard.transform.DOKill();
            choosenCard.Reset();
        }

        choosenCard = newCard;
        choosenCard.transform.DOScale(choosenCard.transform.localScale * 1.2f, 0.2f);
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, stats.MaxHealth);
        UpdateHealthBar();
        
    
    }
     
    public void UpdateHealthBar()
    {
        //healthbar
        healthBar.UpdateBar(Health / stats.MaxHealth);

        //text
        healthText.text = Health + "/" + stats.MaxHealth;
    }

    public void AnimateAttack()
    {
        Tweener tweener = choosenCard.transform
              .DOMove(atkPosRef.position, 1);
    }
    public void AnimateDamage()
    {
        audioSource.PlayOneShot(damageClip);
        var image = choosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.5f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(0.2f);
    }

    public void AnimateDraw()
    {
        Tweener tweener = choosenCard.transform
            .DOMove(choosenCard.OriginalPosition, 1)
            .SetEase(Ease.InBack)
            .SetDelay(0.5f);
    }


    public bool isAnimating()
    {
        return animationTweener.IsActive();
    }

    public void IsClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetClickable(value);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
