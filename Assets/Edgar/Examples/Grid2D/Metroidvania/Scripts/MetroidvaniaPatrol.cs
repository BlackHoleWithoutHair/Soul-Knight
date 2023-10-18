using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    // https://www.youtube.com/watch?v=aRxuKoJH9Y0
    public class MetroidvaniaPatrol : MonoBehaviour
    {
        public float Speed = 5;
        public float Distance = 2;
        public Transform GroundDetection;

        private bool movingRight = true;

        public void Update()
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);

            var groundInfo = Physics2D.Raycast(GroundDetection.position, Vector2.down, Distance);
            var forwardInfo = Physics2D.Raycast(GroundDetection.position, movingRight ? Vector2.right : Vector2.left, Distance);

            if (groundInfo.collider == null || forwardInfo.collider != null)
            {
                if (movingRight)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }
        }
    }
}