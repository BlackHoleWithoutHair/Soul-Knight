using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Edgar.Unity.Examples.Scripts
{
    public class Platformer1GameManager : GameManagerBase<Platformer1GameManager>
    {
        // To make sure that we do not start the generator multiple times
        private bool isGenerating;

        public void Update()
        {
            if (InputHelper.GetKeyDown(KeyCode.G) && !isGenerating)
            {
                LoadNextLevel();
            }
        }

        public override void LoadNextLevel()
        {
            isGenerating = true;

            // Show loading screen
            ShowLoadingScreen("Platformer 1", "loading..");

            // Find the generator runner
            var generator = GameObject.Find("Platformer Generator").GetComponent<PlatformerGeneratorGrid2D>();

            // Start the generator coroutine
            StartCoroutine(GeneratorCoroutine(generator));
        }

        /// <summary>
        /// Coroutine that generates the level.
        /// It is sometimes useful to yield return before we hide the loading screen to make sure that
        /// all the scripts that were possibly created during the process are properly initialized.
        /// </summary>
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

            SetLevelInfo($"Generated in {stopwatch.ElapsedMilliseconds / 1000d:F}s");
            HideLoadingScreen();
        }
    }
}