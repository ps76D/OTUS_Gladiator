using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Animations;

namespace AnimTools
{
    public class PlaySoundOnStateEnter : StateMachineBehaviour
    {
        [SoundGroup]
        public string Sound;
        public int SkipFrames;


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!MasterAudio.SafeInstance)
            {
                return;
            }
            
            base.OnStateEnter(animator, stateInfo, layerIndex, controller);
            var transform = animator.transform;
            var animatorClipInfos = animator.GetCurrentAnimatorClipInfo(layerIndex);
            float delay = 0;
            if (animatorClipInfos.Length > 0 && SkipFrames > 0)
            {
                var clipInfo = animatorClipInfos[0];
                delay = SkipFrames / clipInfo.clip.frameRate;
                Debug.Log($"Play sound {Sound} with delay {SkipFrames} frames. It should be ~{delay}s. Current frame: {Time.frameCount} Target frame: {Time.frameCount + SkipFrames}");
            }

            MasterAudio.PlaySound3DFollowTransformAndForget(Sound, transform, delaySoundTime:delay);
        }
    }
}