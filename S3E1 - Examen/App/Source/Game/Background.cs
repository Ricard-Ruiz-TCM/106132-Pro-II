using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace TcGame
{
    public class Background : Actor
    {
        private TileMap m_TileMap;

        private uint[] m_TileMapData =
        {
            0, 0, 0, 0, 0, 0, 7, 20, 20, 20, 20, 20,
            0, 0, 0, 0, 0, 0, 7, 20, 20, 20, 20, 20,
            0, 0,13, 2, 2, 2, 27, 22, 22,34, 20, 20,
            0, 0, 1, 0, 0, 0, 7, 20, 20, 21, 20, 20,
            0, 0, 1, 0, 0, 0, 7, 20, 20, 21, 20, 20,
            0, 0, 1, 0, 0, 0, 7, 20, 20, 21, 20, 20,
            0, 0,15, 2, 2, 2, 27, 22, 22,36, 20, 20,
            0, 0, 0, 0, 0, 0, 7, 20, 20, 20, 20, 20,
            0, 0, 0, 0, 0, 0, 7, 20, 20, 20, 20, 20
        };

        public Background()
        {
            const uint tileWidth = 64;
            const uint tileHeight = 64;
            const uint tilePerRow = 12;
            const uint tilesPerColumn = 9;

            Layer = ELayer.Background;

            m_TileMap = new TileMap();

            m_TileMap.Init("Textures/tilesheet", tileWidth, tileHeight, m_TileMapData, tilePerRow, tilesPerColumn);                 // Esta linea pinta el tile map de forma normal
            //m_TileMap.Init("Textures/tilesheet_withIndexes", tileWidth, tileHeight, m_TileMapData, tilePerRow, tilesPerColumn);   // Esta linea pinta el tile map con informacion de debug

            Engine.Get.ViewportSize = new Vector2f(tilePerRow * tileWidth, tilesPerColumn * tileHeight);                // Update the viewport size
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            m_TileMap.Draw(target, states);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }
    }
}

