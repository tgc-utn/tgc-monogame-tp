using BepuPhysics.Constraints;
using BepuPhysics;
using BepuUtilities.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.Samples.Physics.Bepu;
using NumericVector3 = System.Numerics.Vector3;


namespace TGC.MonoGame.TP.Physics
{
    public class CarSimulation
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
                new NarrowPhaseCallbacks(new SpringSettings(30, 1)),
                new PoseIntegratorCallbacks(new NumericVector3(0, -25, 0)),
                new SolveDescription(8, 1));

            return Simulation;
        }

        public void Update()
        {
            Simulation.Timestep(1 / 60f, ThreadDispatcher);
        }
    }
}
