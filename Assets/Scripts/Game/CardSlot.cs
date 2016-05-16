using UnityEngine;
using Assets.Scripts.Game.Cards;

namespace Assets.Scripts.Game
{
    public class CardSlot : TweenBehaviour
    {
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;
        private Card card;

        public void AssignPosition(Vector3 localPos, Quaternion localRot)
        {
            defaultPosition = localPos;
            defaultRotation = localRot;

            transform.localPosition = localPos;
            transform.localRotation = localRot;
        }

        public static void BattlePositions(CardSlot c1, CardSlot c2)
        {
            Vector2 u = new Vector2(c1.transform.localPosition.x, c1.transform.localPosition.z);
            Vector2 v = new Vector2(c2.transform.localPosition.x, c2.transform.localPosition.z);
            float t = u.y / (u.y - v.y);
            float w = u.x + t * (u.x - v.x);
            float theta = Mathf.Atan(u.y / (w - u.x)) * Mathf.Rad2Deg;
            c1.Tween(c1.defaultPosition, Quaternion.Euler(0, 90 - theta, 90), 20);
            c2.Tween(c2.defaultPosition, Quaternion.Euler(0, 90 - theta, 90), 20);
        }

        public void ToScreenSpace(Vector2 position, float distance = 1f)
        {
            Tween(CameraControl.singleton.ScreenSpaceToWorldSpace(position, distance), Quaternion.Euler(0, 0, 310), 20);
        }
    }
}
