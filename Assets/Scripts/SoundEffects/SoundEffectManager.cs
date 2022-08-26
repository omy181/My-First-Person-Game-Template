using UnityEngine;
using System.Collections.Generic;

namespace Holylib.SoundEffects
{
    public class SoundEffectManager : MonoBehaviour
    {
        public static SoundEffectManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                Instance = this;
            }

        }

        public GameObject EmptySoundObject;

        public GameObject CreateSFXObj(GameObject EmptySoundObject)
        {
            return Instantiate(EmptySoundObject, null);
        }

        public void DestroySFXObject(GameObject SFXobject)
        {
            Destroy(SFXobject);
        }

        public float StartSFXVolume = 1;
        public List<GameObject> PlayinSounds;

        public GameObject PlayinMusic;
        public float StartMusicVolume = 1;
    }

    
    public static class SoundEffectController
    {
        public static GameObject PlaySFX(SoundClip clip, bool isloop = false, float volumePercentage = 1, float playduration = -1, float startduration = -1,Vector3 pos = new Vector3(),bool isindipendent = false)
        {
            //Create SFX Object
            GameObject SoundEffectsObject = SoundEffectManager.Instance.CreateSFXObj(SoundEffectManager.Instance.EmptySoundObject);
            SoundEffectsObject.transform.position = pos;
            if (!isindipendent)
            {
                SoundEffectManager.Instance.PlayinSounds.Add(SoundEffectsObject);
            }

            //Create New SFX Data
            CreateSound(SoundEffectsObject,SoundType.SFX, clip, isloop, volumePercentage, playduration, startduration, pos);

            return SoundEffectsObject;
        }

        public static GameObject PlayMusic(SoundClip clip, bool isloop = false, float volumePercentage = 1, float playduration = -1, float startduration = -1, Vector3 pos = new Vector3(), bool isindipendent = false)
        {
            //Create Music Object
            GameObject MusicObject = SoundEffectManager.Instance.CreateSFXObj(SoundEffectManager.Instance.EmptySoundObject);
            MusicObject.transform.position = pos;
            if (!isindipendent)
            {
                SoundEffectManager.Instance.PlayinMusic = MusicObject;
            }

            //Create New SFX Data
            CreateSound(MusicObject,SoundType.Music,clip,isloop,volumePercentage,playduration,startduration,pos);

            return MusicObject;
        }
        public static void StopSFX(GameObject SFXobject)
        {
            SoundEffectManager.Instance.PlayinSounds.Remove(SFXobject);
            SoundEffectManager.Instance.DestroySFXObject(SFXobject);
        }

        public static void StopMusic()
        {
            SoundEffectManager.Instance.DestroySFXObject(SoundEffectManager.Instance.PlayinMusic);
            SoundEffectManager.Instance.PlayinMusic = null;
        }

        public static void StopAllSFX()
        {
            foreach(GameObject SFXobject in SoundEffectManager.Instance.PlayinSounds)
            {
                SoundEffectManager.Instance.DestroySFXObject(SFXobject);
            }
            SoundEffectManager.Instance.PlayinSounds.Clear();
        }

        public static void SFXVolume(float volume = -1, float percentage = -1, bool isTemporary = true)
        {
            float finalvolume = 1;
            if (volume != -1)
            {
                finalvolume = volume;

                if (!isTemporary)
                {
                    SoundEffectManager.Instance.StartSFXVolume = finalvolume;
                }
                finalvolume = 1;
            }
            else if (percentage != -1)
            {
                finalvolume = percentage;

                if (!isTemporary)
                {
                    SoundEffectManager.Instance.StartSFXVolume = finalvolume;
                }
            }

            foreach (GameObject sfx in SoundEffectManager.Instance.PlayinSounds)
            {
                ModifySound(sfx, SoundType.SFX, null, default, finalvolume);
            }
        }

        public static void MusicVolume(float volume = -1, float percentage = -1, bool isTemporary = true)
        {
            float finalvolume = 1;

            if (volume != -1)
            {
                finalvolume = volume;

                if (!isTemporary)
                {
                    SoundEffectManager.Instance.StartMusicVolume = finalvolume;
                  
                }
                finalvolume = 1;
            }
            else if (percentage != -1)
            {
                finalvolume = percentage;

                if (!isTemporary)
                {
                    SoundEffectManager.Instance.StartMusicVolume = finalvolume;
                }

            }

            ModifySound(SoundEffectManager.Instance.PlayinMusic, SoundType.Music, null, true, finalvolume);
        }

        static void CreateSound(GameObject sound, SoundType type, SoundClip clip, bool isloop = false, float volumePercentage = 1, float playduration = -1, float startduration = -1, Vector3 pos = new Vector3())
        {
            SoundEffect sfx = new SoundEffect();
            sfx.Clip = clip.Clip;
            sfx.isloop = isloop;
            sfx.playduration = playduration;
            sfx.startduration = startduration;
            sfx.pos = pos;

            switch (type)
            {
                case SoundType.Music:
                    sfx.volume = SoundEffectManager.Instance.StartMusicVolume * volumePercentage; 
                    break;
                case SoundType.SFX:
                    sfx.volume = SoundEffectManager.Instance.StartSFXVolume * volumePercentage; 
                    break;
            }

            sound.GetComponent<SoundSource>().sfx = sfx;
        }

        public enum SoundType { SFX,Music}
        public static void ModifySound(GameObject sound,SoundType type, SoundClip clip = null, bool isloop = default, float volumePercentage = -1, float playduration = -1, float startduration = -1, Vector3 pos = new Vector3())
        {
            SoundEffect sfx = sound.GetComponent<SoundSource>().sfx;

            if(clip != null) { sfx.Clip = clip.Clip; }
            if(isloop != default) { sfx.isloop = isloop; }
            if(playduration != -1) { sfx.playduration = playduration; }
            if(startduration != -1) { sfx.startduration = startduration; }
            if(pos != null) { sfx.pos = pos; }

            switch (type)
            {
                case SoundType.Music:
                    if (volumePercentage != -1) { sfx.volume = SoundEffectManager.Instance.StartMusicVolume * volumePercentage; }
                    break;
                case SoundType.SFX:
                    if (volumePercentage != -1) { sfx.volume = SoundEffectManager.Instance.StartSFXVolume* volumePercentage; }
                    break;
            }

            sound.GetComponent<SoundSource>().Modify(sfx);
        }
    }

}
