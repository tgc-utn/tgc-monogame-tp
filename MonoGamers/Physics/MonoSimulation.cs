using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGamers.Camera;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using TGC.MonoGame.Samples.Physics.Bepu;
using NumericVector3 = System.Numerics.Vector3;

namespace MonoGamers.Physics;

public class MonoSimulation
{
    private BufferPool BufferPool { get; set; }
    private Simulation Simulation { get; set; }
    private SimpleThreadDispatcher ThreadDispatcher { get; set; }
    
    public Simulation Init()
    {
        BufferPool = new BufferPool();
        
        var targetThreadCount = Math.Max(1,
            Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
        ThreadDispatcher = new SimpleThreadDispatcher(targetThreadCount);
        
        Simulation = Simulation.Create(BufferPool,
            new NarrowPhaseCallbacks(new SpringSettings(80, 1)),
            new PoseIntegratorCallbacks(new NumericVector3(0, -100, 0)),
            new SolveDescription(8, 1));

        return Simulation;
    }

   public void Update()
    {
        Simulation.Timestep(1 / 60f, ThreadDispatcher);
    }
}