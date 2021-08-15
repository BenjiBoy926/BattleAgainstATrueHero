using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace UndertaleStyleText 
{
    [System.Serializable]
    public class CharacterSettings
    {
        // public accessors
        public string CharacterName => characterName;
        public string[] FontNames => fonts.Select(x => x.name).ToArray();
        public string[] VoiceClipNames => voiceClips.Select(x => x.name).ToArray();
        public string[] AnimationTriggers => animationTriggers;
        public string[] FaceNames => faces.Select(x => x.name).ToArray();
        public string[] PoseNames => poses.Select(x => x.name).ToArray();

        // private editor data
        [SerializeField]
        [Tooltip("The name of the character")]
        private string characterName = "Gaster";
        [SerializeField]
        [Tooltip("List of fonts usable by this character")]
        private TMP_FontAsset[] fonts;
        [SerializeField]
        [Tooltip("The voice sound that plays when the character speaks")]
        private AudioClip[] voiceClips;
        [SerializeField]
        [Tooltip("List of animation triggers on the character's animator")]
        private string[] animationTriggers;
        [SerializeField]
        [Tooltip("List of still sprites for the character's facial expressions")]
        private Sprite[] faces;
        [SerializeField]
        [Tooltip("List of still sprites for the character's body poses")]
        private Sprite[] poses;

        public TMP_FontAsset GetFont(int index) => fonts[index];
        public AudioClip GetVoiceClip(int index) => voiceClips[index];
        public string GetAnimationTrigger(int index) => animationTriggers[index];
        public Sprite GetFace(int index) => faces[index];
        public Sprite GetPose(int index) => poses[index];
    }
}