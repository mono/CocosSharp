/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org


Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

/*
 * The usage of #define is defferent from c++. It can not assign the value the the macro, such as
 * #define XXX 1
 * And it only make effect in the file define it. It means that, macro A defined in file1.cs can not
 * be use in file2.cs.
 * So I should define the macros in project configuration. If the value of the macro is 0, then not
 * define it. The macro will be define like:
 * #define A
 * #define A 1
 * 
 * If the macro like this
 * #define A 0
 * It will not be defined. Take attension this, in c++, it is defined, but the value is 0.
 * 
 * Macros defined are:
 * CC_FONT_LABEL_SUPPORT
 * CC_DIRECTOR_FAST_FPS
 * CC_DIRECTOR_MAC_USE_DISPLAY_LINK_THREAD
 * CC_COCOSNODE_RENDER_SUBPIXEL
 * CC_SPRITEBATCHNODE_RENDER_SUBPIXEL
 * #define CC_USES_VBO 1
 * CC_NODE_TRANSFORM_USING_AFFINE_MATRIX
 * 
 * Macros not defined are:
 * CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
 * CC_DIRECTOR_DISPATCH_FAST_EVENTS
 * CC_TEXTURE_ATLAS_USE_TRIANGLE_STRIP
 * CC_TEXTURE_NPOT_SUPPORT
 * CC_USE_LA88_LABELS_ON_NEON_ARCH
 * CC_SPRITE_DEBUG_DRAW
 * CC_SPRITEBATCHNODE_DEBUG_DRAW
 * CC_LABELBMFONT_DEBUG_DRAW
 * CC_LABELATLAS_DEBUG_DRAW
 * CC_ENABLE_PROFILERS
 * 
 * 
 */
