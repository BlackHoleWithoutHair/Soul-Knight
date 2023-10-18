namespace Edgar.Unity.Editor
{
    public class EdgarSettings : EdgarScriptableSingleton<EdgarSettings>
    {
        internal const string FilePath = "UserSettings/EdgarSettings.asset";

        public EdgarSettingsGeneral General = new EdgarSettingsGeneral();

        public EdgarSettingsGrid2D Grid2D = new EdgarSettingsGrid2D();

        public EdgarSettingsGrid3D Grid3D = new EdgarSettingsGrid3D();

        public void Save()
        {
            Save(true);
        }
    }
}