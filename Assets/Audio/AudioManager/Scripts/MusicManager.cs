/*
ECX UTILITY SCRIPTS
Music Manager (Scriptable Object)
Last updated: Apr 16, 2022
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcxUtilities {

    /// <summary>
    /// This MusicManager is primarily used as a readily accessible Music AudioClip storage.
    /// Other scripts can access audioclips as needed using MusicManager.Instance.xxx
    /// It also handles the automatic playback of music as soon as a specific Scene is loaded.
    /// </summary>
    [CreateAssetMenu(fileName = "MusicManager-X", menuName = "ECX Utilities/MusicManager", order = 1)] 
    public class MusicManager : ScriptableObject {

        [Header("Standard Music Tracks")]
        public AudioClip MainMenuMusic; // Splash
        public AudioClip Canal;
        public AudioClip Respite;
        public AudioClip TheStructure;
        public AudioClip FilledToTheBrim;
        public AudioClip GameOverMusic;

        [Header("Win/Loss Stingers")]
        public AudioClip ObjectiveCompleteStinger;
        public AudioClip VictoryStinger;
        public AudioClip DefeatStinger;

        // [Header("Game Specific Music Tracks")]   // UNCOMMENT THIS HEADER, RENAME IT, AND ADD ANY ADDITIONAL AUDIO CLIPS BELOW. THEN DRAG/DROP THEM IN THE UNITY EDITOR.

        
        // METHODS:

    }
}