using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas1 : AtlasDemo
    {
        CCTextureAtlas m_textureAtlas;

        public Atlas1()
        {
            m_textureAtlas = CCTextureAtlas.Create(TestResource.s_AtlasTest, 3);
            //m_textureAtlas.retain();

            CCSize s = CCDirector.SharedDirector.WinSize;

            //
            // Notice: u,v tex coordinates are inverted
            //
            ccV3F_C4B_T2F_Quad[] quads = 
            {
               new  ccV3F_C4B_T2F_Quad() {
                    bl = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(0,0,0), colors = new CCColor4B(0,0,255,255),texCoords = new ccTex2F(0.0f,1.0f)},				// bottom left
                    br = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(s.Width,0,0), colors = new CCColor4B(0,0,255,0),texCoords = new ccTex2F(1.0f,1.0f)},			// bottom right
                    tl = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(0,s.Height,0), colors = new CCColor4B(0,0,255,0),texCoords = new ccTex2F(0.0f,0.0f)},			// bottom right
                    tr = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(s.Width,s.Height,0), colors = new CCColor4B(0,0,255,255),texCoords = new ccTex2F(1.0f,0.0f)},			// bottom right
                },		
               new  ccV3F_C4B_T2F_Quad() {
                    bl = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(40,40,0), colors = new CCColor4B(255,255,255,255),texCoords = new ccTex2F(0.0f,0.2f)},				// bottom left
                    br = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(120,80,0), colors = new CCColor4B(255,0,0,255),texCoords = new ccTex2F(0.5f,0.2f)},			// bottom right
                    tl = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(40,160,0), colors = new CCColor4B(255,255,255,255),texCoords = new ccTex2F(0.0f,0.0f)},			// bottom right
                    tr = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(160,160,0), colors = new CCColor4B(0,255,0,255),texCoords = new ccTex2F(0.5f,0.0f)},			// bottom right
                },		
               new  ccV3F_C4B_T2F_Quad() {
                    bl = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(s.Width/2,40,0), colors = new CCColor4B(255,0,0,255),texCoords = new ccTex2F(0.0f,1.0f)},				// bottom left
                    br = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(s.Width,40,0), colors = new CCColor4B(0,255,0,255),texCoords = new ccTex2F(1.0f,1.0f)},			// bottom right
                    tl = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(s.Width/2-50,200,0), colors = new CCColor4B(0,0,255,255),texCoords = new ccTex2F(0.0f,0.0f)},			// bottom right
                    tr = new ccV3F_C4B_T2F() { vertices = new ccVertex3F(s.Width,100,0), colors = new CCColor4B(255,255,0,255),texCoords = new ccTex2F(1.0f,0.0f)},			// bottom right
                },		
            };

            for( int i=0;i<3;i++) 
            {
                m_textureAtlas.UpdateQuad(ref quads[i], i);
            }
        }

        public override string title()
        {
            return "CCTextureAtlas";
        }

        public override string subtitle()
        {
            return "Manual creation of CCTextureAtlas";
        }

        public override void Draw()
        {
            // GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // GL_TEXTURE_2D

            m_textureAtlas.DrawQuads();

            //	[textureAtlas drawNumberOfQuads:3];
        }
    }
}
