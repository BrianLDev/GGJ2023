/*
ECX UTILITY SCRIPTS
Sfx Manager (Scriptable Object)
Last updated: Apr 16, 2022
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcxUtilities {
    /// <summary>
    /// This SfxManager is primarily used as a readily accessible SFX AudioClip storage.
    /// Other scripts can access audioclips as needed using SfxManager.xxx
    /// </summary>
    [CreateAssetMenu(fileName = "SfxManager", menuName = "ECX Utilities/SfxManager", order = 1)] 
    public class SfxManager : ScriptableObject {
        [Header("General FX")]
        public AudioClip AmbientNoise;
        public AudioClip ShutDown;

        [Header("Weapon SFX")]
        public AudioClip BigGun01;
        public AudioClip BigGun02;
        public AudioClip BigGun03;
        public AudioClip MedGunAuto;
        public AudioClip TinyGunAuto;


        // METHODS:
        // THE AUDIOMANAGER WILL HANDLE PLAYING MOST OF THE CLIPS, BUT IF YOU HAVE ANY CUSTOM SFX METHODS NEEDED, ADD THEM BELOW AS PUBLIC METHODS

    }
}