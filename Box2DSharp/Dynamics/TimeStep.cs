namespace Box2DSharp.Dynamics
{
    /// This is an internal structure.
    public struct TimeStep
    {
        /// time step
        public float Dt;
        /// inverse time step (0 if dt == 0).
        public float InvDt;
        /// dt * inv_dt0
        public float DtRatio;

        public int VelocityIterations;

        public int PositionIterations;

        public bool WarmStarting;
    }
}