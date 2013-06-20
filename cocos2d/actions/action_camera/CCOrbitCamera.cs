using System;

namespace Cocos2D
{
    /// <summary>
    /// CCOrbitCamera action
    /// Orbits the camera around the center of the screen using spherical coordinates
    /// </summary>
    public class CCOrbitCamera : CCActionCamera
    {
        protected float m_fAngleX;
        protected float m_fAngleZ;
        protected float m_fDeltaAngleX;
        protected float m_fDeltaAngleZ;
        protected float m_fDeltaRadius;
        protected float m_fRadDeltaX;
        protected float m_fRadDeltaZ;
        protected float m_fRadX;
        protected float m_fRadZ;
        protected float m_fRadius;

        public CCOrbitCamera()
        {
            m_fRadius = 0.0f;
            m_fDeltaRadius = 0.0f;
            m_fAngleZ = 0.0f;
            m_fDeltaAngleZ = 0.0f;
            m_fAngleX = 0.0f;
            m_fDeltaAngleX = 0.0f;
            m_fRadZ = 0.0f;
            m_fRadDeltaZ = 0.0f;
            m_fRadX = 0.0f;
            m_fRadDeltaX = 0.0f;
        }

        protected CCOrbitCamera(CCOrbitCamera copy) : base(copy)
        {
            Init(copy.m_fRadius, copy.m_fDeltaRadius, copy.m_fAngleZ, copy.m_fDeltaAngleZ, copy.m_fAngleX,
                 copy.m_fDeltaAngleX);
        }

        public CCOrbitCamera(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX,
                             float deltaAngleX) : base(t)
        {
            Init(radius, deltaRadius, angleZ, deltaAngleZ, angleX, deltaAngleX);
        }

        private void Init(float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX,
                          float deltaAngleX)
        {
            m_fRadius = radius;
            m_fDeltaRadius = deltaRadius;
            m_fAngleZ = angleZ;
            m_fDeltaAngleZ = deltaAngleZ;
            m_fAngleX = angleX;
            m_fDeltaAngleX = deltaAngleX;

            m_fRadDeltaZ = CCMacros.CCDegreesToRadians(deltaAngleZ);
            m_fRadDeltaX = CCMacros.CCDegreesToRadians(deltaAngleX);
        }

        public void SphericalRadius(out float newRadius, out float zenith, out float azimuth)
        {
            float ex, ey, ez, cx, cy, cz, x, y, z;
            float r; // radius
            float s;

            CCCamera pCamera = m_pTarget.Camera;
            pCamera.GetEyeXyz(out ex, out ey, out ez);
            pCamera.GetCenterXyz(out cx, out cy, out cz);

            x = ex - cx;
            y = ey - cy;
            z = ez - cz;

            r = (float) Math.Sqrt(x * x + y * y + z * z);
            s = (float) Math.Sqrt(x * x + y * y);
            if (s == 0.0f)
                s = float.Epsilon;
            if (r == 0.0f)
                r = float.Epsilon;

            zenith = (float) Math.Acos(z / r);
            if (x < 0)
                azimuth = (float) Math.PI - (float) Math.Sin(y / s);
            else
                azimuth = (float) Math.Sin(y / s);

            newRadius = r / CCCamera.GetZEye();
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCOrbitCamera;
                base.Copy(zone);
                Init(ret.m_fRadius, ret.m_fDeltaRadius, ret.m_fAngleZ, ret.m_fDeltaAngleZ, ret.m_fAngleX,
                     ret.m_fDeltaAngleX);
                return ret;
            }
            else
            {
                return new CCOrbitCamera(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            float r, zenith, azimuth;
            SphericalRadius(out r, out zenith, out azimuth);

            if (float.IsNaN(m_fRadius))
                m_fRadius = r;

            if (float.IsNaN(m_fAngleZ))
                m_fAngleZ = CCMacros.CCRadiansToDegrees(zenith);

            if (float.IsNaN(m_fAngleX))
                m_fAngleX = CCMacros.CCRadiansToDegrees(azimuth);

            m_fRadZ = CCMacros.CCDegreesToRadians(m_fAngleZ);
            m_fRadX = CCMacros.CCDegreesToRadians(m_fAngleX);
        }

        public override void Update(float time)
        {
            float r = (m_fRadius + m_fDeltaRadius * time) * CCCamera.GetZEye();
            float za = m_fRadZ + m_fRadDeltaZ * time;
            float xa = m_fRadX + m_fRadDeltaX * time;

            float i = (float) Math.Sin(za) * (float) Math.Cos(xa) * r + m_fCenterXOrig;
            float j = (float) Math.Sin(za) * (float) Math.Sin(xa) * r + m_fCenterYOrig;
            float k = (float) Math.Cos(za) * r + m_fCenterZOrig;

            m_pTarget.Camera.SetEyeXyz(i, j, k);
        }
    }
}