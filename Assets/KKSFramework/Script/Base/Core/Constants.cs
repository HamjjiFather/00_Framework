namespace KKSFramework
{
    /// <summary>
    /// 모든 프레임워크 상수 관리 클래스.
    /// </summary>
    public class Constants
    {
        public static class Framework
        {
            /// <summary>
            /// Framework data j
            /// </summary>
            public const string FrameworkDataJsonFileName = "JsonFrameworkData";

            /// <summary>
            /// Framework data json file asset path.
            /// </summary>
            public const string FrameworkDataJsonFileAssetPath = "/Resources/_Data/Json/JsonFrameworkData.json";

            /// <summary>
            /// Sound json file name.
            /// </summary>
            public const string SoundJsonFileName = "JsonSoundClipData";

            /// <summary>
            /// Sound json file asset path.
            /// </summary>
            public const string SoundJsonFileAssetPath = "/Resources/_Data/Json/JsonSoundClipData.json";

            /// <summary>
            /// created sound enum file asset path.
            /// </summary>
            public const string SoundTypeFileAssetPath = "/KKSFramework/Script/Sound/SoundTypeEnum.cs";

            /// <summary>
            /// Sound enum file summary.
            /// </summary>
            public const string CsFileSummary = "/// <summary>\n/// Don't edit this file, It will be edited automatically on {0} class.\n/// </summary>\n";

            /// <summary>
            /// Sound enum file declaration.
            /// </summary>
            public const string SoundTypeEnumClassDeclare = "public enum SoundTypeEnum";
        }
    }
}