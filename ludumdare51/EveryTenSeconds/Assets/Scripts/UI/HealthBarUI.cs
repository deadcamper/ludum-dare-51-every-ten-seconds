using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform gridParent;
    public Image iconTemplate;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite noHeart;
    public Sprite fullArmor;
    public Sprite halfArmor;

    private List<Image> barIcons = new List<Image>();

    private int lastHeartCount;
    private int lastArmorCount;

    GameStateTracker gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameStateTracker.GetInstance();
        iconTemplate.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckHearts())
        {
            UpdateHearts();
        }
    }

    bool CheckHearts()
    {
        GameState gs = gameState.GetGameState();

        return gs.playerHearts == lastHeartCount && gs.playerArmor == lastArmorCount;
    }

    void UpdateHearts()
    {
        GameState gs = gameState.GetGameState();

        while ((gs.playerTotalHearts + gs.playerArmor + 1)/2 > barIcons.Count)
        {
            Image icon = Instantiate(iconTemplate, iconTemplate.transform.parent);
            icon.gameObject.SetActive(true);
            barIcons.Add(icon);
            icon.transform.SetParent(gridParent);
        }

        int currentIconIndex = 0;
        for (int heart=0; heart<gs.playerTotalHearts; heart+=2)
        {
            Image barIcon = barIcons[currentIconIndex];
            int heartDiff = gs.playerHearts - heart;
            if (heartDiff <= 0)
            {
                barIcon.sprite = noHeart;
            }
            else if (heartDiff == 1)
            {
                barIcon.sprite = halfHeart;
            }
            else
            {
                barIcon.sprite = fullHeart;
            }
            barIcon.enabled = true;
            currentIconIndex++;
        }

        for (int armor=0; armor<gs.playerArmor; armor+=2)
        {
            Image barIcon = barIcons[currentIconIndex];
            int armorDiff = gs.playerArmor - armor;
            if (armorDiff == 1)
            {
                barIcon.sprite = halfArmor;
            }
            else
            {
                barIcon.sprite = fullArmor;
            }
            barIcon.enabled = true;
            currentIconIndex++;
        }

        for (; currentIconIndex < barIcons.Count; currentIconIndex++)
        {
            Image barIcon = barIcons[currentIconIndex];
            barIcon.enabled = false;
        }

        lastHeartCount = gs.playerHearts;
        lastArmorCount = gs.playerArmor;
    }
}
