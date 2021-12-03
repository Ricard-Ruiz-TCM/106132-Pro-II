using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using System;
using SFML.System;
using System.Diagnostics;

namespace TcGame
{
    public class Tank : StaticActor
    {
        private const float SHOOT_FREQUENCY = 0.2f;
        private const float TIME_TO_COOL_DOWN = 2.0f;
        private const float MAX_TIME_SHOOTING = 4.0f;


        private float m_Speed = 200.0f;
        private Vector2f m_Forward = new Vector2f(0, 1);
        private bool m_IsMovingForward = true;

        private Sprite m_Canon;
        private Vector2f m_BaseVector = new Vector2f(0, 1);
        private Vector2f m_CanonForward = new Vector2f(0, 1);
        private float m_CadenceTimer = SHOOT_FREQUENCY;
        private float m_CanonRotationSpeed = 90.0f;
        private float m_CanonRotationDestiny = 0.0f;

        private float m_CooldownTimer = 0.0f;
        private float m_TimeShooting = 0.0f;

        private TankCircuit m_TankCircuit;

        enum State
        {
            ReadyToShoot,
            CoolingDownWeapon
        }
        private State m_State;

        public Tank()
        {
            Layer = ELayer.Middle;

            m_Canon = new Sprite(Resources.Texture("Textures/Player/canon"));
            m_Canon.Origin = new Vector2f(12, 0);

            Sprite = new Sprite(Resources.Texture("Textures/Player/tank"));
            Center();

            m_TankCircuit = new TankCircuit();
            Position = m_TankCircuit.ComputeInitialRandomPosition();

            m_State = State.ReadyToShoot;
        }

        public override void Update(float _dt)
        {
            base.Update(_dt);

            UpdateTimers(_dt);

            if (m_State == State.CoolingDownWeapon)
            {
                UpdateWhenCoolingDownWeapon(_dt);
            } 
            else
            {
                UpdateWhenReadyToShoot(_dt);
            }

            UpdateGameHUD();
        }

        private void UpdateWhenCoolingDownWeapon(float _dt)
        {
            m_TimeShooting -= _dt * 2;
            Rotation += m_Speed * _dt * 1.8f;
            if (m_TimeShooting <= 0.0f) m_State = State.ReadyToShoot;
        }

        private void UpdateWhenReadyToShoot(float _dt)
        {
            CheckInputEvents(_dt);

            m_Forward = m_TankCircuit.ComputeForward(m_IsMovingForward);

            Move(_dt);
            UpdateCanonPositionAndRotation(_dt);

            m_TankCircuit.CheckCircuitLimits(WorldPosition);
            Rotation = m_TankCircuit.ComputeRotation();
        }

        private void CheckInputEvents(float _dt)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                m_IsMovingForward = true;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                m_IsMovingForward = false;
            }


            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (m_CadenceTimer < 0.0f)
                {
                    Shoot();
                    m_CadenceTimer = SHOOT_FREQUENCY;
                }
                
                if(m_TimeShooting <= MAX_TIME_SHOOTING)
                {
                    m_TimeShooting += _dt;
                }
                else
                {
                    m_TimeShooting = MAX_TIME_SHOOTING;
                    m_State = State.CoolingDownWeapon;
                }

            }
            else if(m_TimeShooting > 0.0f)
            {
                m_TimeShooting -= _dt;
            }
        }

        private void UpdateTimers(float _dt)
        {
            if (m_CadenceTimer >= 0.0f)
            {
                m_CadenceTimer -= _dt;
            }
        }

        private void Move(float _dt)
        {
            Position += m_Forward * m_Speed * _dt;
        }

        private void UpdateCanonPositionAndRotation(float _dt)
        {
            // Actualizar position del cañon
            m_Canon.Position = Position;

            m_CanonForward = (Engine.Get.MousePos - m_Canon.Position).Normal();

            if (m_CanonForward.X > 0) m_CanonRotationDestiny = -90 + (float)Math.Atan((m_CanonForward.Y) / m_CanonForward.X) * MathUtil.RAD2DEG;
            else m_CanonRotationDestiny = 180 - 90 + (float)Math.Atan((m_CanonForward.Y) / m_CanonForward.X) * MathUtil.RAD2DEG;

            if (m_Canon.Rotation < m_CanonRotationDestiny) m_Canon.Rotation += m_CanonRotationSpeed * _dt;
            if (m_Canon.Rotation > m_CanonRotationDestiny) m_Canon.Rotation -= m_CanonRotationSpeed * _dt;
            
        }

        private void Shoot()
        {
            Bullet bullet = Engine.Get.Scene.Create<Bullet>();

            bullet.m_Forward = -bullet.m_Forward.Rotate(m_Canon.Rotation);
            bullet.Position = Position + bullet.m_Forward * 60;
            bullet.Rotation = m_Canon.Rotation;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            states.Transform = Transform.Identity;
            m_Canon.Draw(target, states);
        }

        private void UpdateGameHUD()
        {
            float warmingRatio = (m_TimeShooting / MAX_TIME_SHOOTING);
            float coolingDownRatio = 1.0f - (m_CooldownTimer / TIME_TO_COOL_DOWN);

            MyGame.Get.GameHUD.UpdateWarmWeaponBar(warmingRatio);
        }
    }
}

