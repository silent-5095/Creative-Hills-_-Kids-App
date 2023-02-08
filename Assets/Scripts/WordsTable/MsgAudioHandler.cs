using UnityEngine;

namespace WordsTable
{
    public class MsgAudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private AudioSource msgSource;

        public void PlayAudio()
        {
            var index = PlayerPrefs.GetInt(nameof(MsgAudioHandler));
            msgSource.PlayOneShot(clips[index]);
            index++;
            if (index >= clips.Length)
            {
                index = 0;
            }

            PlayerPrefs.SetInt(nameof(MsgAudioHandler), index);
        }
    }
}