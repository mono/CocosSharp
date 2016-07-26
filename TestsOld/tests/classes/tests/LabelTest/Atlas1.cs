using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Atlas1 : AtlasDemo
    {
        CCTextureAtlas m_textureAtlas;

        public Atlas1()
        {
			m_textureAtlas = new CCTextureAtlas (TestResource.s_AtlasTest, 3);
            //m_textureAtlas.retain();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

			//
			// Notice: u,v tex coordinates are inverted
			//
			CCV3F_C4B_T2F_Quad[] quads = 
				{
					new  CCV3F_C4B_T2F_Quad() {
						BottomLeft = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(0,0,0), Colors = new CCColor4B(0,0,255,255),TexCoords = new CCTex2F(0.0f,1.0f)},				// bottom left
						BottomRight = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(s.Width,0,0), Colors = new CCColor4B(0,0,255,0),TexCoords = new CCTex2F(1.0f,1.0f)},			// bottom right
						TopLeft = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(0,s.Height,0), Colors = new CCColor4B(0,0,255,0),TexCoords = new CCTex2F(0.0f,0.0f)},			// bottom right
						TopRight = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(s.Width,s.Height,0), Colors = new CCColor4B(0,0,255,255),TexCoords = new CCTex2F(1.0f,0.0f)},			// bottom right
					},		
					new  CCV3F_C4B_T2F_Quad() {
						BottomLeft = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(40,40,0), Colors = new CCColor4B(255,255,255,255),TexCoords = new CCTex2F(0.0f,0.2f)},				// bottom left
						BottomRight = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(120,80,0), Colors = new CCColor4B(255,0,0,255),TexCoords = new CCTex2F(0.5f,0.2f)},			// bottom right
						TopLeft = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(40,160,0), Colors = new CCColor4B(255,255,255,255),TexCoords = new CCTex2F(0.0f,0.0f)},			// bottom right
						TopRight = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(160,160,0), Colors = new CCColor4B(0,255,0,255),TexCoords = new CCTex2F(0.5f,0.0f)},			// bottom right
					},		
					new  CCV3F_C4B_T2F_Quad() {
						BottomLeft = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(s.Width/2,40,0), Colors = new CCColor4B(255,0,0,255),TexCoords = new CCTex2F(0.0f,1.0f)},				// bottom left
						BottomRight = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(s.Width,40,0), Colors = new CCColor4B(0,255,0,255),TexCoords = new CCTex2F(1.0f,1.0f)},			// bottom right
						TopLeft = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(s.Width/2-50,200,0), Colors = new CCColor4B(0,0,255,255),TexCoords = new CCTex2F(0.0f,0.0f)},			// bottom right
						TopRight = new CCV3F_C4B_T2F() { Vertices = new CCVertex3F(s.Width,100,0), Colors = new CCColor4B(255,255,0,255),TexCoords = new CCTex2F(1.0f,0.0f)},			// bottom right
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

//        protected override void Draw()
//        {
//            // GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
//            // GL_TEXTURE_2D
//
//            m_textureAtlas.DrawQuads();
//
//            //	[textureAtlas drawNumberOfQuads:3];
//        }
    }
}
