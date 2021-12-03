using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Diagnostics;

namespace TcGame
{
    public class TileMap
    {
        // A tile map is a grid or table, where every cell shows a part of a texture

        private Texture m_tileSetTexture;
        private uint m_tileWidth;        // Measured in pixels
        private uint m_tileHeight;       // Measured in pixels

        private uint m_tilesPerRow;      // Number of tiles per row in our grid
        private uint m_tilesPerColumn;   // Number of tiles per column in our grid

        // List of tiles (the size of the list is m_tilesPerRow * m_tilesPerColumn) to draw. This is our canvas.
        //    Every element in this list contains an unsigned integer that represents the index
        //    in m_tileSetTexture of the tile that we want to draw
        // Example:
        //    - We have a m_tileSetTexture with 15 tiles (3 rows and 5 columns)
        //    - We assign an index to every tile, in order. This could be represented as
        //           0,  1,  2,  3,  4,
        //           5,  6,  7,  8,  9,
        //          10, 11, 12, 13, 14
        //    - We can imagine that each number represents a different element (rock, grasp, clay, etc.)
        //      that is visually unique
        //    - Then, we can fill m_tilesToDraw with all the elements we want, from the texture.
        //    - We could think of this as if m_tileSetTexture if our colors palette and m_tilesToDraw is
        //      our canvas.
        //          - The size of the canvas is totally independent of the size of the palette
        //          - We can choose whatever element we have in our palette, in order to paint our canvas
        //    - For example, we could have a m_tilesToDraw with these values:
        //            1, 1, 1, 1,  1,  1,  1, 1
        //            1, 6, 6, 6, 13, 13,  1, 1
        //           10, 1, 6, 1,  1,  6, 13, 1
        //           10, 6, 1, 0,  0, 12,  0, 1
        //    - As you can see, some values are repeated (same palette element), and the size does not need
        //      to be the same as the m_tileSetTexture
        private uint[] m_tilesToDraw;

        private VertexArray m_vertices;
        enum VertexCornerType
        {
            TopLeftCorner,
            TopRightCorner,
            BottomLeftCorner,
            BottomRightCorner
        }

        public TileMap()
        {
        }

        public void Init(string _textureFilename, uint _tileWidth, uint _tileHeight, uint[] _tiles, uint _tilesPerRow, uint _tilesPerColumn)
        {
            m_tileSetTexture = Resources.Texture(_textureFilename);
            m_tileWidth = _tileWidth;
            m_tileHeight = _tileHeight;
            m_tilesToDraw = _tiles;
            m_tilesPerRow = _tilesPerRow;
            m_tilesPerColumn = _tilesPerColumn;

            uint numberOfVertices = m_tilesPerRow * m_tilesPerColumn * 4;
            m_vertices = new VertexArray(PrimitiveType.Quads, numberOfVertices);
            m_vertices.Clear();

            for (uint j = 0; j < m_tilesPerColumn; j++)
            {
                for (uint i = 0; i < m_tilesPerRow; i++)
                {
                    Vertex[] quad = new Vertex[4];
                    
                    quad[0].Position = ComputeTileCornerPosition(VertexCornerType.TopLeftCorner, i, j);
                    quad[1].Position = ComputeTileCornerPosition(VertexCornerType.TopRightCorner, i, j);
                    quad[2].Position = ComputeTileCornerPosition(VertexCornerType.BottomRightCorner, i, j);
                    quad[3].Position = ComputeTileCornerPosition(VertexCornerType.BottomLeftCorner, i, j);

                    quad[0].Color = Color.White;
                    quad[1].Color = Color.White;
                    quad[2].Color = Color.White;
                    quad[3].Color = Color.White;

                    Vector2u texturUV = ComputeTextureUV(i, j);
                    quad[0].TexCoords = ComputeTileCornerPosition(VertexCornerType.TopLeftCorner, texturUV.X, texturUV.Y);
                    quad[1].TexCoords = ComputeTileCornerPosition(VertexCornerType.TopRightCorner, texturUV.X, texturUV.Y);
                    quad[2].TexCoords = ComputeTileCornerPosition(VertexCornerType.BottomRightCorner, texturUV.X, texturUV.Y);
                    quad[3].TexCoords = ComputeTileCornerPosition(VertexCornerType.BottomLeftCorner, texturUV.X, texturUV.Y);

                    m_vertices.Append(quad[0]);
                    m_vertices.Append(quad[1]);
                    m_vertices.Append(quad[2]);
                    m_vertices.Append(quad[3]);
                }
            }
        }

        public void Draw(RenderTarget rt, RenderStates rs)
        {
            rs.Texture = m_tileSetTexture;
            rt.Draw(m_vertices, rs);
        }

        private Vector2f ComputeTileCornerPosition(VertexCornerType _vertexCornerType, uint _column, uint _row)
        {
            Vector2f vertexCornerPosition;
            switch(_vertexCornerType)
            {
                case VertexCornerType.TopLeftCorner:
                    vertexCornerPosition = new Vector2f(_column * m_tileWidth, _row * m_tileHeight);
                    break;
                case VertexCornerType.TopRightCorner:
                    vertexCornerPosition = new Vector2f((_column + 1) * m_tileWidth, _row * m_tileHeight);
                    break;
                case VertexCornerType.BottomLeftCorner:
                    vertexCornerPosition = new Vector2f(_column * m_tileWidth, (_row + 1) * m_tileHeight);
                    break;
                case VertexCornerType.BottomRightCorner:
                    vertexCornerPosition = new Vector2f((_column + 1) * m_tileWidth, (_row + 1) * m_tileHeight);
                    break;
                default:
                    Debug.Assert(false, "Not implemented");
                    vertexCornerPosition = new Vector2f();
                    break;
            }
            return vertexCornerPosition;
        }

        private Vector2u ComputeTextureUV(uint _column, uint _row)
        {
            uint numberOfTilesPerRowInTexture = m_tileSetTexture.Size.X / m_tileWidth;

            uint tileIndex = _column + _row * m_tilesPerRow;
            uint textureIndex = m_tilesToDraw[tileIndex];
            uint textureU = Convert.ToUInt32(textureIndex % numberOfTilesPerRowInTexture);
            uint textureV = Convert.ToUInt32(Math.Floor((double)textureIndex / numberOfTilesPerRowInTexture));

            return new Vector2u(textureU, textureV);
        }
    }
}

