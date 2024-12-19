using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance;

    public enum HintName
    {
        BeerBottle,
        PlantsVase,
        SinkPlates,
        Blunt,
        DoorKnocked,
        CheckedNotifs,
        NightNoises,
        NickClue,
        JonathanCrates
    }

    private Dictionary<HintName, bool> collectedHints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            collectedHints = new Dictionary<HintName, bool>();

            foreach (HintName hint in System.Enum.GetValues(typeof(HintName)))
            {
                collectedHints[hint] = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddHint(HintName hintName)
    {
        if (!collectedHints[hintName])
        {
            collectedHints[hintName] = true;
            Debug.Log($"Hint collected: {hintName}");
        }
    }
    public bool HasHint(List<HintName> requiredHints)
    {
        foreach (HintName hint in requiredHints)
        {
            if (!collectedHints[hint])
            {
                return false; 
            }
        }
        return true; 
    }

    public List<HintName> GetCollectedHints()
    {
        List<HintName> collected = new List<HintName>();

        foreach (var hint in collectedHints)
        {
            if (hint.Value)
            {
                collected.Add(hint.Key);
            }
        }

        return collected;
    }
}


