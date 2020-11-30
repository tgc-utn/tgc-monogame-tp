using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chinchulines.Entities
{
    public class LaserManager
    {
        private const int DELAY_BETWEEN_LASERS = 1000;
        private const float LASER_SPEED = 5.0f;

        private List<LaserStruct> _bulletList = new List<LaserStruct>();
        private double _lastBulletTime = 0;
        private Texture2D _texture;
        private Effect _effect;

        private SoundEffect shot;

        struct LaserStruct
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        public void LoadContent(string texturePath, string effect, ContentManager Content, GraphicsDeviceManager graphics)
        {
            _texture = Content.Load<Texture2D>(texturePath);
            _effect = Content.Load<Effect>(effect);

            shot = Content.Load<SoundEffect>("Music/shot");
        }

        public void ShootLaser(GameTime gameTime, Vector3 shipPosition, Quaternion shipRotation)
        {
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (currentTime - _lastBulletTime > DELAY_BETWEEN_LASERS)
            {
                LaserStruct newBullet = new LaserStruct();
                newBullet.position = shipPosition;
                newBullet.rotation = shipRotation;
                _bulletList.Add(newBullet);

                _lastBulletTime = currentTime;

                shot.Play();
            }
        }

        public CollisionType UpdateLaserAndCheckCollision(float moveSpeed, Vector3 spaceshipPosition, int spaceshipHealth)
        {
            for (int i = 0; i < _bulletList.Count; i++)
            {
                LaserStruct currentBullet = _bulletList[i];
                MoveForward(ref currentBullet.position, currentBullet.rotation, moveSpeed * LASER_SPEED);
                _bulletList[i] = currentBullet;

                BoundingSphere bulletSphere = new BoundingSphere(currentBullet.position, 0.05f);

                CollisionType colType = CheckCollision(bulletSphere, spaceshipPosition);
                if (colType != CollisionType.None)
                {
                    _bulletList.RemoveAt(i);
                    i--;

                    return CollisionType.Laser;
                }
            }

            return CollisionType.None;
        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
            position += addVector * speed;
        }

        public void DrawLasers(Matrix view, Matrix projection, Vector3 cameraPosition, Vector3 cameraDirection, GraphicsDeviceManager graphics)
        {
            if (_bulletList.Count > 0)
            {
                VertexPositionTexture[] bulletVertices = new VertexPositionTexture[_bulletList.Count * 6];
                int i = 0;

                foreach (LaserStruct currentBullet in _bulletList)
                {
                    Vector3 center = currentBullet.position;

                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(1, 1));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(0, 0));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(1, 0));

                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(1, 1));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(0, 1));
                    bulletVertices[i++] = new VertexPositionTexture(center, new Vector2(0, 0));
                }

                _effect.CurrentTechnique = _effect.Techniques["PointSprites"];
                _effect.Parameters["xWorld"].SetValue(Matrix.Identity);
                _effect.Parameters["xProjection"].SetValue(projection);
                _effect.Parameters["xView"].SetValue(view);
                _effect.Parameters["xCamPos"].SetValue(cameraPosition);
                _effect.Parameters["xTexture"].SetValue(_texture);
                _effect.Parameters["xCamUp"].SetValue(cameraDirection);
                _effect.Parameters["xPointSpriteSize"].SetValue(0.1f);

                graphics.GraphicsDevice.BlendState = BlendState.Additive;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bulletVertices, 0, _bulletList.Count * 2);
                }
                graphics.GraphicsDevice.BlendState = BlendState.Opaque;

            }

        }

        private CollisionType CheckCollision(BoundingSphere laserSphere, Vector3 spaceshipPosition)
        {
            BoundingSphere spaceshipSphere = new BoundingSphere(spaceshipPosition, 0.04f);

            if (laserSphere.Intersects(spaceshipSphere))
                return CollisionType.Laser;

            return CollisionType.None;
        }
    }
}
