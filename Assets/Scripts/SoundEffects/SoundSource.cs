using UnityEngine;
using System.Collections;

namespace Holylib.SoundEffects
{
    public class SoundSource : MonoBehaviour
    {
        public SoundEffect sfx;
        AudioSource audiosource;
        private void Start()
        {
            audiosource = GetComponent<AudioSource>();

            Modify();

            if(sfx.startduration > 0)
            {
                StartCoroutine(PlayAfterTime());
            }
            else 
            {
                audiosource.Play();
                StartCoroutine(StopAfterTime());
            }   
   
        }

        public void Modify(SoundEffect s = null)
        {
            if(s != null) { sfx = s; }

            audiosource = GetComponent<AudioSource>();

            audiosource.clip = sfx.Clip[Random.Range(0, sfx.Clip.Length)];
            audiosource.loop = sfx.isloop;
            audiosource.volume = sfx.volume;

        }

        IEnumerator StopAfterTime()
        {
            if (sfx.playduration > 0)
            {
                yield return new WaitForSeconds(sfx.playduration);
            }
            else
            {
                while (audiosource.isPlaying)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            SoundEffectController.StopSFX(this.gameObject);
        }

        IEnumerator PlayAfterTime()
        {
            yield return new WaitForSeconds(sfx.startduration);

            audiosource.Play();

            StartCoroutine(StopAfterTime());
        }
    }
    
}
