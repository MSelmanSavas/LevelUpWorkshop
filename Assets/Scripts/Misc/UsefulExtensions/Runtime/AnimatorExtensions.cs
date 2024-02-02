using UnityEngine;

namespace UsefulExtensions.Animator
{
    public static class AnimatorExtensions
    {
        public static float GetCurrentClipLength(this UnityEngine.Animator animator, float defaultValue = 1f)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            return clipInfo.Length < 1 ? defaultValue : clipInfo[0].clip.length;
        }

        public static void ResetVariables(this UnityEngine.Animator animator)
        {
            foreach (var parameter in animator.parameters)
            {
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Trigger:
                        {
                            animator.ResetTrigger(parameter.name);
                            break;
                        }
                    case AnimatorControllerParameterType.Bool:
                        {
                            animator.SetBool(parameter.name, false);
                            break;
                        }
                }
            }
        }
    }

}
