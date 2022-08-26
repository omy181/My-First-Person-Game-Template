
using UnityEngine;

namespace Holylib.SoundEffects
{
    public class SoundEffect
    {
        public AudioClip[] Clip;
        public bool isloop = false;
        public float volume = 1;
        public float playduration = -1;
        public float startduration = -1;
        public Vector3 pos = new Vector3(0, 0, 0);
    }
}
