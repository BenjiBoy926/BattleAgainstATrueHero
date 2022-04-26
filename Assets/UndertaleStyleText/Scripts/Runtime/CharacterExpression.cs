using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UndertaleStyleText
{
    [System.Serializable]
    public class CharacterExpression
    {
        public enum VisualExpressionType
        {
            None, Animation, Face, Pose
        }

        public CharacterSettings CharacterSettings => Settings.GetCharacter(characterIndex);
        public TMP_FontAsset Font => CharacterSettings.GetFont(fontIndex);
        public AudioClip VoiceClip => CharacterSettings.GetVoiceClip(voiceClipIndex);
        public string AnimationTrigger => CharacterSettings.GetAnimationTrigger(visualElementIndex);
        public Sprite Face => CharacterSettings.GetFace(visualElementIndex);
        public Sprite Pose => CharacterSettings.GetPose(visualElementIndex);

        [SerializeField]
        [Tooltip("Index of the character in the character roster")]
        private int characterIndex;
        [SerializeField]
        [Tooltip("Index of the font to use for the character")]
        private int fontIndex;
        [SerializeField]
        [Tooltip("Index selected for the character's voice clip")]
        private int voiceClipIndex;
        [SerializeField]
        [Tooltip("Determine the type of visual expression for the character")]
        private VisualExpressionType visualType;
        [SerializeField]
        [Tooltip("Index selected for the visual element")]
        private int visualElementIndex;

        public void PlayVoiceClip(AudioSource voiceSource)
        {
            if(voiceSource != null && VoiceClip != null)
            {
                voiceSource.clip = VoiceClip;
                voiceSource.Play();
            }
        }
        public void SetFont(TextMeshProUGUI text)
        {
            if (Font != null) text.font = Font;
        }
        public void SetVisualExpression(Animator animator, ImageOrSpriteRenderer visualRenderer)
        {
            switch(visualType)
            {
                case VisualExpressionType.None: break;
                case VisualExpressionType.Animation:
                    if (!string.IsNullOrEmpty(AnimationTrigger)) animator.SetTrigger(AnimationTrigger);
                    break;
                case VisualExpressionType.Face:
                    if (Face != null) visualRenderer.SetSprite(Face);
                    break;
                case VisualExpressionType.Pose:
                    if (Pose != null) visualRenderer.SetSprite(Pose);
                    break;
            }
        }
    }
}
