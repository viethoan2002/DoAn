﻿using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Rope
{
    public class G4_GrappleRope : MonoBehaviour
    {
        [FormerlySerializedAs("grapplingGun")] [Header("General refrences:")]
        public G4_GrapplingGun g4GrapplingGun;
        [SerializeField] LineRenderer m_lineRenderer;

        [Header("General Settings:")]
        [SerializeField] private int percision = 20;
        [Range(0, 100)][SerializeField] private float straightenLineSpeed = 4;

        [Header("Animation:")]
        public AnimationCurve ropeAnimationCurve;
        [SerializeField] [Range(0.01f, 4)] private float WaveSize = 20;
        float waveSize;

        [Header("Rope Speed:")]
        public AnimationCurve ropeLaunchSpeedCurve;
        [SerializeField] [Range(1, 50)] private float ropeLaunchSpeedMultiplayer = 4;

        float moveTime = 0;

        [SerializeField]public bool isGrappling = false;
    
        private bool drawLine = true;
        bool straightLine = true;
    

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_lineRenderer.enabled = false;
            m_lineRenderer.positionCount = percision;
            waveSize = WaveSize;
        }

        private void OnEnable()
        {
            moveTime = 0;
            m_lineRenderer.enabled = true;
            m_lineRenderer.positionCount = percision;
            waveSize = WaveSize;
            straightLine = false;
            LinePointToFirePoint();
        }

        private void OnDisable()
        {
            m_lineRenderer.enabled = false;
            isGrappling = false;
        }

        void LinePointToFirePoint()
        {
            for (int i = 0; i < percision; i++)
            {
                m_lineRenderer.SetPosition(i, g4GrapplingGun.firePoint.position);
            }
        }

        void Update()
        {
            moveTime += Time.deltaTime;

            if (drawLine)
            {
                DrawRope();
            }
        }

        void DrawRope()
        {
            if (!straightLine) 
            {
                if (m_lineRenderer.GetPosition(percision - 1).x != g4GrapplingGun.grapplePoint.x)
                {
                    DrawRopeWaves();
                }
                else 
                {
                    straightLine = true;
                }
            }
            else 
            {
                if (!isGrappling) 
                {
                    isGrappling = true;
                }
                if (waveSize > 0)
                {
                    waveSize -= Time.deltaTime * straightenLineSpeed;
                    DrawRopeWaves();
                }
                else 
                {
                    waveSize = 0;
                    DrawRopeNoWaves();
                }
            }
        }

        void DrawRopeWaves() 
        {
            for (int i = 0; i < percision; i++)
            {
                float delta = (float)i / ((float)percision - 1f);
                Vector2 offset = Vector2.Perpendicular(g4GrapplingGun.distanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector2 targetPosition = Vector2.Lerp(g4GrapplingGun.firePoint.position, g4GrapplingGun.grapplePoint, delta) + offset;
                Vector2 currentPosition = Vector2.Lerp(g4GrapplingGun.firePoint.position, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }
        }

        void DrawRopeNoWaves() 
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, g4GrapplingGun.grapplePoint);
            m_lineRenderer.SetPosition(1, g4GrapplingGun.firePoint.position);
        }
    }
}
