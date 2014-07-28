using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    // Orbits the camera around the center of the screen using spherical coordinates
    public class CCOrbitCamera : CCActionCamera
    {
        #region Properties

        public float AngleX { get; private set; }
        public float AngleZ { get; private set; }
        public float DeltaAngleX { get; private set; }
        public float DeltaAngleZ { get; private set; }
        public float DeltaRadius { get; private set; }
        public float Radius { get; private set; }

        #endregion Properties


        #region Constructors

        public CCOrbitCamera (float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX,
            float deltaAngleX) : base (t)
        {
            Radius = radius;
            DeltaRadius = deltaRadius;
            AngleZ = angleZ;
            DeltaAngleZ = deltaAngleZ;
            AngleX = angleX;
            DeltaAngleX = deltaAngleX;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCOrbitCameraState (this, target);
        }

    }

    internal class CCOrbitCameraState : CCActionCameraState
    {

        #region Properties

        protected float AngleX { get; set; }
        protected float AngleZ { get; set; }
        protected float DeltaAngleX { get; set; }
        protected float DeltaAngleZ { get; set; }
        protected float DeltaRadius { get; set; }
        protected float RadDeltaX  { get; set; }
        protected float RadDeltaZ { get; set; }
        protected float RadX { get; set; }
        protected float RadZ { get; set; }
        protected float Radius { get; set; }

        #endregion Properties


        #region Constructors

        public CCOrbitCameraState (CCOrbitCamera action, CCNode target)
            : base (action, target)
        {       
            AngleX = action.AngleX;
            AngleZ = action.AngleZ;
            DeltaAngleX = action.DeltaAngleX;
            DeltaAngleZ = action.DeltaAngleZ;
            DeltaRadius = action.DeltaRadius;
            Radius = action.Radius;

            // We are assuming the local camera will be centered directly at the target
            // => Add radius to the z position of local camera
            CCPoint3 fauxLocalCameraCenter = target.FauxLocalCameraCenter;
            fauxLocalCameraCenter.Z += Radius;
            target.FauxLocalCameraCenter = fauxLocalCameraCenter;

            //CameraCenter = fauxLocalCameraCenter;

            RadDeltaZ = CCMacros.CCDegreesToRadians (DeltaAngleZ);
            RadDeltaX = CCMacros.CCDegreesToRadians (DeltaAngleX);

            // Only calculate the SpericalRadius when needed.
            if (float.IsNaN (Radius) || float.IsNaN (AngleZ) || float.IsNaN (AngleX))
            {
                float r, zenith, azimuth;
                SphericalRadius (out r, out zenith, out azimuth);

                if (float.IsNaN (Radius))
                    Radius = r;

                if (float.IsNaN (AngleZ))
                    AngleZ = CCMacros.CCRadiansToDegrees (zenith);

                if (float.IsNaN (AngleX))
                    AngleX = CCMacros.CCRadiansToDegrees (azimuth);
            }

            RadZ = CCMacros.CCDegreesToRadians (AngleZ);
            RadX = CCMacros.CCDegreesToRadians (AngleX);
        }

        #endregion Constructors


        void SphericalRadius (out float newRadius, out float zenith, out float azimuth)
        {
            float ex, ey, ez, cx, cy, cz, x, y, z;
            float r; // radius
            float s;

            CCPoint3 cameraCenter = CameraCenter;
            CCPoint3 cameraTarget = CameraTarget;

            ex = cameraTarget.X;
            ey = cameraTarget.Y;
            ez = cameraTarget.Z;

            cx = cameraCenter.X;
            cy = cameraCenter.Y;
            cz = cameraCenter.Z;

            x = ex - cx;
            y = ey - cy;
            z = ez - cz;

            r = (float)Math.Sqrt (x * x + y * y + z * z);
            s = (float)Math.Sqrt (x * x + y * y);
            if (s == 0.0f)
                s = float.Epsilon;
            if (r == 0.0f)
                r = float.Epsilon;
                
            zenith = (float)Math.Acos (z / r);
            if (x < 0)
                azimuth = (float)Math.PI - (float)Math.Sin (y / s);
            else
                azimuth = (float)Math.Sin (y / s);

            newRadius = r;
        }

        public override void Update(float time)
        {
            float r = (Radius + DeltaRadius * time);
            float za = RadZ + RadDeltaZ * time;
            float xa = RadX + RadDeltaX * time;

            float i = (float)Math.Sin (za) * (float)Math.Cos (xa) * r + CameraCenter.X;
            float j = (float)Math.Sin (za) * (float)Math.Sin (xa) * r + CameraCenter.Y;
            float k = (float)Math.Cos (za) * r + CameraCenter.Z;

            Target.FauxLocalCameraCenter = new CCPoint3(i, j, k);
        }
    }
}