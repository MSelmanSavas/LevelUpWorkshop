namespace UsefulExtensions.RuntimePlatform
{
    public static class RuntimePlatformExtensions
    {
        public static bool IsEditor(this UnityEngine.RuntimePlatform runtimePlatform) =>
            runtimePlatform == UnityEngine.RuntimePlatform.LinuxEditor ||
            runtimePlatform == UnityEngine.RuntimePlatform.WindowsEditor ||
            runtimePlatform == UnityEngine.RuntimePlatform.OSXEditor;
    }


}
