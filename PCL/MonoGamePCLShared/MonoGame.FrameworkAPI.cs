namespace Microsoft.Xna.Framework {
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct BoundingBox : System.IEquatable<Microsoft.Xna.Framework.BoundingBox> {
		public const int CornerCount = 8;
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Max;
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Min;
		public BoundingBox(Microsoft.Xna.Framework.Vector3 min, Microsoft.Xna.Framework.Vector3 max) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingBox box) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.BoundingBox box, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingFrustum frustum) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.BoundingSphere sphere, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.Vector3 point) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.Vector3 point, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public static Microsoft.Xna.Framework.BoundingBox CreateFromPoints(System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Vector3> points) { return default(Microsoft.Xna.Framework.BoundingBox); }
		public static Microsoft.Xna.Framework.BoundingBox CreateFromSphere(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(Microsoft.Xna.Framework.BoundingBox); }
		public static void CreateFromSphere(ref Microsoft.Xna.Framework.BoundingSphere sphere, out Microsoft.Xna.Framework.BoundingBox result) { result = default(Microsoft.Xna.Framework.BoundingBox); }
		public static Microsoft.Xna.Framework.BoundingBox CreateMerged(Microsoft.Xna.Framework.BoundingBox original, Microsoft.Xna.Framework.BoundingBox additional) { return default(Microsoft.Xna.Framework.BoundingBox); }
		public static void CreateMerged(ref Microsoft.Xna.Framework.BoundingBox original, ref Microsoft.Xna.Framework.BoundingBox additional, out Microsoft.Xna.Framework.BoundingBox result) { result = default(Microsoft.Xna.Framework.BoundingBox); }
		public bool Equals(Microsoft.Xna.Framework.BoundingBox other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public Microsoft.Xna.Framework.Vector3[] GetCorners() { return default(Microsoft.Xna.Framework.Vector3[]); }
		public void GetCorners(Microsoft.Xna.Framework.Vector3[] corners) { }
		public override int GetHashCode() { return default(int); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingBox box) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingBox box, out bool result) { result = default(bool); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingFrustum frustum) { return default(bool); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingSphere sphere, out bool result) { result = default(bool); }
		public Microsoft.Xna.Framework.PlaneIntersectionType Intersects(Microsoft.Xna.Framework.Plane plane) { return default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public void Intersects(ref Microsoft.Xna.Framework.Plane plane, out Microsoft.Xna.Framework.PlaneIntersectionType result) { result = default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public System.Nullable<System.Single> Intersects(Microsoft.Xna.Framework.Ray ray) { return default(System.Nullable<System.Single>); }
		public void Intersects(ref Microsoft.Xna.Framework.Ray ray, out System.Nullable<System.Single> result) { result = default(System.Nullable<System.Single>); }
		public static bool operator ==(Microsoft.Xna.Framework.BoundingBox a, Microsoft.Xna.Framework.BoundingBox b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.BoundingBox a, Microsoft.Xna.Framework.BoundingBox b) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	public partial class BoundingFrustum : System.IEquatable<Microsoft.Xna.Framework.BoundingFrustum> {
		public const int CornerCount = 8;
		public BoundingFrustum(Microsoft.Xna.Framework.Matrix value) { }
		public Microsoft.Xna.Framework.Plane Bottom { get { return default(Microsoft.Xna.Framework.Plane); } }
		public Microsoft.Xna.Framework.Plane Far { get { return default(Microsoft.Xna.Framework.Plane); } }
		public Microsoft.Xna.Framework.Plane Left { get { return default(Microsoft.Xna.Framework.Plane); } }
		public Microsoft.Xna.Framework.Matrix Matrix { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Plane Near { get { return default(Microsoft.Xna.Framework.Plane); } }
		public Microsoft.Xna.Framework.Plane Right { get { return default(Microsoft.Xna.Framework.Plane); } }
		public Microsoft.Xna.Framework.Plane Top { get { return default(Microsoft.Xna.Framework.Plane); } }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingBox box) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.BoundingBox box, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.BoundingSphere sphere, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.Vector3 point) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.Vector3 point, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public bool Equals(Microsoft.Xna.Framework.BoundingFrustum other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public Microsoft.Xna.Framework.Vector3[] GetCorners() { return default(Microsoft.Xna.Framework.Vector3[]); }
		public void GetCorners(Microsoft.Xna.Framework.Vector3[] corners) { }
		public override int GetHashCode() { return default(int); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingBox box) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingBox box, out bool result) { result = default(bool); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingSphere sphere, out bool result) { result = default(bool); }
		public static bool operator ==(Microsoft.Xna.Framework.BoundingFrustum a, Microsoft.Xna.Framework.BoundingFrustum b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.BoundingFrustum a, Microsoft.Xna.Framework.BoundingFrustum b) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct BoundingSphere : System.IEquatable<Microsoft.Xna.Framework.BoundingSphere> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Center;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Radius;
		public BoundingSphere(Microsoft.Xna.Framework.Vector3 center, float radius) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingBox box) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.BoundingBox box, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingFrustum frustum) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.BoundingSphere sphere, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public Microsoft.Xna.Framework.ContainmentType Contains(Microsoft.Xna.Framework.Vector3 point) { return default(Microsoft.Xna.Framework.ContainmentType); }
		public void Contains(ref Microsoft.Xna.Framework.Vector3 point, out Microsoft.Xna.Framework.ContainmentType result) { result = default(Microsoft.Xna.Framework.ContainmentType); }
		public static Microsoft.Xna.Framework.BoundingSphere CreateFromBoundingBox(Microsoft.Xna.Framework.BoundingBox box) { return default(Microsoft.Xna.Framework.BoundingSphere); }
		public static void CreateFromBoundingBox(ref Microsoft.Xna.Framework.BoundingBox box, out Microsoft.Xna.Framework.BoundingSphere result) { result = default(Microsoft.Xna.Framework.BoundingSphere); }
		public static Microsoft.Xna.Framework.BoundingSphere CreateFromFrustum(Microsoft.Xna.Framework.BoundingFrustum frustum) { return default(Microsoft.Xna.Framework.BoundingSphere); }
		public static Microsoft.Xna.Framework.BoundingSphere CreateFromPoints(System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Vector3> points) { return default(Microsoft.Xna.Framework.BoundingSphere); }
		public static Microsoft.Xna.Framework.BoundingSphere CreateMerged(Microsoft.Xna.Framework.BoundingSphere original, Microsoft.Xna.Framework.BoundingSphere additional) { return default(Microsoft.Xna.Framework.BoundingSphere); }
		public static void CreateMerged(ref Microsoft.Xna.Framework.BoundingSphere original, ref Microsoft.Xna.Framework.BoundingSphere additional, out Microsoft.Xna.Framework.BoundingSphere result) { result = default(Microsoft.Xna.Framework.BoundingSphere); }
		public bool Equals(Microsoft.Xna.Framework.BoundingSphere other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingBox box) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingBox box, out bool result) { result = default(bool); }
		public bool Intersects(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingSphere sphere, out bool result) { result = default(bool); }
		public Microsoft.Xna.Framework.PlaneIntersectionType Intersects(Microsoft.Xna.Framework.Plane plane) { return default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public void Intersects(ref Microsoft.Xna.Framework.Plane plane, out Microsoft.Xna.Framework.PlaneIntersectionType result) { result = default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public System.Nullable<System.Single> Intersects(Microsoft.Xna.Framework.Ray ray) { return default(System.Nullable<System.Single>); }
		public void Intersects(ref Microsoft.Xna.Framework.Ray ray, out System.Nullable<System.Single> result) { result = default(System.Nullable<System.Single>); }
		public static bool operator ==(Microsoft.Xna.Framework.BoundingSphere a, Microsoft.Xna.Framework.BoundingSphere b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.BoundingSphere a, Microsoft.Xna.Framework.BoundingSphere b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.BoundingSphere Transform(Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.BoundingSphere); }
		public void Transform(ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.BoundingSphere result) { result = default(Microsoft.Xna.Framework.BoundingSphere); }
	}
	public partial class ButtonDefinition {
		public ButtonDefinition() { }
		public Microsoft.Xna.Framework.Vector2 Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector2); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Rectangle TextureRect { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Rectangle); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Buttons Type { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Buttons); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Color : System.IEquatable<Microsoft.Xna.Framework.Color> {
		public Color(Microsoft.Xna.Framework.Color color, int alpha) { throw new System.NotImplementedException(); }
		public Color(Microsoft.Xna.Framework.Color color, float alpha) { throw new System.NotImplementedException(); }
		public Color(Microsoft.Xna.Framework.Vector3 color) { throw new System.NotImplementedException(); }
		public Color(Microsoft.Xna.Framework.Vector4 color) { throw new System.NotImplementedException(); }
		public Color(int r, int g, int b) { throw new System.NotImplementedException(); }
		public Color(int r, int g, int b, int alpha) { throw new System.NotImplementedException(); }
		public Color(float r, float g, float b) { throw new System.NotImplementedException(); }
		public Color(float r, float g, float b, float alpha) { throw new System.NotImplementedException(); }
		[System.Runtime.Serialization.DataMemberAttribute]
		public byte A { get { return default(byte); } set { } }
		public static Microsoft.Xna.Framework.Color AliceBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color AntiqueWhite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Aqua { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Aquamarine { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Azure { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public byte B { get { return default(byte); } set { } }
		public static Microsoft.Xna.Framework.Color Beige { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Bisque { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Black { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color BlanchedAlmond { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Blue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color BlueViolet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Brown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color BurlyWood { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color CadetBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Chartreuse { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Chocolate { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Coral { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color CornflowerBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Cornsilk { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Crimson { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Cyan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkCyan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkGoldenrod { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkGray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkKhaki { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkMagenta { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkOliveGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkOrange { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkOrchid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkRed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkSalmon { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkSeaGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkSlateBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkSlateGray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkTurquoise { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DarkViolet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DeepPink { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DeepSkyBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DimGray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color DodgerBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Firebrick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color FloralWhite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color ForestGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Fuchsia { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public byte G { get { return default(byte); } set { } }
		public static Microsoft.Xna.Framework.Color Gainsboro { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color GhostWhite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Gold { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Goldenrod { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Gray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Green { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color GreenYellow { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Honeydew { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color HotPink { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color IndianRed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Indigo { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Ivory { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Khaki { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Lavender { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LavenderBlush { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LawnGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LemonChiffon { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightCoral { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightCyan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightGoldenrodYellow { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightGray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightPink { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightSalmon { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightSeaGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightSkyBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightSlateGray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightSteelBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LightYellow { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Lime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color LimeGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Linen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Magenta { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Maroon { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumAquamarine { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumOrchid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumPurple { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumSeaGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumSlateBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumSpringGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumTurquoise { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MediumVioletRed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MidnightBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MintCream { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color MistyRose { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Moccasin { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color NavajoWhite { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Navy { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color OldLace { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Olive { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color OliveDrab { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Orange { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color OrangeRed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Orchid { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		[System.CLSCompliantAttribute(false)]
		public uint PackedValue { get { return default(uint); } set { } }
		public static Microsoft.Xna.Framework.Color PaleGoldenrod { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color PaleGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color PaleTurquoise { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color PaleVioletRed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color PapayaWhip { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color PeachPuff { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Peru { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Pink { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Plum { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color PowderBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Purple { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public byte R { get { return default(byte); } set { } }
		public static Microsoft.Xna.Framework.Color Red { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color RosyBrown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color RoyalBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SaddleBrown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Salmon { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SandyBrown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SeaGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SeaShell { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Sienna { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Silver { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SkyBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SlateBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SlateGray { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Snow { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SpringGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color SteelBlue { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Tan { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Teal { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Thistle { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Tomato { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Transparent { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color TransparentBlack { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Turquoise { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Violet { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Wheat { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color White { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color WhiteSmoke { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color Yellow { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public static Microsoft.Xna.Framework.Color YellowGreen { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Color); } }
		public bool Equals(Microsoft.Xna.Framework.Color other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public static Microsoft.Xna.Framework.Color FromNonPremultiplied(Microsoft.Xna.Framework.Vector4 vector) { return default(Microsoft.Xna.Framework.Color); }
		public static Microsoft.Xna.Framework.Color FromNonPremultiplied(int r, int g, int b, int a) { return default(Microsoft.Xna.Framework.Color); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Color Lerp(Microsoft.Xna.Framework.Color value1, Microsoft.Xna.Framework.Color value2, float amount) { return default(Microsoft.Xna.Framework.Color); }
		public static Microsoft.Xna.Framework.Color Multiply(Microsoft.Xna.Framework.Color value, float scale) { return default(Microsoft.Xna.Framework.Color); }
		public static bool operator ==(Microsoft.Xna.Framework.Color a, Microsoft.Xna.Framework.Color b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Color a, Microsoft.Xna.Framework.Color b) { return default(bool); }
		public static Microsoft.Xna.Framework.Color operator *(Microsoft.Xna.Framework.Color value, float scale) { return default(Microsoft.Xna.Framework.Color); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector3 ToVector3() { return default(Microsoft.Xna.Framework.Vector3); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	public enum ContainmentType {
		Contains = 1,
		Disjoint = 0,
		Intersects = 2,
	}
	[System.Runtime.Serialization.DataContractAttribute]
	public partial class Curve {
		public Curve() { }
		[System.Runtime.Serialization.DataMemberAttribute]
		public bool IsConstant { get { return default(bool); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.CurveKeyCollection Keys { get { return default(Microsoft.Xna.Framework.CurveKeyCollection); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.CurveLoopType PostLoop { get { return default(Microsoft.Xna.Framework.CurveLoopType); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.CurveLoopType PreLoop { get { return default(Microsoft.Xna.Framework.CurveLoopType); } set { } }
		public Microsoft.Xna.Framework.Curve Clone() { return default(Microsoft.Xna.Framework.Curve); }
		public void ComputeTangent(int keyIndex, Microsoft.Xna.Framework.CurveTangent tangentType) { }
		public void ComputeTangent(int keyIndex, Microsoft.Xna.Framework.CurveTangent tangentInType, Microsoft.Xna.Framework.CurveTangent tangentOutType) { }
		public void ComputeTangents(Microsoft.Xna.Framework.CurveTangent tangentType) { }
		public void ComputeTangents(Microsoft.Xna.Framework.CurveTangent tangentInType, Microsoft.Xna.Framework.CurveTangent tangentOutType) { }
		public float Evaluate(float position) { return default(float); }
	}
	public enum CurveContinuity {
		Smooth = 0,
		Step = 1,
	}
	[System.Runtime.Serialization.DataContractAttribute]
	public partial class CurveKey : System.IComparable<Microsoft.Xna.Framework.CurveKey>, System.IEquatable<Microsoft.Xna.Framework.CurveKey> {
		public CurveKey(float position, float value) { }
		public CurveKey(float position, float value, float tangentIn, float tangentOut) { }
		public CurveKey(float position, float value, float tangentIn, float tangentOut, Microsoft.Xna.Framework.CurveContinuity continuity) { }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.CurveContinuity Continuity { get { return default(Microsoft.Xna.Framework.CurveContinuity); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Position { get { return default(float); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public float TangentIn { get { return default(float); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public float TangentOut { get { return default(float); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Value { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.CurveKey Clone() { return default(Microsoft.Xna.Framework.CurveKey); }
		public int CompareTo(Microsoft.Xna.Framework.CurveKey other) { return default(int); }
		public bool Equals(Microsoft.Xna.Framework.CurveKey other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.CurveKey a, Microsoft.Xna.Framework.CurveKey b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.CurveKey a, Microsoft.Xna.Framework.CurveKey b) { return default(bool); }
	}
	public partial class CurveKeyCollection : System.Collections.Generic.ICollection<Microsoft.Xna.Framework.CurveKey>, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.CurveKey>, System.Collections.IEnumerable {
		public CurveKeyCollection() { }
		public int Count { get { return default(int); } }
		public bool IsReadOnly { get { return default(bool); } }
		public Microsoft.Xna.Framework.CurveKey this[int index] { get { return default(Microsoft.Xna.Framework.CurveKey); } set { } }
		public void Add(Microsoft.Xna.Framework.CurveKey item) { }
		public void Clear() { }
		public Microsoft.Xna.Framework.CurveKeyCollection Clone() { return default(Microsoft.Xna.Framework.CurveKeyCollection); }
		public bool Contains(Microsoft.Xna.Framework.CurveKey item) { return default(bool); }
		public void CopyTo(Microsoft.Xna.Framework.CurveKey[] array, int arrayIndex) { }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.CurveKey> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.CurveKey>); }
		public int IndexOf(Microsoft.Xna.Framework.CurveKey item) { return default(int); }
		public bool Remove(Microsoft.Xna.Framework.CurveKey item) { return default(bool); }
		public void RemoveAt(int index) { }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public enum CurveLoopType {
		Constant = 0,
		Cycle = 1,
		CycleOffset = 2,
		Linear = 4,
		Oscillate = 3,
	}
	public enum CurveTangent {
		Flat = 0,
		Linear = 1,
		Smooth = 2,
	}
	[System.FlagsAttribute]
	public enum DisplayOrientation {
		Default = 0,
		LandscapeLeft = 1,
		LandscapeRight = 2,
		Portrait = 4,
		PortraitDown = 8,
		Unknown = 16,
	}
	public partial class DrawableGameComponent : Microsoft.Xna.Framework.GameComponent, Microsoft.Xna.Framework.IDrawable {
		public DrawableGameComponent(Microsoft.Xna.Framework.Game game) : base (default(Microsoft.Xna.Framework.Game)) { }
		public int DrawOrder { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsDevice); } }
		public bool Visible { get { return default(bool); } set { } }
		public event System.EventHandler<System.EventArgs> DrawOrderChanged { add { } remove { } }
		public event System.EventHandler<System.EventArgs> VisibleChanged { add { } remove { } }
		public virtual void Draw(Microsoft.Xna.Framework.GameTime gameTime) { }
		public override void Initialize() { }
		protected virtual void LoadContent() { }
		protected virtual void OnDrawOrderChanged(object sender, System.EventArgs args) { }
		protected virtual void OnVisibleChanged(object sender, System.EventArgs args) { }
		protected virtual void UnloadContent() { }
	}
	public static partial class FrameworkDispatcher {
		public static void Update() { }
	}
	public partial class Game : System.IDisposable {
		public Game() { }
		public Microsoft.Xna.Framework.GameComponentCollection Components { get { return default(Microsoft.Xna.Framework.GameComponentCollection); } }
		public Microsoft.Xna.Framework.Content.ContentManager Content { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Content.ContentManager); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsDevice); } }
		public System.TimeSpan InactiveSleepTime { get { return default(System.TimeSpan); } set { } }
		public bool IsActive { get { return default(bool); } }
		public bool IsFixedTimeStep { get { return default(bool); } set { } }
		public bool IsMouseVisible { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.LaunchParameters LaunchParameters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.LaunchParameters); } }
		public Microsoft.Xna.Framework.GameServiceContainer Services { get { return default(Microsoft.Xna.Framework.GameServiceContainer); } }
		public System.TimeSpan TargetElapsedTime { get { return default(System.TimeSpan); } set { } }
		[System.CLSCompliantAttribute(false)]
		public event System.EventHandler<System.EventArgs> Activated { add { } remove { } }
		public event System.EventHandler<System.EventArgs> Deactivated { add { } remove { } }
		public event System.EventHandler<System.EventArgs> Disposed { add { } remove { } }
		public event System.EventHandler<System.EventArgs> Exiting { add { } remove { } }
		protected virtual bool BeginDraw() { return default(bool); }
		protected virtual void BeginRun() { }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		protected virtual void Draw(Microsoft.Xna.Framework.GameTime gameTime) { }
		protected virtual void EndDraw() { }
		protected virtual void EndRun() { }
		public void Exit() { }
		~Game() { }
		protected virtual void Initialize() { }
		protected virtual void LoadContent() { }
		protected virtual void OnActivated(object sender, System.EventArgs args) { }
		protected virtual void OnDeactivated(object sender, System.EventArgs args) { }
		protected virtual void OnExiting(object sender, System.EventArgs args) { }
		public void ResetElapsedTime() { }
		public void Run() { }
		public void Run(Microsoft.Xna.Framework.GameRunBehavior runBehavior) { }
		public void RunOneFrame() { }
		public void SuppressDraw() { }
		public void Tick() { }
		protected virtual void UnloadContent() { }
		protected virtual void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
	}
	public partial class GameComponent : Microsoft.Xna.Framework.IGameComponent, Microsoft.Xna.Framework.IUpdateable, System.IComparable<Microsoft.Xna.Framework.GameComponent>, System.IDisposable {
		public GameComponent(Microsoft.Xna.Framework.Game game) { }
		public bool Enabled { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Game Game { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Game); } }
		public int UpdateOrder { get { return default(int); } set { } }
		public event System.EventHandler<System.EventArgs> EnabledChanged { add { } remove { } }
		public event System.EventHandler<System.EventArgs> UpdateOrderChanged { add { } remove { } }
		public int CompareTo(Microsoft.Xna.Framework.GameComponent other) { return default(int); }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		~GameComponent() { }
		public virtual void Initialize() { }
		protected virtual void OnEnabledChanged(object sender, System.EventArgs args) { }
		protected virtual void OnUpdateOrderChanged(object sender, System.EventArgs args) { }
		public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
	}
	public sealed partial class GameComponentCollection : System.Collections.ObjectModel.Collection<Microsoft.Xna.Framework.IGameComponent> {
		public GameComponentCollection() { }
		public event System.EventHandler<Microsoft.Xna.Framework.GameComponentCollectionEventArgs> ComponentAdded { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.GameComponentCollectionEventArgs> ComponentRemoved { add { } remove { } }
		protected override void ClearItems() { }
		protected override void InsertItem(int index, Microsoft.Xna.Framework.IGameComponent item) { }
		protected override void RemoveItem(int index) { }
		protected override void SetItem(int index, Microsoft.Xna.Framework.IGameComponent item) { }
	}
	public partial class GameComponentCollectionEventArgs : System.EventArgs {
		public GameComponentCollectionEventArgs(Microsoft.Xna.Framework.IGameComponent gameComponent) { }
		public Microsoft.Xna.Framework.IGameComponent GameComponent { get { return default(Microsoft.Xna.Framework.IGameComponent); } }
	}
	public enum GameRunBehavior {
		Asynchronous = 0,
		Synchronous = 1,
	}
	public partial class GameServiceContainer : System.IServiceProvider {
		public GameServiceContainer() { }
		public void AddService(System.Type type, object provider) { }
		public object GetService(System.Type type) { return default(object); }
		public void RemoveService(System.Type type) { }
	}
	public partial class GameTime {
		public GameTime() { }
		public GameTime(System.TimeSpan totalGameTime, System.TimeSpan elapsedGameTime) { }
		public GameTime(System.TimeSpan totalRealTime, System.TimeSpan elapsedRealTime, bool isRunningSlowly) { }
		public System.TimeSpan ElapsedGameTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool IsRunningSlowly { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public System.TimeSpan TotalGameTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public partial class GameUpdateRequiredException : System.Exception {
		public GameUpdateRequiredException() { }
	}
	public partial class GraphicsDeviceInformation {
		public GraphicsDeviceInformation() { }
		public Microsoft.Xna.Framework.Graphics.GraphicsAdapter Adapter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.GraphicsAdapter); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.GraphicsProfile GraphicsProfile { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.GraphicsProfile); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.PresentationParameters PresentationParameters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.PresentationParameters); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public partial class GraphicsDeviceManager : Microsoft.Xna.Framework.Graphics.IGraphicsDeviceService, Microsoft.Xna.Framework.IGraphicsDeviceManager, System.IDisposable {
		public static readonly int DefaultBackBufferHeight;
		public static readonly int DefaultBackBufferWidth;
		public GraphicsDeviceManager(Microsoft.Xna.Framework.Game game) { }
		public Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsDevice); } }
		public Microsoft.Xna.Framework.Graphics.GraphicsProfile GraphicsProfile { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.GraphicsProfile); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool IsFullScreen { get { return default(bool); } set { } }
		public bool PreferMultiSampling { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Graphics.SurfaceFormat PreferredBackBufferFormat { get { return default(Microsoft.Xna.Framework.Graphics.SurfaceFormat); } set { } }
		public int PreferredBackBufferHeight { get { return default(int); } set { } }
		public int PreferredBackBufferWidth { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Graphics.DepthFormat PreferredDepthStencilFormat { get { return default(Microsoft.Xna.Framework.Graphics.DepthFormat); } set { } }
		public Microsoft.Xna.Framework.DisplayOrientation SupportedOrientations { get { return default(Microsoft.Xna.Framework.DisplayOrientation); } set { } }
		public bool SynchronizeWithVerticalRetrace { get { return default(bool); } set { } }
		public event System.EventHandler<System.EventArgs> DeviceCreated { add { } remove { } }
		public event System.EventHandler<System.EventArgs> DeviceDisposing { add { } remove { } }
		public event System.EventHandler<System.EventArgs> DeviceReset { add { } remove { } }
		public event System.EventHandler<System.EventArgs> DeviceResetting { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs> PreparingDeviceSettings { add { } remove { } }
		public void ApplyChanges() { }
		public bool BeginDraw() { return default(bool); }
		public void CreateDevice() { }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		public void EndDraw() { }
		~GraphicsDeviceManager() { }
		public void ToggleFullScreen() { }
	}
	public partial interface IDrawable {
		int DrawOrder { get; }
		bool Visible { get; }
		event System.EventHandler<System.EventArgs> DrawOrderChanged;
		event System.EventHandler<System.EventArgs> VisibleChanged;
		void Draw(Microsoft.Xna.Framework.GameTime gameTime);
	}
	public partial interface IGameComponent {
		void Initialize();
	}
	public partial interface IGraphicsDeviceManager {
		bool BeginDraw();
		void CreateDevice();
		void EndDraw();
	}
	public partial interface IUpdateable {
		bool Enabled { get; }
		int UpdateOrder { get; }
		event System.EventHandler<System.EventArgs> EnabledChanged;
		event System.EventHandler<System.EventArgs> UpdateOrderChanged;
		void Update(Microsoft.Xna.Framework.GameTime gameTime);
	}
	public partial class LaunchParameters : System.Collections.Generic.Dictionary<System.String, System.String> {
		public LaunchParameters() { }
	}
	public static partial class MathHelper {
		public const float E = 2.71828175f;
		public const float Log10E = 0.4342945f;
		public const float Log2E = 1.442695f;
		public const float Pi = 3.14159274f;
		public const float PiOver2 = 1.57079637f;
		public const float PiOver4 = 0.7853982f;
		public const float TwoPi = 6.28318548f;
		public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2) { return default(float); }
		public static float CatmullRom(float value1, float value2, float value3, float value4, float amount) { return default(float); }
		public static int Clamp(int value, int min, int max) { return default(int); }
		public static float Clamp(float value, float min, float max) { return default(float); }
		public static float Distance(float value1, float value2) { return default(float); }
		public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount) { return default(float); }
		public static bool IsPowerOfTwo(int value) { return default(bool); }
		public static float Lerp(float value1, float value2, float amount) { return default(float); }
		public static float Max(float value1, float value2) { return default(float); }
		public static float Min(float value1, float value2) { return default(float); }
		public static float SmoothStep(float value1, float value2, float amount) { return default(float); }
		public static float ToDegrees(float radians) { return default(float); }
		public static float ToRadians(float degrees) { return default(float); }
		public static float WrapAngle(float angle) { return default(float); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Matrix : System.IEquatable<Microsoft.Xna.Framework.Matrix> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M11;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M12;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M13;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M14;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M21;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M22;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M23;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M24;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M31;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M32;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M33;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M34;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M41;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M42;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M43;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float M44;
		public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34, float m41, float m42, float m43, float m44) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Vector3 Backward { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 Down { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 Forward { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public static Microsoft.Xna.Framework.Matrix Identity { get { return default(Microsoft.Xna.Framework.Matrix); } }
		public float this[int index] { get { return default(float); } set { } }
		public float this[int row, int column] { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 Left { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 Right { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 Translation { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 Up { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public static Microsoft.Xna.Framework.Matrix Add(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Add(ref Microsoft.Xna.Framework.Matrix matrix1, ref Microsoft.Xna.Framework.Matrix matrix2, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateBillboard(Microsoft.Xna.Framework.Vector3 objectPosition, Microsoft.Xna.Framework.Vector3 cameraPosition, Microsoft.Xna.Framework.Vector3 cameraUpVector, System.Nullable<Microsoft.Xna.Framework.Vector3> cameraForwardVector) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateBillboard(ref Microsoft.Xna.Framework.Vector3 objectPosition, ref Microsoft.Xna.Framework.Vector3 cameraPosition, ref Microsoft.Xna.Framework.Vector3 cameraUpVector, System.Nullable<Microsoft.Xna.Framework.Vector3> cameraForwardVector, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateConstrainedBillboard(Microsoft.Xna.Framework.Vector3 objectPosition, Microsoft.Xna.Framework.Vector3 cameraPosition, Microsoft.Xna.Framework.Vector3 rotateAxis, System.Nullable<Microsoft.Xna.Framework.Vector3> cameraForwardVector, System.Nullable<Microsoft.Xna.Framework.Vector3> objectForwardVector) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateConstrainedBillboard(ref Microsoft.Xna.Framework.Vector3 objectPosition, ref Microsoft.Xna.Framework.Vector3 cameraPosition, ref Microsoft.Xna.Framework.Vector3 rotateAxis, System.Nullable<Microsoft.Xna.Framework.Vector3> cameraForwardVector, System.Nullable<Microsoft.Xna.Framework.Vector3> objectForwardVector, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateFromAxisAngle(Microsoft.Xna.Framework.Vector3 axis, float angle) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateFromAxisAngle(ref Microsoft.Xna.Framework.Vector3 axis, float angle, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateFromQuaternion(Microsoft.Xna.Framework.Quaternion quaternion) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateFromQuaternion(ref Microsoft.Xna.Framework.Quaternion quaternion, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateFromYawPitchRoll(float yaw, float pitch, float roll) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateLookAt(Microsoft.Xna.Framework.Vector3 cameraPosition, Microsoft.Xna.Framework.Vector3 cameraTarget, Microsoft.Xna.Framework.Vector3 cameraUpVector) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateLookAt(ref Microsoft.Xna.Framework.Vector3 cameraPosition, ref Microsoft.Xna.Framework.Vector3 cameraTarget, ref Microsoft.Xna.Framework.Vector3 cameraUpVector, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateReflection(Microsoft.Xna.Framework.Plane value) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateReflection(ref Microsoft.Xna.Framework.Plane value, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateRotationX(float radians) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateRotationX(float radians, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateRotationY(float radians) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateRotationY(float radians, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateRotationZ(float radians) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateRotationZ(float radians, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateScale(Microsoft.Xna.Framework.Vector3 scales) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateScale(ref Microsoft.Xna.Framework.Vector3 scales, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateScale(float scale) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateScale(float scale, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateScale(float xScale, float yScale, float zScale) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateScale(float xScale, float yScale, float zScale, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateShadow(Microsoft.Xna.Framework.Vector3 lightDirection, Microsoft.Xna.Framework.Plane plane) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateShadow(ref Microsoft.Xna.Framework.Vector3 lightDirection, ref Microsoft.Xna.Framework.Plane plane, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateTranslation(Microsoft.Xna.Framework.Vector3 position) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateTranslation(ref Microsoft.Xna.Framework.Vector3 position, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateTranslation(float xPosition, float yPosition, float zPosition) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateTranslation(float xPosition, float yPosition, float zPosition, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix CreateWorld(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Vector3 forward, Microsoft.Xna.Framework.Vector3 up) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void CreateWorld(ref Microsoft.Xna.Framework.Vector3 position, ref Microsoft.Xna.Framework.Vector3 forward, ref Microsoft.Xna.Framework.Vector3 up, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public bool Decompose(out Microsoft.Xna.Framework.Vector3 scale, out Microsoft.Xna.Framework.Quaternion rotation, out Microsoft.Xna.Framework.Vector3 translation) { scale = default(Microsoft.Xna.Framework.Vector3); rotation = default(Microsoft.Xna.Framework.Quaternion); translation = default(Microsoft.Xna.Framework.Vector3); return default(bool); }
		public float Determinant() { return default(float); }
		public static Microsoft.Xna.Framework.Matrix Divide(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix Divide(Microsoft.Xna.Framework.Matrix matrix1, float divider) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Divide(ref Microsoft.Xna.Framework.Matrix matrix1, ref Microsoft.Xna.Framework.Matrix matrix2, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static void Divide(ref Microsoft.Xna.Framework.Matrix matrix1, float divider, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public bool Equals(Microsoft.Xna.Framework.Matrix other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Matrix Invert(Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Invert(ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix Lerp(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2, float amount) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Lerp(ref Microsoft.Xna.Framework.Matrix matrix1, ref Microsoft.Xna.Framework.Matrix matrix2, float amount, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix Multiply(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix Multiply(Microsoft.Xna.Framework.Matrix matrix1, float factor) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Multiply(ref Microsoft.Xna.Framework.Matrix matrix1, ref Microsoft.Xna.Framework.Matrix matrix2, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static void Multiply(ref Microsoft.Xna.Framework.Matrix matrix1, float factor, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix Negate(Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Negate(ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix operator +(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix operator /(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix operator /(Microsoft.Xna.Framework.Matrix matrix, float divider) { return default(Microsoft.Xna.Framework.Matrix); }
		public static bool operator ==(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(bool); }
		public static Microsoft.Xna.Framework.Matrix operator *(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix operator *(Microsoft.Xna.Framework.Matrix matrix, float scaleFactor) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix operator -(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix operator -(Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Matrix); }
		public static Microsoft.Xna.Framework.Matrix Subtract(Microsoft.Xna.Framework.Matrix matrix1, Microsoft.Xna.Framework.Matrix matrix2) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Subtract(ref Microsoft.Xna.Framework.Matrix matrix1, ref Microsoft.Xna.Framework.Matrix matrix2, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
		public static System.Single[] ToFloatArray(Microsoft.Xna.Framework.Matrix mat) { return default(System.Single[]); }
		public override string ToString() { return default(string); }
		public static Microsoft.Xna.Framework.Matrix Transpose(Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Matrix); }
		public static void Transpose(ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Matrix result) { result = default(Microsoft.Xna.Framework.Matrix); }
	}
	public static partial class PerformanceCounter {
		public static long ElapsedTime { get { return default(long); } }
		public static void Begin() { }
		public static void BeginMensure(string Name) { }
		public static void Dump() { }
		public static void EndMensure(string Name) { }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Plane : System.IEquatable<Microsoft.Xna.Framework.Plane> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public float D;
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Normal;
		public Plane(Microsoft.Xna.Framework.Vector3 a, Microsoft.Xna.Framework.Vector3 b, Microsoft.Xna.Framework.Vector3 c) { throw new System.NotImplementedException(); }
		public Plane(Microsoft.Xna.Framework.Vector3 normal, float d) { throw new System.NotImplementedException(); }
		public Plane(Microsoft.Xna.Framework.Vector4 value) { throw new System.NotImplementedException(); }
		public Plane(float a, float b, float c, float d) { throw new System.NotImplementedException(); }
		public float Dot(Microsoft.Xna.Framework.Vector4 value) { return default(float); }
		public void Dot(ref Microsoft.Xna.Framework.Vector4 value, out float result) { result = default(float); }
		public float DotCoordinate(Microsoft.Xna.Framework.Vector3 value) { return default(float); }
		public void DotCoordinate(ref Microsoft.Xna.Framework.Vector3 value, out float result) { result = default(float); }
		public float DotNormal(Microsoft.Xna.Framework.Vector3 value) { return default(float); }
		public void DotNormal(ref Microsoft.Xna.Framework.Vector3 value, out float result) { result = default(float); }
		public bool Equals(Microsoft.Xna.Framework.Plane other) { return default(bool); }
		public override bool Equals(object other) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public Microsoft.Xna.Framework.PlaneIntersectionType Intersects(Microsoft.Xna.Framework.BoundingBox box) { return default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingBox box, out Microsoft.Xna.Framework.PlaneIntersectionType result) { result = default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public Microsoft.Xna.Framework.PlaneIntersectionType Intersects(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingSphere sphere, out Microsoft.Xna.Framework.PlaneIntersectionType result) { result = default(Microsoft.Xna.Framework.PlaneIntersectionType); }
		public void Normalize() { }
		public static Microsoft.Xna.Framework.Plane Normalize(Microsoft.Xna.Framework.Plane value) { return default(Microsoft.Xna.Framework.Plane); }
		public static void Normalize(ref Microsoft.Xna.Framework.Plane value, out Microsoft.Xna.Framework.Plane result) { result = default(Microsoft.Xna.Framework.Plane); }
		public static bool operator ==(Microsoft.Xna.Framework.Plane plane1, Microsoft.Xna.Framework.Plane plane2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Plane plane1, Microsoft.Xna.Framework.Plane plane2) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	public enum PlaneIntersectionType {
		Back = 1,
		Front = 0,
		Intersecting = 2,
	}
	public enum PlayerIndex {
		Four = 3,
		One = 0,
		Three = 2,
		Two = 1,
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Point : System.IEquatable<Microsoft.Xna.Framework.Point> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public int X;
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Y;
		public Point(int x, int y) { throw new System.NotImplementedException(); }
		public static Microsoft.Xna.Framework.Point Zero { get { return default(Microsoft.Xna.Framework.Point); } }
		public bool Equals(Microsoft.Xna.Framework.Point other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Point operator +(Microsoft.Xna.Framework.Point a, Microsoft.Xna.Framework.Point b) { return default(Microsoft.Xna.Framework.Point); }
		public static Microsoft.Xna.Framework.Point operator /(Microsoft.Xna.Framework.Point a, Microsoft.Xna.Framework.Point b) { return default(Microsoft.Xna.Framework.Point); }
		public static bool operator ==(Microsoft.Xna.Framework.Point a, Microsoft.Xna.Framework.Point b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Point a, Microsoft.Xna.Framework.Point b) { return default(bool); }
		public static Microsoft.Xna.Framework.Point operator *(Microsoft.Xna.Framework.Point a, Microsoft.Xna.Framework.Point b) { return default(Microsoft.Xna.Framework.Point); }
		public static Microsoft.Xna.Framework.Point operator -(Microsoft.Xna.Framework.Point a, Microsoft.Xna.Framework.Point b) { return default(Microsoft.Xna.Framework.Point); }
		public override string ToString() { return default(string); }
	}
	public partial class PreparingDeviceSettingsEventArgs : System.EventArgs {
		public PreparingDeviceSettingsEventArgs(Microsoft.Xna.Framework.GraphicsDeviceInformation graphicsDeviceInformation) { }
		public Microsoft.Xna.Framework.GraphicsDeviceInformation GraphicsDeviceInformation { get { return default(Microsoft.Xna.Framework.GraphicsDeviceInformation); } }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Quaternion : System.IEquatable<Microsoft.Xna.Framework.Quaternion> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public float W;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float X;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Y;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Z;
		public Quaternion(Microsoft.Xna.Framework.Vector3 vectorPart, float scalarPart) { throw new System.NotImplementedException(); }
		public Quaternion(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		public static Microsoft.Xna.Framework.Quaternion Identity { get { return default(Microsoft.Xna.Framework.Quaternion); } }
		public static Microsoft.Xna.Framework.Quaternion Add(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Add(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Concatenate(Microsoft.Xna.Framework.Quaternion value1, Microsoft.Xna.Framework.Quaternion value2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Concatenate(ref Microsoft.Xna.Framework.Quaternion value1, ref Microsoft.Xna.Framework.Quaternion value2, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public void Conjugate() { }
		public static Microsoft.Xna.Framework.Quaternion Conjugate(Microsoft.Xna.Framework.Quaternion value) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Conjugate(ref Microsoft.Xna.Framework.Quaternion value, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion CreateFromAxisAngle(Microsoft.Xna.Framework.Vector3 axis, float angle) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void CreateFromAxisAngle(ref Microsoft.Xna.Framework.Vector3 axis, float angle, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion CreateFromRotationMatrix(Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void CreateFromRotationMatrix(ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Divide(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Divide(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static float Dot(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(float); }
		public static void Dot(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, out float result) { result = default(float); }
		public bool Equals(Microsoft.Xna.Framework.Quaternion other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Quaternion Inverse(Microsoft.Xna.Framework.Quaternion quaternion) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Inverse(ref Microsoft.Xna.Framework.Quaternion quaternion, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public float Length() { return default(float); }
		public float LengthSquared() { return default(float); }
		public static Microsoft.Xna.Framework.Quaternion Lerp(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2, float amount) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Lerp(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, float amount, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Multiply(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Multiply(Microsoft.Xna.Framework.Quaternion quaternion1, float scaleFactor) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Multiply(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static void Multiply(ref Microsoft.Xna.Framework.Quaternion quaternion1, float scaleFactor, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Negate(Microsoft.Xna.Framework.Quaternion quaternion) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Negate(ref Microsoft.Xna.Framework.Quaternion quaternion, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public void Normalize() { }
		public static Microsoft.Xna.Framework.Quaternion Normalize(Microsoft.Xna.Framework.Quaternion quaternion) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Normalize(ref Microsoft.Xna.Framework.Quaternion quaternion, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion operator +(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion operator /(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static bool operator ==(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(bool); }
		public static Microsoft.Xna.Framework.Quaternion operator *(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion operator *(Microsoft.Xna.Framework.Quaternion quaternion1, float scaleFactor) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion operator -(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion operator -(Microsoft.Xna.Framework.Quaternion quaternion) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Slerp(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2, float amount) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Slerp(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, float amount, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public static Microsoft.Xna.Framework.Quaternion Subtract(Microsoft.Xna.Framework.Quaternion quaternion1, Microsoft.Xna.Framework.Quaternion quaternion2) { return default(Microsoft.Xna.Framework.Quaternion); }
		public static void Subtract(ref Microsoft.Xna.Framework.Quaternion quaternion1, ref Microsoft.Xna.Framework.Quaternion quaternion2, out Microsoft.Xna.Framework.Quaternion result) { result = default(Microsoft.Xna.Framework.Quaternion); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Ray : System.IEquatable<Microsoft.Xna.Framework.Ray> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Direction;
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Position;
		public Ray(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Vector3 direction) { throw new System.NotImplementedException(); }
		public bool Equals(Microsoft.Xna.Framework.Ray other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public System.Nullable<System.Single> Intersects(Microsoft.Xna.Framework.BoundingBox box) { return default(System.Nullable<System.Single>); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingBox box, out System.Nullable<System.Single> result) { result = default(System.Nullable<System.Single>); }
		public System.Nullable<System.Single> Intersects(Microsoft.Xna.Framework.BoundingSphere sphere) { return default(System.Nullable<System.Single>); }
		public void Intersects(ref Microsoft.Xna.Framework.BoundingSphere sphere, out System.Nullable<System.Single> result) { result = default(System.Nullable<System.Single>); }
		public System.Nullable<System.Single> Intersects(Microsoft.Xna.Framework.Plane plane) { return default(System.Nullable<System.Single>); }
		public void Intersects(ref Microsoft.Xna.Framework.Plane plane, out System.Nullable<System.Single> result) { result = default(System.Nullable<System.Single>); }
		public static bool operator ==(Microsoft.Xna.Framework.Ray a, Microsoft.Xna.Framework.Ray b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Ray a, Microsoft.Xna.Framework.Ray b) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Rectangle : System.IEquatable<Microsoft.Xna.Framework.Rectangle> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Height;
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Width;
		[System.Runtime.Serialization.DataMemberAttribute]
		public int X;
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Y;
		public Rectangle(int x, int y, int width, int height) { throw new System.NotImplementedException(); }
		public int Bottom { get { return default(int); } }
		public Microsoft.Xna.Framework.Point Center { get { return default(Microsoft.Xna.Framework.Point); } }
		public static Microsoft.Xna.Framework.Rectangle Empty { get { return default(Microsoft.Xna.Framework.Rectangle); } }
		public bool IsEmpty { get { return default(bool); } }
		public int Left { get { return default(int); } }
		public Microsoft.Xna.Framework.Point Location { get { return default(Microsoft.Xna.Framework.Point); } set { } }
		public int Right { get { return default(int); } }
		public int Top { get { return default(int); } }
		public bool Contains(Microsoft.Xna.Framework.Point value) { return default(bool); }
		public bool Contains(Microsoft.Xna.Framework.Rectangle value) { return default(bool); }
		public bool Contains(Microsoft.Xna.Framework.Vector2 value) { return default(bool); }
		public bool Contains(int x, int y) { return default(bool); }
		public bool Contains(float x, float y) { return default(bool); }
		public bool Equals(Microsoft.Xna.Framework.Rectangle other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public void Inflate(int horizontalValue, int verticalValue) { }
		public static Microsoft.Xna.Framework.Rectangle Intersect(Microsoft.Xna.Framework.Rectangle value1, Microsoft.Xna.Framework.Rectangle value2) { return default(Microsoft.Xna.Framework.Rectangle); }
		public static void Intersect(ref Microsoft.Xna.Framework.Rectangle value1, ref Microsoft.Xna.Framework.Rectangle value2, out Microsoft.Xna.Framework.Rectangle result) { result = default(Microsoft.Xna.Framework.Rectangle); }
		public bool Intersects(Microsoft.Xna.Framework.Rectangle value) { return default(bool); }
		public void Intersects(ref Microsoft.Xna.Framework.Rectangle value, out bool result) { result = default(bool); }
		public void Offset(Microsoft.Xna.Framework.Point offset) { }
		public void Offset(int offsetX, int offsetY) { }
		public static bool operator ==(Microsoft.Xna.Framework.Rectangle a, Microsoft.Xna.Framework.Rectangle b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Rectangle a, Microsoft.Xna.Framework.Rectangle b) { return default(bool); }
		public override string ToString() { return default(string); }
		public static Microsoft.Xna.Framework.Rectangle Union(Microsoft.Xna.Framework.Rectangle value1, Microsoft.Xna.Framework.Rectangle value2) { return default(Microsoft.Xna.Framework.Rectangle); }
		public static void Union(ref Microsoft.Xna.Framework.Rectangle value1, ref Microsoft.Xna.Framework.Rectangle value2, out Microsoft.Xna.Framework.Rectangle result) { result = default(Microsoft.Xna.Framework.Rectangle); }
	}
	public partial class TextInputEventArgs : System.EventArgs {
		public TextInputEventArgs(char character) { }
		public char Character { get { return default(char); } }
	}
	public partial class ThumbStickDefinition {
		public ThumbStickDefinition() { }
		public Microsoft.Xna.Framework.Vector2 Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector2); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Rectangle TextureRect { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Rectangle); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public static partial class TitleContainer {
		public static System.IO.Stream OpenStream(string name) { return default(System.IO.Stream); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Vector2 : System.IEquatable<Microsoft.Xna.Framework.Vector2> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public float X;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Y;
		public Vector2(float value) { throw new System.NotImplementedException(); }
		public Vector2(float x, float y) { throw new System.NotImplementedException(); }
		public static Microsoft.Xna.Framework.Vector2 One { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public static Microsoft.Xna.Framework.Vector2 UnitX { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public static Microsoft.Xna.Framework.Vector2 UnitY { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public static Microsoft.Xna.Framework.Vector2 Zero { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public static Microsoft.Xna.Framework.Vector2 Add(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Add(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Barycentric(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2, Microsoft.Xna.Framework.Vector2 value3, float amount1, float amount2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Barycentric(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, ref Microsoft.Xna.Framework.Vector2 value3, float amount1, float amount2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 CatmullRom(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2, Microsoft.Xna.Framework.Vector2 value3, Microsoft.Xna.Framework.Vector2 value4, float amount) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void CatmullRom(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, ref Microsoft.Xna.Framework.Vector2 value3, ref Microsoft.Xna.Framework.Vector2 value4, float amount, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Clamp(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 min, Microsoft.Xna.Framework.Vector2 max) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Clamp(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 min, ref Microsoft.Xna.Framework.Vector2 max, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static float Distance(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(float); }
		public static void Distance(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out float result) { result = default(float); }
		public static float DistanceSquared(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(float); }
		public static void DistanceSquared(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out float result) { result = default(float); }
		public static Microsoft.Xna.Framework.Vector2 Divide(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Divide(Microsoft.Xna.Framework.Vector2 value1, float divider) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Divide(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static void Divide(ref Microsoft.Xna.Framework.Vector2 value1, float divider, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static float Dot(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(float); }
		public static void Dot(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out float result) { result = default(float); }
		public bool Equals(Microsoft.Xna.Framework.Vector2 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Vector2 Hermite(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 tangent1, Microsoft.Xna.Framework.Vector2 value2, Microsoft.Xna.Framework.Vector2 tangent2, float amount) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Hermite(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 tangent1, ref Microsoft.Xna.Framework.Vector2 value2, ref Microsoft.Xna.Framework.Vector2 tangent2, float amount, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public float Length() { return default(float); }
		public float LengthSquared() { return default(float); }
		public static Microsoft.Xna.Framework.Vector2 Lerp(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2, float amount) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Lerp(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, float amount, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Max(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Max(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Min(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Min(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Multiply(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Multiply(Microsoft.Xna.Framework.Vector2 value1, float scaleFactor) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Multiply(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static void Multiply(ref Microsoft.Xna.Framework.Vector2 value1, float scaleFactor, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Negate(Microsoft.Xna.Framework.Vector2 value) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Negate(ref Microsoft.Xna.Framework.Vector2 value, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public void Normalize() { }
		public static Microsoft.Xna.Framework.Vector2 Normalize(Microsoft.Xna.Framework.Vector2 value) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Normalize(ref Microsoft.Xna.Framework.Vector2 value, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator +(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator /(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator /(Microsoft.Xna.Framework.Vector2 value1, float divider) { return default(Microsoft.Xna.Framework.Vector2); }
		public static bool operator ==(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(bool); }
		public static Microsoft.Xna.Framework.Vector2 operator *(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator *(Microsoft.Xna.Framework.Vector2 value, float scaleFactor) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator *(float scaleFactor, Microsoft.Xna.Framework.Vector2 value) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator -(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 operator -(Microsoft.Xna.Framework.Vector2 value) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Reflect(Microsoft.Xna.Framework.Vector2 vector, Microsoft.Xna.Framework.Vector2 normal) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Reflect(ref Microsoft.Xna.Framework.Vector2 vector, ref Microsoft.Xna.Framework.Vector2 normal, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 SmoothStep(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2, float amount) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void SmoothStep(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, float amount, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Subtract(Microsoft.Xna.Framework.Vector2 value1, Microsoft.Xna.Framework.Vector2 value2) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Subtract(ref Microsoft.Xna.Framework.Vector2 value1, ref Microsoft.Xna.Framework.Vector2 value2, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public override string ToString() { return default(string); }
		public static Microsoft.Xna.Framework.Vector2 Transform(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector2); }
		public static Microsoft.Xna.Framework.Vector2 Transform(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Quaternion quat) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector2 position, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector2 position, ref Microsoft.Xna.Framework.Quaternion quat, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
		public static void Transform(Microsoft.Xna.Framework.Vector2[] sourceArray, ref Microsoft.Xna.Framework.Matrix matrix, Microsoft.Xna.Framework.Vector2[] destinationArray) { }
		public static void Transform(Microsoft.Xna.Framework.Vector2[] sourceArray, int sourceIndex, ref Microsoft.Xna.Framework.Matrix matrix, Microsoft.Xna.Framework.Vector2[] destinationArray, int destinationIndex, int length) { }
		public static Microsoft.Xna.Framework.Vector2 TransformNormal(Microsoft.Xna.Framework.Vector2 normal, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector2); }
		public static void TransformNormal(ref Microsoft.Xna.Framework.Vector2 normal, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector2 result) { result = default(Microsoft.Xna.Framework.Vector2); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Vector3 : System.IEquatable<Microsoft.Xna.Framework.Vector3> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public float X;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Y;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Z;
		public Vector3(Microsoft.Xna.Framework.Vector2 value, float z) { throw new System.NotImplementedException(); }
		public Vector3(float value) { throw new System.NotImplementedException(); }
		public Vector3(float x, float y, float z) { throw new System.NotImplementedException(); }
		public static Microsoft.Xna.Framework.Vector3 Backward { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Down { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Forward { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Left { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 One { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Right { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 UnitX { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 UnitY { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 UnitZ { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Up { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Zero { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public static Microsoft.Xna.Framework.Vector3 Add(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Add(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Barycentric(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2, Microsoft.Xna.Framework.Vector3 value3, float amount1, float amount2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Barycentric(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, ref Microsoft.Xna.Framework.Vector3 value3, float amount1, float amount2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 CatmullRom(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2, Microsoft.Xna.Framework.Vector3 value3, Microsoft.Xna.Framework.Vector3 value4, float amount) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void CatmullRom(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, ref Microsoft.Xna.Framework.Vector3 value3, ref Microsoft.Xna.Framework.Vector3 value4, float amount, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Clamp(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 min, Microsoft.Xna.Framework.Vector3 max) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Clamp(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 min, ref Microsoft.Xna.Framework.Vector3 max, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Cross(Microsoft.Xna.Framework.Vector3 vector1, Microsoft.Xna.Framework.Vector3 vector2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Cross(ref Microsoft.Xna.Framework.Vector3 vector1, ref Microsoft.Xna.Framework.Vector3 vector2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static float Distance(Microsoft.Xna.Framework.Vector3 vector1, Microsoft.Xna.Framework.Vector3 vector2) { return default(float); }
		public static void Distance(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out float result) { result = default(float); }
		public static float DistanceSquared(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(float); }
		public static void DistanceSquared(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out float result) { result = default(float); }
		public static Microsoft.Xna.Framework.Vector3 Divide(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Divide(Microsoft.Xna.Framework.Vector3 value1, float value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Divide(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static void Divide(ref Microsoft.Xna.Framework.Vector3 value1, float divisor, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static float Dot(Microsoft.Xna.Framework.Vector3 vector1, Microsoft.Xna.Framework.Vector3 vector2) { return default(float); }
		public static void Dot(ref Microsoft.Xna.Framework.Vector3 vector1, ref Microsoft.Xna.Framework.Vector3 vector2, out float result) { result = default(float); }
		public bool Equals(Microsoft.Xna.Framework.Vector3 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Vector3 Hermite(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 tangent1, Microsoft.Xna.Framework.Vector3 value2, Microsoft.Xna.Framework.Vector3 tangent2, float amount) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Hermite(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 tangent1, ref Microsoft.Xna.Framework.Vector3 value2, ref Microsoft.Xna.Framework.Vector3 tangent2, float amount, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public float Length() { return default(float); }
		public float LengthSquared() { return default(float); }
		public static Microsoft.Xna.Framework.Vector3 Lerp(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2, float amount) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Lerp(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, float amount, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Max(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Max(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Min(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Min(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Multiply(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Multiply(Microsoft.Xna.Framework.Vector3 value1, float scaleFactor) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Multiply(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static void Multiply(ref Microsoft.Xna.Framework.Vector3 value1, float scaleFactor, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Negate(Microsoft.Xna.Framework.Vector3 value) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Negate(ref Microsoft.Xna.Framework.Vector3 value, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public void Normalize() { }
		public static Microsoft.Xna.Framework.Vector3 Normalize(Microsoft.Xna.Framework.Vector3 vector) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Normalize(ref Microsoft.Xna.Framework.Vector3 value, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator +(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator /(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator /(Microsoft.Xna.Framework.Vector3 value, float divider) { return default(Microsoft.Xna.Framework.Vector3); }
		public static bool operator ==(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(bool); }
		public static Microsoft.Xna.Framework.Vector3 operator *(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator *(Microsoft.Xna.Framework.Vector3 value, float scaleFactor) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator *(float scaleFactor, Microsoft.Xna.Framework.Vector3 value) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator -(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 operator -(Microsoft.Xna.Framework.Vector3 value) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Reflect(Microsoft.Xna.Framework.Vector3 vector, Microsoft.Xna.Framework.Vector3 normal) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Reflect(ref Microsoft.Xna.Framework.Vector3 vector, ref Microsoft.Xna.Framework.Vector3 normal, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 SmoothStep(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2, float amount) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void SmoothStep(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, float amount, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Subtract(Microsoft.Xna.Framework.Vector3 value1, Microsoft.Xna.Framework.Vector3 value2) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Subtract(ref Microsoft.Xna.Framework.Vector3 value1, ref Microsoft.Xna.Framework.Vector3 value2, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public override string ToString() { return default(string); }
		public static Microsoft.Xna.Framework.Vector3 Transform(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector3); }
		public static Microsoft.Xna.Framework.Vector3 Transform(Microsoft.Xna.Framework.Vector3 vec, Microsoft.Xna.Framework.Quaternion quat) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector3 position, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector3 value, ref Microsoft.Xna.Framework.Quaternion rotation, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
		public static void Transform(Microsoft.Xna.Framework.Vector3[] sourceArray, ref Microsoft.Xna.Framework.Matrix matrix, Microsoft.Xna.Framework.Vector3[] destinationArray) { }
		public static void Transform(Microsoft.Xna.Framework.Vector3[] sourceArray, ref Microsoft.Xna.Framework.Quaternion rotation, Microsoft.Xna.Framework.Vector3[] destinationArray) { }
		public static void Transform(Microsoft.Xna.Framework.Vector3[] sourceArray, int sourceIndex, ref Microsoft.Xna.Framework.Matrix matrix, Microsoft.Xna.Framework.Vector3[] destinationArray, int destinationIndex, int length) { }
		public static void Transform(Microsoft.Xna.Framework.Vector3[] sourceArray, int sourceIndex, ref Microsoft.Xna.Framework.Quaternion rotation, Microsoft.Xna.Framework.Vector3[] destinationArray, int destinationIndex, int length) { }
		public static Microsoft.Xna.Framework.Vector3 TransformNormal(Microsoft.Xna.Framework.Vector3 normal, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector3); }
		public static void TransformNormal(ref Microsoft.Xna.Framework.Vector3 normal, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector3 result) { result = default(Microsoft.Xna.Framework.Vector3); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Vector4 : System.IEquatable<Microsoft.Xna.Framework.Vector4> {
		[System.Runtime.Serialization.DataMemberAttribute]
		public float W;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float X;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Y;
		[System.Runtime.Serialization.DataMemberAttribute]
		public float Z;
		public Vector4(Microsoft.Xna.Framework.Vector2 value, float z, float w) { throw new System.NotImplementedException(); }
		public Vector4(Microsoft.Xna.Framework.Vector3 value, float w) { throw new System.NotImplementedException(); }
		public Vector4(float value) { throw new System.NotImplementedException(); }
		public Vector4(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		public static Microsoft.Xna.Framework.Vector4 One { get { return default(Microsoft.Xna.Framework.Vector4); } }
		public static Microsoft.Xna.Framework.Vector4 UnitW { get { return default(Microsoft.Xna.Framework.Vector4); } }
		public static Microsoft.Xna.Framework.Vector4 UnitX { get { return default(Microsoft.Xna.Framework.Vector4); } }
		public static Microsoft.Xna.Framework.Vector4 UnitY { get { return default(Microsoft.Xna.Framework.Vector4); } }
		public static Microsoft.Xna.Framework.Vector4 UnitZ { get { return default(Microsoft.Xna.Framework.Vector4); } }
		public static Microsoft.Xna.Framework.Vector4 Zero { get { return default(Microsoft.Xna.Framework.Vector4); } }
		public static Microsoft.Xna.Framework.Vector4 Add(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Add(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Barycentric(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2, Microsoft.Xna.Framework.Vector4 value3, float amount1, float amount2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Barycentric(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, ref Microsoft.Xna.Framework.Vector4 value3, float amount1, float amount2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 CatmullRom(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2, Microsoft.Xna.Framework.Vector4 value3, Microsoft.Xna.Framework.Vector4 value4, float amount) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void CatmullRom(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, ref Microsoft.Xna.Framework.Vector4 value3, ref Microsoft.Xna.Framework.Vector4 value4, float amount, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Clamp(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 min, Microsoft.Xna.Framework.Vector4 max) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Clamp(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 min, ref Microsoft.Xna.Framework.Vector4 max, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static float Distance(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(float); }
		public static void Distance(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out float result) { result = default(float); }
		public static float DistanceSquared(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(float); }
		public static void DistanceSquared(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out float result) { result = default(float); }
		public static Microsoft.Xna.Framework.Vector4 Divide(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Divide(Microsoft.Xna.Framework.Vector4 value1, float divider) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Divide(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static void Divide(ref Microsoft.Xna.Framework.Vector4 value1, float divider, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static float Dot(Microsoft.Xna.Framework.Vector4 vector1, Microsoft.Xna.Framework.Vector4 vector2) { return default(float); }
		public static void Dot(ref Microsoft.Xna.Framework.Vector4 vector1, ref Microsoft.Xna.Framework.Vector4 vector2, out float result) { result = default(float); }
		public bool Equals(Microsoft.Xna.Framework.Vector4 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static Microsoft.Xna.Framework.Vector4 Hermite(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 tangent1, Microsoft.Xna.Framework.Vector4 value2, Microsoft.Xna.Framework.Vector4 tangent2, float amount) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Hermite(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 tangent1, ref Microsoft.Xna.Framework.Vector4 value2, ref Microsoft.Xna.Framework.Vector4 tangent2, float amount, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public float Length() { return default(float); }
		public float LengthSquared() { return default(float); }
		public static Microsoft.Xna.Framework.Vector4 Lerp(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2, float amount) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Lerp(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, float amount, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Max(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Max(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Min(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Min(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Multiply(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Multiply(Microsoft.Xna.Framework.Vector4 value1, float scaleFactor) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Multiply(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static void Multiply(ref Microsoft.Xna.Framework.Vector4 value1, float scaleFactor, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Negate(Microsoft.Xna.Framework.Vector4 value) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Negate(ref Microsoft.Xna.Framework.Vector4 value, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public void Normalize() { }
		public static Microsoft.Xna.Framework.Vector4 Normalize(Microsoft.Xna.Framework.Vector4 vector) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Normalize(ref Microsoft.Xna.Framework.Vector4 vector, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator +(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator /(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator /(Microsoft.Xna.Framework.Vector4 value1, float divider) { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(bool); }
		public static Microsoft.Xna.Framework.Vector4 operator *(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator *(Microsoft.Xna.Framework.Vector4 value1, float scaleFactor) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator *(float scaleFactor, Microsoft.Xna.Framework.Vector4 value1) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator -(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 operator -(Microsoft.Xna.Framework.Vector4 value) { return default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 SmoothStep(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2, float amount) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void SmoothStep(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, float amount, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Subtract(Microsoft.Xna.Framework.Vector4 value1, Microsoft.Xna.Framework.Vector4 value2) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Subtract(ref Microsoft.Xna.Framework.Vector4 value1, ref Microsoft.Xna.Framework.Vector4 value2, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public override string ToString() { return default(string); }
		public static Microsoft.Xna.Framework.Vector4 Transform(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector2 position, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Transform(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector3 position, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
		public static Microsoft.Xna.Framework.Vector4 Transform(Microsoft.Xna.Framework.Vector4 vector, Microsoft.Xna.Framework.Matrix matrix) { return default(Microsoft.Xna.Framework.Vector4); }
		public static void Transform(ref Microsoft.Xna.Framework.Vector4 vector, ref Microsoft.Xna.Framework.Matrix matrix, out Microsoft.Xna.Framework.Vector4 result) { result = default(Microsoft.Xna.Framework.Vector4); }
	}
}
namespace Microsoft.Xna.Framework.Audio {
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct AudioCategory : System.IEquatable<Microsoft.Xna.Framework.Audio.AudioCategory> {
		public string Name { get { return default(string); } }
		public bool Equals(Microsoft.Xna.Framework.Audio.AudioCategory other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Audio.AudioCategory first, Microsoft.Xna.Framework.Audio.AudioCategory second) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Audio.AudioCategory first, Microsoft.Xna.Framework.Audio.AudioCategory second) { return default(bool); }
		public void Pause() { }
		public void Resume() { }
		public void SetVolume(float volume) { }
		public void Stop() { }
		public override string ToString() { return default(string); }
	}
	public enum AudioChannels {
		Mono = 1,
		Stereo = 2,
	}
	public partial class AudioEmitter {
		public AudioEmitter() { }
		public float DopplerScale { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 Forward { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector3 Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector3 Up { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector3 Velocity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public partial class AudioEngine : System.IDisposable {
		public const int ContentVersion = 46;
		public AudioEngine(string settingsFile) { }
		public AudioEngine(string settingsFile, System.TimeSpan lookAheadTime, string rendererId) { }
		public void Dispose() { }
		public Microsoft.Xna.Framework.Audio.AudioCategory GetCategory(string name) { return default(Microsoft.Xna.Framework.Audio.AudioCategory); }
		public float GetGlobalVariable(string name) { return default(float); }
		public void SetGlobalVariable(string name, float value) { }
		public void Update() { }
	}
	public partial class AudioListener {
		public AudioListener() { }
		public Microsoft.Xna.Framework.Vector3 Forward { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector3 Position { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector3 Up { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector3 Velocity { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Vector3); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public enum AudioStopOptions {
		AsAuthored = 0,
		Immediate = 1,
	}
	public partial class Cue : System.IDisposable {
		internal Cue() { }
		public bool IsDisposed { get { return default(bool); } }
		public bool IsPaused { get { return default(bool); } }
		public bool IsPlaying { get { return default(bool); } }
		public bool IsStopped { get { return default(bool); } }
		public string Name { get { return default(string); } }
		public void Apply3D(Microsoft.Xna.Framework.Audio.AudioListener listener, Microsoft.Xna.Framework.Audio.AudioEmitter emitter) { }
		public void Dispose() { }
		public float GetVariable(string name, float value) { return default(float); }
		public void Pause() { }
		public void Play() { }
		public void Resume() { }
		public void SetVariable(string name, float value) { }
		public void Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions options) { }
	}
	public partial class SoundBank : System.IDisposable {
		public SoundBank(Microsoft.Xna.Framework.Audio.AudioEngine audioEngine, string fileName) { }
		public void Dispose() { }
		public Microsoft.Xna.Framework.Audio.Cue GetCue(string name) { return default(Microsoft.Xna.Framework.Audio.Cue); }
		public void PlayCue(string name) { }
	}
	public sealed partial class SoundEffect : System.IDisposable {
		public SoundEffect(System.Byte[] buffer, int sampleRate, Microsoft.Xna.Framework.Audio.AudioChannels channels) { }
		public SoundEffect(System.Byte[] buffer, int offset, int count, int sampleRate, Microsoft.Xna.Framework.Audio.AudioChannels channels, int loopStart, int loopLength) { }
		public static float DistanceScale { get { return default(float); } set { } }
		public static float DopplerScale { get { return default(float); } set { } }
		public System.TimeSpan Duration { get { return default(System.TimeSpan); } }
		public bool IsDisposed { get { return default(bool); } }
		public static float MasterVolume { get { return default(float); } set { } }
		public string Name { get { return default(string); } set { } }
		public static float SpeedOfSound { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Audio.SoundEffectInstance CreateInstance() { return default(Microsoft.Xna.Framework.Audio.SoundEffectInstance); }
		public void Dispose() { }
		public static Microsoft.Xna.Framework.Audio.SoundEffect FromStream(System.IO.Stream s) { return default(Microsoft.Xna.Framework.Audio.SoundEffect); }
		public static System.TimeSpan GetSampleDuration(int sizeInBytes, int sampleRate, Microsoft.Xna.Framework.Audio.AudioChannels channels) { return default(System.TimeSpan); }
		public static int GetSampleSizeInBytes(System.TimeSpan duration, int sampleRate, Microsoft.Xna.Framework.Audio.AudioChannels channels) { return default(int); }
		public bool Play() { return default(bool); }
		public bool Play(float volume, float pitch, float pan) { return default(bool); }
	}
	public sealed partial class SoundEffectInstance : System.IDisposable {
		internal SoundEffectInstance() { }
		public bool IsDisposed { get { return default(bool); } }
		public bool IsLooped { get { return default(bool); } set { } }
		public float Pan { get { return default(float); } set { } }
		public float Pitch { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Audio.SoundState State { get { return default(Microsoft.Xna.Framework.Audio.SoundState); } }
		public float Volume { get { return default(float); } set { } }
		public void Apply3D(Microsoft.Xna.Framework.Audio.AudioListener listener, Microsoft.Xna.Framework.Audio.AudioEmitter emitter) { }
		public void Apply3D(Microsoft.Xna.Framework.Audio.AudioListener[] listeners, Microsoft.Xna.Framework.Audio.AudioEmitter emitter) { }
		public void Dispose() { }
		public void Pause() { }
		public void Play() { }
		public void Resume() { }
		public void Stop() { }
		public void Stop(bool immediate) { }
	}
	public enum SoundState {
		Paused = 1,
		Playing = 0,
		Stopped = 2,
	}
	public partial class WaveBank : System.IDisposable {
		public WaveBank(Microsoft.Xna.Framework.Audio.AudioEngine audioEngine, string nonStreamingWaveBankFilename) { }
		public WaveBank(Microsoft.Xna.Framework.Audio.AudioEngine audioEngine, string streamingWaveBankFilename, int offset, short packetsize) { }
		public void Dispose() { }
	}
}
namespace Microsoft.Xna.Framework.Content {
	public partial class ArrayReader<T> : Microsoft.Xna.Framework.Content.ContentTypeReader<T[]> {
		public ArrayReader() { }
		protected internal override void Initialize(Microsoft.Xna.Framework.Content.ContentTypeReaderManager manager) { }
		protected internal override T[] Read(Microsoft.Xna.Framework.Content.ContentReader input, T[] existingInstance) { return default(T[]); }
	}
	public static partial class ContentExtensions {
		public static System.Reflection.FieldInfo[] GetAllFields(this System.Type type) { return default(System.Reflection.FieldInfo[]); }
		public static System.Reflection.PropertyInfo[] GetAllProperties(this System.Type type) { return default(System.Reflection.PropertyInfo[]); }
		public static System.Reflection.ConstructorInfo GetDefaultConstructor(this System.Type type) { return default(System.Reflection.ConstructorInfo); }
	}
	public partial class ContentLoadException : System.Exception {
		public ContentLoadException() { }
		public ContentLoadException(string message) { }
		public ContentLoadException(string message, System.Exception innerException) { }
	}
	public partial class ContentManager : System.IDisposable {
		public ContentManager(System.IServiceProvider serviceProvider) { }
		public ContentManager(System.IServiceProvider serviceProvider, string rootDirectory) { }
		protected virtual System.Collections.Generic.Dictionary<System.String, System.Object> LoadedAssets { get { return default(System.Collections.Generic.Dictionary<System.String, System.Object>); } }
		public string RootDirectory { get { return default(string); } set { } }
		public System.IServiceProvider ServiceProvider { get { return default(System.IServiceProvider); } }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		~ContentManager() { }
		public virtual T Load<T>(string assetName) { return default(T); }
		protected virtual string Normalize<T>(string assetName) { return default(string); }
		protected virtual System.IO.Stream OpenStream(string assetName) { return default(System.IO.Stream); }
		protected T ReadAsset<T>(string assetName, System.Action<System.IDisposable> recordDisposableObject) { return default(T); }
		protected virtual object ReadRawAsset<T>(string assetName, string originalAssetName) { return default(object); }
		protected virtual void ReloadAsset<T>(string originalAssetName, T currentAsset) { }
		protected virtual void ReloadGraphicsAssets() { }
		protected virtual void ReloadRawAsset<T>(T asset, string assetName, string originalAssetName) { }
		public virtual void Unload() { }
	}
	public sealed partial class ContentReader : System.IO.BinaryReader {
		internal ContentReader() : base (default(System.IO.Stream)) { }
		public string AssetName { get { return default(string); } }
		public Microsoft.Xna.Framework.Content.ContentManager ContentManager { get { return default(Microsoft.Xna.Framework.Content.ContentManager); } }
		public Microsoft.Xna.Framework.Color ReadColor() { return default(Microsoft.Xna.Framework.Color); }
		public T ReadExternalReference<T>() { return default(T); }
		public Microsoft.Xna.Framework.Matrix ReadMatrix() { return default(Microsoft.Xna.Framework.Matrix); }
		public T ReadObject<T>() { return default(T); }
		public T ReadObject<T>(T existingInstance) { return default(T); }
		public T ReadObject<T>(Microsoft.Xna.Framework.Content.ContentTypeReader typeReader) { return default(T); }
		public T ReadObject<T>(Microsoft.Xna.Framework.Content.ContentTypeReader typeReader, T existingInstance) { return default(T); }
		public Microsoft.Xna.Framework.Quaternion ReadQuaternion() { return default(Microsoft.Xna.Framework.Quaternion); }
		public T ReadRawObject<T>() { return default(T); }
		public T ReadRawObject<T>(T existingInstance) { return default(T); }
		public T ReadRawObject<T>(Microsoft.Xna.Framework.Content.ContentTypeReader typeReader) { return default(T); }
		public T ReadRawObject<T>(Microsoft.Xna.Framework.Content.ContentTypeReader typeReader, T existingInstance) { return default(T); }
		public void ReadSharedResource<T>(System.Action<T> fixup) { }
		public Microsoft.Xna.Framework.Vector2 ReadVector2() { return default(Microsoft.Xna.Framework.Vector2); }
		public Microsoft.Xna.Framework.Vector3 ReadVector3() { return default(Microsoft.Xna.Framework.Vector3); }
		public Microsoft.Xna.Framework.Vector4 ReadVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	[System.AttributeUsageAttribute((System.AttributeTargets)(384))]
	public sealed partial class ContentSerializerAttribute : System.Attribute {
		public ContentSerializerAttribute() { }
		public bool AllowNull { get { return default(bool); } set { } }
		public string CollectionItemName { get { return default(string); } set { } }
		public string ElementName { get { return default(string); } set { } }
		public bool FlattenContent { get { return default(bool); } set { } }
		public bool HasCollectionItemName { get { return default(bool); } }
		public bool Optional { get { return default(bool); } set { } }
		public bool SharedResource { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Content.ContentSerializerAttribute Clone() { return default(Microsoft.Xna.Framework.Content.ContentSerializerAttribute); }
	}
	[System.AttributeUsageAttribute((System.AttributeTargets)(384))]
	public sealed partial class ContentSerializerIgnoreAttribute : System.Attribute {
		public ContentSerializerIgnoreAttribute() { }
	}
	public abstract partial class ContentTypeReader {
		protected ContentTypeReader(System.Type targetType) { }
		public System.Type TargetType { get { return default(System.Type); } }
		public virtual int TypeVersion { get { return default(int); } }
		protected internal virtual void Initialize(Microsoft.Xna.Framework.Content.ContentTypeReaderManager manager) { }
		public static string Normalize(string fileName, System.String[] extensions) { return default(string); }
		protected internal abstract object Read(Microsoft.Xna.Framework.Content.ContentReader input, object existingInstance);
	}
	public abstract partial class ContentTypeReader<T> : Microsoft.Xna.Framework.Content.ContentTypeReader {
		protected ContentTypeReader() : base (default(System.Type)) { }
		protected internal abstract T Read(Microsoft.Xna.Framework.Content.ContentReader input, T existingInstance);
		protected internal override object Read(Microsoft.Xna.Framework.Content.ContentReader input, object existingInstance) { return default(object); }
	}
	public sealed partial class ContentTypeReaderManager {
		public ContentTypeReaderManager(Microsoft.Xna.Framework.Content.ContentReader reader) { }
		public static void AddTypeCreator(string typeString, System.Func<Microsoft.Xna.Framework.Content.ContentTypeReader> createFunction) { }
		public static void ClearTypeCreators() { }
		public Microsoft.Xna.Framework.Content.ContentTypeReader GetTypeReader(System.Type targetType) { return default(Microsoft.Xna.Framework.Content.ContentTypeReader); }
		public static string PrepareType(string type) { return default(string); }
	}
	public partial class DictionaryReader<TKey, TValue> : Microsoft.Xna.Framework.Content.ContentTypeReader<System.Collections.Generic.Dictionary<TKey, TValue>> {
		public DictionaryReader() { }
		protected internal override void Initialize(Microsoft.Xna.Framework.Content.ContentTypeReaderManager manager) { }
		protected internal override System.Collections.Generic.Dictionary<TKey, TValue> Read(Microsoft.Xna.Framework.Content.ContentReader input, System.Collections.Generic.Dictionary<TKey, TValue> existingInstance) { return default(System.Collections.Generic.Dictionary<TKey, TValue>); }
	}
	public partial class EnumReader<T> : Microsoft.Xna.Framework.Content.ContentTypeReader<T> {
		public EnumReader() { }
		protected internal override void Initialize(Microsoft.Xna.Framework.Content.ContentTypeReaderManager manager) { }
		protected internal override T Read(Microsoft.Xna.Framework.Content.ContentReader input, T existingInstance) { return default(T); }
	}
	public partial class ListReader<T> : Microsoft.Xna.Framework.Content.ContentTypeReader<System.Collections.Generic.List<T>> {
		public ListReader() { }
		protected internal override void Initialize(Microsoft.Xna.Framework.Content.ContentTypeReaderManager manager) { }
		protected internal override System.Collections.Generic.List<T> Read(Microsoft.Xna.Framework.Content.ContentReader input, System.Collections.Generic.List<T> existingInstance) { return default(System.Collections.Generic.List<T>); }
	}
	public partial class ModelReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Microsoft.Xna.Framework.Graphics.Model> {
		public ModelReader() { }
		protected internal override Microsoft.Xna.Framework.Graphics.Model Read(Microsoft.Xna.Framework.Content.ContentReader reader, Microsoft.Xna.Framework.Graphics.Model existingInstance) { return default(Microsoft.Xna.Framework.Graphics.Model); }
	}
	public partial class TextureReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Microsoft.Xna.Framework.Graphics.Texture> {
		public TextureReader() { }
		protected internal override Microsoft.Xna.Framework.Graphics.Texture Read(Microsoft.Xna.Framework.Content.ContentReader reader, Microsoft.Xna.Framework.Graphics.Texture existingInstance) { return default(Microsoft.Xna.Framework.Graphics.Texture); }
	}
	public partial class VertexDeclarationReader : Microsoft.Xna.Framework.Content.ContentTypeReader<Microsoft.Xna.Framework.Graphics.VertexDeclaration> {
		public VertexDeclarationReader() { }
		protected internal override Microsoft.Xna.Framework.Graphics.VertexDeclaration Read(Microsoft.Xna.Framework.Content.ContentReader reader, Microsoft.Xna.Framework.Graphics.VertexDeclaration existingInstance) { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); }
	}
}
namespace Microsoft.Xna.Framework.GamerServices {
	[System.Runtime.Serialization.DataContractAttribute]
	public partial class Achievement {
		public Achievement() { }
		[System.Runtime.Serialization.DataMemberAttribute]
		public string Description { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public bool DisplayBeforeEarned { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public System.DateTime EarnedDateTime { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.DateTime); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public bool EarnedOnline { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public int GamerScore { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public string HowToEarn { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public bool IsEarned { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public string Key { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
	}
	public partial class AchievementCollection : System.Collections.Generic.ICollection<Microsoft.Xna.Framework.GamerServices.Achievement>, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.Achievement>, System.Collections.Generic.IList<Microsoft.Xna.Framework.GamerServices.Achievement>, System.Collections.IEnumerable, System.IDisposable {
		public AchievementCollection() { }
		public int Count { get { return default(int); } }
		public bool IsReadOnly { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.Achievement this[int index] { get { return default(Microsoft.Xna.Framework.GamerServices.Achievement); } set { } }
		public void Add(Microsoft.Xna.Framework.GamerServices.Achievement item) { }
		public void Clear() { }
		public bool Contains(Microsoft.Xna.Framework.GamerServices.Achievement item) { return default(bool); }
		public void CopyTo(Microsoft.Xna.Framework.GamerServices.Achievement[] array, int arrayIndex) { }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		~AchievementCollection() { }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.GamerServices.Achievement> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.GamerServices.Achievement>); }
		public int IndexOf(Microsoft.Xna.Framework.GamerServices.Achievement item) { return default(int); }
		public void Insert(int index, Microsoft.Xna.Framework.GamerServices.Achievement item) { }
		public bool Remove(Microsoft.Xna.Framework.GamerServices.Achievement item) { return default(bool); }
		public void RemoveAt(int index) { }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public partial class FriendCollection : System.Collections.Generic.ICollection<Microsoft.Xna.Framework.GamerServices.FriendGamer>, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.FriendGamer>, System.Collections.Generic.IList<Microsoft.Xna.Framework.GamerServices.FriendGamer>, System.Collections.IEnumerable, System.IDisposable {
		public FriendCollection() { }
		public int Count { get { return default(int); } }
		public bool IsReadOnly { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.FriendGamer this[int index] { get { return default(Microsoft.Xna.Framework.GamerServices.FriendGamer); } set { } }
		public void Add(Microsoft.Xna.Framework.GamerServices.FriendGamer item) { }
		public void Clear() { }
		public bool Contains(Microsoft.Xna.Framework.GamerServices.FriendGamer item) { return default(bool); }
		public void CopyTo(Microsoft.Xna.Framework.GamerServices.FriendGamer[] array, int arrayIndex) { }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		~FriendCollection() { }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.GamerServices.FriendGamer> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.GamerServices.FriendGamer>); }
		public int IndexOf(Microsoft.Xna.Framework.GamerServices.FriendGamer item) { return default(int); }
		public void Insert(int index, Microsoft.Xna.Framework.GamerServices.FriendGamer item) { }
		public bool Remove(Microsoft.Xna.Framework.GamerServices.FriendGamer item) { return default(bool); }
		public void RemoveAt(int index) { }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public partial class FriendGamer : Microsoft.Xna.Framework.GamerServices.Gamer {
		public FriendGamer() { }
	}
	public sealed partial class GameDefaults {
		public GameDefaults() { }
		public bool AccelerateWithButtons { get { return default(bool); } }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	public abstract partial class Gamer {
		protected Gamer() { }
		[System.Runtime.Serialization.DataMemberAttribute]
		public string DisplayName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public string Gamertag { get { return default(string); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public bool IsDisposed { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.LeaderboardWriter LeaderboardWriter { get { return default(Microsoft.Xna.Framework.GamerServices.LeaderboardWriter); } }
		public static Microsoft.Xna.Framework.GamerServices.SignedInGamerCollection SignedInGamers { get { return default(Microsoft.Xna.Framework.GamerServices.SignedInGamerCollection); } }
		public object Tag { get { return default(object); } set { } }
		public override string ToString() { return default(string); }
	}
	public partial class GamerCollection<T> : System.Collections.ObjectModel.ReadOnlyCollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable where T : Microsoft.Xna.Framework.GamerServices.Gamer {
		internal GamerCollection() : base (default(System.Collections.Generic.IList<T>)) { }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public sealed partial class GamerPresence {
		public GamerPresence() { }
		public Microsoft.Xna.Framework.GamerServices.GamerPresenceMode PresenceMode { get { return default(Microsoft.Xna.Framework.GamerServices.GamerPresenceMode); } set { } }
		public int PresenceValue { get { return default(int); } set { } }
	}
	public enum GamerPresenceMode {
		ArcadeMode = 0,
		AtMenu = 1,
		BattlingBoss = 2,
		CampaignMode = 3,
		ChallengeMode = 4,
		ConfiguringSettings = 5,
		CoOpLevel = 6,
		CoOpStage = 7,
		CornflowerBlue = 8,
		CustomizingPlayer = 9,
		DifficultyEasy = 10,
		DifficultyExtreme = 11,
		DifficultyHard = 12,
		DifficultyMedium = 13,
		EditingLevel = 14,
		ExplorationMode = 15,
		FoundSecret = 16,
		FreePlay = 17,
		GameOver = 18,
		InCombat = 19,
		InGameStore = 20,
		Level = 21,
		LocalCoOp = 22,
		LocalVersus = 23,
		LookingForGames = 24,
		Losing = 25,
		Multiplayer = 26,
		NearlyFinished = 27,
		None = 28,
		OnARoll = 29,
		OnlineCoOp = 30,
		OnlineVersus = 31,
		Outnumbered = 32,
		Paused = 33,
		PlayingMinigame = 34,
		PlayingWithFriends = 35,
		PracticeMode = 36,
		PuzzleMode = 37,
		ScenarioMode = 38,
		Score = 39,
		ScoreIsTied = 40,
		SettingUpMatch = 41,
		SinglePlayer = 42,
		Stage = 43,
		StartingGame = 44,
		StoryMode = 45,
		StuckOnAHardBit = 46,
		SurvivalMode = 47,
		TimeAttack = 48,
		TryingForRecord = 49,
		TutorialMode = 50,
		VersusComputer = 51,
		VersusScore = 52,
		WaitingForPlayers = 53,
		WaitingInLobby = 54,
		WastingTime = 55,
		WatchingCredits = 56,
		WatchingCutscene = 57,
		Winning = 58,
		WonTheGame = 59,
	}
	public partial class GamerPrivilegeException : System.Exception {
		public GamerPrivilegeException() { }
	}
	public partial class GamerPrivileges {
		public GamerPrivileges() { }
		public Microsoft.Xna.Framework.GamerServices.GamerPrivilegeSetting AllowCommunication { get { return default(Microsoft.Xna.Framework.GamerServices.GamerPrivilegeSetting); } }
		public bool AllowOnlineSessions { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.GamerPrivilegeSetting AllowProfileViewing { get { return default(Microsoft.Xna.Framework.GamerServices.GamerPrivilegeSetting); } }
		public bool AllowPurchaseContent { get { return default(bool); } }
		public bool AllowTradeContent { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.GamerPrivilegeSetting AllowUserCreatedContent { get { return default(Microsoft.Xna.Framework.GamerServices.GamerPrivilegeSetting); } }
	}
	public enum GamerPrivilegeSetting {
		Blocked = 0,
		Everyone = 1,
		FriendsOnly = 2,
	}
	public sealed partial class GamerProfile : System.IDisposable {
		public GamerProfile() { }
		public void Dispose() { }
		~GamerProfile() { }
	}
	public partial class GamerServicesComponent : Microsoft.Xna.Framework.GameComponent {
		public GamerServicesComponent(Microsoft.Xna.Framework.Game game) : base (default(Microsoft.Xna.Framework.Game)) { }
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
	}
	public static partial class GamerServicesDispatcher {
		public static bool IsInitialized { get { return default(bool); } }
		public static System.IntPtr WindowHandle { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.IntPtr); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static event System.EventHandler<System.EventArgs> InstallingTitleUpdate { add { } remove { } }
		public static void Update() { }
	}
	public enum GamerZone {
		Family = 0,
		Pro = 1,
		Recreation = 2,
		Underground = 3,
		Unknown = 4,
	}
	public static partial class Guide {
		public static bool IsScreenSaverEnabled { get { return default(bool); } set { } }
		public static bool IsTrialMode { get { return default(bool); } set { } }
		public static bool IsVisible { get { return default(bool); } set { } }
		public static bool SimulateTrialMode { get { return default(bool); } set { } }
		[System.CLSCompliantAttribute(false)]
		public static System.IAsyncResult BeginShowKeyboardInput(Microsoft.Xna.Framework.PlayerIndex player, string title, string description, string defaultText, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowKeyboardInput(Microsoft.Xna.Framework.PlayerIndex player, string title, string description, string defaultText, System.AsyncCallback callback, object state, bool usePasswordMode) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowMessageBox(Microsoft.Xna.Framework.PlayerIndex player, string title, string text, System.Collections.Generic.IEnumerable<System.String> buttons, int focusButton, Microsoft.Xna.Framework.GamerServices.MessageBoxIcon icon, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowMessageBox(string title, string text, System.Collections.Generic.IEnumerable<System.String> buttons, int focusButton, Microsoft.Xna.Framework.GamerServices.MessageBoxIcon icon, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowStorageDeviceSelector(System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static string EndShowKeyboardInput(System.IAsyncResult result) { return default(string); }
		public static System.Nullable<System.Int32> EndShowMessageBox(System.IAsyncResult result) { return default(System.Nullable<System.Int32>); }
		public static Microsoft.Xna.Framework.Storage.StorageDevice EndShowStorageDeviceSelector(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.Storage.StorageDevice); }
		public static void Show() { }
		public static void ShowAchievements() { }
		public static string ShowKeyboardInput(Microsoft.Xna.Framework.PlayerIndex player, string title, string description, string defaultText, bool usePasswordMode) { return default(string); }
		public static void ShowLeaderboard() { }
		public static void ShowMarketplace(Microsoft.Xna.Framework.PlayerIndex player) { }
		public static System.Nullable<System.Int32> ShowMessageBox(string title, string text, System.Collections.Generic.IEnumerable<System.String> buttons, int focusButton, Microsoft.Xna.Framework.GamerServices.MessageBoxIcon icon) { return default(System.Nullable<System.Int32>); }
		public static void ShowSignIn(int paneCount, bool onlineOnly) { }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	public partial class GuideAlreadyVisibleException : System.Exception {
		public GuideAlreadyVisibleException() { }
		public GuideAlreadyVisibleException(string message) { }
		public GuideAlreadyVisibleException(string message, System.Exception innerException) { }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	public sealed partial class LeaderboardEntry {
		public LeaderboardEntry() { }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.GamerServices.PropertyDictionary Columns { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.GamerServices.PropertyDictionary); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.GamerServices.Gamer Gamer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.GamerServices.Gamer); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public long Rating { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(long); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
	public partial struct LeaderboardIdentity {
		[System.Runtime.Serialization.DataMemberAttribute]
		public int GameMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.GamerServices.LeaderboardKey Key { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.GamerServices.LeaderboardKey); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.GamerServices.LeaderboardIdentity Create(Microsoft.Xna.Framework.GamerServices.LeaderboardKey aKey) { return default(Microsoft.Xna.Framework.GamerServices.LeaderboardIdentity); }
		public static Microsoft.Xna.Framework.GamerServices.LeaderboardIdentity Create(Microsoft.Xna.Framework.GamerServices.LeaderboardKey aKey, int aGameMode) { return default(Microsoft.Xna.Framework.GamerServices.LeaderboardIdentity); }
	}
	public enum LeaderboardKey {
		BestScoreLifeTime = 0,
		BestScoreRecent = 1,
		BestTimeLifeTime = 2,
		BestTimeRecent = 3,
	}
	public sealed partial class LeaderboardReader : System.IDisposable {
		public LeaderboardReader() { }
		public bool CanPageDown { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool CanPageUp { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.LeaderboardEntry> Entries { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.LeaderboardEntry>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public void Dispose() { }
	}
	public sealed partial class LeaderboardWriter : System.IDisposable {
		public LeaderboardWriter() { }
		void System.IDisposable.Dispose() { }
	}
	public enum MessageBoxIcon {
		Alert = 0,
		Error = 1,
		None = 2,
		Warning = 3,
	}
	public partial class MonoGameGamerServicesComponent : Microsoft.Xna.Framework.GamerServices.GamerServicesComponent {
		public MonoGameGamerServicesComponent(Microsoft.Xna.Framework.Game game) : base (default(Microsoft.Xna.Framework.Game)) { }
	}
	public partial class PropertyDictionary {
		public PropertyDictionary() { }
		public System.DateTime GetValueDateTime(string aKey) { return default(System.DateTime); }
		public int GetValueInt32(string aKey) { return default(int); }
		public void SetValue(string aKey, System.DateTime aValue) { }
	}
	public partial class SignedInEventArgs : System.EventArgs {
		public SignedInEventArgs(Microsoft.Xna.Framework.GamerServices.SignedInGamer gamer) { }
	}
	public partial class SignedInGamer : Microsoft.Xna.Framework.GamerServices.Gamer {
		public SignedInGamer() { }
		public Microsoft.Xna.Framework.GamerServices.GameDefaults GameDefaults { get { return default(Microsoft.Xna.Framework.GamerServices.GameDefaults); } }
		public bool IsGuest { get { return default(bool); } }
		public bool IsSignedInToLive { get { return default(bool); } }
		public int PartySize { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.PlayerIndex PlayerIndex { get { return default(Microsoft.Xna.Framework.PlayerIndex); } }
		public Microsoft.Xna.Framework.GamerServices.GamerPresence Presence { get { return default(Microsoft.Xna.Framework.GamerServices.GamerPresence); } }
		public Microsoft.Xna.Framework.GamerServices.GamerPrivileges Privileges { get { return default(Microsoft.Xna.Framework.GamerServices.GamerPrivileges); } }
		public static event System.EventHandler<Microsoft.Xna.Framework.GamerServices.SignedInEventArgs> SignedIn { add { } remove { } }
		public static event System.EventHandler<Microsoft.Xna.Framework.GamerServices.SignedOutEventArgs> SignedOut { add { } remove { } }
		public void AwardAchievement(string achievementId) { }
		public void AwardAchievement(string achievementId, double percentageComplete) { }
		public System.IAsyncResult BeginAuthentication(System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public System.IAsyncResult BeginAwardAchievement(string achievementId, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public System.IAsyncResult BeginAwardAchievement(string achievementId, double percentageComplete, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public System.IAsyncResult BeginGetAchievements(System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public void DoAwardAchievement(string achievementId, double percentageComplete) { }
		public void EndAuthentication(System.IAsyncResult result) { }
		public void EndAwardAchievement(System.IAsyncResult result) { }
		public Microsoft.Xna.Framework.GamerServices.AchievementCollection EndGetAchievements(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.GamerServices.AchievementCollection); }
		public Microsoft.Xna.Framework.GamerServices.AchievementCollection GetAchievements() { return default(Microsoft.Xna.Framework.GamerServices.AchievementCollection); }
		public Microsoft.Xna.Framework.GamerServices.FriendCollection GetFriends() { return default(Microsoft.Xna.Framework.GamerServices.FriendCollection); }
		public bool IsFriend(Microsoft.Xna.Framework.GamerServices.Gamer gamer) { return default(bool); }
		protected virtual void OnSignedIn(Microsoft.Xna.Framework.GamerServices.SignedInEventArgs e) { }
		protected virtual void OnSignedOut(Microsoft.Xna.Framework.GamerServices.SignedOutEventArgs e) { }
		public void ResetAchievements() { }
		public void UpdateScore(string aCategory, long aScore) { }
	}
	public partial class SignedInGamerCollection : System.Collections.Generic.List<Microsoft.Xna.Framework.GamerServices.SignedInGamer> {
		public SignedInGamerCollection() { }
		public Microsoft.Xna.Framework.GamerServices.SignedInGamer this[Microsoft.Xna.Framework.PlayerIndex index] { get { return default(Microsoft.Xna.Framework.GamerServices.SignedInGamer); } }
	}
	public partial class SignedOutEventArgs : System.EventArgs {
		public SignedOutEventArgs(Microsoft.Xna.Framework.GamerServices.SignedInGamer gamer) { }
	}
}
namespace Microsoft.Xna.Framework.Graphics {
	public partial class AlphaTestEffect : Microsoft.Xna.Framework.Graphics.Effect, Microsoft.Xna.Framework.Graphics.IEffectFog, Microsoft.Xna.Framework.Graphics.IEffectMatrices {
		protected AlphaTestEffect(Microsoft.Xna.Framework.Graphics.AlphaTestEffect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public AlphaTestEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public float Alpha { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Graphics.CompareFunction AlphaFunction { get { return default(Microsoft.Xna.Framework.Graphics.CompareFunction); } set { } }
		public Microsoft.Xna.Framework.Vector3 DiffuseColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 FogColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public bool FogEnabled { get { return default(bool); } set { } }
		public float FogEnd { get { return default(float); } set { } }
		public float FogStart { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Matrix Projection { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public int ReferenceAlpha { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } set { } }
		public bool VertexColorEnabled { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Matrix View { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Matrix World { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public override Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		protected internal override bool OnApply() { return default(bool); }
	}
	public partial class BasicEffect : Microsoft.Xna.Framework.Graphics.Effect, Microsoft.Xna.Framework.Graphics.IEffectFog, Microsoft.Xna.Framework.Graphics.IEffectLights, Microsoft.Xna.Framework.Graphics.IEffectMatrices {
		protected BasicEffect(Microsoft.Xna.Framework.Graphics.BasicEffect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public BasicEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public float Alpha { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 AmbientLightColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 DiffuseColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight0 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight1 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight2 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Vector3 EmissiveColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 FogColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public bool FogEnabled { get { return default(bool); } set { } }
		public float FogEnd { get { return default(float); } set { } }
		public float FogStart { get { return default(float); } set { } }
		public bool LightingEnabled { get { return default(bool); } set { } }
		public bool PreferPerPixelLighting { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Matrix Projection { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Vector3 SpecularColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public float SpecularPower { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } set { } }
		public bool TextureEnabled { get { return default(bool); } set { } }
		public bool VertexColorEnabled { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Matrix View { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Matrix World { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public override Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		public void EnableDefaultLighting() { }
		protected internal override bool OnApply() { return default(bool); }
	}
	public enum Blend {
		BlendFactor = 10,
		DestinationAlpha = 8,
		DestinationColor = 6,
		InverseBlendFactor = 11,
		InverseDestinationAlpha = 9,
		InverseDestinationColor = 7,
		InverseSourceAlpha = 5,
		InverseSourceColor = 3,
		One = 0,
		SourceAlpha = 4,
		SourceAlphaSaturation = 12,
		SourceColor = 2,
		Zero = 1,
	}
	public enum BlendFunction {
		Add = 0,
		Max = 3,
		Min = 4,
		ReverseSubtract = 2,
		Subtract = 1,
	}
	public partial class BlendState : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public BlendState() { }
		public static Microsoft.Xna.Framework.Graphics.BlendState Additive { get { return default(Microsoft.Xna.Framework.Graphics.BlendState); } }
		public static Microsoft.Xna.Framework.Graphics.BlendState AlphaBlend { get { return default(Microsoft.Xna.Framework.Graphics.BlendState); } }
		public Microsoft.Xna.Framework.Graphics.BlendFunction AlphaBlendFunction { get { return default(Microsoft.Xna.Framework.Graphics.BlendFunction); } set { } }
		public Microsoft.Xna.Framework.Graphics.Blend AlphaDestinationBlend { get { return default(Microsoft.Xna.Framework.Graphics.Blend); } set { } }
		public Microsoft.Xna.Framework.Graphics.Blend AlphaSourceBlend { get { return default(Microsoft.Xna.Framework.Graphics.Blend); } set { } }
		public Microsoft.Xna.Framework.Color BlendFactor { get { return default(Microsoft.Xna.Framework.Color); } set { } }
		public Microsoft.Xna.Framework.Graphics.BlendFunction ColorBlendFunction { get { return default(Microsoft.Xna.Framework.Graphics.BlendFunction); } set { } }
		public Microsoft.Xna.Framework.Graphics.Blend ColorDestinationBlend { get { return default(Microsoft.Xna.Framework.Graphics.Blend); } set { } }
		public Microsoft.Xna.Framework.Graphics.Blend ColorSourceBlend { get { return default(Microsoft.Xna.Framework.Graphics.Blend); } set { } }
		public Microsoft.Xna.Framework.Graphics.ColorWriteChannels ColorWriteChannels { get { return default(Microsoft.Xna.Framework.Graphics.ColorWriteChannels); } set { } }
		public Microsoft.Xna.Framework.Graphics.ColorWriteChannels ColorWriteChannels1 { get { return default(Microsoft.Xna.Framework.Graphics.ColorWriteChannels); } set { } }
		public Microsoft.Xna.Framework.Graphics.ColorWriteChannels ColorWriteChannels2 { get { return default(Microsoft.Xna.Framework.Graphics.ColorWriteChannels); } set { } }
		public Microsoft.Xna.Framework.Graphics.ColorWriteChannels ColorWriteChannels3 { get { return default(Microsoft.Xna.Framework.Graphics.ColorWriteChannels); } set { } }
		public bool IndependentBlendEnable { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Graphics.TargetBlendState this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.TargetBlendState); } }
		public int MultiSampleMask { get { return default(int); } set { } }
		public static Microsoft.Xna.Framework.Graphics.BlendState NonPremultiplied { get { return default(Microsoft.Xna.Framework.Graphics.BlendState); } }
		public static Microsoft.Xna.Framework.Graphics.BlendState Opaque { get { return default(Microsoft.Xna.Framework.Graphics.BlendState); } }
	}
	public enum BufferUsage {
		None = 0,
		WriteOnly = 1,
	}
	[System.FlagsAttribute]
	public enum ClearOptions {
		DepthBuffer = 2,
		Stencil = 4,
		Target = 1,
	}
	[System.FlagsAttribute]
	public enum ColorWriteChannels {
		All = 15,
		Alpha = 8,
		Blue = 4,
		Green = 2,
		None = 0,
		Red = 1,
	}
	public enum CompareFunction {
		Always = 0,
		Equal = 4,
		Greater = 6,
		GreaterEqual = 5,
		Less = 2,
		LessEqual = 3,
		Never = 1,
		NotEqual = 7,
	}
	public enum CubeMapFace {
		NegativeX = 1,
		NegativeY = 3,
		NegativeZ = 5,
		PositiveX = 0,
		PositiveY = 2,
		PositiveZ = 4,
	}
	public enum CullMode {
		CullClockwiseFace = 1,
		CullCounterClockwiseFace = 2,
		None = 0,
	}
	public enum DepthFormat {
		Depth16 = 54,
		Depth24 = 51,
		Depth24Stencil8 = 48,
		None = -1,
	}
	public partial class DepthStencilState : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public DepthStencilState() { }
		public Microsoft.Xna.Framework.Graphics.StencilOperation CounterClockwiseStencilDepthBufferFail { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.StencilOperation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.StencilOperation CounterClockwiseStencilFail { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.StencilOperation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.CompareFunction CounterClockwiseStencilFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.CompareFunction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.StencilOperation CounterClockwiseStencilPass { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.StencilOperation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.Graphics.DepthStencilState Default { get { return default(Microsoft.Xna.Framework.Graphics.DepthStencilState); } }
		public bool DepthBufferEnable { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.CompareFunction DepthBufferFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.CompareFunction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool DepthBufferWriteEnable { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.Graphics.DepthStencilState DepthRead { get { return default(Microsoft.Xna.Framework.Graphics.DepthStencilState); } }
		public static Microsoft.Xna.Framework.Graphics.DepthStencilState None { get { return default(Microsoft.Xna.Framework.Graphics.DepthStencilState); } }
		public int ReferenceStencil { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.StencilOperation StencilDepthBufferFail { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.StencilOperation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool StencilEnable { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.StencilOperation StencilFail { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.StencilOperation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.CompareFunction StencilFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.CompareFunction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int StencilMask { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.StencilOperation StencilPass { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.StencilOperation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int StencilWriteMask { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool TwoSidedStencilMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public sealed partial class DirectionalLight {
		public DirectionalLight(Microsoft.Xna.Framework.Graphics.EffectParameter directionParameter, Microsoft.Xna.Framework.Graphics.EffectParameter diffuseColorParameter, Microsoft.Xna.Framework.Graphics.EffectParameter specularColorParameter, Microsoft.Xna.Framework.Graphics.DirectionalLight cloneSource) { }
		public Microsoft.Xna.Framework.Vector3 DiffuseColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 Direction { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public bool Enabled { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Vector3 SpecularColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	public partial class DisplayMode {
		internal DisplayMode() { }
		public float AspectRatio { get { return default(float); } }
		public Microsoft.Xna.Framework.Graphics.SurfaceFormat Format { get { return default(Microsoft.Xna.Framework.Graphics.SurfaceFormat); } }
		public int Height { get { return default(int); } }
		public int RefreshRate { get { return default(int); } }
		public Microsoft.Xna.Framework.Rectangle TitleSafeArea { get { return default(Microsoft.Xna.Framework.Rectangle); } }
		public int Width { get { return default(int); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.DisplayMode left, Microsoft.Xna.Framework.Graphics.DisplayMode right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.DisplayMode left, Microsoft.Xna.Framework.Graphics.DisplayMode right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	public partial class DisplayModeCollection : System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.DisplayMode>, System.Collections.IEnumerable {
		public DisplayModeCollection(System.Collections.Generic.List<Microsoft.Xna.Framework.Graphics.DisplayMode> setmodes) { }
		public System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.DisplayMode> this[Microsoft.Xna.Framework.Graphics.SurfaceFormat format] { get { return default(System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.DisplayMode>); } }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.DisplayMode> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.DisplayMode>); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public partial class DualTextureEffect : Microsoft.Xna.Framework.Graphics.Effect, Microsoft.Xna.Framework.Graphics.IEffectFog, Microsoft.Xna.Framework.Graphics.IEffectMatrices {
		protected DualTextureEffect(Microsoft.Xna.Framework.Graphics.DualTextureEffect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public DualTextureEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public float Alpha { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 DiffuseColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 FogColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public bool FogEnabled { get { return default(bool); } set { } }
		public float FogEnd { get { return default(float); } set { } }
		public float FogStart { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Matrix Projection { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture2 { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } set { } }
		public bool VertexColorEnabled { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Matrix View { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Matrix World { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public override Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		protected internal override bool OnApply() { return default(bool); }
	}
	public partial class DynamicIndexBuffer : Microsoft.Xna.Framework.Graphics.IndexBuffer {
		public DynamicIndexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.IndexElementSize indexElementSize, int indexCount, Microsoft.Xna.Framework.Graphics.BufferUsage usage) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(System.Type), default(int), default(Microsoft.Xna.Framework.Graphics.BufferUsage), default(bool)) { }
		public DynamicIndexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Type indexType, int indexCount, Microsoft.Xna.Framework.Graphics.BufferUsage usage) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(System.Type), default(int), default(Microsoft.Xna.Framework.Graphics.BufferUsage), default(bool)) { }
		public bool IsContentLost { get { return default(bool); } }
		public void SetData<T>(T[] data, int startIndex, int elementCount, Microsoft.Xna.Framework.Graphics.SetDataOptions options) where T : struct { }
		public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, Microsoft.Xna.Framework.Graphics.SetDataOptions options) where T : struct { }
	}
	public partial class DynamicVertexBuffer : Microsoft.Xna.Framework.Graphics.VertexBuffer {
		public DynamicVertexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration, int vertexCount, Microsoft.Xna.Framework.Graphics.BufferUsage bufferUsage) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(Microsoft.Xna.Framework.Graphics.VertexDeclaration), default(int), default(Microsoft.Xna.Framework.Graphics.BufferUsage), default(bool)) { }
		public DynamicVertexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Type type, int vertexCount, Microsoft.Xna.Framework.Graphics.BufferUsage bufferUsage) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(Microsoft.Xna.Framework.Graphics.VertexDeclaration), default(int), default(Microsoft.Xna.Framework.Graphics.BufferUsage), default(bool)) { }
		public bool IsContentLost { get { return default(bool); } }
		public void SetData<T>(T[] data, int startIndex, int elementCount, Microsoft.Xna.Framework.Graphics.SetDataOptions options) where T : struct { }
		public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, Microsoft.Xna.Framework.Graphics.SetDataOptions options) where T : struct { }
	}
	public partial class Effect : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		protected Effect(Microsoft.Xna.Framework.Graphics.Effect cloneSource) { }
		public Effect(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Byte[] effectCode) { }
		public Microsoft.Xna.Framework.Graphics.EffectTechnique CurrentTechnique { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectTechnique); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterCollection Parameters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterCollection); } }
		public Microsoft.Xna.Framework.Graphics.EffectTechniqueCollection Techniques { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectTechniqueCollection); } }
		public virtual Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		protected override void Dispose(bool disposing) { }
		protected internal override void GraphicsDeviceResetting() { }
		protected internal virtual bool OnApply() { return default(bool); }
	}
	public partial class EffectAnnotation {
		internal EffectAnnotation() { }
		public int ColumnCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterClass ParameterClass { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterClass); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterType ParameterType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterType); } }
		public int RowCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public string Semantic { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
	}
	public partial class EffectAnnotationCollection : System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.EffectAnnotation>, System.Collections.IEnumerable {
		internal EffectAnnotationCollection() { }
		public int Count { get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.EffectAnnotation this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.EffectAnnotation); } }
		public Microsoft.Xna.Framework.Graphics.EffectAnnotation this[string name] { get { return default(Microsoft.Xna.Framework.Graphics.EffectAnnotation); } }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectAnnotation> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectAnnotation>); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public partial class EffectMaterial : Microsoft.Xna.Framework.Graphics.Effect {
		public EffectMaterial(Microsoft.Xna.Framework.Graphics.Effect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
	}
	[System.Diagnostics.DebuggerDisplayAttribute("{ParameterClass} {ParameterType} {Name} : {Semantic}")]
	public partial class EffectParameter {
		internal EffectParameter() { }
		public Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection Annotations { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection); } }
		public int ColumnCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterCollection Elements { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterCollection); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterClass ParameterClass { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterClass); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterType ParameterType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterType); } }
		public int RowCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public string Semantic { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameterCollection StructureMembers { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectParameterCollection); } }
		public bool GetValueBoolean() { return default(bool); }
		public int GetValueInt32() { return default(int); }
		public Microsoft.Xna.Framework.Matrix GetValueMatrix() { return default(Microsoft.Xna.Framework.Matrix); }
		public Microsoft.Xna.Framework.Matrix[] GetValueMatrixArray(int count) { return default(Microsoft.Xna.Framework.Matrix[]); }
		public Microsoft.Xna.Framework.Quaternion GetValueQuaternion() { return default(Microsoft.Xna.Framework.Quaternion); }
		public float GetValueSingle() { return default(float); }
		public System.Single[] GetValueSingleArray() { return default(System.Single[]); }
		public string GetValueString() { return default(string); }
		public Microsoft.Xna.Framework.Graphics.Texture2D GetValueTexture2D() { return default(Microsoft.Xna.Framework.Graphics.Texture2D); }
		public Microsoft.Xna.Framework.Graphics.Texture3D GetValueTexture3D() { return default(Microsoft.Xna.Framework.Graphics.Texture3D); }
		public Microsoft.Xna.Framework.Graphics.TextureCube GetValueTextureCube() { return default(Microsoft.Xna.Framework.Graphics.TextureCube); }
		public Microsoft.Xna.Framework.Vector2 GetValueVector2() { return default(Microsoft.Xna.Framework.Vector2); }
		public Microsoft.Xna.Framework.Vector2[] GetValueVector2Array() { return default(Microsoft.Xna.Framework.Vector2[]); }
		public Microsoft.Xna.Framework.Vector3 GetValueVector3() { return default(Microsoft.Xna.Framework.Vector3); }
		public Microsoft.Xna.Framework.Vector3[] GetValueVector3Array() { return default(Microsoft.Xna.Framework.Vector3[]); }
		public Microsoft.Xna.Framework.Vector4 GetValueVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public Microsoft.Xna.Framework.Vector4[] GetValueVector4Array() { return default(Microsoft.Xna.Framework.Vector4[]); }
		public void SetValue(Microsoft.Xna.Framework.Graphics.Texture value) { }
		public void SetValue(Microsoft.Xna.Framework.Matrix value) { }
		public void SetValue(Microsoft.Xna.Framework.Matrix[] value) { }
		public void SetValue(Microsoft.Xna.Framework.Quaternion value) { }
		public void SetValue(Microsoft.Xna.Framework.Vector2 value) { }
		public void SetValue(Microsoft.Xna.Framework.Vector2[] value) { }
		public void SetValue(Microsoft.Xna.Framework.Vector3 value) { }
		public void SetValue(Microsoft.Xna.Framework.Vector3[] value) { }
		public void SetValue(Microsoft.Xna.Framework.Vector4 value) { }
		public void SetValue(Microsoft.Xna.Framework.Vector4[] value) { }
		public void SetValue(bool value) { }
		public void SetValue(int value) { }
		public void SetValue(float value) { }
		public void SetValue(System.Single[] value) { }
		public void SetValueTranspose(Microsoft.Xna.Framework.Matrix value) { }
	}
	public enum EffectParameterClass {
		Matrix = 2,
		Object = 3,
		Scalar = 0,
		Struct = 4,
		Vector = 1,
	}
	public partial class EffectParameterCollection : System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.EffectParameter>, System.Collections.IEnumerable {
		internal EffectParameterCollection() { }
		public int Count { get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameter this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.EffectParameter); } }
		public Microsoft.Xna.Framework.Graphics.EffectParameter this[string name] { get { return default(Microsoft.Xna.Framework.Graphics.EffectParameter); } }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectParameter> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectParameter>); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public enum EffectParameterType {
		Bool = 1,
		Int32 = 2,
		Single = 3,
		String = 4,
		Texture = 5,
		Texture1D = 6,
		Texture2D = 7,
		Texture3D = 8,
		TextureCube = 9,
		Void = 0,
	}
	public partial class EffectPass {
		internal EffectPass() { }
		public Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection Annotations { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public void Apply() { }
	}
	public partial class EffectPassCollection : System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.EffectPass>, System.Collections.IEnumerable {
		internal EffectPassCollection() { }
		public int Count { get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.EffectPass this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.EffectPass); } }
		public Microsoft.Xna.Framework.Graphics.EffectPass this[string name] { get { return default(Microsoft.Xna.Framework.Graphics.EffectPass); } }
		System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectPass> System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.EffectPass>.GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectPass>); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public partial class EffectTechnique {
		internal EffectTechnique() { }
		public Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection Annotations { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectAnnotationCollection); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public Microsoft.Xna.Framework.Graphics.EffectPassCollection Passes { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.EffectPassCollection); } }
	}
	public partial class EffectTechniqueCollection : System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Graphics.EffectTechnique>, System.Collections.IEnumerable {
		internal EffectTechniqueCollection() { }
		public int Count { get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.EffectTechnique this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.EffectTechnique); } }
		public Microsoft.Xna.Framework.Graphics.EffectTechnique this[string name] { get { return default(Microsoft.Xna.Framework.Graphics.EffectTechnique); } }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectTechnique> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.EffectTechnique>); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public partial class EnvironmentMapEffect : Microsoft.Xna.Framework.Graphics.Effect, Microsoft.Xna.Framework.Graphics.IEffectFog, Microsoft.Xna.Framework.Graphics.IEffectLights, Microsoft.Xna.Framework.Graphics.IEffectMatrices {
		protected EnvironmentMapEffect(Microsoft.Xna.Framework.Graphics.EnvironmentMapEffect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public EnvironmentMapEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public float Alpha { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 AmbientLightColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 DiffuseColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight0 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight1 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight2 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Vector3 EmissiveColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Graphics.TextureCube EnvironmentMap { get { return default(Microsoft.Xna.Framework.Graphics.TextureCube); } set { } }
		public float EnvironmentMapAmount { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 EnvironmentMapSpecular { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 FogColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public bool FogEnabled { get { return default(bool); } set { } }
		public float FogEnd { get { return default(float); } set { } }
		public float FogStart { get { return default(float); } set { } }
		public float FresnelFactor { get { return default(float); } set { } }
		bool Microsoft.Xna.Framework.Graphics.IEffectLights.LightingEnabled { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Matrix Projection { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } set { } }
		public Microsoft.Xna.Framework.Matrix View { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Matrix World { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public override Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		public void EnableDefaultLighting() { }
		protected internal override bool OnApply() { return default(bool); }
	}
	public enum FillMode {
		Solid = 0,
		WireFrame = 1,
	}
	public sealed partial class GraphicsAdapter : System.IDisposable {
		internal GraphicsAdapter() { }
		public static System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Graphics.GraphicsAdapter> Adapters { get { return default(System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Graphics.GraphicsAdapter>); } }
		public Microsoft.Xna.Framework.Graphics.DisplayMode CurrentDisplayMode { get { return default(Microsoft.Xna.Framework.Graphics.DisplayMode); } }
		public static Microsoft.Xna.Framework.Graphics.GraphicsAdapter DefaultAdapter { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsAdapter); } }
		public Microsoft.Xna.Framework.Graphics.DisplayModeCollection SupportedDisplayModes { get { return default(Microsoft.Xna.Framework.Graphics.DisplayModeCollection); } }
		public void Dispose() { }
	}
	public partial class GraphicsDevice : System.IDisposable {
		public GraphicsDevice(Microsoft.Xna.Framework.Graphics.GraphicsAdapter adapter, Microsoft.Xna.Framework.Graphics.GraphicsProfile graphicsProfile, Microsoft.Xna.Framework.Graphics.PresentationParameters presentationParameters) { }
		public Microsoft.Xna.Framework.Graphics.GraphicsAdapter Adapter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.GraphicsAdapter); } }
		public Microsoft.Xna.Framework.Graphics.BlendState BlendState { get { return default(Microsoft.Xna.Framework.Graphics.BlendState); } set { } }
		public Microsoft.Xna.Framework.Graphics.DepthStencilState DepthStencilState { get { return default(Microsoft.Xna.Framework.Graphics.DepthStencilState); } set { } }
		public Microsoft.Xna.Framework.Graphics.DisplayMode DisplayMode { get { return default(Microsoft.Xna.Framework.Graphics.DisplayMode); } }
		public Microsoft.Xna.Framework.Graphics.GraphicsDeviceStatus GraphicsDeviceStatus { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsDeviceStatus); } }
		public Microsoft.Xna.Framework.Graphics.GraphicsProfile GraphicsProfile { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsProfile); } }
		public Microsoft.Xna.Framework.Graphics.IndexBuffer Indices { get { return default(Microsoft.Xna.Framework.Graphics.IndexBuffer); } set { } }
		public bool IsContentLost { get { return default(bool); } }
		public bool IsDisposed { get { return default(bool); } }
		public Microsoft.Xna.Framework.Graphics.PresentationParameters PresentationParameters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.PresentationParameters); } }
		public Microsoft.Xna.Framework.Graphics.RasterizerState RasterizerState { get { return default(Microsoft.Xna.Framework.Graphics.RasterizerState); } set { } }
		public int RenderTargetCount { get { return default(int); } }
		public bool ResourcesLost { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.SamplerStateCollection SamplerStates { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.SamplerStateCollection); } }
		public Microsoft.Xna.Framework.Rectangle ScissorRectangle { get { return default(Microsoft.Xna.Framework.Rectangle); } set { } }
		public Microsoft.Xna.Framework.Graphics.TextureCollection Textures { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.TextureCollection); } }
		public Microsoft.Xna.Framework.Graphics.Viewport Viewport { get { return default(Microsoft.Xna.Framework.Graphics.Viewport); } set { } }
		public event System.EventHandler<System.EventArgs> DeviceLost { add { } remove { } }
		public event System.EventHandler<System.EventArgs> DeviceReset { add { } remove { } }
		public event System.EventHandler<System.EventArgs> DeviceResetting { add { } remove { } }
		public event System.EventHandler<System.EventArgs> Disposing { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Graphics.ResourceCreatedEventArgs> ResourceCreated { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Graphics.ResourceDestroyedEventArgs> ResourceDestroyed { add { } remove { } }
		public void Clear(Microsoft.Xna.Framework.Color color) { }
		public void Clear(Microsoft.Xna.Framework.Graphics.ClearOptions options, Microsoft.Xna.Framework.Color color, float depth, int stencil) { }
		public void Clear(Microsoft.Xna.Framework.Graphics.ClearOptions options, Microsoft.Xna.Framework.Vector4 color, float depth, int stencil) { }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		public void DrawIndexedPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount) { }
		public void DrawPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, int vertexStart, int primitiveCount) { }
		public void DrawUserIndexedPrimitives<T>(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, System.Int16[] indexData, int indexOffset, int primitiveCount) where T : struct, Microsoft.Xna.Framework.Graphics.IVertexType { }
		public void DrawUserIndexedPrimitives<T>(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, System.Int16[] indexData, int indexOffset, int primitiveCount, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration) where T : struct { }
		public void DrawUserIndexedPrimitives<T>(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, System.Int32[] indexData, int indexOffset, int primitiveCount) where T : struct, Microsoft.Xna.Framework.Graphics.IVertexType { }
		public void DrawUserIndexedPrimitives<T>(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, System.Int32[] indexData, int indexOffset, int primitiveCount, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration) where T : struct, Microsoft.Xna.Framework.Graphics.IVertexType { }
		public void DrawUserPrimitives<T>(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct, Microsoft.Xna.Framework.Graphics.IVertexType { }
		public void DrawUserPrimitives<T>(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration) where T : struct { }
		~GraphicsDevice() { }
		public Microsoft.Xna.Framework.Graphics.RenderTargetBinding[] GetRenderTargets() { return default(Microsoft.Xna.Framework.Graphics.RenderTargetBinding[]); }
		public void GetRenderTargets(Microsoft.Xna.Framework.Graphics.RenderTargetBinding[] outTargets) { }
		public void PlatformClear(Microsoft.Xna.Framework.Graphics.ClearOptions options, Microsoft.Xna.Framework.Vector4 color, float depth, int stencil) { }
		public void PlatformPresent() { }
		public void Present() { }
		public void SetRenderTarget(Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget) { }
		public void SetRenderTarget(Microsoft.Xna.Framework.Graphics.RenderTargetCube renderTarget, Microsoft.Xna.Framework.Graphics.CubeMapFace cubeMapFace) { }
		public void SetRenderTargets(params Microsoft.Xna.Framework.Graphics.RenderTargetBinding[] renderTargets) { }
		public void SetVertexBuffer(Microsoft.Xna.Framework.Graphics.VertexBuffer vertexBuffer) { }
	}
	public enum GraphicsDeviceStatus {
		Lost = 1,
		Normal = 0,
		NotReset = 2,
	}

	public enum GraphicsProfile {
		HiDef = 1,
		Reach = 0,
	}
	public abstract partial class GraphicsResource : System.IDisposable {
		internal GraphicsResource() { }
		public Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice { get { return default(Microsoft.Xna.Framework.Graphics.GraphicsDevice); } }
		public bool IsDisposed { get { return default(bool); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public object Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public event System.EventHandler<System.EventArgs> Disposing { add { } remove { } }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		~GraphicsResource() { }
		protected internal virtual void GraphicsDeviceResetting() { }
		public override string ToString() { return default(string); }
	}
	public partial interface IEffectFog {
		Microsoft.Xna.Framework.Vector3 FogColor { get; set; }
		bool FogEnabled { get; set; }
		float FogEnd { get; set; }
		float FogStart { get; set; }
	}
	public partial interface IEffectLights {
		Microsoft.Xna.Framework.Vector3 AmbientLightColor { get; set; }
		Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight0 { get; }
		Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight1 { get; }
		Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight2 { get; }
		bool LightingEnabled { get; set; }
		void EnableDefaultLighting();
	}
	public partial interface IEffectMatrices {
		Microsoft.Xna.Framework.Matrix Projection { get; set; }
		Microsoft.Xna.Framework.Matrix View { get; set; }
		Microsoft.Xna.Framework.Matrix World { get; set; }
	}
	public partial interface IGraphicsDeviceService {
		Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice { get; }
		event System.EventHandler<System.EventArgs> DeviceCreated;
		event System.EventHandler<System.EventArgs> DeviceDisposing;
		event System.EventHandler<System.EventArgs> DeviceReset;
		event System.EventHandler<System.EventArgs> DeviceResetting;
	}
	public partial class IndexBuffer : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public IndexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.IndexElementSize indexElementSize, int indexCount, Microsoft.Xna.Framework.Graphics.BufferUsage bufferUsage) { }
		protected IndexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.IndexElementSize indexElementSize, int indexCount, Microsoft.Xna.Framework.Graphics.BufferUsage usage, bool dynamic) { }
		public IndexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Type indexType, int indexCount, Microsoft.Xna.Framework.Graphics.BufferUsage usage) { }
		protected IndexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Type indexType, int indexCount, Microsoft.Xna.Framework.Graphics.BufferUsage usage, bool dynamic) { }
		public Microsoft.Xna.Framework.Graphics.BufferUsage BufferUsage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.BufferUsage); } }
		public int IndexCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.IndexElementSize IndexElementSize { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.IndexElementSize); } }
		protected override void Dispose(bool disposing) { }
		public void GetData<T>(T[] data) where T : struct { }
		public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct { }
		protected internal override void GraphicsDeviceResetting() { }
		public void SetData<T>(T[] data) where T : struct { }
		public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct { }
		protected void SetDataInternal<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, Microsoft.Xna.Framework.Graphics.SetDataOptions options) where T : struct { }
	}
	public enum IndexElementSize {
		SixteenBits = 0,
		ThirtyTwoBits = 1,
	}
	public partial interface IVertexType {
		Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration { get; }
	}
	public partial class Model {
		public Model() { }
		public Model(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Collections.Generic.List<Microsoft.Xna.Framework.Graphics.ModelBone> bones, System.Collections.Generic.List<Microsoft.Xna.Framework.Graphics.ModelMesh> meshes) { }
		public Microsoft.Xna.Framework.Graphics.ModelBoneCollection Bones { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelBoneCollection); } }
		public Microsoft.Xna.Framework.Graphics.ModelMeshCollection Meshes { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelMeshCollection); } }
		public Microsoft.Xna.Framework.Graphics.ModelBone Root { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelBone); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public object Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public void BuildHierarchy() { }
		public void CopyAbsoluteBoneTransformsTo(Microsoft.Xna.Framework.Matrix[] destinationBoneTransforms) { }
		public void Draw(Microsoft.Xna.Framework.Matrix world, Microsoft.Xna.Framework.Matrix view, Microsoft.Xna.Framework.Matrix projection) { }
	}
	public sealed partial class ModelBone {
		public ModelBone() { }
		public Microsoft.Xna.Framework.Graphics.ModelBoneCollection Children { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelBoneCollection); } }
		public int Index { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public System.Collections.Generic.List<Microsoft.Xna.Framework.Graphics.ModelMesh> Meshes { get { return default(System.Collections.Generic.List<Microsoft.Xna.Framework.Graphics.ModelMesh>); } }
		public Microsoft.Xna.Framework.Matrix ModelTransform { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Matrix); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.ModelBone Parent { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelBone); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Matrix Transform { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public void AddChild(Microsoft.Xna.Framework.Graphics.ModelBone modelBone) { }
		public void AddMesh(Microsoft.Xna.Framework.Graphics.ModelMesh mesh) { }
	}
	public partial class ModelBoneCollection : System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Graphics.ModelBone> {
		public ModelBoneCollection(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.ModelBone> list) : base (default(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.ModelBone>)) { }
		public Microsoft.Xna.Framework.Graphics.ModelBone this[string boneName] { get { return default(Microsoft.Xna.Framework.Graphics.ModelBone); } }
		public new Microsoft.Xna.Framework.Graphics.ModelBoneCollection.Enumerator GetEnumerator() { return default(Microsoft.Xna.Framework.Graphics.ModelBoneCollection.Enumerator); }
		public bool TryGetValue(string boneName, out Microsoft.Xna.Framework.Graphics.ModelBone value) { value = default(Microsoft.Xna.Framework.Graphics.ModelBone); return default(bool); }
		[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
		public partial struct Enumerator : System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.ModelBone>, System.Collections.IEnumerator, System.IDisposable {
			public Microsoft.Xna.Framework.Graphics.ModelBone Current { get { return default(Microsoft.Xna.Framework.Graphics.ModelBone); } }
			object System.Collections.IEnumerator.Current { get { return default(object); } }
			public void Dispose() { }
			public bool MoveNext() { return default(bool); }
			public void Reset() { }
		}
	}
	public sealed partial class ModelEffectCollection : System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Graphics.Effect> {
		public ModelEffectCollection(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.Effect> list) : base (default(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.Effect>)) { }
		public new Microsoft.Xna.Framework.Graphics.ModelEffectCollection.Enumerator GetEnumerator() { return default(Microsoft.Xna.Framework.Graphics.ModelEffectCollection.Enumerator); }
		[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
		public partial struct Enumerator : System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.Effect>, System.Collections.IEnumerator, System.IDisposable {
			public Microsoft.Xna.Framework.Graphics.Effect Current { get { return default(Microsoft.Xna.Framework.Graphics.Effect); } }
			object System.Collections.IEnumerator.Current { get { return default(object); } }
			public void Dispose() { }
			public bool MoveNext() { return default(bool); }
			void System.Collections.IEnumerator.Reset() { }
		}
	}
	public sealed partial class ModelMesh {
		public ModelMesh(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Collections.Generic.List<Microsoft.Xna.Framework.Graphics.ModelMeshPart> parts) { }
		public Microsoft.Xna.Framework.BoundingSphere BoundingSphere { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.BoundingSphere); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.ModelEffectCollection Effects { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelEffectCollection); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.ModelMeshPartCollection MeshParts { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelMeshPartCollection); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.ModelBone ParentBone { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ModelBone); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public object Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public void Draw() { }
	}
	public sealed partial class ModelMeshCollection : System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Graphics.ModelMesh> {
		internal ModelMeshCollection() : base (default(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.ModelMesh>)) { }
		public Microsoft.Xna.Framework.Graphics.ModelMesh this[string meshName] { get { return default(Microsoft.Xna.Framework.Graphics.ModelMesh); } }
		public new Microsoft.Xna.Framework.Graphics.ModelMeshCollection.Enumerator GetEnumerator() { return default(Microsoft.Xna.Framework.Graphics.ModelMeshCollection.Enumerator); }
		public bool TryGetValue(string meshName, out Microsoft.Xna.Framework.Graphics.ModelMesh value) { value = default(Microsoft.Xna.Framework.Graphics.ModelMesh); return default(bool); }
		[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
		public partial struct Enumerator : System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Graphics.ModelMesh>, System.Collections.IEnumerator, System.IDisposable {
			public Microsoft.Xna.Framework.Graphics.ModelMesh Current { get { return default(Microsoft.Xna.Framework.Graphics.ModelMesh); } }
			object System.Collections.IEnumerator.Current { get { return default(object); } }
			public void Dispose() { }
			public bool MoveNext() { return default(bool); }
			public void Reset() { }
		}
	}
	public sealed partial class ModelMeshPart {
		public ModelMeshPart() { }
		public Microsoft.Xna.Framework.Graphics.Effect Effect { get { return default(Microsoft.Xna.Framework.Graphics.Effect); } set { } }
		public Microsoft.Xna.Framework.Graphics.IndexBuffer IndexBuffer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.IndexBuffer); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int NumVertices { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int PrimitiveCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int StartIndex { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public object Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.VertexBuffer VertexBuffer { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.VertexBuffer); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int VertexOffset { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public sealed partial class ModelMeshPartCollection : System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Graphics.ModelMeshPart> {
		public ModelMeshPartCollection(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.ModelMeshPart> list) : base (default(System.Collections.Generic.IList<Microsoft.Xna.Framework.Graphics.ModelMeshPart>)) { }
	}
	public partial class MonoGameGLException : System.Exception {
		public MonoGameGLException(string message) { }
	}
	public enum MultiSampleType {
		EightSamples = 8,
		ElevenSamples = 11,
		FifteenSamples = 15,
		FiveSamples = 5,
		FourSamples = 4,
		FourteenSamples = 14,
		NineSamples = 9,
		None = 0,
		NonMaskable = 1,
		SevenSamples = 7,
		SixSamples = 6,
		SixteenSamples = 16,
		TenSamples = 10,
		ThirteenSamples = 13,
		ThreeSamples = 3,
		TwelveSamples = 12,
		TwoSamples = 2,
	}
	public partial class OcclusionQuery : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public OcclusionQuery(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice) { }
		public bool IsComplete { get { return default(bool); } }
		public int PixelCount { get { return default(int); } }
		public void Begin() { }
		protected override void Dispose(bool disposing) { }
		public void End() { }
	}
	public partial class PresentationParameters : System.IDisposable {
		public const int DefaultPresentRate = 60;
		public PresentationParameters() { }
		public Microsoft.Xna.Framework.Graphics.SurfaceFormat BackBufferFormat { get { return default(Microsoft.Xna.Framework.Graphics.SurfaceFormat); } set { } }
		public int BackBufferHeight { get { return default(int); } set { } }
		public int BackBufferWidth { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Rectangle Bounds { get { return default(Microsoft.Xna.Framework.Rectangle); } }
		public Microsoft.Xna.Framework.Graphics.DepthFormat DepthStencilFormat { get { return default(Microsoft.Xna.Framework.Graphics.DepthFormat); } set { } }
		public System.IntPtr DeviceWindowHandle { get { return default(System.IntPtr); } set { } }
		public Microsoft.Xna.Framework.DisplayOrientation DisplayOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.DisplayOrientation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool IsFullScreen { get { return default(bool); } set { } }
		public int MultiSampleCount { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Graphics.PresentInterval PresentationInterval { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.PresentInterval); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.RenderTargetUsage RenderTargetUsage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.RenderTargetUsage); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public void Clear() { }
		public Microsoft.Xna.Framework.Graphics.PresentationParameters Clone() { return default(Microsoft.Xna.Framework.Graphics.PresentationParameters); }
		public void Dispose() { }
		protected virtual void Dispose(bool disposing) { }
		~PresentationParameters() { }
	}
	public enum PresentInterval {
		Default = 0,
		Immediate = 3,
		One = 1,
		Two = 2,
	}
	public enum PrimitiveType {
		LineList = 2,
		LineStrip = 3,
		TriangleList = 0,
		TriangleStrip = 1,
	}
	public partial class RasterizerState : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public RasterizerState() { }
		public static Microsoft.Xna.Framework.Graphics.RasterizerState CullClockwise { get { return default(Microsoft.Xna.Framework.Graphics.RasterizerState); } }
		public static Microsoft.Xna.Framework.Graphics.RasterizerState CullCounterClockwise { get { return default(Microsoft.Xna.Framework.Graphics.RasterizerState); } }
		public Microsoft.Xna.Framework.Graphics.CullMode CullMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.CullMode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.Graphics.RasterizerState CullNone { get { return default(Microsoft.Xna.Framework.Graphics.RasterizerState); } }
		public float DepthBias { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.FillMode FillMode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.FillMode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool MultiSampleAntiAlias { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool ScissorTestEnable { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public float SlopeScaleDepthBias { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public partial class RenderTarget2D : Microsoft.Xna.Framework.Graphics.Texture2D {
		public RenderTarget2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(int)) { }
		public RenderTarget2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat preferredFormat, Microsoft.Xna.Framework.Graphics.DepthFormat preferredDepthFormat) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(int)) { }
		public RenderTarget2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat preferredFormat, Microsoft.Xna.Framework.Graphics.DepthFormat preferredDepthFormat, int preferredMultiSampleCount, Microsoft.Xna.Framework.Graphics.RenderTargetUsage usage) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(int)) { }
		protected RenderTarget2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat format, Microsoft.Xna.Framework.Graphics.DepthFormat depthFormat, int preferredMultiSampleCount, Microsoft.Xna.Framework.Graphics.RenderTargetUsage usage, Microsoft.Xna.Framework.Graphics.Texture2D.SurfaceType surfaceType) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(int)) { }
		public RenderTarget2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat preferredFormat, Microsoft.Xna.Framework.Graphics.DepthFormat preferredDepthFormat, int preferredMultiSampleCount, Microsoft.Xna.Framework.Graphics.RenderTargetUsage usage, bool shared) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(int)) { }
		public Microsoft.Xna.Framework.Graphics.DepthFormat DepthStencilFormat { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.DepthFormat); } }
		public bool IsContentLost { get { return default(bool); } }
		public int MultiSampleCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.RenderTargetUsage RenderTargetUsage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.RenderTargetUsage); } }
		public event System.EventHandler<System.EventArgs> ContentLost { add { } remove { } }
		protected override void Dispose(bool disposing) { }
		protected internal override void GraphicsDeviceResetting() { }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct RenderTargetBinding {
		public RenderTargetBinding(Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget) { throw new System.NotImplementedException(); }
		public RenderTargetBinding(Microsoft.Xna.Framework.Graphics.RenderTargetCube renderTarget, Microsoft.Xna.Framework.Graphics.CubeMapFace cubeMapFace) { throw new System.NotImplementedException(); }
		public int ArraySlice { get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.Texture RenderTarget { get { return default(Microsoft.Xna.Framework.Graphics.Texture); } }
		public static implicit operator Microsoft.Xna.Framework.Graphics.RenderTargetBinding (Microsoft.Xna.Framework.Graphics.RenderTarget2D renderTarget) { return default(Microsoft.Xna.Framework.Graphics.RenderTargetBinding); }
	}
	public partial class RenderTargetCube : Microsoft.Xna.Framework.Graphics.TextureCube {
		public RenderTargetCube(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int size, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat preferredFormat, Microsoft.Xna.Framework.Graphics.DepthFormat preferredDepthFormat) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(bool), default(Microsoft.Xna.Framework.Graphics.SurfaceFormat)) { }
		public RenderTargetCube(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int size, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat preferredFormat, Microsoft.Xna.Framework.Graphics.DepthFormat preferredDepthFormat, int preferredMultiSampleCount, Microsoft.Xna.Framework.Graphics.RenderTargetUsage usage) : base (default(Microsoft.Xna.Framework.Graphics.GraphicsDevice), default(int), default(bool), default(Microsoft.Xna.Framework.Graphics.SurfaceFormat)) { }
		public Microsoft.Xna.Framework.Graphics.DepthFormat DepthStencilFormat { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.DepthFormat); } }
		public int MultiSampleCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.RenderTargetUsage RenderTargetUsage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.RenderTargetUsage); } }
		protected override void Dispose(bool disposing) { }
	}
	public enum RenderTargetUsage {
		DiscardContents = 0,
		PlatformContents = 2,
		PreserveContents = 1,
	}
	public sealed partial class ResourceCreatedEventArgs : System.EventArgs {
		public ResourceCreatedEventArgs() { }
		public object Resource { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
	}
	public sealed partial class ResourceDestroyedEventArgs : System.EventArgs {
		public ResourceDestroyedEventArgs() { }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public object Tag { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(object); } }
	}
	public partial class SamplerState : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public SamplerState() { }
		public Microsoft.Xna.Framework.Graphics.TextureAddressMode AddressU { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.TextureAddressMode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.TextureAddressMode AddressV { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.TextureAddressMode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.TextureAddressMode AddressW { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.TextureAddressMode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.Graphics.SamplerState AnisotropicClamp { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } }
		public static Microsoft.Xna.Framework.Graphics.SamplerState AnisotropicWrap { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } }
		public Microsoft.Xna.Framework.Graphics.TextureFilter Filter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.TextureFilter); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.Graphics.SamplerState LinearClamp { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } }
		public static Microsoft.Xna.Framework.Graphics.SamplerState LinearWrap { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } }
		public int MaxAnisotropy { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int MaxMipLevel { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public float MipMapLevelOfDetailBias { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static Microsoft.Xna.Framework.Graphics.SamplerState PointClamp { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } }
		public static Microsoft.Xna.Framework.Graphics.SamplerState PointWrap { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } }
	}
	public sealed partial class SamplerStateCollection {
		internal SamplerStateCollection() { }
		public Microsoft.Xna.Framework.Graphics.SamplerState this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.SamplerState); } set { } }
	}
	[System.FlagsAttribute]
	public enum SetDataOptions {
		Discard = 1,
		None = 0,
		NoOverwrite = 2,
	}
	public partial class SkinnedEffect : Microsoft.Xna.Framework.Graphics.Effect, Microsoft.Xna.Framework.Graphics.IEffectFog, Microsoft.Xna.Framework.Graphics.IEffectLights, Microsoft.Xna.Framework.Graphics.IEffectMatrices {
		public const int MaxBones = 72;
		public SkinnedEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		protected SkinnedEffect(Microsoft.Xna.Framework.Graphics.SkinnedEffect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public float Alpha { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Vector3 AmbientLightColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 DiffuseColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight0 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight1 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Graphics.DirectionalLight DirectionalLight2 { get { return default(Microsoft.Xna.Framework.Graphics.DirectionalLight); } }
		public Microsoft.Xna.Framework.Vector3 EmissiveColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public Microsoft.Xna.Framework.Vector3 FogColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public bool FogEnabled { get { return default(bool); } set { } }
		public float FogEnd { get { return default(float); } set { } }
		public float FogStart { get { return default(float); } set { } }
		bool Microsoft.Xna.Framework.Graphics.IEffectLights.LightingEnabled { get { return default(bool); } set { } }
		public bool PreferPerPixelLighting { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Matrix Projection { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public Microsoft.Xna.Framework.Vector3 SpecularColor { get { return default(Microsoft.Xna.Framework.Vector3); } set { } }
		public float SpecularPower { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Graphics.Texture2D Texture { get { return default(Microsoft.Xna.Framework.Graphics.Texture2D); } set { } }
		public Microsoft.Xna.Framework.Matrix View { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public int WeightsPerVertex { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Matrix World { get { return default(Microsoft.Xna.Framework.Matrix); } set { } }
		public override Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		public void EnableDefaultLighting() { }
		public Microsoft.Xna.Framework.Matrix[] GetBoneTransforms(int count) { return default(Microsoft.Xna.Framework.Matrix[]); }
		protected internal override bool OnApply() { return default(bool); }
		public void SetBoneTransforms(Microsoft.Xna.Framework.Matrix[] boneTransforms) { }
	}
	public partial class SpriteBatch : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public SpriteBatch(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice) { }
		public void Begin() { }
		public void Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode sortMode, Microsoft.Xna.Framework.Graphics.BlendState blendState) { }
		public void Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode sortMode, Microsoft.Xna.Framework.Graphics.BlendState blendState, Microsoft.Xna.Framework.Graphics.SamplerState samplerState, Microsoft.Xna.Framework.Graphics.DepthStencilState depthStencilState, Microsoft.Xna.Framework.Graphics.RasterizerState rasterizerState) { }
		public void Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode sortMode, Microsoft.Xna.Framework.Graphics.BlendState blendState, Microsoft.Xna.Framework.Graphics.SamplerState samplerState, Microsoft.Xna.Framework.Graphics.DepthStencilState depthStencilState, Microsoft.Xna.Framework.Graphics.RasterizerState rasterizerState, Microsoft.Xna.Framework.Graphics.Effect effect) { }
		public void Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode sortMode, Microsoft.Xna.Framework.Graphics.BlendState blendState, Microsoft.Xna.Framework.Graphics.SamplerState samplerState, Microsoft.Xna.Framework.Graphics.DepthStencilState depthStencilState, Microsoft.Xna.Framework.Graphics.RasterizerState rasterizerState, Microsoft.Xna.Framework.Graphics.Effect effect, Microsoft.Xna.Framework.Matrix transformMatrix) { }
		protected override void Dispose(bool disposing) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Rectangle rectangle, Microsoft.Xna.Framework.Color color) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Rectangle destinationRectangle, System.Nullable<Microsoft.Xna.Framework.Rectangle> sourceRectangle, Microsoft.Xna.Framework.Color color) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Rectangle destinationRectangle, System.Nullable<Microsoft.Xna.Framework.Rectangle> sourceRectangle, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Graphics.SpriteEffects effect, float depth) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Vector2 position, System.Nullable<Microsoft.Xna.Framework.Rectangle> sourceRectangle, Microsoft.Xna.Framework.Color color) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Vector2 position, System.Nullable<Microsoft.Xna.Framework.Rectangle> sourceRectangle, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effect, float depth) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, Microsoft.Xna.Framework.Vector2 position, System.Nullable<Microsoft.Xna.Framework.Rectangle> sourceRectangle, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, float scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effect, float depth) { }
		public void Draw(Microsoft.Xna.Framework.Graphics.Texture2D texture, System.Nullable<Microsoft.Xna.Framework.Vector2> position=null, System.Nullable<Microsoft.Xna.Framework.Rectangle> drawRectangle=null, System.Nullable<Microsoft.Xna.Framework.Rectangle> sourceRectangle=null, System.Nullable<Microsoft.Xna.Framework.Vector2> origin=null, float rotation=0f, System.Nullable<Microsoft.Xna.Framework.Vector2> scale=null, System.Nullable<Microsoft.Xna.Framework.Color> color=null, Microsoft.Xna.Framework.Graphics.SpriteEffects effect=(Microsoft.Xna.Framework.Graphics.SpriteEffects)(0), float depth=0f) { }
		public void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color) { }
		public void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effect, float depth) { }
		public void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, string text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, float scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effects, float depth) { }
		public void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, System.Text.StringBuilder text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color) { }
		public void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, System.Text.StringBuilder text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, Microsoft.Xna.Framework.Vector2 scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effect, float depth) { }
		public void DrawString(Microsoft.Xna.Framework.Graphics.SpriteFont spriteFont, System.Text.StringBuilder text, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Color color, float rotation, Microsoft.Xna.Framework.Vector2 origin, float scale, Microsoft.Xna.Framework.Graphics.SpriteEffects effects, float depth) { }
		public void End() { }
	}
	public partial class SpriteEffect : Microsoft.Xna.Framework.Graphics.Effect {
		public SpriteEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		protected SpriteEffect(Microsoft.Xna.Framework.Graphics.SpriteEffect cloneSource) : base (default(Microsoft.Xna.Framework.Graphics.Effect)) { }
		public override Microsoft.Xna.Framework.Graphics.Effect Clone() { return default(Microsoft.Xna.Framework.Graphics.Effect); }
		protected internal override bool OnApply() { return default(bool); }
	}
	[System.FlagsAttribute]
	public enum SpriteEffects {
		FlipHorizontally = 1,
		FlipVertically = 2,
		None = 0,
	}
	public sealed partial class SpriteFont {
		internal SpriteFont() { }
		public System.Collections.ObjectModel.ReadOnlyCollection<System.Char> Characters { get { return default(System.Collections.ObjectModel.ReadOnlyCollection<System.Char>); } }
		public System.Nullable<System.Char> DefaultCharacter { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Nullable<System.Char>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int LineSpacing { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public float Spacing { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Vector2 MeasureString(string text) { return default(Microsoft.Xna.Framework.Vector2); }
		public Microsoft.Xna.Framework.Vector2 MeasureString(System.Text.StringBuilder text) { return default(Microsoft.Xna.Framework.Vector2); }
	}
	public enum SpriteSortMode {
		BackToFront = 3,
		Deferred = 0,
		FrontToBack = 4,
		Immediate = 1,
		Texture = 2,
	}
	public enum StencilOperation {
		Decrement = 4,
		DecrementSaturation = 6,
		Increment = 3,
		IncrementSaturation = 5,
		Invert = 7,
		Keep = 0,
		Replace = 2,
		Zero = 1,
	}
	public enum SurfaceFormat {
		Alpha8 = 12,
		Bgr32 = 20,
		Bgr565 = 1,
		Bgra32 = 21,
		Bgra4444 = 3,
		Bgra5551 = 2,
		Color = 0,
		Dxt1 = 4,
		Dxt1a = 70,
		Dxt3 = 5,
		Dxt5 = 6,
		HalfSingle = 16,
		HalfVector2 = 17,
		HalfVector4 = 18,
		HdrBlendable = 19,
		NormalizedByte2 = 7,
		NormalizedByte4 = 8,
		Rg32 = 10,
		Rgba1010102 = 9,
		Rgba64 = 11,
		RgbaPvrtc2Bpp = 52,
		RgbaPvrtc4Bpp = 53,
		RgbEtc1 = 60,
		RgbPvrtc2Bpp = 50,
		RgbPvrtc4Bpp = 51,
		Single = 13,
		Vector2 = 14,
		Vector4 = 15,
	}
	public enum SurfaceFormat_Legacy {
		Alpha8 = 15,
		Bgr233 = 16,
		Bgr24 = 17,
		Bgr32 = 2,
		Bgr444 = 13,
		Bgr555 = 11,
		Bgr565 = 9,
		Bgra1010102 = 3,
		Bgra2338 = 14,
		Bgra4444 = 12,
		Bgra5551 = 10,
		Color = 1,
		Depth15Stencil1 = 56,
		Depth16 = 54,
		Depth24 = 51,
		Depth24Stencil4 = 50,
		Depth24Stencil8 = 48,
		Depth24Stencil8Single = 49,
		Depth32 = 52,
		Dxt1 = 28,
		Dxt2 = 29,
		Dxt3 = 30,
		Dxt4 = 31,
		Dxt5 = 32,
		HalfSingle = 25,
		HalfVector2 = 26,
		HalfVector4 = 27,
		Luminance16 = 34,
		Luminance8 = 33,
		LuminanceAlpha16 = 36,
		LuminanceAlpha8 = 35,
		Multi2Bgra32 = 47,
		NormalizedAlpha1010102 = 41,
		NormalizedByte2 = 18,
		NormalizedByte2Computed = 42,
		NormalizedByte4 = 19,
		NormalizedLuminance16 = 39,
		NormalizedLuminance32 = 40,
		NormalizedShort2 = 20,
		NormalizedShort4 = 21,
		Palette8 = 37,
		PaletteAlpha16 = 38,
		Rg32 = 7,
		Rgb32 = 5,
		Rgba1010102 = 6,
		Rgba32 = 4,
		Rgba64 = 8,
		Single = 22,
		Unknown = -1,
		Vector2 = 23,
		Vector4 = 24,
		VideoGrGb = 45,
		VideoRgBg = 46,
		VideoUyVy = 44,
		VideoYuYv = 43,
	}
	public partial class TargetBlendState {
		internal TargetBlendState() { }
		public Microsoft.Xna.Framework.Graphics.BlendFunction AlphaBlendFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.BlendFunction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.Blend AlphaDestinationBlend { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.Blend); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.Blend AlphaSourceBlend { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.Blend); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.BlendFunction ColorBlendFunction { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.BlendFunction); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.Blend ColorDestinationBlend { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.Blend); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.Blend ColorSourceBlend { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.Blend); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Graphics.ColorWriteChannels ColorWriteChannels { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.ColorWriteChannels); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public abstract partial class Texture : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		protected Texture() { }
		public Microsoft.Xna.Framework.Graphics.SurfaceFormat Format { get { return default(Microsoft.Xna.Framework.Graphics.SurfaceFormat); } }
		public int LevelCount { get { return default(int); } }
		protected override void Dispose(bool disposing) { }
		protected internal override void GraphicsDeviceResetting() { }
	}
	public partial class Texture2D : Microsoft.Xna.Framework.Graphics.Texture {
		public Texture2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height) { }
		public Texture2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, bool mipmap, Microsoft.Xna.Framework.Graphics.SurfaceFormat format) { }
		protected Texture2D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, bool mipmap, Microsoft.Xna.Framework.Graphics.SurfaceFormat format, Microsoft.Xna.Framework.Graphics.Texture2D.SurfaceType type, bool shared) { }
		public Microsoft.Xna.Framework.Rectangle Bounds { get { return default(Microsoft.Xna.Framework.Rectangle); } }
		public int Height { get { return default(int); } }
		public int Width { get { return default(int); } }
		public static Microsoft.Xna.Framework.Graphics.Texture2D FromStream(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.IO.Stream stream) { return default(Microsoft.Xna.Framework.Graphics.Texture2D); }
		public void GetData<T>(T[] data) where T : struct { }
		public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void GetData<T>(int level, System.Nullable<Microsoft.Xna.Framework.Rectangle> rect, T[] data, int startIndex, int elementCount) where T : struct { }
		public void Reload(System.IO.Stream textureStream) { }
		public void SaveAsJpeg(System.IO.Stream stream, int width, int height) { }
		public void SaveAsPng(System.IO.Stream stream, int width, int height) { }
		public void SetData<T>(T[] data) where T : struct { }
		public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void SetData<T>(int level, System.Nullable<Microsoft.Xna.Framework.Rectangle> rect, T[] data, int startIndex, int elementCount) where T : struct { }
		protected internal enum SurfaceType {
			RenderTarget = 1,
			SwapChainRenderTarget = 2,
			Texture = 0,
		}
	}
	public partial class Texture3D : Microsoft.Xna.Framework.Graphics.Texture {
		public Texture3D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat format) { }
		protected Texture3D(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat format, bool renderTarget) { }
		public int Depth { get { return default(int); } }
		public int Height { get { return default(int); } }
		public int Width { get { return default(int); } }
		public void GetData<T>(T[] data) where T : struct { }
		public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void GetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount) where T : struct { }
		public void SetData<T>(T[] data) where T : struct { }
		public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void SetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount) where T : struct { }
	}
	public enum TextureAddressMode {
		Clamp = 1,
		Mirror = 2,
		Wrap = 0,
	}
	public sealed partial class TextureCollection {
		internal TextureCollection() { }
		public Microsoft.Xna.Framework.Graphics.Texture this[int index] { get { return default(Microsoft.Xna.Framework.Graphics.Texture); } set { } }
	}
	public partial class TextureCube : Microsoft.Xna.Framework.Graphics.Texture {
		public TextureCube(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, int size, bool mipMap, Microsoft.Xna.Framework.Graphics.SurfaceFormat format) { }
		public int Size { get { return default(int); } }
		public void GetData<T>(Microsoft.Xna.Framework.Graphics.CubeMapFace cubeMapFace, T[] data) where T : struct { }
		public void SetData<T>(Microsoft.Xna.Framework.Graphics.CubeMapFace face, T[] data) where T : struct { }
		public void SetData<T>(Microsoft.Xna.Framework.Graphics.CubeMapFace face, T[] data, int startIndex, int elementCount) where T : struct { }
		public void SetData<T>(Microsoft.Xna.Framework.Graphics.CubeMapFace face, int level, System.Nullable<Microsoft.Xna.Framework.Rectangle> rect, T[] data, int startIndex, int elementCount) where T : struct { }
	}
	public enum TextureFilter {
		Anisotropic = 2,
		Linear = 0,
		LinearMipPoint = 3,
		MinLinearMagPointMipLinear = 5,
		MinLinearMagPointMipPoint = 6,
		MinPointMagLinearMipLinear = 7,
		MinPointMagLinearMipPoint = 8,
		Point = 1,
		PointMipLinear = 4,
	}
	[System.FlagsAttribute]
	public enum TextureUsage {
		AutoGenerateMipMap = 1024,
		Linear = 1073741824,
		None = 0,
		Tiled = -2147483648,
	}
	public partial class VertexBuffer : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public VertexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration, int vertexCount, Microsoft.Xna.Framework.Graphics.BufferUsage bufferUsage) { }
		protected VertexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration, int vertexCount, Microsoft.Xna.Framework.Graphics.BufferUsage bufferUsage, bool dynamic) { }
		public VertexBuffer(Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice, System.Type type, int vertexCount, Microsoft.Xna.Framework.Graphics.BufferUsage bufferUsage) { }
		public Microsoft.Xna.Framework.Graphics.BufferUsage BufferUsage { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.BufferUsage); } }
		public int VertexCount { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
		protected override void Dispose(bool disposing) { }
		public void GetData<T>(T[] data) where T : struct { }
		public void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct { }
		protected internal override void GraphicsDeviceResetting() { }
		public void SetData<T>(T[] data) where T : struct { }
		public void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct { }
		public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct { }
		protected void SetDataInternal<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride, Microsoft.Xna.Framework.Graphics.SetDataOptions options) where T : struct { }
	}
	public partial class VertexDeclaration : Microsoft.Xna.Framework.Graphics.GraphicsResource {
		public VertexDeclaration(params Microsoft.Xna.Framework.Graphics.VertexElement[] elements) { }
		public VertexDeclaration(int vertexStride, params Microsoft.Xna.Framework.Graphics.VertexElement[] elements) { }
		public int VertexStride { get { return default(int); } }
		public Microsoft.Xna.Framework.Graphics.VertexElement[] GetVertexElements() { return default(Microsoft.Xna.Framework.Graphics.VertexElement[]); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct VertexElement {
		public VertexElement(int offset, Microsoft.Xna.Framework.Graphics.VertexElementFormat elementFormat, Microsoft.Xna.Framework.Graphics.VertexElementUsage elementUsage, int usageIndex) { throw new System.NotImplementedException(); }
		public int Offset { get { return default(int); } set { } }
		public int UsageIndex { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Graphics.VertexElementFormat VertexElementFormat { get { return default(Microsoft.Xna.Framework.Graphics.VertexElementFormat); } set { } }
		public Microsoft.Xna.Framework.Graphics.VertexElementUsage VertexElementUsage { get { return default(Microsoft.Xna.Framework.Graphics.VertexElementUsage); } set { } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.VertexElement left, Microsoft.Xna.Framework.Graphics.VertexElement right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.VertexElement left, Microsoft.Xna.Framework.Graphics.VertexElement right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	public enum VertexElementFormat {
		Byte4 = 5,
		Color = 4,
		HalfVector2 = 10,
		HalfVector4 = 11,
		NormalizedShort2 = 8,
		NormalizedShort4 = 9,
		Short2 = 6,
		Short4 = 7,
		Single = 0,
		Vector2 = 1,
		Vector3 = 2,
		Vector4 = 3,
	}
	public enum VertexElementUsage {
		Binormal = 4,
		BlendIndices = 6,
		BlendWeight = 7,
		Color = 1,
		Depth = 8,
		Fog = 9,
		Normal = 3,
		PointSize = 10,
		Position = 0,
		Sample = 11,
		Tangent = 5,
		TessellateFactor = 12,
		TextureCoordinate = 2,
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct VertexPositionColor : Microsoft.Xna.Framework.Graphics.IVertexType {
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Color Color;
		[System.Runtime.Serialization.DataMemberAttribute]
		public Microsoft.Xna.Framework.Vector3 Position;
		public static readonly Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration;
		public VertexPositionColor(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Color color) { throw new System.NotImplementedException(); }
		Microsoft.Xna.Framework.Graphics.VertexDeclaration Microsoft.Xna.Framework.Graphics.IVertexType.VertexDeclaration { get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.VertexPositionColor left, Microsoft.Xna.Framework.Graphics.VertexPositionColor right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.VertexPositionColor left, Microsoft.Xna.Framework.Graphics.VertexPositionColor right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct VertexPositionColorTexture : Microsoft.Xna.Framework.Graphics.IVertexType {
		public Microsoft.Xna.Framework.Color Color;
		public Microsoft.Xna.Framework.Vector3 Position;
		public Microsoft.Xna.Framework.Vector2 TextureCoordinate;
		public static readonly Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration;
		public VertexPositionColorTexture(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Vector2 textureCoordinate) { throw new System.NotImplementedException(); }
		Microsoft.Xna.Framework.Graphics.VertexDeclaration Microsoft.Xna.Framework.Graphics.IVertexType.VertexDeclaration { get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture left, Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture left, Microsoft.Xna.Framework.Graphics.VertexPositionColorTexture right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct VertexPositionNormalTexture : Microsoft.Xna.Framework.Graphics.IVertexType {
		public Microsoft.Xna.Framework.Vector3 Normal;
		public Microsoft.Xna.Framework.Vector3 Position;
		public Microsoft.Xna.Framework.Vector2 TextureCoordinate;
		public static readonly Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration;
		public VertexPositionNormalTexture(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Vector3 normal, Microsoft.Xna.Framework.Vector2 textureCoordinate) { throw new System.NotImplementedException(); }
		Microsoft.Xna.Framework.Graphics.VertexDeclaration Microsoft.Xna.Framework.Graphics.IVertexType.VertexDeclaration { get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture left, Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture left, Microsoft.Xna.Framework.Graphics.VertexPositionNormalTexture right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct VertexPositionTexture : Microsoft.Xna.Framework.Graphics.IVertexType {
		public Microsoft.Xna.Framework.Vector3 Position;
		public Microsoft.Xna.Framework.Vector2 TextureCoordinate;
		public static readonly Microsoft.Xna.Framework.Graphics.VertexDeclaration VertexDeclaration;
		public VertexPositionTexture(Microsoft.Xna.Framework.Vector3 position, Microsoft.Xna.Framework.Vector2 textureCoordinate) { throw new System.NotImplementedException(); }
		Microsoft.Xna.Framework.Graphics.VertexDeclaration Microsoft.Xna.Framework.Graphics.IVertexType.VertexDeclaration { get { return default(Microsoft.Xna.Framework.Graphics.VertexDeclaration); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.VertexPositionTexture left, Microsoft.Xna.Framework.Graphics.VertexPositionTexture right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.VertexPositionTexture left, Microsoft.Xna.Framework.Graphics.VertexPositionTexture right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.Serialization.DataContractAttribute]
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Viewport {
		public Viewport(Microsoft.Xna.Framework.Rectangle bounds) { throw new System.NotImplementedException(); }
		public Viewport(int x, int y, int width, int height) { throw new System.NotImplementedException(); }
		public float AspectRatio { get { return default(float); } }
		public Microsoft.Xna.Framework.Rectangle Bounds { get { return default(Microsoft.Xna.Framework.Rectangle); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Height { get { return default(int); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public float MaxDepth { get { return default(float); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public float MinDepth { get { return default(float); } set { } }
		public Microsoft.Xna.Framework.Rectangle TitleSafeArea { get { return default(Microsoft.Xna.Framework.Rectangle); } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Width { get { return default(int); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public int X { get { return default(int); } set { } }
		[System.Runtime.Serialization.DataMemberAttribute]
		public int Y { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Vector3 Project(Microsoft.Xna.Framework.Vector3 source, Microsoft.Xna.Framework.Matrix projection, Microsoft.Xna.Framework.Matrix view, Microsoft.Xna.Framework.Matrix world) { return default(Microsoft.Xna.Framework.Vector3); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector3 Unproject(Microsoft.Xna.Framework.Vector3 source, Microsoft.Xna.Framework.Matrix projection, Microsoft.Xna.Framework.Matrix view, Microsoft.Xna.Framework.Matrix world) { return default(Microsoft.Xna.Framework.Vector3); }
	}
}
namespace Microsoft.Xna.Framework.Graphics.PackedVector {
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Bgr565 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt16>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.Bgr565> {
		public Bgr565(Microsoft.Xna.Framework.Vector3 vector) { throw new System.NotImplementedException(); }
		public Bgr565(float x, float y, float z) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ushort PackedValue { get { return default(ushort); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.Bgr565 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		Microsoft.Xna.Framework.Vector4 Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.Bgr565 lhs, Microsoft.Xna.Framework.Graphics.PackedVector.Bgr565 rhs) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.Bgr565 lhs, Microsoft.Xna.Framework.Graphics.PackedVector.Bgr565 rhs) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector3 ToVector3() { return default(Microsoft.Xna.Framework.Vector3); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Bgra4444 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt16>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.Bgra4444> {
		public Bgra4444(Microsoft.Xna.Framework.Vector4 vector) { throw new System.NotImplementedException(); }
		public Bgra4444(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ushort PackedValue { get { return default(ushort); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.Bgra4444 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.Bgra4444 lhs, Microsoft.Xna.Framework.Graphics.PackedVector.Bgra4444 rhs) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.Bgra4444 lhs, Microsoft.Xna.Framework.Graphics.PackedVector.Bgra4444 rhs) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Byte4 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt32>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.Byte4> {
		public Byte4(Microsoft.Xna.Framework.Vector4 vector) { throw new System.NotImplementedException(); }
		public Byte4(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public uint PackedValue { get { return default(uint); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 a, Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 a, Microsoft.Xna.Framework.Graphics.PackedVector.Byte4 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct HalfSingle : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt16>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle> {
		public HalfSingle(float single) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ushort PackedValue { get { return default(ushort); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		Microsoft.Xna.Framework.Vector4 Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle lhs, Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle rhs) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle lhs, Microsoft.Xna.Framework.Graphics.PackedVector.HalfSingle rhs) { return default(bool); }
		public float ToSingle() { return default(float); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct HalfVector2 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt32>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2> {
		public HalfVector2(Microsoft.Xna.Framework.Vector2 vector) { throw new System.NotImplementedException(); }
		public HalfVector2(float x, float y) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public uint PackedValue { get { return default(uint); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		Microsoft.Xna.Framework.Vector4 Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2 a, Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2 a, Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector2 ToVector2() { return default(Microsoft.Xna.Framework.Vector2); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct HalfVector4 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt64>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4> {
		public HalfVector4(Microsoft.Xna.Framework.Vector4 vector) { throw new System.NotImplementedException(); }
		public HalfVector4(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ulong PackedValue { get { return default(ulong); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 a, Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 a, Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector4 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	public partial interface IPackedVector {
		void PackFromVector4(Microsoft.Xna.Framework.Vector4 vector);
		Microsoft.Xna.Framework.Vector4 ToVector4();
	}
	public partial interface IPackedVector<TPacked> : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector {
		TPacked PackedValue { get; set; }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct NormalizedByte2 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt16>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte2> {
		public NormalizedByte2(Microsoft.Xna.Framework.Vector2 vector) { throw new System.NotImplementedException(); }
		public NormalizedByte2(float x, float y) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ushort PackedValue { get { return default(ushort); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte2 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		Microsoft.Xna.Framework.Vector4 Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte2 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte2 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte2 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte2 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector2 ToVector2() { return default(Microsoft.Xna.Framework.Vector2); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct NormalizedByte4 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt32>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4> {
		public NormalizedByte4(Microsoft.Xna.Framework.Vector4 vector) { throw new System.NotImplementedException(); }
		public NormalizedByte4(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public uint PackedValue { get { return default(uint); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedByte4 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct NormalizedShort2 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt32>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2> {
		public NormalizedShort2(Microsoft.Xna.Framework.Vector2 vector) { throw new System.NotImplementedException(); }
		public NormalizedShort2(float x, float y) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public uint PackedValue { get { return default(uint); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		Microsoft.Xna.Framework.Vector4 Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort2 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector2 ToVector2() { return default(Microsoft.Xna.Framework.Vector2); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct NormalizedShort4 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt64>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4> {
		public NormalizedShort4(Microsoft.Xna.Framework.Vector4 vector) { throw new System.NotImplementedException(); }
		public NormalizedShort4(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ulong PackedValue { get { return default(ulong); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 a, Microsoft.Xna.Framework.Graphics.PackedVector.NormalizedShort4 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Short2 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt32>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.Short2> {
		public Short2(Microsoft.Xna.Framework.Vector2 vector) { throw new System.NotImplementedException(); }
		public Short2(float x, float y) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public uint PackedValue { get { return default(uint); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.Short2 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		Microsoft.Xna.Framework.Vector4 Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.Short2 a, Microsoft.Xna.Framework.Graphics.PackedVector.Short2 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.Short2 a, Microsoft.Xna.Framework.Graphics.PackedVector.Short2 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector2 ToVector2() { return default(Microsoft.Xna.Framework.Vector2); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Short4 : Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector, Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector<System.UInt64>, System.IEquatable<Microsoft.Xna.Framework.Graphics.PackedVector.Short4> {
		public Short4(Microsoft.Xna.Framework.Vector4 vector) { throw new System.NotImplementedException(); }
		public Short4(float x, float y, float z, float w) { throw new System.NotImplementedException(); }
		[System.CLSCompliantAttribute(false)]
		public ulong PackedValue { get { return default(ulong); } set { } }
		public bool Equals(Microsoft.Xna.Framework.Graphics.PackedVector.Short4 other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		void Microsoft.Xna.Framework.Graphics.PackedVector.IPackedVector.PackFromVector4(Microsoft.Xna.Framework.Vector4 vector) { }
		public static bool operator ==(Microsoft.Xna.Framework.Graphics.PackedVector.Short4 a, Microsoft.Xna.Framework.Graphics.PackedVector.Short4 b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Graphics.PackedVector.Short4 a, Microsoft.Xna.Framework.Graphics.PackedVector.Short4 b) { return default(bool); }
		public override string ToString() { return default(string); }
		public Microsoft.Xna.Framework.Vector4 ToVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
}
namespace Microsoft.Xna.Framework.Input {
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct AccelerometerState {
		public Microsoft.Xna.Framework.Vector3 Acceleration { get { return default(Microsoft.Xna.Framework.Vector3); } }
		public bool IsConnected { get { return default(bool); } }
	}
	public partial class Axis {
		public Axis() { }
		public Microsoft.Xna.Framework.Input.Input Negative { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Positive { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.InputType Type { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.InputType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public float ReadAxis(System.IntPtr device) { return default(float); }
	}
	[System.FlagsAttribute]
	public enum Buttons {
		A = 4096,
		B = 8192,
		Back = 32,
		BigButton = 2048,
		DPadDown = 2,
		DPadLeft = 4,
		DPadRight = 8,
		DPadUp = 1,
		LeftShoulder = 256,
		LeftStick = 64,
		LeftThumbstickDown = 536870912,
		LeftThumbstickLeft = 2097152,
		LeftThumbstickRight = 1073741824,
		LeftThumbstickUp = 268435456,
		LeftTrigger = 8388608,
		RightShoulder = 512,
		RightStick = 128,
		RightThumbstickDown = 33554432,
		RightThumbstickLeft = 134217728,
		RightThumbstickRight = 67108864,
		RightThumbstickUp = 16777216,
		RightTrigger = 4194304,
		Start = 16,
		X = 16384,
		Y = 32768,
	}
	public enum ButtonState {
		Pressed = 1,
		Released = 0,
	}
	public partial class Capabilities {
		public Capabilities(System.IntPtr device) { }
		public int NumberOfAxis { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public int NumberOfButtons { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public int NumberOfPovHats { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
	}
	public partial class DPad {
		public DPad() { }
		public Microsoft.Xna.Framework.Input.Input Down { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Up { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public static partial class GamePad {
		public static Microsoft.Xna.Framework.Input.GamePadCapabilities GetCapabilities(Microsoft.Xna.Framework.PlayerIndex playerIndex) { return default(Microsoft.Xna.Framework.Input.GamePadCapabilities); }
		public static Microsoft.Xna.Framework.Input.GamePadState GetState(Microsoft.Xna.Framework.PlayerIndex playerIndex) { return default(Microsoft.Xna.Framework.Input.GamePadState); }
		public static Microsoft.Xna.Framework.Input.GamePadState GetState(Microsoft.Xna.Framework.PlayerIndex playerIndex, Microsoft.Xna.Framework.Input.GamePadDeadZone deadZoneMode) { return default(Microsoft.Xna.Framework.Input.GamePadState); }
		public static bool SetVibration(Microsoft.Xna.Framework.PlayerIndex playerIndex, float leftMotor, float rightMotor) { return default(bool); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct GamePadButtons {
		public GamePadButtons(Microsoft.Xna.Framework.Input.Buttons buttons) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Input.ButtonState A { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState B { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState Back { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState BigButton { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState LeftShoulder { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState LeftStick { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState RightShoulder { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState RightStick { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState Start { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState X { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState Y { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.GamePadButtons left, Microsoft.Xna.Framework.Input.GamePadButtons right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.GamePadButtons left, Microsoft.Xna.Framework.Input.GamePadButtons right) { return default(bool); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
	public partial struct GamePadCapabilities {
		public Microsoft.Xna.Framework.Input.GamePadType GamePadType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.GamePadType); } }
		public bool HasAButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasBackButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasBButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasBigButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasDPadDownButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasDPadLeftButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasDPadRightButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasDPadUpButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasLeftShoulderButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasLeftStickButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasLeftTrigger { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasLeftVibrationMotor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasLeftXThumbStick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasLeftYThumbStick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasRightShoulderButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasRightStickButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasRightTrigger { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasRightVibrationMotor { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasRightXThumbStick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasRightYThumbStick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasStartButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasVoiceSupport { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasXButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool HasYButton { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public bool IsConnected { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
	}
	public enum GamePadDeadZone {
		Circular = 2,
		IndependentAxes = 1,
		None = 0,
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
	public partial struct GamePadDPad {
		public GamePadDPad(Microsoft.Xna.Framework.Input.ButtonState upValue, Microsoft.Xna.Framework.Input.ButtonState downValue, Microsoft.Xna.Framework.Input.ButtonState leftValue, Microsoft.Xna.Framework.Input.ButtonState rightValue) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Input.ButtonState Down { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState Left { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState Right { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState Up { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.GamePadDPad left, Microsoft.Xna.Framework.Input.GamePadDPad right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.GamePadDPad left, Microsoft.Xna.Framework.Input.GamePadDPad right) { return default(bool); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=1)]
	public partial struct GamePadState {
		public static readonly Microsoft.Xna.Framework.Input.GamePadState Default;
		public GamePadState(Microsoft.Xna.Framework.Input.GamePadThumbSticks thumbSticks, Microsoft.Xna.Framework.Input.GamePadTriggers triggers, Microsoft.Xna.Framework.Input.GamePadButtons buttons, Microsoft.Xna.Framework.Input.GamePadDPad dPad) { throw new System.NotImplementedException(); }
		public GamePadState(Microsoft.Xna.Framework.Vector2 leftThumbStick, Microsoft.Xna.Framework.Vector2 rightThumbStick, float leftTrigger, float rightTrigger, params Microsoft.Xna.Framework.Input.Buttons[] buttons) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Input.GamePadButtons Buttons { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.GamePadButtons); } }
		public Microsoft.Xna.Framework.Input.GamePadDPad DPad { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.GamePadDPad); } }
		public bool IsConnected { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public int PacketNumber { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Input.GamePadThumbSticks ThumbSticks { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.GamePadThumbSticks); } }
		public Microsoft.Xna.Framework.Input.GamePadTriggers Triggers { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.GamePadTriggers); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public bool IsButtonDown(Microsoft.Xna.Framework.Input.Buttons button) { return default(bool); }
		public bool IsButtonUp(Microsoft.Xna.Framework.Input.Buttons button) { return default(bool); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.GamePadState left, Microsoft.Xna.Framework.Input.GamePadState right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.GamePadState left, Microsoft.Xna.Framework.Input.GamePadState right) { return default(bool); }
		public override string ToString() { return default(string); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct GamePadThumbSticks {
		public static Microsoft.Xna.Framework.Input.GamePadThumbSticks.GateType Gate;
		public GamePadThumbSticks(Microsoft.Xna.Framework.Vector2 leftPosition, Microsoft.Xna.Framework.Vector2 rightPosition) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Vector2 Left { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public Microsoft.Xna.Framework.Vector2 Right { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.GamePadThumbSticks left, Microsoft.Xna.Framework.Input.GamePadThumbSticks right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.GamePadThumbSticks left, Microsoft.Xna.Framework.Input.GamePadThumbSticks right) { return default(bool); }
		public enum GateType {
			None = 0,
			Round = 1,
			Square = 2,
		}
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct GamePadTriggers {
		public GamePadTriggers(float leftTrigger, float rightTrigger) { throw new System.NotImplementedException(); }
		public float Left { get { return default(float); } }
		public float Right { get { return default(float); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.GamePadTriggers left, Microsoft.Xna.Framework.Input.GamePadTriggers right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.GamePadTriggers left, Microsoft.Xna.Framework.Input.GamePadTriggers right) { return default(bool); }
	}
	public enum GamePadType {
		AlternateGuitar = 6,
		ArcadeStick = 1,
		BigButtonPad = 7,
		DancePad = 2,
		DrumKit = 8,
		FlightStick = 3,
		GamePad = 9,
		Guitar = 4,
		Unknown = 0,
		Wheel = 5,
	}
	public partial class Input {
		public int ID;
		public Microsoft.Xna.Framework.Input.InputType Type;
		public Input() { }
		public bool Negative { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public enum InputType {
		Axis = 32,
		Button = 16,
		None = -1,
		PovDown = 4,
		PovLeft = 8,
		PovRight = 2,
		PovUp = 1,
	}
	public partial class Joystick {
		public Joystick(int id) { }
		public Microsoft.Xna.Framework.Input.PadConfig Config { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.PadConfig); } }
		public Microsoft.Xna.Framework.Input.Capabilities Details { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Capabilities); } }
		public int ID { get { return default(int); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public bool Open { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public static System.Collections.Generic.List<Microsoft.Xna.Framework.Input.Joystick> GrabJoysticks() { return default(System.Collections.Generic.List<Microsoft.Xna.Framework.Input.Joystick>); }
		public static bool Init() { return default(bool); }
	}
	public static partial class Keyboard {
		public static Microsoft.Xna.Framework.Input.KeyboardState GetState() { return default(Microsoft.Xna.Framework.Input.KeyboardState); }
		[System.ObsoleteAttribute("Use GetState() instead. In future versions this method can be removed.")]
		public static Microsoft.Xna.Framework.Input.KeyboardState GetState(Microsoft.Xna.Framework.PlayerIndex playerIndex) { return default(Microsoft.Xna.Framework.Input.KeyboardState); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct KeyboardState {
		public KeyboardState(params Microsoft.Xna.Framework.Input.Keys[] keys) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Input.KeyState this[Microsoft.Xna.Framework.Input.Keys key] { get { return default(Microsoft.Xna.Framework.Input.KeyState); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public Microsoft.Xna.Framework.Input.Keys[] GetPressedKeys() { return default(Microsoft.Xna.Framework.Input.Keys[]); }
		public bool IsKeyDown(Microsoft.Xna.Framework.Input.Keys key) { return default(bool); }
		public bool IsKeyUp(Microsoft.Xna.Framework.Input.Keys key) { return default(bool); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.KeyboardState a, Microsoft.Xna.Framework.Input.KeyboardState b) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.KeyboardState a, Microsoft.Xna.Framework.Input.KeyboardState b) { return default(bool); }
	}
	public enum Keys {
		A = 65,
		Add = 107,
		Apps = 93,
		Attn = 246,
		B = 66,
		Back = 8,
		BrowserBack = 166,
		BrowserFavorites = 171,
		BrowserForward = 167,
		BrowserHome = 172,
		BrowserRefresh = 168,
		BrowserSearch = 170,
		BrowserStop = 169,
		C = 67,
		CapsLock = 20,
		ChatPadGreen = 202,
		ChatPadOrange = 203,
		Crsel = 247,
		D = 68,
		D0 = 48,
		D1 = 49,
		D2 = 50,
		D3 = 51,
		D4 = 52,
		D5 = 53,
		D6 = 54,
		D7 = 55,
		D8 = 56,
		D9 = 57,
		Decimal = 110,
		Delete = 46,
		Divide = 111,
		Down = 40,
		E = 69,
		End = 35,
		Enter = 13,
		EraseEof = 249,
		Escape = 27,
		Execute = 43,
		Exsel = 248,
		F = 70,
		F1 = 112,
		F10 = 121,
		F11 = 122,
		F12 = 123,
		F13 = 124,
		F14 = 125,
		F15 = 126,
		F16 = 127,
		F17 = 128,
		F18 = 129,
		F19 = 130,
		F2 = 113,
		F20 = 131,
		F21 = 132,
		F22 = 133,
		F23 = 134,
		F24 = 135,
		F3 = 114,
		F4 = 115,
		F5 = 116,
		F6 = 117,
		F7 = 118,
		F8 = 119,
		F9 = 120,
		G = 71,
		H = 72,
		Help = 47,
		Home = 36,
		I = 73,
		ImeConvert = 28,
		ImeNoConvert = 29,
		Insert = 45,
		J = 74,
		K = 75,
		Kana = 21,
		Kanji = 25,
		L = 76,
		LaunchApplication1 = 182,
		LaunchApplication2 = 183,
		LaunchMail = 180,
		Left = 37,
		LeftAlt = 164,
		LeftControl = 162,
		LeftShift = 160,
		LeftWindows = 91,
		M = 77,
		MediaNextTrack = 176,
		MediaPlayPause = 179,
		MediaPreviousTrack = 177,
		MediaStop = 178,
		Multiply = 106,
		N = 78,
		None = 0,
		NumLock = 144,
		NumPad0 = 96,
		NumPad1 = 97,
		NumPad2 = 98,
		NumPad3 = 99,
		NumPad4 = 100,
		NumPad5 = 101,
		NumPad6 = 102,
		NumPad7 = 103,
		NumPad8 = 104,
		NumPad9 = 105,
		O = 79,
		Oem8 = 223,
		OemAuto = 243,
		OemBackslash = 226,
		OemClear = 254,
		OemCloseBrackets = 221,
		OemComma = 188,
		OemCopy = 242,
		OemEnlW = 244,
		OemMinus = 189,
		OemOpenBrackets = 219,
		OemPeriod = 190,
		OemPipe = 220,
		OemPlus = 187,
		OemQuestion = 191,
		OemQuotes = 222,
		OemSemicolon = 186,
		OemTilde = 192,
		P = 80,
		Pa1 = 253,
		PageDown = 34,
		PageUp = 33,
		Pause = 19,
		Play = 250,
		Print = 42,
		PrintScreen = 44,
		ProcessKey = 229,
		Q = 81,
		R = 82,
		Right = 39,
		RightAlt = 165,
		RightControl = 163,
		RightShift = 161,
		RightWindows = 92,
		S = 83,
		Scroll = 145,
		Select = 41,
		SelectMedia = 181,
		Separator = 108,
		Sleep = 95,
		Space = 32,
		Subtract = 109,
		T = 84,
		Tab = 9,
		U = 85,
		Up = 38,
		V = 86,
		VolumeDown = 174,
		VolumeMute = 173,
		VolumeUp = 175,
		W = 87,
		X = 88,
		Y = 89,
		Z = 90,
		Zoom = 251,
	}
	public enum KeyState {
		Down = 1,
		Up = 0,
	}
	public static partial class Mouse {
		public static System.IntPtr WindowHandle { get { return default(System.IntPtr); } set { } }
		public static Microsoft.Xna.Framework.Input.MouseState GetState() { return default(Microsoft.Xna.Framework.Input.MouseState); }
		[System.CLSCompliantAttribute(false)]
		public static void SetPosition(int x, int y) { }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct MouseState {
		public MouseState(int x, int y, int scrollWheel, Microsoft.Xna.Framework.Input.ButtonState leftButton, Microsoft.Xna.Framework.Input.ButtonState middleButton, Microsoft.Xna.Framework.Input.ButtonState rightButton, Microsoft.Xna.Framework.Input.ButtonState xButton1, Microsoft.Xna.Framework.Input.ButtonState xButton2) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Input.ButtonState LeftButton { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState MiddleButton { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Point Position { get { return default(Microsoft.Xna.Framework.Point); } }
		public Microsoft.Xna.Framework.Input.ButtonState RightButton { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public int ScrollWheelValue { get { return default(int); } }
		public int X { get { return default(int); } }
		public Microsoft.Xna.Framework.Input.ButtonState XButton1 { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public Microsoft.Xna.Framework.Input.ButtonState XButton2 { get { return default(Microsoft.Xna.Framework.Input.ButtonState); } }
		public int Y { get { return default(int); } }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.MouseState left, Microsoft.Xna.Framework.Input.MouseState right) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.MouseState left, Microsoft.Xna.Framework.Input.MouseState right) { return default(bool); }
	}
	public partial class PadConfig {
		public PadConfig(string name, int index) { }
		public Microsoft.Xna.Framework.Input.Input Button_A { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_B { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_Back { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_LB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_RB { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_Start { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input Button_Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.DPad Dpad { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.DPad); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int Index { get { return default(int); } }
		public Microsoft.Xna.Framework.Input.Input this[int index] { get { return default(Microsoft.Xna.Framework.Input.Input); } }
		public string JoystickName { get { return default(string); } }
		public Microsoft.Xna.Framework.Input.Stick LeftStick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Stick); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input LeftTrigger { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Stick RightStick { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Stick); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Input RightTrigger { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public partial class Settings {
		public Settings() { }
		public Microsoft.Xna.Framework.Input.PadConfig this[int index] { get { return default(Microsoft.Xna.Framework.Input.PadConfig); } set { } }
		public Microsoft.Xna.Framework.Input.PadConfig Player1 { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.PadConfig); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.PadConfig Player2 { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.PadConfig); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.PadConfig Player3 { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.PadConfig); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.PadConfig Player4 { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.PadConfig); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
	public partial class Stick {
		public Stick() { }
		public Microsoft.Xna.Framework.Input.Input Press { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Input); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Axis X { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Axis); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Axis Y { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Axis); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
	}
}
namespace Microsoft.Xna.Framework.Input.Touch {
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct GestureSample {
		public GestureSample(Microsoft.Xna.Framework.Input.Touch.GestureType gestureType, System.TimeSpan timestamp, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 position2, Microsoft.Xna.Framework.Vector2 delta, Microsoft.Xna.Framework.Vector2 delta2) { throw new System.NotImplementedException(); }
		public Microsoft.Xna.Framework.Vector2 Delta { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public Microsoft.Xna.Framework.Vector2 Delta2 { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public Microsoft.Xna.Framework.Input.Touch.GestureType GestureType { get { return default(Microsoft.Xna.Framework.Input.Touch.GestureType); } }
		public Microsoft.Xna.Framework.Vector2 Position { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public Microsoft.Xna.Framework.Vector2 Position2 { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public System.TimeSpan Timestamp { get { return default(System.TimeSpan); } }
	}
	[System.FlagsAttribute]
	public enum GestureType {
		DoubleTap = 256,
		DragComplete = 2,
		Flick = 4,
		FreeDrag = 8,
		Hold = 16,
		HorizontalDrag = 32,
		None = 0,
		Pinch = 64,
		PinchComplete = 128,
		Tap = 1,
		VerticalDrag = 512,
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct TouchCollection : System.Collections.Generic.ICollection<Microsoft.Xna.Framework.Input.Touch.TouchLocation>, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Input.Touch.TouchLocation>, System.Collections.Generic.IList<Microsoft.Xna.Framework.Input.Touch.TouchLocation>, System.Collections.IEnumerable {
		public TouchCollection(Microsoft.Xna.Framework.Input.Touch.TouchLocation[] touches) { throw new System.NotImplementedException(); }
		public int Count { get { return default(int); } }
		public bool IsConnected { get { return default(bool); } }
		public bool IsReadOnly { get { return default(bool); } }
		public Microsoft.Xna.Framework.Input.Touch.TouchLocation this[int index] { get { return default(Microsoft.Xna.Framework.Input.Touch.TouchLocation); } set { } }
		public void Add(Microsoft.Xna.Framework.Input.Touch.TouchLocation item) { }
		public void Clear() { }
		public bool Contains(Microsoft.Xna.Framework.Input.Touch.TouchLocation item) { return default(bool); }
		public void CopyTo(Microsoft.Xna.Framework.Input.Touch.TouchLocation[] array, int arrayIndex) { }
		public bool FindById(int id, out Microsoft.Xna.Framework.Input.Touch.TouchLocation touchLocation) { touchLocation = default(Microsoft.Xna.Framework.Input.Touch.TouchLocation); return default(bool); }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Input.Touch.TouchLocation> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Input.Touch.TouchLocation>); }
		public int IndexOf(Microsoft.Xna.Framework.Input.Touch.TouchLocation item) { return default(int); }
		public void Insert(int index, Microsoft.Xna.Framework.Input.Touch.TouchLocation item) { }
		public bool Remove(Microsoft.Xna.Framework.Input.Touch.TouchLocation item) { return default(bool); }
		public void RemoveAt(int index) { }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct TouchLocation : System.IEquatable<Microsoft.Xna.Framework.Input.Touch.TouchLocation> {
		public TouchLocation(int id, Microsoft.Xna.Framework.Input.Touch.TouchLocationState state, Microsoft.Xna.Framework.Vector2 position) { throw new System.NotImplementedException(); }
		public TouchLocation(int id, Microsoft.Xna.Framework.Input.Touch.TouchLocationState state, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Input.Touch.TouchLocationState previousState, Microsoft.Xna.Framework.Vector2 previousPosition) { throw new System.NotImplementedException(); }
		public int Id { get { return default(int); } }
		public Microsoft.Xna.Framework.Vector2 Position { get { return default(Microsoft.Xna.Framework.Vector2); } }
		public float Pressure { get { return default(float); } }
		public Microsoft.Xna.Framework.Input.Touch.TouchLocationState State { get { return default(Microsoft.Xna.Framework.Input.Touch.TouchLocationState); } }
		public bool Equals(Microsoft.Xna.Framework.Input.Touch.TouchLocation other) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Input.Touch.TouchLocation value1, Microsoft.Xna.Framework.Input.Touch.TouchLocation value2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Input.Touch.TouchLocation value1, Microsoft.Xna.Framework.Input.Touch.TouchLocation value2) { return default(bool); }
		public override string ToString() { return default(string); }
		public bool TryGetPreviousLocation(out Microsoft.Xna.Framework.Input.Touch.TouchLocation aPreviousLocation) { aPreviousLocation = default(Microsoft.Xna.Framework.Input.Touch.TouchLocation); return default(bool); }
	}
	public enum TouchLocationState {
		Invalid = 0,
		Moved = 1,
		Pressed = 2,
		Released = 3,
	}
	public static partial class TouchPanel {
		public static int DisplayHeight { get { return default(int); } set { } }
		public static Microsoft.Xna.Framework.DisplayOrientation DisplayOrientation { get { return default(Microsoft.Xna.Framework.DisplayOrientation); } set { } }
		public static int DisplayWidth { get { return default(int); } set { } }
		public static Microsoft.Xna.Framework.Input.Touch.GestureType EnabledGestures { get { return default(Microsoft.Xna.Framework.Input.Touch.GestureType); } set { } }
		public static bool EnableMouseGestures { get { return default(bool); } set { } }
		public static bool EnableMouseTouchPoint { get { return default(bool); } set { } }
		public static bool IsGestureAvailable { get { return default(bool); } }
		public static System.IntPtr WindowHandle { get { return default(System.IntPtr); } set { } }
		public static Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities GetCapabilities() { return default(Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities); }
		public static Microsoft.Xna.Framework.Input.Touch.TouchCollection GetState() { return default(Microsoft.Xna.Framework.Input.Touch.TouchCollection); }
		public static Microsoft.Xna.Framework.Input.Touch.GestureSample ReadGesture() { return default(Microsoft.Xna.Framework.Input.Touch.GestureSample); }
	}
	[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct TouchPanelCapabilities {
		public bool HasPressure { get { return default(bool); } }
		public bool IsConnected { get { return default(bool); } }
		public int MaximumTouchCount { get { return default(int); } }
	}
	public partial class TouchPanelState {
		internal TouchPanelState() { }
		public int DisplayHeight { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.DisplayOrientation DisplayOrientation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.DisplayOrientation); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public int DisplayWidth { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Input.Touch.GestureType EnabledGestures { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Input.Touch.GestureType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool EnableMouseGestures { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool EnableMouseTouchPoint { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool IsGestureAvailable { get { return default(bool); } }
		public System.IntPtr WindowHandle { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.IntPtr); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities GetCapabilities() { return default(Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities); }
		public Microsoft.Xna.Framework.Input.Touch.TouchCollection GetState() { return default(Microsoft.Xna.Framework.Input.Touch.TouchCollection); }
		public Microsoft.Xna.Framework.Input.Touch.GestureSample ReadGesture() { return default(Microsoft.Xna.Framework.Input.Touch.GestureSample); }
	}
}
namespace Microsoft.Xna.Framework.Media {
	public partial class MediaLibrary : System.IDisposable {
		public MediaLibrary() { }
		public MediaLibrary(Microsoft.Xna.Framework.Media.MediaSource mediaSource) { }
		public Microsoft.Xna.Framework.Media.SongCollection Songs { get { return default(Microsoft.Xna.Framework.Media.SongCollection); } }
		public void Dispose() { }
	}
	public static partial class MediaPlayer {
		public static bool GameHasControl { get { return default(bool); } }
		public static bool IsMuted { get { return default(bool); } set { } }
		public static bool IsRepeating { get { return default(bool); } set { } }
		public static bool IsShuffled { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public static bool IsVisualizationEnabled { get { return default(bool); } }
		public static System.TimeSpan PlayPosition { get { return default(System.TimeSpan); } }
		public static Microsoft.Xna.Framework.Media.MediaQueue Queue { get { return default(Microsoft.Xna.Framework.Media.MediaQueue); } }
		public static Microsoft.Xna.Framework.Media.MediaState State { get { return default(Microsoft.Xna.Framework.Media.MediaState); } }
		public static float Volume { get { return default(float); } set { } }
		public static event System.EventHandler<System.EventArgs> ActiveSongChanged { add { } remove { } }
		public static event System.EventHandler<System.EventArgs> MediaStateChanged { add { } remove { } }
		public static void MoveNext() { }
		public static void MovePrevious() { }
		public static void Pause() { }
		public static void Play(Microsoft.Xna.Framework.Media.Song song) { }
		public static void Play(Microsoft.Xna.Framework.Media.SongCollection collection, int index=0) { }
		public static void Resume() { }
		public static void Stop() { }
	}
	public sealed partial class MediaQueue {
		public MediaQueue() { }
		public Microsoft.Xna.Framework.Media.Song ActiveSong { get { return default(Microsoft.Xna.Framework.Media.Song); } }
		public int ActiveSongIndex { get { return default(int); } set { } }
	}
	public sealed partial class MediaSource {
		internal MediaSource() { }
		public Microsoft.Xna.Framework.Media.MediaSourceType MediaSourceType { get { return default(Microsoft.Xna.Framework.Media.MediaSourceType); } }
		public string Name { get { return default(string); } }
		public static System.Collections.Generic.IList<Microsoft.Xna.Framework.Media.MediaSource> GetAvailableMediaSources() { return default(System.Collections.Generic.IList<Microsoft.Xna.Framework.Media.MediaSource>); }
	}
	public enum MediaSourceType {
		LocalDevice = 0,
		WindowsMediaConnect = 4,
	}
	public enum MediaState {
		Paused = 2,
		Playing = 1,
		Stopped = 0,
	}
	public sealed partial class Playlist : System.IDisposable {
		public Playlist() { }
		public System.TimeSpan Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } }
		public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public void Dispose() { }
	}
	public sealed partial class PlaylistCollection : System.Collections.Generic.ICollection<Microsoft.Xna.Framework.Media.Playlist>, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Media.Playlist>, System.Collections.IEnumerable, System.IDisposable {
		public PlaylistCollection() { }
		public int Count { get { return default(int); } }
		public bool IsReadOnly { get { return default(bool); } }
		public Microsoft.Xna.Framework.Media.Playlist this[int index] { get { return default(Microsoft.Xna.Framework.Media.Playlist); } }
		public void Add(Microsoft.Xna.Framework.Media.Playlist item) { }
		public void Clear() { }
		public Microsoft.Xna.Framework.Media.PlaylistCollection Clone() { return default(Microsoft.Xna.Framework.Media.PlaylistCollection); }
		public bool Contains(Microsoft.Xna.Framework.Media.Playlist item) { return default(bool); }
		public void CopyTo(Microsoft.Xna.Framework.Media.Playlist[] array, int arrayIndex) { }
		public void Dispose() { }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Media.Playlist> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Media.Playlist>); }
		public int IndexOf(Microsoft.Xna.Framework.Media.Playlist item) { return default(int); }
		public bool Remove(Microsoft.Xna.Framework.Media.Playlist item) { return default(bool); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public sealed partial class Song : System.IDisposable, System.IEquatable<Microsoft.Xna.Framework.Media.Song> {
		internal Song() { }
		public System.TimeSpan Duration { get { return default(System.TimeSpan); } }
		public bool IsProtected { get { return default(bool); } }
		public bool IsRated { get { return default(bool); } }
		public string Name { get { return default(string); } }
		public int PlayCount { get { return default(int); } }
		public int Rating { get { return default(int); } }
		public int TrackNumber { get { return default(int); } }
		public void Dispose() { }
		public bool Equals(Microsoft.Xna.Framework.Media.Song song) { return default(bool); }
		public override bool Equals(object obj) { return default(bool); }
		~Song() { }
		public override int GetHashCode() { return default(int); }
		public static bool operator ==(Microsoft.Xna.Framework.Media.Song song1, Microsoft.Xna.Framework.Media.Song song2) { return default(bool); }
		public static bool operator !=(Microsoft.Xna.Framework.Media.Song song1, Microsoft.Xna.Framework.Media.Song song2) { return default(bool); }
	}
	public partial class SongCollection : System.Collections.Generic.ICollection<Microsoft.Xna.Framework.Media.Song>, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.Media.Song>, System.Collections.IEnumerable, System.IDisposable {
		public SongCollection() { }
		public int Count { get { return default(int); } }
		public bool IsReadOnly { get { return default(bool); } }
		public Microsoft.Xna.Framework.Media.Song this[int index] { get { return default(Microsoft.Xna.Framework.Media.Song); } }
		public void Add(Microsoft.Xna.Framework.Media.Song item) { }
		public void Clear() { }
		public Microsoft.Xna.Framework.Media.SongCollection Clone() { return default(Microsoft.Xna.Framework.Media.SongCollection); }
		public bool Contains(Microsoft.Xna.Framework.Media.Song item) { return default(bool); }
		public void CopyTo(Microsoft.Xna.Framework.Media.Song[] array, int arrayIndex) { }
		public void Dispose() { }
		public System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Media.Song> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<Microsoft.Xna.Framework.Media.Song>); }
		public int IndexOf(Microsoft.Xna.Framework.Media.Song item) { return default(int); }
		public bool Remove(Microsoft.Xna.Framework.Media.Song item) { return default(bool); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
	}
	public sealed partial class Video : System.IDisposable {
		internal Video() { }
		public System.TimeSpan Duration { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.TimeSpan); } }
		public string FileName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
		public float FramesPerSecond { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(float); } }
		public int Height { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public Microsoft.Xna.Framework.Media.VideoSoundtrackType VideoSoundtrackType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Microsoft.Xna.Framework.Media.VideoSoundtrackType); } }
		public int Width { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(int); } }
		public void Dispose() { }
		~Video() { }
	}
	public sealed partial class VideoPlayer : System.IDisposable {
		public VideoPlayer() { }
		public bool IsDisposed { get { return default(bool); } }
		public bool IsLooped { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public bool IsMuted { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
		public System.TimeSpan PlayPosition { get { return default(System.TimeSpan); } }
		public Microsoft.Xna.Framework.Media.MediaState State { get { return default(Microsoft.Xna.Framework.Media.MediaState); } }
		public Microsoft.Xna.Framework.Media.Video Video { get { return default(Microsoft.Xna.Framework.Media.Video); } }
		public float Volume { get { return default(float); } set { } }
		public void Dispose() { }
		public Microsoft.Xna.Framework.Graphics.Texture2D GetTexture() { return default(Microsoft.Xna.Framework.Graphics.Texture2D); }
		public void Pause() { }
		public void Play(Microsoft.Xna.Framework.Media.Video video) { }
		public void Resume() { }
		public void Stop() { }
	}
	public enum VideoSoundtrackType {
		Dialog = 0,
		Music = 1,
		MusicAndDialog = 2,
	}
}
namespace Microsoft.Xna.Framework.Net {
	public sealed partial class AvailableNetworkSession {
		public AvailableNetworkSession() { }
		public int CurrentGamerCount { get { return default(int); } }
		public string HostGamertag { get { return default(string); } }
		public int OpenPrivateGamerSlots { get { return default(int); } }
		public int OpenPublicGamerSlots { get { return default(int); } }
		public Microsoft.Xna.Framework.Net.QualityOfService QualityOfService { get { return default(Microsoft.Xna.Framework.Net.QualityOfService); } }
		public Microsoft.Xna.Framework.Net.NetworkSessionProperties SessionProperties { get { return default(Microsoft.Xna.Framework.Net.NetworkSessionProperties); } }
	}
	public sealed partial class AvailableNetworkSessionCollection : System.Collections.ObjectModel.ReadOnlyCollection<Microsoft.Xna.Framework.Net.AvailableNetworkSession>, System.IDisposable {
		public AvailableNetworkSessionCollection(System.Collections.Generic.IList<Microsoft.Xna.Framework.Net.AvailableNetworkSession> list) : base (default(System.Collections.Generic.IList<Microsoft.Xna.Framework.Net.AvailableNetworkSession>)) { }
		public void Dispose() { }
	}
	public enum CommandEventType {
		GamerJoined = 0,
		GamerLeft = 1,
		GamerStateChange = 5,
		ReceiveData = 4,
		SendData = 3,
		SessionStateChange = 2,
	}
	public partial class GameEndedEventArgs : System.EventArgs {
		public GameEndedEventArgs() { }
	}
	public partial class GamerJoinedEventArgs : System.EventArgs {
		public GamerJoinedEventArgs(Microsoft.Xna.Framework.Net.NetworkGamer aGamer) { }
		public Microsoft.Xna.Framework.Net.NetworkGamer Gamer { get { return default(Microsoft.Xna.Framework.Net.NetworkGamer); } }
	}
	public partial class GamerLeftEventArgs : System.EventArgs {
		public GamerLeftEventArgs(Microsoft.Xna.Framework.Net.NetworkGamer aGamer) { }
		public Microsoft.Xna.Framework.Net.NetworkGamer Gamer { get { return default(Microsoft.Xna.Framework.Net.NetworkGamer); } }
	}
	[System.FlagsAttribute]
	public enum GamerStates {
		Guest = 4096,
		HasVoice = 256,
		Host = 16,
		Local = 1,
		MutedByLocalUser = 65536,
		PrivateSlot = 1048576,
		Ready = 16777216,
	}
	public partial class GameStartedEventArgs : System.EventArgs {
		public GameStartedEventArgs() { }
	}
	public partial class HostChangedEventArgs : System.EventArgs {
		public HostChangedEventArgs(Microsoft.Xna.Framework.Net.NetworkGamer aNewHost, Microsoft.Xna.Framework.Net.NetworkGamer aOldHost) { }
		public Microsoft.Xna.Framework.Net.NetworkGamer NewHost { get { return default(Microsoft.Xna.Framework.Net.NetworkGamer); } }
		public Microsoft.Xna.Framework.Net.NetworkGamer OldHost { get { return default(Microsoft.Xna.Framework.Net.NetworkGamer); } }
	}
	public partial class InviteAcceptedEventArgs : System.EventArgs {
		public InviteAcceptedEventArgs(Microsoft.Xna.Framework.GamerServices.SignedInGamer aGamer) { }
		public Microsoft.Xna.Framework.GamerServices.SignedInGamer Gamer { get { return default(Microsoft.Xna.Framework.GamerServices.SignedInGamer); } }
		public bool IsCurrentSession { get { return default(bool); } }
	}
	public sealed partial class LocalNetworkGamer : Microsoft.Xna.Framework.Net.NetworkGamer {
		public LocalNetworkGamer() : base (default(Microsoft.Xna.Framework.Net.NetworkSession), default(byte), default(Microsoft.Xna.Framework.Net.GamerStates)) { }
		public LocalNetworkGamer(Microsoft.Xna.Framework.Net.NetworkSession session, byte id, Microsoft.Xna.Framework.Net.GamerStates state) : base (default(Microsoft.Xna.Framework.Net.NetworkSession), default(byte), default(Microsoft.Xna.Framework.Net.GamerStates)) { }
		public bool IsDataAvailable { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.SignedInGamer SignedInGamer { get { return default(Microsoft.Xna.Framework.GamerServices.SignedInGamer); } }
		public int ReceiveData(Microsoft.Xna.Framework.Net.PacketReader data, out Microsoft.Xna.Framework.Net.NetworkGamer sender) { sender = default(Microsoft.Xna.Framework.Net.NetworkGamer); return default(int); }
		public int ReceiveData(System.Byte[] data, out Microsoft.Xna.Framework.Net.NetworkGamer sender) { sender = default(Microsoft.Xna.Framework.Net.NetworkGamer); return default(int); }
		public int ReceiveData(System.Byte[] data, int offset, out Microsoft.Xna.Framework.Net.NetworkGamer sender) { sender = default(Microsoft.Xna.Framework.Net.NetworkGamer); return default(int); }
		public void SendData(Microsoft.Xna.Framework.Net.PacketWriter data, Microsoft.Xna.Framework.Net.SendDataOptions options) { }
		public void SendData(Microsoft.Xna.Framework.Net.PacketWriter data, Microsoft.Xna.Framework.Net.SendDataOptions options, Microsoft.Xna.Framework.Net.NetworkGamer recipient) { }
		public void SendData(System.Byte[] data, Microsoft.Xna.Framework.Net.SendDataOptions options) { }
		public void SendData(System.Byte[] data, Microsoft.Xna.Framework.Net.SendDataOptions options, Microsoft.Xna.Framework.Net.NetworkGamer recipient) { }
		public void SendData(System.Byte[] data, int offset, int count, Microsoft.Xna.Framework.Net.SendDataOptions options) { }
		public void SendData(System.Byte[] data, int offset, int count, Microsoft.Xna.Framework.Net.SendDataOptions options, Microsoft.Xna.Framework.Net.NetworkGamer recipient) { }
	}
	public partial class NetworkException : System.Exception {
		public NetworkException() { }
		public NetworkException(string message) { }
		public NetworkException(string message, System.Exception innerException) { }
	}
	public partial class NetworkGamer : Microsoft.Xna.Framework.GamerServices.Gamer, System.ComponentModel.INotifyPropertyChanged {
		public NetworkGamer(Microsoft.Xna.Framework.Net.NetworkSession session, byte id, Microsoft.Xna.Framework.Net.GamerStates state) { }
		public bool HasLeftSession { get { return default(bool); } }
		public bool HasVoice { get { return default(bool); } }
		public byte Id { get { return default(byte); } }
		public bool IsGuest { get { return default(bool); } }
		public bool IsHost { get { return default(bool); } }
		public bool IsLocal { get { return default(bool); } }
		public bool IsMutedByLocalUser { get { return default(bool); } }
		public bool IsPrivateSlot { get { return default(bool); } }
		public bool IsReady { get { return default(bool); } set { } }
		public bool IsTalking { get { return default(bool); } }
		public Microsoft.Xna.Framework.Net.NetworkMachine Machine { get { return default(Microsoft.Xna.Framework.Net.NetworkMachine); } set { } }
		public System.TimeSpan RoundtripTime { get { return default(System.TimeSpan); } }
		public Microsoft.Xna.Framework.Net.NetworkSession Session { get { return default(Microsoft.Xna.Framework.Net.NetworkSession); } }
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged { add { } remove { } }
		protected void OnPropertyChanged(string name) { }
	}
	public sealed partial class NetworkMachine {
		public NetworkMachine() { }
		public Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer> Gamers { get { return default(Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer>); } }
	}
	public partial class NetworkNotAvailableException : Microsoft.Xna.Framework.Net.NetworkException {
		public NetworkNotAvailableException() { }
		public NetworkNotAvailableException(string message) { }
		public NetworkNotAvailableException(string message, System.Exception innerException) { }
	}
	public sealed partial class NetworkSession : System.IDisposable {
		internal NetworkSession() { }
		public Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer> AllGamers { get { return default(Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer>); } }
		public bool AllowHostMigration { get { return default(bool); } set { } }
		public bool AllowJoinInProgress { get { return default(bool); } set { } }
		public Microsoft.Xna.Framework.Net.NetworkGamer Host { get { return default(Microsoft.Xna.Framework.Net.NetworkGamer); } }
		public bool IsDisposed { get { return default(bool); } }
		public bool IsEveryoneReady { get { return default(bool); } }
		public bool IsHost { get { return default(bool); } }
		public Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.LocalNetworkGamer> LocalGamers { get { return default(Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.LocalNetworkGamer>); } }
		public int MaxGamers { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer> PreviousGamers { get { return default(Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer>); } }
		public int PrivateGamerSlots { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer> RemoteGamers { get { return default(Microsoft.Xna.Framework.GamerServices.GamerCollection<Microsoft.Xna.Framework.Net.NetworkGamer>); } }
		public Microsoft.Xna.Framework.Net.NetworkSessionProperties SessionProperties { get { return default(Microsoft.Xna.Framework.Net.NetworkSessionProperties); } }
		public Microsoft.Xna.Framework.Net.NetworkSessionState SessionState { get { return default(Microsoft.Xna.Framework.Net.NetworkSessionState); } }
		public Microsoft.Xna.Framework.Net.NetworkSessionType SessionType { get { return default(Microsoft.Xna.Framework.Net.NetworkSessionType); } }
		public System.TimeSpan SimulatedLatency { get { return default(System.TimeSpan); } set { } }
		public float SimulatedPacketLoss { get { return default(float); } set { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Net.GameEndedEventArgs> GameEnded { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Net.GamerJoinedEventArgs> GamerJoined { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Net.GamerLeftEventArgs> GamerLeft { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Net.GameStartedEventArgs> GameStarted { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Net.HostChangedEventArgs> HostChanged { add { } remove { } }
		public static event System.EventHandler<Microsoft.Xna.Framework.Net.InviteAcceptedEventArgs> InviteAccepted { add { } remove { } }
		public event System.EventHandler<Microsoft.Xna.Framework.Net.NetworkSessionEndedEventArgs> SessionEnded { add { } remove { } }
		public void AddLocalGamer(Microsoft.Xna.Framework.GamerServices.SignedInGamer gamer) { }
		public static System.IAsyncResult BeginCreate(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.SignedInGamer> localGamers, int maxGamers, int privateGamerSlots, Microsoft.Xna.Framework.Net.NetworkSessionProperties sessionProperties, System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginCreate(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, int maxGamers, System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginCreate(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, int maxGamers, int privateGamerSlots, Microsoft.Xna.Framework.Net.NetworkSessionProperties sessionProperties, System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginFind(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.SignedInGamer> localGamers, Microsoft.Xna.Framework.Net.NetworkSessionProperties searchProperties, System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginFind(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, Microsoft.Xna.Framework.Net.NetworkSessionProperties searchProperties, System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginJoin(Microsoft.Xna.Framework.Net.AvailableNetworkSession availableSession, System.AsyncCallback callback, object asyncState) { return default(System.IAsyncResult); }
		public static Microsoft.Xna.Framework.Net.NetworkSession Create(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.SignedInGamer> localGamers, int maxGamers, int privateGamerSlots, Microsoft.Xna.Framework.Net.NetworkSessionProperties sessionProperties) { return default(Microsoft.Xna.Framework.Net.NetworkSession); }
		public static Microsoft.Xna.Framework.Net.NetworkSession Create(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, int maxGamers) { return default(Microsoft.Xna.Framework.Net.NetworkSession); }
		public static Microsoft.Xna.Framework.Net.NetworkSession Create(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, int maxGamers, int privateGamerSlots, Microsoft.Xna.Framework.Net.NetworkSessionProperties sessionProperties) { return default(Microsoft.Xna.Framework.Net.NetworkSession); }
		public void Dispose() { }
		public void Dispose(bool disposing) { }
		public static Microsoft.Xna.Framework.Net.NetworkSession EndCreate(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.Net.NetworkSession); }
		public static Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection EndFind(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection); }
		public void EndGame() { }
		public static Microsoft.Xna.Framework.Net.NetworkSession EndJoin(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.Net.NetworkSession); }
		~NetworkSession() { }
		public static Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection Find(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, System.Collections.Generic.IEnumerable<Microsoft.Xna.Framework.GamerServices.SignedInGamer> localGamers, Microsoft.Xna.Framework.Net.NetworkSessionProperties searchProperties) { return default(Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection); }
		public static Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection Find(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, Microsoft.Xna.Framework.Net.NetworkSessionProperties searchProperties) { return default(Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection); }
		public Microsoft.Xna.Framework.Net.NetworkGamer FindGamerById(byte gamerId) { return default(Microsoft.Xna.Framework.Net.NetworkGamer); }
		public static Microsoft.Xna.Framework.Net.NetworkSession Join(Microsoft.Xna.Framework.Net.AvailableNetworkSession availableSession) { return default(Microsoft.Xna.Framework.Net.NetworkSession); }
		public void ResetReady() { }
		public void StartGame() { }
		public void Update() { }
	}
	public delegate Microsoft.Xna.Framework.Net.NetworkSession NetworkSessionAsynchronousCreate(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int maxLocalGamers, int maxGamers, int privateGamerSlots, Microsoft.Xna.Framework.Net.NetworkSessionProperties sessionProperties, int hostGamer, bool isHost);
	public delegate Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection NetworkSessionAsynchronousFind(Microsoft.Xna.Framework.Net.NetworkSessionType sessionType, int hostGamer, int maxLocalGamers, Microsoft.Xna.Framework.Net.NetworkSessionProperties searchProperties);
	public delegate Microsoft.Xna.Framework.Net.NetworkSession NetworkSessionAsynchronousJoin(Microsoft.Xna.Framework.Net.AvailableNetworkSession availableSession);
	public delegate Microsoft.Xna.Framework.Net.NetworkSession NetworkSessionAsynchronousJoinInvited(int maxLocalGamers);
	public partial class NetworkSessionEndedEventArgs : System.EventArgs {
		public NetworkSessionEndedEventArgs(Microsoft.Xna.Framework.Net.NetworkSessionEndReason aEndReason) { }
		public Microsoft.Xna.Framework.Net.NetworkSessionEndReason EndReason { get { return default(Microsoft.Xna.Framework.Net.NetworkSessionEndReason); } }
	}
	public enum NetworkSessionEndReason {
		ClientSignedOut = 0,
		Disconnected = 3,
		HostEndedSession = 1,
		RemovedByHost = 2,
	}
	public enum NetworkSessionJoinError {
		SessionFull = 2,
		SessionNotFound = 0,
		SessionNotJoinable = 1,
	}
	public partial class NetworkSessionJoinException : Microsoft.Xna.Framework.Net.NetworkException {
		public NetworkSessionJoinException() { }
		public NetworkSessionJoinException(string message) { }
		public NetworkSessionJoinException(string message, Microsoft.Xna.Framework.Net.NetworkSessionJoinError joinError) { }
		public NetworkSessionJoinException(string message, System.Exception innerException) { }
		public Microsoft.Xna.Framework.Net.NetworkSessionJoinError JoinError { get { return default(Microsoft.Xna.Framework.Net.NetworkSessionJoinError); } set { } }
	}
	public partial class NetworkSessionProperties : System.Collections.Generic.List<System.Nullable<System.Int32>> {
		public NetworkSessionProperties() { }
		public static void ReadProperties(Microsoft.Xna.Framework.Net.NetworkSessionProperties properties, System.Int32[] propertyData) { }
		public static void WriteProperties(Microsoft.Xna.Framework.Net.NetworkSessionProperties properties, System.Int32[] propertyData) { }
	}
	public enum NetworkSessionState {
		Ended = 2,
		Lobby = 0,
		Playing = 1,
	}
	public enum NetworkSessionType {
		Local = 0,
		PlayerMatch = 2,
		Ranked = 3,
		SystemLink = 1,
	}
	public partial class PacketReader : System.IO.BinaryReader {
		public PacketReader() : base (default(System.IO.Stream)) { }
		public PacketReader(int capacity) : base (default(System.IO.Stream)) { }
		public int Length { get { return default(int); } }
		public int Position { get { return default(int); } set { } }
		public Microsoft.Xna.Framework.Color ReadColor() { return default(Microsoft.Xna.Framework.Color); }
		public override double ReadDouble() { return default(double); }
		public Microsoft.Xna.Framework.Matrix ReadMatrix() { return default(Microsoft.Xna.Framework.Matrix); }
		public Microsoft.Xna.Framework.Quaternion ReadQuaternion() { return default(Microsoft.Xna.Framework.Quaternion); }
		public Microsoft.Xna.Framework.Vector2 ReadVector2() { return default(Microsoft.Xna.Framework.Vector2); }
		public Microsoft.Xna.Framework.Vector3 ReadVector3() { return default(Microsoft.Xna.Framework.Vector3); }
		public Microsoft.Xna.Framework.Vector4 ReadVector4() { return default(Microsoft.Xna.Framework.Vector4); }
	}
	public partial class PacketWriter : System.IO.BinaryWriter {
		public PacketWriter() { }
		public PacketWriter(int capacity) { }
		public int Length { get { return default(int); } }
		public int Position { get { return default(int); } set { } }
		public void Write(Microsoft.Xna.Framework.Color Value) { }
		public void Write(Microsoft.Xna.Framework.Matrix Value) { }
		public void Write(Microsoft.Xna.Framework.Quaternion Value) { }
		public void Write(Microsoft.Xna.Framework.Vector2 Value) { }
		public void Write(Microsoft.Xna.Framework.Vector3 Value) { }
		public void Write(Microsoft.Xna.Framework.Vector4 Value) { }
		public override void Write(double Value) { }
		public override void Write(float Value) { }
	}
	public partial class QualityOfService {
		public QualityOfService() { }
		public System.TimeSpan AverageRoundtripTime { get { return default(System.TimeSpan); } }
		public int BytesPerSecondDownstream { get { return default(int); } }
		public int BytesPerSecondUpstream { get { return default(int); } }
		public bool IsAvailable { get { return default(bool); } }
		public System.TimeSpan MinimumRoundtripTime { get { return default(System.TimeSpan); } }
	}
	public enum SendDataOptions {
		Chat = 4,
		InOrder = 2,
		None = 0,
		Reliable = 1,
		ReliableInOrder = 3,
	}
}
namespace Microsoft.Xna.Framework.Storage {
	public delegate Microsoft.Xna.Framework.Storage.StorageContainer OpenContainerAsynchronous(string displayName);
	public delegate Microsoft.Xna.Framework.Storage.StorageDevice ShowSelectorAsynchronousShow(Microsoft.Xna.Framework.PlayerIndex player, int sizeInBytes, int directoryCount);
	public delegate Microsoft.Xna.Framework.Storage.StorageDevice ShowSelectorAsynchronousShowNoPlayer(int sizeInBytes, int directoryCount);
	public partial class StorageContainer : System.IDisposable {
		internal StorageContainer() { }
		public string DisplayName { get { return default(string); } }
		public bool IsDisposed { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
		public Microsoft.Xna.Framework.Storage.StorageDevice StorageDevice { get { return default(Microsoft.Xna.Framework.Storage.StorageDevice); } }
		public event System.EventHandler<System.EventArgs> Disposing { add { } remove { } }
		public void CreateDirectory(string directory) { }
		public System.IO.Stream CreateFile(string file) { return default(System.IO.Stream); }
		public void DeleteDirectory(string directory) { }
		public void DeleteFile(string file) { }
		public bool DirectoryExists(string directory) { return default(bool); }
		public void Dispose() { }
		public bool FileExists(string file) { return default(bool); }
		public System.String[] GetDirectoryNames() { return default(System.String[]); }
		public System.String[] GetFileNames() { return default(System.String[]); }
		public System.String[] GetFileNames(string searchPattern) { return default(System.String[]); }
	}
	public sealed partial class StorageDevice {
		internal StorageDevice() { }
		public long FreeSpace { get { return default(long); } }
		public bool IsConnected { get { return default(bool); } }
		public long TotalSpace { get { return default(long); } }
		public static event System.EventHandler<System.EventArgs> DeviceChanged { add { } remove { } }
		public System.IAsyncResult BeginOpenContainer(string displayName, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowSelector(Microsoft.Xna.Framework.PlayerIndex player, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowSelector(Microsoft.Xna.Framework.PlayerIndex player, int sizeInBytes, int directoryCount, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowSelector(System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public static System.IAsyncResult BeginShowSelector(int sizeInBytes, int directoryCount, System.AsyncCallback callback, object state) { return default(System.IAsyncResult); }
		public Microsoft.Xna.Framework.Storage.StorageContainer EndOpenContainer(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.Storage.StorageContainer); }
		public static Microsoft.Xna.Framework.Storage.StorageDevice EndShowSelector(System.IAsyncResult result) { return default(Microsoft.Xna.Framework.Storage.StorageDevice); }
	}
}
