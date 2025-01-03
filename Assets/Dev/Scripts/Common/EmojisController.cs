using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoodType { CuteHappy, Happy, Sad, Angry }
public class EmojisController : MonoBehaviour
{

    public Transform[] emojis;

    public void PlayEmoji(MoodType mood)
    {
        foreach (var emoji in emojis)
        {
            emoji.gameObject.SetActive(false);
        }
        int index = (int)mood;


        if (index >= 0 && index < emojis.Length)
        {
            Transform selectedEmoji = emojis[index];

            if (selectedEmoji != null)
            {
                selectedEmoji.gameObject.SetActive(true);

            }
            else
            {
                Debug.LogWarning($"No particle system assigned for {mood} emoji.");
            }
        }
        else
        {
            Debug.LogWarning("Mood index is out of range.");
        }
    }


}
