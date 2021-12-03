using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TcGame
{
    public class HUD : StaticActor
    {
        private Text m_KilledEnemiesUIText;
        private Text m_KilledEnemiesCountUIText;
        private string m_KilledEnemiesTitle;
        private uint m_KilledEnemiesCount = 0;

        private const float BLINK_DURATION = 0.2f;
        private bool m_BlinkEffectActivated = false;
        private float m_blinkTimer = 0.0f;

        private RectangleShape m_WarmWeaponUIExteriorBar;
        private RectangleShape m_WarmWeaponUIRatioBar;

        public HUD()
        {
            Layer = ELayer.HUD;

            m_KilledEnemiesTitle = "Slaugthered enemies:   ";
            m_KilledEnemiesCount = 0;

            const uint characterSize = 30;
            const float outlineThickness = 2.0f;
            const float leftOffset = 30.0f;

            m_KilledEnemiesUIText = new Text(m_KilledEnemiesTitle, Resources.Font("Fonts/LuckiestGuy"));
            m_KilledEnemiesUIText.CharacterSize = characterSize;
            m_KilledEnemiesUIText.OutlineThickness = outlineThickness;
            m_KilledEnemiesUIText.Position = new Vector2f(30.0f, 30.0f);

            m_KilledEnemiesCountUIText = new Text(m_KilledEnemiesCount.ToString(), Resources.Font("Fonts/LuckiestGuy"));
            m_KilledEnemiesCountUIText.CharacterSize = characterSize;
            m_KilledEnemiesCountUIText.OutlineThickness = outlineThickness;
            m_KilledEnemiesCountUIText.Position = new Vector2f(leftOffset + m_KilledEnemiesUIText.GetLocalBounds().Width, 30.0f);

            UpdateKilledEnemies();

            m_WarmWeaponUIExteriorBar = new RectangleShape(new Vector2f(200.0f, 20.0f));
            m_WarmWeaponUIExteriorBar.Position = new Vector2f(Engine.Get.ViewportSize.X - m_WarmWeaponUIExteriorBar.GetLocalBounds().Width - leftOffset, 40.0f);
            m_WarmWeaponUIExteriorBar.FillColor = Color.White;
            m_WarmWeaponUIExteriorBar.OutlineColor = Color.Black;
            m_WarmWeaponUIExteriorBar.OutlineThickness = outlineThickness;

            m_WarmWeaponUIRatioBar = new RectangleShape();
            m_WarmWeaponUIRatioBar.Position = m_WarmWeaponUIExteriorBar.Position;
            m_WarmWeaponUIRatioBar.FillColor = Color.Yellow;

            UpdateWarmWeaponBar(0.0f);
        }


        public override void Update(float _dt)
        {
            base.Update(_dt);

            if(m_BlinkEffectActivated)
            {
                UpdateBlinkEffect(_dt);
            }
        }

        public override void Draw(RenderTarget rt, RenderStates rs)
        {
            base.Draw(rt, rs);

            rt.Draw(m_KilledEnemiesUIText, rs);
            rt.Draw(m_KilledEnemiesCountUIText, rs);
            rt.Draw(m_WarmWeaponUIExteriorBar, rs);
            rt.Draw(m_WarmWeaponUIRatioBar, rs);
        }

        private void UpdateKilledEnemies()
        {
            Debug.Assert(m_KilledEnemiesCountUIText != null);
            m_KilledEnemiesCountUIText.DisplayedString = m_KilledEnemiesCount.ToString();
            StartBlinkEffect();
        }

        public void OnEnemyKilled()
        {
            m_KilledEnemiesCount++;
            UpdateKilledEnemies();
        }

        private void StartBlinkEffect()
        {
            m_BlinkEffectActivated = true;
            m_blinkTimer = BLINK_DURATION;
        }

        private void UpdateBlinkEffect(float _dt)
        {
            const float HALF_PI = MathF.PI * 0.5f;
            float angle = MathUtil.Lerp(-HALF_PI, HALF_PI, m_blinkTimer / BLINK_DURATION);
            float alphaValue = 1.0f - MathF.Cos(angle);
            byte alphaColor = (byte)(alphaValue * (float)Byte.MaxValue);

            Color textColor = m_KilledEnemiesCountUIText.FillColor;
            textColor.A = alphaColor;

            Color outlineColor = m_KilledEnemiesCountUIText.OutlineColor;
            outlineColor.A = alphaColor;

            m_KilledEnemiesCountUIText.FillColor = textColor;
            m_KilledEnemiesCountUIText.OutlineColor = outlineColor;

            m_blinkTimer -= _dt;
            if( m_blinkTimer <= 0.0f)
            {
                m_BlinkEffectActivated = false;
            }
        }

        public void UpdateWarmWeaponBar(float _warmingRatio)
        {
            _warmingRatio = Math.Clamp(_warmingRatio, 0.0f, 1.0f);

            float maxBarSize = m_WarmWeaponUIExteriorBar.Size.X;
            float newBarSize = MathUtil.Lerp(0.0f, maxBarSize, _warmingRatio);

            m_WarmWeaponUIRatioBar.Size = new Vector2f(newBarSize, m_WarmWeaponUIExteriorBar.Size.Y);
            Color barColor = m_WarmWeaponUIRatioBar.FillColor;
            barColor.G = (byte)((1.0f - _warmingRatio) * Byte.MaxValue);
            m_WarmWeaponUIRatioBar.FillColor = barColor;
        }

    }
}

