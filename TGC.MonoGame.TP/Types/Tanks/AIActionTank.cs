using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Maps;

namespace TGC.MonoGame.TP.Types.Tanks;

public class AIActionTank : ActionTank
{
    public bool perseguir = false;
    private const float VELOCIDAD_MAX = 0.03f;
    private int PathIndex = 0;
    public float BotNum;
    public Map PlaneMap;
    public Tank Objective;
    public bool hasObjective = false;
    public List<Tank> PossibleObjectives;
    private float angle = 0f;

    public AIActionTank(bool isAEnemy, int Index, Map plane)
    {
        PlaneMap = plane;
        BotNum = Index;
        isEnemy = isAEnemy;
    }

    public override void Update(GameTime gameTime, Tank tank)
    {
        var elapsedTime = (float)gameTime.ElapsedGameTime.Milliseconds;
        List<Vector3> paths = new List<Vector3>();
        Vector3 positionXZ = new Vector3(tank.Position.X, 0f, tank.Position.Z);

        if (tank.health <= 0)
        {
            tank.Respawn();
        }

        if (hasObjective && Objective.health <= 0)
        {
            hasObjective = false;
        }

        if (isEnemy)
        {
            paths.Add(new Vector3(-225f, 0f, (float)(3.5 + BotNum * 3)));
            if (BotNum % 2 == 0)
            {
                paths.Add(new Vector3(-225f, 0f, 273f));
                paths.Add(new Vector3(-145f, 0f, 273 + BotNum * 3));
            }
            else
            {
                paths.Add(new Vector3(-225f, 0f, -273f));
                paths.Add(new Vector3(-145f, 0f, (-273 + BotNum * 3)));
            }
        }

        if (!isEnemy)
        {
            paths.Add(new Vector3(334f, 0f, (float)(11 + BotNum * 3)));
            if (BotNum % 2 == 0)
            {
                paths.Add(new Vector3(179f, 0f, 340f));
                paths.Add(new Vector3(160f, 0f, 263 + BotNum * 3));
            }
            else
            {
                paths.Add(new Vector3(293f, 0f, -324f));
                paths.Add(new Vector3(160f, 0f, (-250 + BotNum * 3)));
            }
        }

        if (!perseguir)
        {
            Vector3 direction;
            if (isEnemy)
            {
                direction = paths[PathIndex] - positionXZ;
            }
            else
            {
                direction = paths[PathIndex] - positionXZ;
            }

            Vector3 forward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(tank.Angle));


            if (Math.Round(direction.Length()) != 0)
            {
                angle = MathF.Acos(Vector3.Dot(direction, forward) / (direction.Length() * forward.Length()));
            }
            else
            {
                PathIndex++;
                if (PathIndex == 3)
                {
                    perseguir = true;
                }
            }
        }
        else
        {
            if (!hasObjective)
            {
                if (isEnemy)
                {
                    PossibleObjectives = PlaneMap.Tanks.Where(tank => tank.Action.isEnemy == false).ToList();
                    PossibleObjectives.Add(PlaneMap.Player);
                    PossibleObjectives = PossibleObjectives.FindAll(tank => tank.health != 0);
                }
                else
                {
                    PossibleObjectives = PlaneMap.Tanks.Where(tank => tank.Action.isEnemy).ToList();
                }

                Random r = new Random();

                Objective = PossibleObjectives[r.Next(PossibleObjectives.Count)];

                hasObjective = true;
            }

            Vector3 objectiveXZ = new Vector3(Objective.Position.X, 0f, Objective.Position.Z);

            Vector3 direction = objectiveXZ - positionXZ;

            Vector3 forward = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(tank.Angle));

            if (Math.Round(direction.Length()) != 0)
            {
                angle = MathF.Acos(Vector3.Dot(direction, forward) / (direction.Length() * forward.Length()));
            }
        }

        Matrix Rotation;
        if (!isEnemy)
        {
            Rotation = Matrix.CreateRotationY(angle);
        }
        else
        {
            Rotation = Matrix.CreateRotationY(-angle);
        }


        tank.Position += Vector3.Transform(Vector3.Forward, Rotation) * VELOCIDAD_MAX * elapsedTime;
        var Traslation = Matrix.CreateTranslation(tank.Position);
        tank.World = Matrix.CreateScale(tank.Reference.Scale) * tank.Reference.Rotation * Rotation * Traslation;

        if (!tank.hasShot && perseguir)
        {
            var bulletPosition = tank.Position; //TODO por ahi es la position del cannon
            var yawRadians = MathHelper.ToRadians(tank.yaw);
            var pitchRadians = MathHelper.ToRadians(tank.pitch);
            var bulletDirection = Vector3.Transform(
                Vector3.Transform(
                    tank.cannonBone.Transform.Forward,
                    Matrix.CreateFromYawPitchRoll(yawRadians, pitchRadians, 0f)
                ),
                Matrix.CreateRotationY(tank.Angle));
            var bullet = new Bullet(
                tank.BulletModel,
                tank.BulletEffect,
                tank.BulletReference,
                Matrix.CreateFromYawPitchRoll(yawRadians, -pitchRadians, 0f),
                Matrix.CreateRotationY(tank.Angle),
                bulletPosition,
                bulletDirection);
            tank.Bullets.Add(bullet);
            tank.hasShot = true;
            tank.shootTime = 1.25f;
        }
    }

    public override void Respawn(Tank tank)
    {
        PathIndex = 0;
        perseguir = false;
    }
}