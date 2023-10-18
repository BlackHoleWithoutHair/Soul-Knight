using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Edgar.Unity.Examples.Metroidvania
{
    public class MetroidvaniaGameManager : GameManagerBase<MetroidvaniaGameManager>
    {
        public MetroidvaniaLevelType LevelType;
        private long generatorElapsedMilliseconds;

        // To make sure that we do not start the generator multiple times
        private bool isGenerating;

        public static readonly string LevelMapLayer = "LevelMap";
        public static readonly string StaticEnvironmentLayer = "StaticEnvironment";

        protected override void SingletonAwake()
        {
            if (LayerMask.NameToLayer(StaticEnvironmentLayer) == -1)
            {
                throw new Exception($"\"{StaticEnvironmentLayer}\" layer is needed for this example to work. Please create this layer.");
            }

            LoadNextLevel();
        }

        public void Update()
        {
            if (InputHelper.GetKeyDown(KeyCode.G) && !isGenerating)
            {
                LoadNextLevel();
            }

            if (InputHelper.GetKeyDown(KeyCode.U))
            {
                Canvas.SetActive(!Canvas.activeSelf);
            }
        }

        public override void LoadNextLevel()
        {
            isGenerating = true;

            // Show loading screen
            ShowLoadingScreen($"Metroidvania - {LevelType}", "loading..");

            // Find the generator runner
            var generator = GameObject.Find($"Platformer Generator").GetComponent<PlatformerGeneratorGrid2D>();

            // Start the generator coroutine
            StartCoroutine(GeneratorCoroutine(generator));
        }


        private IEnumerator GeneratorCoroutine(PlatformerGeneratorGrid2D generator)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var generatorCoroutine = this.StartSmartCoroutine(generator.GenerateCoroutine());

            yield return generatorCoroutine.Coroutine;

            yield return null;

            stopwatch.Stop();

            isGenerating = false;

            generatorCoroutine.ThrowIfNotSuccessful();

            generatorElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            RefreshLevelInfo();
            HideLoadingScreen();
        }

        private void RefreshLevelInfo()
        {
            SetLevelInfo($"Generated in {generatorElapsedMilliseconds / 1000d:F}s\nLevel type: {LevelType}");
        }

        public bool LevelMapSupported()
        {
            var layer = LayerMask.NameToLayer(LevelMapLayer);

            if (layer == -1)
            {
                Debug.Log($"Level map is currently not supported. Please add a layer called \"{LevelMapLayer}\" to enable this feature and then restart the game.");
            }

            return layer != -1;
        }
    }
}