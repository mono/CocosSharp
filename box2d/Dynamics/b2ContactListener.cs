using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Dynamics.Contacts;
using Box2D.Collision;

namespace Box2D.Dynamics
{
    /// <summary>
    /// Implement this class to get contact information. You can use these results for
    /// things like sounds and game logic. You can also get contact results by
    /// traversing the contact lists after the time step. However, you might miss
    /// some contacts because continuous physics leads to sub-stepping.
    /// Additionally you may receive multiple callbacks for the same contact in a
    /// single time step.
    /// You should strive to make your callbacks efficient because there may be
    /// many callbacks per time step.
    /// @warning You cannot create/destroy Box2D entities inside these callbacks.    /// </summary>
    public abstract class b2ContactListener
    {
        /// <summary>
        /// Called when two fixtures begin to touch.
        /// </summary>
        public virtual void BeginContact(b2Contact contact) { }

        /// <summary>
        /// Called when two fixtures cease to touch.
        /// </summary>
        public virtual void EndContact(b2Contact contact) { }

        /// <summary>
        /// This is called after a contact is updated. This allows you to inspect a
        /// contact before it goes to the solver. If you are careful, you can modify the
        /// contact manifold (e.g. disable contact).
        /// A copy of the old manifold is provided so that you can detect changes.
        /// Note: this is called only for awake bodies.
        /// Note: this is called even when the number of contact points is zero.
        /// Note: this is not called for sensors.
        /// Note: if you set the number of contact points to zero, you will not
        /// get an EndContact callback. However, you may get a BeginContact callback
        /// the next step.
        /// </summary>
        public abstract void PreSolve(b2Contact contact, b2Manifold oldManifold);

        /// <summary>
        /// This lets you inspect a contact after the solver is finished. This is useful
        /// for inspecting impulses.
        /// Note: the contact manifold does not include time of impact impulses, which can be
        /// arbitrarily large if the sub-step is small. Hence the impulse is provided explicitly
        /// in a separate data structure.
        /// Note: this is only called for contacts that are touching, solid, and awake.
        /// </summary>
        public abstract void PostSolve(b2Contact contact, b2ContactImpulse impulse);

        public static b2ContactListener b2_defaultListener;
    }
}
