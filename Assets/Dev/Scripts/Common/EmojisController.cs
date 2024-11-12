using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoodType { GlassHappy, Happy, Lovely, Vomaite, Sad, Shok, Angry }
public class EmojisController : MonoBehaviour
{

    public ParticleSystem[] emojis;

    public void PlayEmoji(MoodType mood)
    {
        foreach (var emoji in emojis)
        {
            emoji.gameObject.SetActive(false);
        }
        int index = (int)mood;


        if (index >= 0 && index < emojis.Length)
        {
            ParticleSystem selectedEmoji = emojis[index];
            selectedEmoji.gameObject.SetActive(true);

            if (selectedEmoji != null)
            {
                selectedEmoji.Play();
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
