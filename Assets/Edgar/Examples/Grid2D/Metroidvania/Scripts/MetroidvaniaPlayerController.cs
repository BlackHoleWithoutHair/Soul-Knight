using Edgar.Unity.Examples.PC2D;
using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    [RequireComponent(typeof(PlatformerMotor2D))]
    public class MetroidvaniaPlayerController : MonoBehaviour
    {
        private PlatformerMotor2D motor;

        public void Start()
        {
            motor = GetComponent<PlatformerMotor2D>();
        }

        void Update()
        {
            // Disable one-way platforms when pressing DOWN button
            if (InputHelper.GetVerticalAxis() < 0)
            {
                motor.enableOneWayPlatforms = false;
                motor.oneWayPlatformsAreWalls = false;
                motor.Jump(0f);
            }
            else
            {
                motor.enableOneWayPlatforms = true;
                motor.oneWayPlatformsAreWalls = true;
            }

            // Jump?
            // If you want to jump in ladders, leave it here, otherwise move it down
            if (InputHelper.GetKeyDown(PC2D.Input.JUMP))
            {
                motor.Jump();
                motor.DisableRestrictedArea();
            }

            motor.jumpingHeld = InputHelper.GetKey(PC2D.Input.JUMP);

            // X axis movement
            if (Mathf.Abs(InputHelper.GetHorizontalAxis()) > 0)
            {
                motor.normalizedXMovement = InputHelper.GetHorizontalAxis();
            }
            else
            {
                motor.normalizedXMovement = 0;
            }

            if (InputHelper.GetVerticalAxis() != 0)
            {
                bool up_pressed = InputHelper.GetVerticalAxis() > 0;
            }
            else if (InputHelper.GetVerticalAxis() < -Globals.FAST_FALL_THRESHOLD)
            {
                motor.fallFast = false;
            }

            if (InputHelper.GetKeyDown(PC2D.Input.DASH))
            {
                motor.Dash();
            }
        }
    }
}