using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.InsaneGames.Obstacles;

namespace TGC.MonoGame.InsaneGames.Utils
{
    public static class ObstaclesBuilder
    {
        public static List<BoxObstacle> ObtainStackedBoxesObstacles(int quantity, Matrix initialPosition)
        {

            // add first box
            List<BoxObstacle> boxes = new List<BoxObstacle>();
            BoxObstacle initialBox = new BoxObstacle(initialPosition);
            boxes.Add(initialBox);

            float translationY = 25;

            // add next boxes
            for (int i = 1; i < quantity; i++)
            {
                Matrix position = initialPosition * Matrix.CreateTranslation(0, i * translationY, 0);
                BoxObstacle box = new BoxObstacle(position);
                boxes.Add(box);
            }
            return boxes;
        }

        public static List<BoxObstacle> ObtainBoxesObstaclesInLine(int quantity, Matrix initialPosition, bool inX)
        {
            // Verify if is in x axis or z axis
            float translationX = 0;
            float translationZ = 0;

            if (inX)
                translationX = 25;
            else
                translationZ = 25;

            // add first box
            List<BoxObstacle> boxes = new List<BoxObstacle>();
            BoxObstacle initialBox = new BoxObstacle(initialPosition);
            boxes.Add(initialBox);

            // add next boxes
            for (int i = 1; i < quantity; i++)
            {
                Matrix position = initialPosition * Matrix.CreateTranslation(i * translationX, 0, i * translationZ);
                BoxObstacle box = new BoxObstacle(position);
                boxes.Add(box);
            }
            return boxes;
        }

        public static List<Barrier> ObtainBarriersObstaclesInLine(int quantity, Matrix initialPosition, bool inX)
        {
            // Verify if is in x axis or z axis
            float translationX = 0;
            float translationZ = 0;

            if (inX)
                translationX = 40;
            else
                translationZ = 40;

            // add first barrier
            List<Barrier> barriers = new List<Barrier>();
            Barrier initialBarrier = new Barrier(initialPosition);
            barriers.Add(initialBarrier);

            // add next barriers
            for (int i = 1; i < quantity; i++)
            {
                Matrix position = initialPosition * Matrix.CreateTranslation(i * translationX, 0, i * translationZ);
                Barrier barrier = new Barrier(position);
                barriers.Add(barrier);
            }
            return barriers;
        }

        public static List<Cone> ObtainConesObstaclesInLine(int quantity, Matrix initialPosition, bool inX)
        {
            // Verify if is in x axis or z axis
            float translationX = 0.0f;
            float translationZ = 0;

            if (inX)
                translationX = 15;
            else
                translationZ = 15;

            // add first cone
            List<Cone> cones = new List<Cone>();
            Cone initialCone = new Cone(initialPosition);
            cones.Add(initialCone);

            // add next cones
            for (int i = 1; i < quantity; i++)
            {
                Matrix position = initialPosition * Matrix.CreateTranslation(i * translationX, 0, i * translationZ);
                Cone cone = new Cone(position);
                cones.Add(cone);
            }
            return cones;
        }

        public static List<Sawhorse> ObtainSawhorsesObstaclesInLine(int quantity, Matrix initialPosition, bool inX)
        {
            // Verify if is in x axis or z axis
            float translationX = 0;
            float translationZ = 0;

            if (inX)
                translationX = 75;
            else
                translationZ = 75;

            // add first sawhorse
            List<Sawhorse> sawhorses = new List<Sawhorse>();
            Sawhorse initialSawhorse = new Sawhorse(initialPosition);
            sawhorses.Add(initialSawhorse);

            // add next boxes
            for (int i = 1; i < quantity; i++)
            {
                Matrix position = initialPosition * Matrix.CreateTranslation(i * translationX, 0, i * translationZ);
                Sawhorse sawhorse = new Sawhorse(position);
                sawhorses.Add(sawhorse);
            }
            return sawhorses;
        }

    }
}
