using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using System;
using SFML.System;
using System.Diagnostics;

namespace TcGame
{
    public class TankCircuit
    {
        // Miembros del circuito
        private float m_circuitTopPosition;
        private float m_circuitLeftPosition;
        private int m_circuitWidth;
        private int m_circuitHeight;

        enum CircuitLines
        {
            TopLine,
            RightLine,
            BottomLine,
            LeftLine
        }

        private CircuitLines m_currentLine;

        public TankCircuit()
        {
            SetupCircuit();
        }

        public Vector2f ComputeForward(bool _isMovingForward)
        {
            // Por defecto, siempre nos movemos en sentido contrario a las agujas del reloj. Todo depende de la variable m_isMovingForward
            switch (m_currentLine)
            {
                case CircuitLines.TopLine:
                    return new Vector2f(_isMovingForward ? -1f : 1f, 0f);
                case CircuitLines.RightLine:
                    return new Vector2f(0f, _isMovingForward ? -1f : 1f);
                case CircuitLines.BottomLine:
                    return new Vector2f(_isMovingForward ? 1f : -1f, 0f);
                case CircuitLines.LeftLine:
                    return new Vector2f(0f, _isMovingForward ? 1f : -1f);
            }
            return new Vector2f(0.0f, 0.0f);
        }

        public float ComputeRotation()
        {
            switch (m_currentLine)
            {
                case CircuitLines.TopLine:
                    return 90;
                case CircuitLines.RightLine:
                    return 180;
                case CircuitLines.BottomLine:
                    return -90;
                case CircuitLines.LeftLine:
                    return 0;
            }
            return 0;
        }

        public Vector2f ComputeInitialRandomPosition()
        {
            float xPos = 0.0f;
            float yPos = 0.0f;

            int currentLine = Engine.Get.random.Next(4);
            switch (currentLine)
            {
                case 0:
                    // Top line
                    yPos = m_circuitTopPosition;
                    xPos = Engine.Get.random.Next(m_circuitWidth) + m_circuitLeftPosition;
                    ChangeCircuitLine(CircuitLines.TopLine);
                    break;
                case 1:
                    // Right line
                    yPos = Engine.Get.random.Next(m_circuitHeight) + m_circuitTopPosition;
                    xPos = m_circuitLeftPosition + m_circuitWidth;
                    ChangeCircuitLine(CircuitLines.RightLine);
                    break;
                case 2:
                    // Bottom line
                    yPos = m_circuitTopPosition + m_circuitHeight;
                    xPos = Engine.Get.random.Next(m_circuitWidth) + m_circuitLeftPosition;
                    ChangeCircuitLine(CircuitLines.BottomLine);
                    break;
                case 3:
                    // Left line
                    yPos = Engine.Get.random.Next(m_circuitHeight) + m_circuitTopPosition;
                    xPos = m_circuitLeftPosition;
                    ChangeCircuitLine(CircuitLines.LeftLine);
                    break;
            }

            return new Vector2f(xPos, yPos);
        }

        public void CheckCircuitLimits(Vector2f _actorPosition)
        {
            switch (m_currentLine)
            {
                case CircuitLines.TopLine:
                    CheckCircuitLineLimits(_actorPosition.X <= m_circuitLeftPosition, _actorPosition.X >= m_circuitLeftPosition + m_circuitWidth);
                    break;
                case CircuitLines.RightLine:
                    CheckCircuitLineLimits(_actorPosition.Y <= m_circuitTopPosition, _actorPosition.Y >= m_circuitTopPosition + m_circuitHeight);
                    break;
                case CircuitLines.BottomLine:
                    CheckCircuitLineLimits(_actorPosition.X >= m_circuitLeftPosition + m_circuitWidth, _actorPosition.X <= m_circuitLeftPosition);
                    break;
                case CircuitLines.LeftLine:
                    CheckCircuitLineLimits(_actorPosition.Y >= m_circuitTopPosition + m_circuitHeight, _actorPosition.Y <= m_circuitTopPosition);
                    break;
            }
        }

        private void SetupCircuit()
        {
            m_circuitWidth = 8 * 64 - 64;
            m_circuitHeight = 5 * 64 - 64;

            Vector2f viewportSize = Engine.Get.ViewportSize;
            m_circuitTopPosition = (viewportSize.Y - m_circuitHeight) * 0.5f;
            m_circuitLeftPosition = (viewportSize.X - m_circuitWidth) * 0.5f;
        }

        private void CheckCircuitLineLimits(bool _moveToNextLineCondition, bool _moveToPreviousLineCondition)
        {
            if (_moveToNextLineCondition)
            {
                MoveToNextCircuitLine();
            }
            else if (_moveToPreviousLineCondition)
            {
                MoveToPreviousCircuitLine();
            }
        }

        private void MoveToNextCircuitLine()
        {
            switch (m_currentLine)
            {
                case CircuitLines.TopLine:
                    ChangeCircuitLine(CircuitLines.LeftLine);
                    break;
                case CircuitLines.RightLine:
                    ChangeCircuitLine(CircuitLines.TopLine);
                    break;
                case CircuitLines.BottomLine:
                    ChangeCircuitLine(CircuitLines.RightLine);
                    break;
                case CircuitLines.LeftLine:
                    ChangeCircuitLine(CircuitLines.BottomLine);
                    break;
            }
        }

        private void MoveToPreviousCircuitLine()
        {
            switch (m_currentLine)
            {
                case CircuitLines.TopLine:
                    ChangeCircuitLine(CircuitLines.RightLine);
                    break;
                case CircuitLines.RightLine:
                    ChangeCircuitLine(CircuitLines.BottomLine);
                    break;
                case CircuitLines.BottomLine:
                    ChangeCircuitLine(CircuitLines.LeftLine);
                    break;
                case CircuitLines.LeftLine:
                    ChangeCircuitLine(CircuitLines.TopLine);
                    break;
            }
        }

        private void ChangeCircuitLine(CircuitLines _newCircuitLine)
        {
            m_currentLine = _newCircuitLine;
        }
    }
}

