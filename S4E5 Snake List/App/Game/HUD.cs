using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using SFML.System;

namespace Snake
{
    public class HUD : Transformable, Drawable
    {
        private int puntos;
        Font f;
        Text t;

        // Variable para la interfaz, icono de reciclaje
        Sprite mRecTexture;

        // Varible de sonido para el efecto del icono de reciclaje
        Sound mRecSound;
        
        // Variable de estado
        // true -> esta girando
        // false -> no esta girando
        bool mRecRolling;

        public HUD()
        {
            puntos = 0;
            f = new Font("Data/arial.ttf");
            t = new Text("Puntos: ", f);

            // Cargamos la textura y hacaemos set del Origin y posición
            mRecTexture = new Sprite(new Texture("Data/repos.png"));
            mRecTexture.Origin = new Vector2f(mRecTexture.GetLocalBounds().Width / 2.0f, mRecTexture.GetLocalBounds().Height / 2.0f);
            mRecTexture.Position = new Vector2f(590, 50); mRecTexture.Scale *= 0.20f; mRecTexture.Rotation = 360;

            // Cargamos el sonido y lo dejamos listo
            mRecSound = new Sound(new SoundBuffer("Data/swap.wav"));

            // set de la variable de es tado
            mRecRolling = false;
        }

        public void Update(float dt)
        {
            t.DisplayedString = String.Format("Puntos: {0}", puntos);

            // Hacemos la rotación del icono de reciclaje
            if (mRecTexture.Rotation < 359) mRecTexture.Rotation += 5;
            else mRecRolling = false;
        }

        public void Draw(RenderTarget rt, RenderStates st)
        {
            rt.Draw(t);
            rt.Draw(mRecTexture);
        }
        public void ScoreAdd()
        {
            puntos++;
        }

        // newPosAnimation
        // ****************
        // Método para realizar la animación del simbolo de reciclaje
        public void newPosAnimation()
        {
            if (!mRecRolling)
            {
                mRecRolling = true;
                mRecTexture.Rotation = 0;
                mRecSound.Play();

            }
        }
    }
}

