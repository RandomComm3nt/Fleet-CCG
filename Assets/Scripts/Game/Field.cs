using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
    public class Field : MonoBehaviour
    {
        private const int SLOTS_PER_RING = 10;
        private const float RING_SPACING = 1f;
        private const float INNER_RING_SPACING = 1f;

        private CardSlot planetSlot;
        private CardSlot[][] ringSlots;

        [SerializeField] private GameObject slotPrefab;
        
        private void Start()
        {
            ringSlots = new CardSlot[4][];
            for (int i = 0; i < 4; i++)
            {
                ringSlots[i] = new CardSlot[SLOTS_PER_RING];
                for (int j = 0; j < SLOTS_PER_RING; j++)
                {
                    ringSlots[i][j] = Instantiate(slotPrefab).GetComponent<CardSlot>();
                    float theta = ((j + 0.5f * (i % 2)) / SLOTS_PER_RING) * Mathf.PI * 2;
                    float r = i * RING_SPACING + RING_SPACING + INNER_RING_SPACING;
                    ringSlots[i][j].AssignPosition(new Vector3(r * Mathf.Cos(theta), 0, r * Mathf.Sin(theta)), Quaternion.Euler(0, -theta * Mathf.Rad2Deg, 0));
                    ringSlots[i][j].transform.SetParent(transform);
                }
            }

            ToBattle();
        }
        
        void Update()
        {

        }

        private void ToBattle()
        {
            CardSlot.BattlePositions(ringSlots[1][1], ringSlots[2][1]);
            CameraControl.singleton.TweenTo(Vector3.Lerp(ringSlots[1][1].transform.position, ringSlots[2][1].transform.position, 0.5f) + Vector3.Cross(Vector3.up, (ringSlots[1][1].transform.position - ringSlots[2][1].transform.position)), 
                Quaternion.LookRotation(Vector3.Cross(ringSlots[1][1].transform.position - ringSlots[2][1].transform.position, Vector3.up), Vector3.up));
        }
    }
}