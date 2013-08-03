namespace Cocos2D
{
    internal class CCParticleSystemQuadLoader : CCNodeLoader
    {
        private const string PROPERTY_EMITERMODE = "emitterMode";
        private const string PROPERTY_POSVAR = "posVar";
        private const string PROPERTY_EMISSIONRATE = "emissionRate";
        private const string PROPERTY_DURATION = "duration";
        private const string PROPERTY_TOTALPARTICLES = "totalParticles";
        private const string PROPERTY_LIFE = "life";
        private const string PROPERTY_STARTSIZE = "startSize";
        private const string PROPERTY_ENDSIZE = "endSize";
        private const string PROPERTY_STARTSPIN = "startSpin";
        private const string PROPERTY_ENDSPIN = "endSpin";
        private const string PROPERTY_ANGLE = "angle";
        private const string PROPERTY_STARTCOLOR = "startColor";
        private const string PROPERTY_ENDCOLOR = "endColor";
        private const string PROPERTY_BLENDFUNC = "blendFunc";
        private const string PROPERTY_GRAVITY = "gravity";
        private const string PROPERTY_SPEED = "speed";
        private const string PROPERTY_TANGENTIALACCEL = "tangentialAccel";
        private const string PROPERTY_RADIALACCEL = "radialAccel";
        private const string PROPERTY_TEXTURE = "texture";
        private const string PROPERTY_STARTRADIUS = "startRadius";
        private const string PROPERTY_ENDRADIUS = "endRadius";
        private const string PROPERTY_ROTATEPERSECOND = "rotatePerSecond";

        public override CCNode CreateCCNode()
        {
            return new CCParticleSystemQuad();
        }

        protected override void OnHandlePropTypeIntegerLabeled(CCNode node, CCNode parent, string propertyName, int pIntegerLabeled,
                                                               CCBReader reader)
        {
            if (propertyName == PROPERTY_EMITERMODE)
            {
                ((CCParticleSystemQuad) node).EmitterMode = (CCEmitterMode) pIntegerLabeled;
            }
            else
            {
                base.OnHandlePropTypeIntegerLabeled(node, parent, propertyName, pIntegerLabeled, reader);
            }
        }

        protected override void OnHandlePropTypePoint(CCNode node, CCNode parent, string propertyName, CCPoint point, CCBReader reader)
        {
            if (propertyName == PROPERTY_POSVAR)
            {
                ((CCParticleSystemQuad) node).PosVar = point;
            }
            else if (propertyName == PROPERTY_GRAVITY)
            {
                ((CCParticleSystemQuad) node).Gravity = point;
            }
            else
            {
                base.OnHandlePropTypePoint(node, parent, propertyName, point, reader);
            }
        }

        protected override void OnHandlePropTypeFloat(CCNode node, CCNode parent, string propertyName, float pFloat, CCBReader reader)
        {
            if (propertyName == PROPERTY_EMISSIONRATE)
            {
                ((CCParticleSystemQuad) node).EmissionRate = pFloat;
            }
            else if (propertyName == PROPERTY_DURATION)
            {
                ((CCParticleSystemQuad) node).Duration = pFloat;
            }
            else
            {
                base.OnHandlePropTypeFloat(node, parent, propertyName, pFloat, reader);
            }
        }

        protected override void OnHandlePropTypeInteger(CCNode node, CCNode parent, string propertyName, int pInteger, CCBReader reader)
        {
            if (propertyName == PROPERTY_TOTALPARTICLES)
            {
                ((CCParticleSystemQuad) node).TotalParticles = pInteger;
            }
            else
            {
                base.OnHandlePropTypeInteger(node, parent, propertyName, pInteger, reader);
            }
        }

        protected override void OnHandlePropTypeFloatVar(CCNode node, CCNode parent, string propertyName, float[] pFloatVar, CCBReader reader)
        {
            if (propertyName == PROPERTY_LIFE)
            {
                ((CCParticleSystemQuad) node).Life = pFloatVar[0];
                ((CCParticleSystemQuad) node).LifeVar = pFloatVar[1];
            }
            else if (propertyName == PROPERTY_STARTSIZE)
            {
                ((CCParticleSystemQuad) node).StartSize = pFloatVar[0];
                ((CCParticleSystemQuad) node).StartSizeVar = pFloatVar[1];
            }
            else if (propertyName == PROPERTY_ENDSIZE)
            {
                ((CCParticleSystemQuad) node).EndSize = pFloatVar[0];
                ((CCParticleSystemQuad) node).EndSizeVar = pFloatVar[1];
            }
            else if (propertyName == PROPERTY_STARTSPIN)
            {
                ((CCParticleSystemQuad) node).StartSpin = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).StartSpinVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_ENDSPIN)
            {
                ((CCParticleSystemQuad) node).EndSpin = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).EndSpinVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_ANGLE)
            {
                ((CCParticleSystemQuad) node).Angle = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).AngleVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_SPEED)
            {
                ((CCParticleSystemQuad) node).Speed = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).SpeedVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_TANGENTIALACCEL)
            {
                ((CCParticleSystemQuad) node).TangentialAccel = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).TangentialAccelVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_RADIALACCEL)
            {
                ((CCParticleSystemQuad) node).RadialAccel = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).RadialAccelVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_STARTRADIUS)
            {
                ((CCParticleSystemQuad) node).StartRadius = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).StartRadiusVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_ENDRADIUS)
            {
                ((CCParticleSystemQuad) node).EndRadius = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).EndRadiusVar = (pFloatVar[1]);
            }
            else if (propertyName == PROPERTY_ROTATEPERSECOND)
            {
                ((CCParticleSystemQuad) node).RotatePerSecond = (pFloatVar[0]);
                ((CCParticleSystemQuad) node).RotatePerSecondVar = (pFloatVar[1]);
            }
            else
            {
                base.OnHandlePropTypeFloatVar(node, parent, propertyName, pFloatVar, reader);
            }
        }

        protected override void OnHandlePropTypeColor4FVar(CCNode node, CCNode parent, string propertyName, CCColor4F[] colorVar,
                                                           CCBReader reader)
        {
            if (propertyName == PROPERTY_STARTCOLOR)
            {
                ((CCParticleSystemQuad) node).StartColor = (colorVar[0]);
                ((CCParticleSystemQuad) node).StartColorVar = (colorVar[1]);
            }
            else if (propertyName == PROPERTY_ENDCOLOR)
            {
                ((CCParticleSystemQuad) node).EndColor = (colorVar[0]);
                ((CCParticleSystemQuad) node).EndColorVar = (colorVar[1]);
            }
            else
            {
                base.OnHandlePropTypeColor4FVar(node, parent, propertyName, colorVar, reader);
            }
        }

        protected override void OnHandlePropTypeBlendFunc(CCNode node, CCNode parent, string propertyName, CCBlendFunc blendFunc,
                                                          CCBReader reader)
        {
            if (propertyName == PROPERTY_BLENDFUNC)
            {
                ((CCParticleSystemQuad) node).BlendFunc = blendFunc;
            }
            else
            {
                base.OnHandlePropTypeBlendFunc(node, parent, propertyName, blendFunc, reader);
            }
        }

        protected override void OnHandlePropTypeTexture(CCNode node, CCNode parent, string propertyName, CCTexture2D texture,
                                                        CCBReader reader)
        {
            if (propertyName == PROPERTY_TEXTURE)
            {
                ((CCParticleSystemQuad) node).Texture = texture;
            }
            else
            {
                base.OnHandlePropTypeTexture(node, parent, propertyName, texture, reader);
            }
        }
    }
}