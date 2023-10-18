using System;

namespace Edgar.Unity
{
    /// <summary>
    /// Enum that allows overriding the per room template "Allow rotation" configuration.
    /// </summary>
    public enum AllowRotationOverrideGrid3D
    {
        /// <summary>
        /// Do not change anything. Keep the configuration from individual room templates.
        /// </summary>
        NoOverride = 0,

        /// <summary>
        /// Allow rotation for all room templates.
        /// </summary>
        Allow = 1,

        /// <summary>
        /// Do not allow rotation of any room template.
        /// </summary>
        DoNotAllow = 2,
    }

    public static class AllowRotationOverrideExtensions
    {
        /// <summary>
        /// Converts the enum into a nullable bool (3 possible values).
        /// </summary>
        public static bool? GetBoolValue(this AllowRotationOverrideGrid3D value)
        {
            switch (value)
            {
                case AllowRotationOverrideGrid3D.NoOverride:
                    return null;

                case AllowRotationOverrideGrid3D.Allow:
                    return true;

                case AllowRotationOverrideGrid3D.DoNotAllow:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}