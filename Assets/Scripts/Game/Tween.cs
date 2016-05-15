using UnityEngine;

namespace Assets.Scripts.Game
{
    public class TweenBehaviour : MonoBehaviour
    {
        private Vector3 startPosition;
        private Vector3 endPosition;
        private Quaternion startRotation;
        private Quaternion endRotation;
        private int tweenLength;
        private int tweenCounter;
        private TweenStyle style;

        public void Tween(Vector3 position, Quaternion rotation, int frames, TweenStyle style = TweenStyle.LINEAR)
        {
            startPosition = transform.localPosition;
            startRotation = transform.localRotation;

            endPosition = position;
            endRotation = rotation;

            tweenLength = frames;
            tweenCounter = 0;
            this.style = style;
        }

        public bool Tweening { get { return tweenCounter < tweenLength;  } }

        private void Update()
        {
            if (Tweening)
            {
                float t;
                switch(style)
                {
                    case TweenStyle.LINEAR: t = (float)tweenCounter / tweenLength; break;
                    default: t = (float)tweenCounter / tweenLength; break;
                }
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
                transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            }
        }
    }

    public enum TweenStyle : int { LINEAR }
}