namespace Box2D
{
    public abstract partial class b2ReusedObject<T> where T : Box2D.b2ReusedObject<T>, new()
    {
        protected b2ReusedObject() { }
        public static T Create() { return default(T); }
        public void Free(bool skipAssert = false) { }
        public static void FreeAll() { }
    }
}
namespace Box2D.Collision
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2AABB
    {
        public static Box2D.Collision.b2AABB Default;
        public Box2D.Common.b2Vec2 LowerBound;
        public Box2D.Common.b2Vec2 UpperBound;
        public Box2D.Common.b2Vec2 Center { get { return default(Box2D.Common.b2Vec2); } }
        public Box2D.Common.b2Vec2 Extents { get { return default(Box2D.Common.b2Vec2); } }
        public float LowerBoundX { get { return default(float); } set { } }
        public float LowerBoundY { get { return default(float); } set { } }
        public float Perimeter { get { return default(float); } }
        public float UpperBoundX { get { return default(float); } set { } }
        public float UpperBoundY { get { return default(float); } set { } }
        public void Combine(ref Box2D.Collision.b2AABB aabb) { }
        public void Combine(ref Box2D.Collision.b2AABB aabb1, ref Box2D.Collision.b2AABB aabb2) { }
        public static void Combine(ref Box2D.Collision.b2AABB aabb1, ref Box2D.Collision.b2AABB aabb2, out Box2D.Collision.b2AABB output) { output = default(Box2D.Collision.b2AABB); }
        public bool Contains(ref Box2D.Collision.b2AABB aabb) { return default(bool); }
        public bool Equals(Box2D.Collision.b2AABB o) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public void Fatten() { }
        public void Fatten(float amt) { }
        [System.ObsoleteAttribute("Use the property accessor")]
        public Box2D.Common.b2Vec2 GetCenter() { return default(Box2D.Common.b2Vec2); }
        [System.ObsoleteAttribute("Use the property accessor")]
        public Box2D.Common.b2Vec2 GetExtents() { return default(Box2D.Common.b2Vec2); }
        [System.ObsoleteAttribute("Use the property accessor")]
        public float GetPerimeter() { return default(float); }
        public bool IsValid() { return default(bool); }
        public static bool operator ==(Box2D.Collision.b2AABB a, Box2D.Collision.b2AABB b) { return default(bool); }
        public static bool operator !=(Box2D.Collision.b2AABB a, Box2D.Collision.b2AABB b) { return default(bool); }
        public bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input) { output = default(Box2D.Collision.b2RayCastOutput); return default(bool); }
        public void Set(Box2D.Common.b2Vec2 lower, Box2D.Common.b2Vec2 upper) { }
        public void Set(float lx, float ly, float ux, float uy) { }
        public void SetLowerBound(float x, float y) { }
        public void SetUpperBound(float x, float y) { }
    }
    public partial class b2BroadPhase : Box2D.Dynamics.Ib2QueryCallback, System.Collections.Generic.IComparer<Box2D.Collision.b2Pair>
    {
        public static int e_nullProxy;
        public b2BroadPhase() { }
        public bool b2PairLessThan(Box2D.Collision.b2Pair pair1, Box2D.Collision.b2Pair pair2) { return default(bool); }
        public void BufferMove(int proxyId) { }
        public int Compare(Box2D.Collision.b2Pair pair1, Box2D.Collision.b2Pair pair2) { return default(int); }
        public int CreateProxy(ref Box2D.Collision.b2AABB aabb, ref Box2D.Dynamics.b2FixtureProxy userData) { return default(int); }
        public void DestroyProxy(int proxyId) { }
        public void GetFatAABB(int proxyId, out Box2D.Collision.b2AABB output) { output = default(Box2D.Collision.b2AABB); }
        public int GetProxyCount() { return default(int); }
        public int GetTreeBalance() { return default(int); }
        public int GetTreeHeight() { return default(int); }
        public float GetTreeQuality() { return default(float); }
        public Box2D.Dynamics.b2FixtureProxy GetUserData(int proxyId) { return default(Box2D.Dynamics.b2FixtureProxy); }
        public void MoveProxy(int proxyId, ref Box2D.Collision.b2AABB aabb, ref Box2D.Common.b2Vec2 displacement) { }
        public virtual void Query(Box2D.Dynamics.Ib2QueryCallback q, Box2D.Collision.b2AABB aabb) { }
        public virtual bool QueryCallback(int proxyId) { return default(bool); }
        public virtual void RayCast(Box2D.Dynamics.b2WorldRayCastWrapper w, Box2D.Collision.b2RayCastInput input) { }
        public bool TestOverlap(int proxyIdA, int proxyIdB) { return default(bool); }
        public void TouchProxy(int proxyId) { }
        public void UnBufferMove(int proxyId) { }
        public void UpdatePairs(Box2D.Dynamics.b2ContactManager callback) { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2ClipVertex
    {
        public Box2D.Collision.b2ContactFeature id;
        public Box2D.Common.b2Vec2 v;
        public void Dump() { }
    }
    public abstract partial class b2Collision
    {
        public static byte b2_nullFeature;
        protected b2Collision() { }
        public static int b2ClipSegmentToLine(Box2D.Collision.b2ClipVertex[] vOut, Box2D.Collision.b2ClipVertex[] vIn, ref Box2D.Common.b2Vec2 normal, float offset, byte vertexIndexA) { return default(int); }
        public static void b2CollideCircles(Box2D.Collision.b2Manifold manifold, Box2D.Collision.Shapes.b2CircleShape circleA, ref Box2D.Common.b2Transform xfA, Box2D.Collision.Shapes.b2CircleShape circleB, ref Box2D.Common.b2Transform xfB) { }
        public static void b2CollideEdgeAndCircle(Box2D.Collision.b2Manifold manifold, Box2D.Collision.Shapes.b2EdgeShape edgeA, ref Box2D.Common.b2Transform xfA, Box2D.Collision.Shapes.b2CircleShape circleB, ref Box2D.Common.b2Transform xfB) { }
        public static void b2CollideEdgeAndPolygon(Box2D.Collision.b2Manifold manifold, Box2D.Collision.Shapes.b2EdgeShape edgeA, ref Box2D.Common.b2Transform xfA, Box2D.Collision.Shapes.b2PolygonShape polygonB, ref Box2D.Common.b2Transform xfB) { }
        public static void b2CollidePolygonAndCircle(Box2D.Collision.b2Manifold manifold, Box2D.Collision.Shapes.b2PolygonShape polygonA, ref Box2D.Common.b2Transform xfA, Box2D.Collision.Shapes.b2CircleShape circleB, ref Box2D.Common.b2Transform xfB) { }
        public static void b2CollidePolygons(Box2D.Collision.b2Manifold manifold, Box2D.Collision.Shapes.b2PolygonShape polyA, ref Box2D.Common.b2Transform xfA, Box2D.Collision.Shapes.b2PolygonShape polyB, ref Box2D.Common.b2Transform xfB) { }
        public static float b2EdgeSeparation(Box2D.Collision.Shapes.b2PolygonShape poly1, ref Box2D.Common.b2Transform xf1, int edge1, Box2D.Collision.Shapes.b2PolygonShape poly2, ref Box2D.Common.b2Transform xf2) { return default(float); }
        public static void b2FindIncidentEdge(Box2D.Collision.b2ClipVertex[] c, Box2D.Collision.Shapes.b2PolygonShape poly1, ref Box2D.Common.b2Transform xf1, int edge1, Box2D.Collision.Shapes.b2PolygonShape poly2, ref Box2D.Common.b2Transform xf2) { }
        public static float b2FindMaxSeparation(out int edgeIndex, Box2D.Collision.Shapes.b2PolygonShape poly1, ref Box2D.Common.b2Transform xf1, Box2D.Collision.Shapes.b2PolygonShape poly2, ref Box2D.Common.b2Transform xf2) { edgeIndex = default(int); return default(float); }
        public static void b2GetPointStates(Box2D.Collision.b2PointState[] state1, Box2D.Collision.b2PointState[] state2, Box2D.Collision.b2Manifold manifold1, Box2D.Collision.b2Manifold manifold2) { }
        public static bool b2TestOverlap(ref Box2D.Collision.b2AABB a, ref Box2D.Collision.b2AABB b) { return default(bool); }
        public static bool b2TestOverlap(Box2D.Collision.Shapes.b2Shape shapeA, int indexA, Box2D.Collision.Shapes.b2Shape shapeB, int indexB, ref Box2D.Common.b2Transform xfA, ref Box2D.Common.b2Transform xfB) { return default(bool); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2ContactFeature
    {
        public byte indexA;
        public byte indexB;
        public Box2D.Collision.b2ContactFeatureType typeA;
        public Box2D.Collision.b2ContactFeatureType typeB;
        public static Box2D.Collision.b2ContactFeature Zero;
        public b2ContactFeature(byte iA, byte iB, Box2D.Collision.b2ContactFeatureType tA, Box2D.Collision.b2ContactFeatureType tB) { throw new System.NotImplementedException(); }
        public int key { get { return default(int); } set { } }
        public bool Equals(ref Box2D.Collision.b2ContactFeature bcf) { return default(bool); }
        public override bool Equals(object o) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        public void Set(Box2D.Collision.b2ContactFeature cf) { }
    }
    public enum b2ContactFeatureType : byte
    {
        e_face = (byte)1,
        e_vertex = (byte)0,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2DistanceInput
    {
        public Box2D.Collision.b2DistanceProxy proxyA;
        public Box2D.Collision.b2DistanceProxy proxyB;
        public Box2D.Common.b2Transform transformA;
        public Box2D.Common.b2Transform transformB;
        public bool useRadii;
        public static Box2D.Collision.b2DistanceInput Create() { return default(Box2D.Collision.b2DistanceInput); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2DistanceOutput
    {
        public float distance;
        public int iterations;
        public Box2D.Common.b2Vec2 pointA;
        public Box2D.Common.b2Vec2 pointB;
    }
    public partial class b2DistanceProxy
    {
        public static int b2_gjkCalls;
        public static int b2_gjkIters;
        public static int b2_gjkMaxIters;
        public b2DistanceProxy() { }
        public int Count { get { return default(int); } set { } }
        public float Radius { get { return default(float); } set { } }
        public static Box2D.Collision.b2DistanceProxy Create() { return default(Box2D.Collision.b2DistanceProxy); }
        public static Box2D.Collision.b2DistanceProxy Create(Box2D.Collision.Shapes.b2Shape shape, int index) { return default(Box2D.Collision.b2DistanceProxy); }
        public int GetSupport(ref Box2D.Common.b2Vec2 d) { return default(int); }
        public Box2D.Common.b2Vec2 GetSupportVertex(Box2D.Common.b2Vec2 d) { return default(Box2D.Common.b2Vec2); }
        public Box2D.Common.b2Vec2 GetVertex(int index) { return default(Box2D.Common.b2Vec2); }
        public Box2D.Common.b2Vec2 GetVertex(uint index) { return default(Box2D.Common.b2Vec2); }
        public int GetVertexCount() { return default(int); }
        public void Set(Box2D.Collision.Shapes.b2Shape shape, int index) { }
    }
    public partial class b2DynamicTree<T>
    {
        public b2DynamicTree() { }
        public int AllocateNode() { return default(int); }
        public int ComputeHeight() { return default(int); }
        public int ComputeHeight(int nodeId) { return default(int); }
        public int CreateProxy(ref Box2D.Collision.b2AABB aabb, ref T userData) { return default(int); }
        public void DestroyProxy(int proxyId) { }
        public float GetAreaRatio() { return default(float); }
        public void GetFatAABB(int proxyId, out Box2D.Collision.b2AABB output) { output = default(Box2D.Collision.b2AABB); }
        public int GetHeight() { return default(int); }
        public int GetMaxBalance() { return default(int); }
        public T GetUserData(int proxyId) { return default(T); }
        public void InsertLeaf(int leaf) { }
        public bool MoveProxy(int proxyId, ref Box2D.Collision.b2AABB aabb, ref Box2D.Common.b2Vec2 displacement) { return default(bool); }
        public void Query(Box2D.Dynamics.Ib2QueryCallback w, Box2D.Collision.b2AABB aabb) { }
        public void RayCast(Box2D.Dynamics.Ib2RayCastCallback callback, Box2D.Collision.b2RayCastInput input) { }
        public void RebuildBottomUp() { }
        public void Validate() { }
        public void ValidateMetrics(int index) { }
        public void ValidateStructure(int index) { }
        public partial class b2TreeNode
        {
            public Box2D.Collision.b2AABB aabb;
            public const int b2_nullNode = -1;
            public int child1;
            public int child2;
            public int height;
            public int parentOrNext;
            public T userData;
            public b2TreeNode() { }
            public bool IsLeaf() { return default(bool); }
        }
    }
    public partial class b2EPCollider : Box2D.b2ReusedObject<Box2D.Collision.b2EPCollider>
    {
        protected Box2D.Common.b2Vec2 m_centroidB;
        protected Box2D.Common.b2Vec2 m_lowerLimit;
        protected Box2D.Common.b2Vec2 m_normal;
        protected Box2D.Common.b2Vec2 m_normal0;
        protected Box2D.Common.b2Vec2 m_normal1;
        protected Box2D.Common.b2Vec2 m_normal2;
        protected float m_radius;
        protected Box2D.Collision.b2EPCollider.VertexType m_type1;
        protected Box2D.Collision.b2EPCollider.VertexType m_type2;
        protected Box2D.Common.b2Vec2 m_upperLimit;
        protected Box2D.Common.b2Vec2 m_v0;
        protected Box2D.Common.b2Vec2 m_v1;
        protected Box2D.Common.b2Vec2 m_v2;
        protected Box2D.Common.b2Vec2 m_v3;
        protected Box2D.Common.b2Transform m_xf;
        public b2EPCollider() { }
        public void Collide(Box2D.Collision.b2Manifold manifold, Box2D.Collision.Shapes.b2EdgeShape edgeA, ref Box2D.Common.b2Transform xfA, Box2D.Collision.Shapes.b2PolygonShape polygonB, ref Box2D.Common.b2Transform xfB) { }
        public Box2D.Collision.b2EPCollider.b2EPAxis ComputeEdgeSeparation() { return default(Box2D.Collision.b2EPCollider.b2EPAxis); }
        public Box2D.Collision.b2EPCollider.b2EPAxis ComputePolygonSeparation() { return default(Box2D.Collision.b2EPCollider.b2EPAxis); }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public partial struct b2EPAxis
        {
            public int index;
            public float separation;
            public Box2D.Collision.b2EPCollider.b2EPAxisType type;
        }
        public enum b2EPAxisType
        {
            e_edgeA = 1,
            e_edgeB = 2,
            e_unknown = 0,
        }
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public partial struct b2ReferenceFace
        {
            public int i1;
            public int i2;
            public Box2D.Common.b2Vec2 normal;
            public Box2D.Common.b2Vec2 sideNormal1;
            public Box2D.Common.b2Vec2 sideNormal2;
            public float sideOffset1;
            public float sideOffset2;
            public Box2D.Common.b2Vec2 v1;
            public Box2D.Common.b2Vec2 v2;
        }
        public enum VertexType
        {
            e_concave = 1,
            e_convex = 2,
            e_isolated = 0,
        }
    }
    public enum b2ImpactState
    {
        e_failed = 1,
        e_overlapped = 2,
        e_separated = 4,
        e_touching = 3,
        e_unknown = 0,
    }
    public partial class b2Manifold
    {
        public Box2D.Common.b2Vec2 localNormal;
        public Box2D.Common.b2Vec2 localPoint;
        public int pointCount;
        public Box2D.Collision.b2ManifoldPoint[] points;
        public Box2D.Collision.b2ManifoldType type;
        public b2Manifold() { }
        public void CopyFrom(Box2D.Collision.b2Manifold other) { }
    }
    public partial class b2ManifoldPoint
    {
        public Box2D.Collision.b2ContactFeature id;
        public Box2D.Common.b2Vec2 localPoint;
        public float normalImpulse;
        public float tangentImpulse;
        public b2ManifoldPoint() { }
    }
    public enum b2ManifoldType
    {
        e_circles = 0,
        e_faceA = 1,
        e_faceB = 2,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Pair
    {
        public int next;
        public int proxyIdA;
        public int proxyIdB;
    }
    public enum b2PointState
    {
        b2_addState = 1,
        b2_nullState = 0,
        b2_persistState = 2,
        b2_removeState = 3,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2RayCastInput
    {
        public float maxFraction;
        public Box2D.Common.b2Vec2 p1;
        public Box2D.Common.b2Vec2 p2;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2RayCastOutput
    {
        public float fraction;
        public Box2D.Common.b2Vec2 normal;
        public static Box2D.Collision.b2RayCastOutput Zero;
        public b2RayCastOutput(Box2D.Common.b2Vec2 b, float f) { throw new System.NotImplementedException(); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2SeparationFunction
    {
        public float Evaluate(int indexA, int indexB, float t) { return default(float); }
        public float FindMinSeparation(out int indexA, out int indexB, float t) { indexA = default(int); indexB = default(int); return default(float); }
        public float Initialize(ref Box2D.Collision.b2SimplexCache cache, Box2D.Collision.b2DistanceProxy proxyA, ref Box2D.Common.b2Sweep sweepA, Box2D.Collision.b2DistanceProxy proxyB, ref Box2D.Common.b2Sweep sweepB, float t1, ref Box2D.Common.b2Transform xfA, ref Box2D.Common.b2Transform xfB) { return default(float); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Simplex
    {
        public static void b2Distance(out Box2D.Collision.b2DistanceOutput output, ref Box2D.Collision.b2SimplexCache cache, ref Box2D.Collision.b2DistanceInput input) { output = default(Box2D.Collision.b2DistanceOutput); }
        public void GetClosestPoint(out Box2D.Common.b2Vec2 point) { point = default(Box2D.Common.b2Vec2); }
        public float GetMetric() { return default(float); }
        public void GetSearchDirection(out Box2D.Common.b2Vec2 dir) { dir = default(Box2D.Common.b2Vec2); }
        public void GetWitnessPoints(out Box2D.Common.b2Vec2 pA, out Box2D.Common.b2Vec2 pB) { pA = default(Box2D.Common.b2Vec2); pB = default(Box2D.Common.b2Vec2); }
        public void Solve2() { }
        public void Solve3() { }
        public void WriteCache(ref Box2D.Collision.b2SimplexCache cache) { }
        public partial class b2SimplexVertex
        {
            public float a;
            public int indexA;
            public int indexB;
            public Box2D.Common.b2Vec2 w;
            public Box2D.Common.b2Vec2 wA;
            public Box2D.Common.b2Vec2 wB;
            public b2SimplexVertex() { }
            public void CopyFrom(Box2D.Collision.b2Simplex.b2SimplexVertex other) { }
        }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2SimplexCache
    {
        public int count;
        public System.Int32[] indexA;
        public System.Int32[] indexB;
        public float metric;
        public static Box2D.Collision.b2SimplexCache Create() { return default(Box2D.Collision.b2SimplexCache); }
        public void Defaults() { }
    }
    public partial class b2TimeOfImpact
    {
        public static int b2_toiCalls;
        public static int b2_toiIters;
        public static int b2_toiMaxIters;
        public static int b2_toiMaxRootIters;
        public static int b2_toiRootIters;
        public b2TimeOfImpact() { }
        public static void Compute(out Box2D.Collision.b2TOIOutput output, ref Box2D.Collision.b2TOIInput input) { output = default(Box2D.Collision.b2TOIOutput); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2TOIInput
    {
        public Box2D.Collision.b2DistanceProxy proxyA;
        public Box2D.Collision.b2DistanceProxy proxyB;
        public Box2D.Common.b2Sweep sweepA;
        public Box2D.Common.b2Sweep sweepB;
        public float tMax;
        public static Box2D.Collision.b2TOIInput Zero;
        public static Box2D.Collision.b2TOIInput Create() { return default(Box2D.Collision.b2TOIInput); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2TOIOutput
    {
        public Box2D.Collision.b2ImpactState state;
        public float t;
    }
    public partial class b2WorldManifold
    {
        public Box2D.Common.b2Vec2 normal;
        public Box2D.Common.b2Vec2[] points;
        public b2WorldManifold() { }
        public void Initialize(Box2D.Collision.b2Manifold manifold, ref Box2D.Common.b2Transform xfA, float radiusA, ref Box2D.Common.b2Transform xfB, float radiusB) { }
    }
    public enum SeparationType
    {
        e_faceA = 1,
        e_faceB = 2,
        e_points = 0,
    }
}
namespace Box2D.Collision.Shapes
{
    public partial class b2ChainShape : Box2D.Collision.Shapes.b2Shape
    {
        public int Count;
        public bool HasNextVertex;
        public bool HasPrevVertex;
        public Box2D.Common.b2Vec2 NextVertex;
        public Box2D.Common.b2Vec2 PrevVertex;
        public Box2D.Common.b2Vec2[] Vertices;
        public b2ChainShape() { }
        public b2ChainShape(Box2D.Collision.Shapes.b2ChainShape clone) { }
        public override Box2D.Collision.Shapes.b2Shape Clone() { return default(Box2D.Collision.Shapes.b2Shape); }
        public override void ComputeAABB(out Box2D.Collision.b2AABB output, ref Box2D.Common.b2Transform xf, int childIndex) { output = default(Box2D.Collision.b2AABB); }
        public override Box2D.Collision.Shapes.b2MassData ComputeMass(float density) { return default(Box2D.Collision.Shapes.b2MassData); }
        public virtual void CreateChain(Box2D.Common.b2Vec2[] vertices, int count) { }
        public virtual void CreateLoop(Box2D.Common.b2Vec2[] vertices, int count) { }
        public override int GetChildCount() { return default(int); }
        public virtual Box2D.Collision.Shapes.b2EdgeShape GetChildEdge(int index) { return default(Box2D.Collision.Shapes.b2EdgeShape); }
        public override bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input, ref Box2D.Common.b2Transform xf, int childIndex) { output = default(Box2D.Collision.b2RayCastOutput); return default(bool); }
        public virtual void SetNextVertex(Box2D.Common.b2Vec2 nextVertex) { }
        public virtual void SetPrevVertex(Box2D.Common.b2Vec2 prevVertex) { }
        public override bool TestPoint(ref Box2D.Common.b2Transform xf, Box2D.Common.b2Vec2 p) { return default(bool); }
    }
    public partial class b2CircleShape : Box2D.Collision.Shapes.b2Shape
    {
        public Box2D.Common.b2Vec2 Position;
        public b2CircleShape() { }
        public b2CircleShape(Box2D.Collision.Shapes.b2CircleShape copy) { }
        public override Box2D.Collision.Shapes.b2Shape Clone() { return default(Box2D.Collision.Shapes.b2Shape); }
        public override void ComputeAABB(out Box2D.Collision.b2AABB output, ref Box2D.Common.b2Transform transform, int childIndex) { output = default(Box2D.Collision.b2AABB); }
        public override Box2D.Collision.Shapes.b2MassData ComputeMass(float density) { return default(Box2D.Collision.Shapes.b2MassData); }
        public override int GetChildCount() { return default(int); }
        public virtual int GetSupport(Box2D.Common.b2Vec2 d) { return default(int); }
        public virtual Box2D.Common.b2Vec2 GetSupportVertex(Box2D.Common.b2Vec2 d) { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetVertex(int index) { return default(Box2D.Common.b2Vec2); }
        public virtual int GetVertexCount() { return default(int); }
        public override bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input, ref Box2D.Common.b2Transform transform, int childIndex) { output = default(Box2D.Collision.b2RayCastOutput); return default(bool); }
        public override bool TestPoint(ref Box2D.Common.b2Transform transform, Box2D.Common.b2Vec2 p) { return default(bool); }
    }
    public partial class b2EdgeShape : Box2D.Collision.Shapes.b2Shape
    {
        public bool HasVertex0;
        public bool HasVertex3;
        public Box2D.Common.b2Vec2 Vertex0;
        public Box2D.Common.b2Vec2 Vertex1;
        public Box2D.Common.b2Vec2 Vertex2;
        public Box2D.Common.b2Vec2 Vertex3;
        public b2EdgeShape() { }
        public b2EdgeShape(Box2D.Collision.Shapes.b2EdgeShape e) { }
        public override Box2D.Collision.Shapes.b2Shape Clone() { return default(Box2D.Collision.Shapes.b2Shape); }
        public override void ComputeAABB(out Box2D.Collision.b2AABB output, ref Box2D.Common.b2Transform xf, int childIndex) { output = default(Box2D.Collision.b2AABB); }
        public override Box2D.Collision.Shapes.b2MassData ComputeMass(float density) { return default(Box2D.Collision.Shapes.b2MassData); }
        public override int GetChildCount() { return default(int); }
        public override bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input, ref Box2D.Common.b2Transform xf, int childIndex) { output = default(Box2D.Collision.b2RayCastOutput); return default(bool); }
        public virtual void Set(Box2D.Common.b2Vec2 v1, Box2D.Common.b2Vec2 v2) { }
        public override bool TestPoint(ref Box2D.Common.b2Transform xf, Box2D.Common.b2Vec2 p) { return default(bool); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2MassData
    {
        public Box2D.Common.b2Vec2 center;
        public static Box2D.Collision.Shapes.b2MassData Default;
        public float I;
        public float mass;
    }
    public partial class b2PolygonShape : Box2D.Collision.Shapes.b2Shape
    {
        public Box2D.Common.b2Vec2 Centroid;
        public Box2D.Common.b2Vec2[] Normals;
        public Box2D.Common.b2Vec2[] Vertices;
        public b2PolygonShape() { }
        public b2PolygonShape(Box2D.Collision.Shapes.b2PolygonShape copy) { }
        public int VertexCount { get { return default(int); } }
        public override Box2D.Collision.Shapes.b2Shape Clone() { return default(Box2D.Collision.Shapes.b2Shape); }
        public override void ComputeAABB(out Box2D.Collision.b2AABB output, ref Box2D.Common.b2Transform xf, int childIndex) { output = default(Box2D.Collision.b2AABB); }
        public static Box2D.Common.b2Vec2 ComputeCentroid(Box2D.Common.b2Vec2[] vs, int count) { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Collision.Shapes.b2MassData ComputeMass(float density) { return default(Box2D.Collision.Shapes.b2MassData); }
        public override int GetChildCount() { return default(int); }
        public override bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input, ref Box2D.Common.b2Transform xf, int childIndex) { output = default(Box2D.Collision.b2RayCastOutput); return default(bool); }
        public virtual void Set(Box2D.Common.b2Vec2[] vertices, int count) { }
        public void SetAsBox(float hx, float hy) { }
        public void SetAsBox(float hx, float hy, Box2D.Common.b2Vec2 center, float angle) { }
        public override bool TestPoint(ref Box2D.Common.b2Transform xf, Box2D.Common.b2Vec2 p) { return default(bool); }
    }
    public abstract partial class b2Shape
    {
        public float Radius;
        public Box2D.Collision.Shapes.b2ShapeType ShapeType;
        public b2Shape() { }
        public b2Shape(Box2D.Collision.Shapes.b2Shape copy) { }
        public abstract Box2D.Collision.Shapes.b2Shape Clone();
        public abstract void ComputeAABB(out Box2D.Collision.b2AABB output, ref Box2D.Common.b2Transform xf, int childIndex);
        public abstract Box2D.Collision.Shapes.b2MassData ComputeMass(float density);
        public abstract int GetChildCount();
        public Box2D.Collision.Shapes.b2ShapeType GetShapeType() { return default(Box2D.Collision.Shapes.b2ShapeType); }
        public abstract bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input, ref Box2D.Common.b2Transform transform, int childIndex);
        public abstract bool TestPoint(ref Box2D.Common.b2Transform xf, Box2D.Common.b2Vec2 p);
    }
    public enum b2ShapeType
    {
        e_chain = 3,
        e_circle = 0,
        e_edge = 1,
        e_polygon = 2,
        e_typeCount = 4,
    }
}
namespace Box2D.Common
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Color
    {
        public float b;
        public float g;
        public float r;
        public b2Color(float xr, float xg, float xb) { throw new System.NotImplementedException(); }
        public void Set(float ri, float gi, float bi) { }
    }
    public abstract partial class b2Draw
    {
        public b2Draw() { }
        public b2Draw(int ptm) { }
        public Box2D.Common.b2DrawFlags Flags { get { return default(Box2D.Common.b2DrawFlags); } set { } }
        public int PTMRatio { get { return default(int); } }
        public void AppendFlags(Box2D.Common.b2DrawFlags flags) { }
        public void ClearFlags(Box2D.Common.b2DrawFlags flags) { }
        public abstract void DrawCircle(Box2D.Common.b2Vec2 center, float radius, Box2D.Common.b2Color color);
        public abstract void DrawPolygon(Box2D.Common.b2Vec2[] vertices, int vertexCount, Box2D.Common.b2Color color);
        public abstract void DrawSegment(Box2D.Common.b2Vec2 p1, Box2D.Common.b2Vec2 p2, Box2D.Common.b2Color color);
        public abstract void DrawSolidCircle(Box2D.Common.b2Vec2 center, float radius, Box2D.Common.b2Vec2 axis, Box2D.Common.b2Color color);
        public abstract void DrawSolidPolygon(Box2D.Common.b2Vec2[] vertices, int vertexCount, Box2D.Common.b2Color color);
        public abstract void DrawTransform(Box2D.Common.b2Transform xf);
        public void SetFlags(Box2D.Common.b2DrawFlags flags) { }
    }
    [System.FlagsAttribute]
    public enum b2DrawFlags
    {
        e_aabbBit = 4,
        e_centerOfMassBit = 16,
        e_jointBit = 2,
        e_pairBit = 8,
        e_shapeBit = 1,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Mat22
    {
        public Box2D.Common.b2Vec2 ex;
        public Box2D.Common.b2Vec2 ey;
        public static readonly Box2D.Common.b2Mat22 Identity;
        public static readonly Box2D.Common.b2Mat22 Zero;
        public b2Mat22(Box2D.Common.b2Vec2 c1, Box2D.Common.b2Vec2 c2) { throw new System.NotImplementedException(); }
        public b2Mat22(float a11, float a12, float a21, float a22) { throw new System.NotImplementedException(); }
        public float exx { set { } }
        public float exy { set { } }
        public float eyx { set { } }
        public float eyy { set { } }
        public Box2D.Common.b2Mat22 GetInverse() { return default(Box2D.Common.b2Mat22); }
        public void GetInverse(out Box2D.Common.b2Mat22 matrix) { matrix = default(Box2D.Common.b2Mat22); }
        public static Box2D.Common.b2Mat22 operator +(Box2D.Common.b2Mat22 A, Box2D.Common.b2Mat22 B) { return default(Box2D.Common.b2Mat22); }
        public static Box2D.Common.b2Mat22 operator *(Box2D.Common.b2Mat22 A, Box2D.Common.b2Mat22 B) { return default(Box2D.Common.b2Mat22); }
        public static Box2D.Common.b2Vec2 operator *(Box2D.Common.b2Mat22 A, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 operator *(Box2D.Common.b2Vec2 v, Box2D.Common.b2Mat22 A) { return default(Box2D.Common.b2Vec2); }
        public void Set(Box2D.Common.b2Vec2 c1, Box2D.Common.b2Vec2 c2) { }
        public void SetIdentity() { }
        public void SetZero() { }
        public Box2D.Common.b2Vec2 Solve(Box2D.Common.b2Vec2 b) { return default(Box2D.Common.b2Vec2); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Mat33
    {
        public Box2D.Common.b2Vec3 ex;
        public Box2D.Common.b2Vec3 ey;
        public Box2D.Common.b2Vec3 ez;
        public b2Mat33(Box2D.Common.b2Vec3 c1, Box2D.Common.b2Vec3 c2, Box2D.Common.b2Vec3 c3) { throw new System.NotImplementedException(); }
        public float exx { get { return default(float); } set { } }
        public float exy { get { return default(float); } set { } }
        public float exz { get { return default(float); } set { } }
        public float eyx { get { return default(float); } set { } }
        public float eyy { get { return default(float); } set { } }
        public float eyz { get { return default(float); } set { } }
        public float ezx { get { return default(float); } set { } }
        public float ezy { get { return default(float); } set { } }
        public float ezz { get { return default(float); } set { } }
        public Box2D.Common.b2Mat33 GetInverse22(Box2D.Common.b2Mat33 M) { return default(Box2D.Common.b2Mat33); }
        public Box2D.Common.b2Mat33 GetSymInverse33(Box2D.Common.b2Mat33 M) { return default(Box2D.Common.b2Mat33); }
        public void SetZero() { }
        public Box2D.Common.b2Vec2 Solve22(Box2D.Common.b2Vec2 b) { return default(Box2D.Common.b2Vec2); }
        public Box2D.Common.b2Vec3 Solve33(Box2D.Common.b2Vec3 b) { return default(Box2D.Common.b2Vec3); }
    }
    public partial class b2Math
    {
        public static Box2D.Common.b2Vec2 b2Vec2_zero;
        public b2Math() { }
        public static Box2D.Common.b2Mat22 b2Abs(Box2D.Common.b2Mat22 A) { return default(Box2D.Common.b2Mat22); }
        public static Box2D.Common.b2Vec2 b2Abs(Box2D.Common.b2Vec2 a) { return default(Box2D.Common.b2Vec2); }
        public static float b2Abs(float a) { return default(float); }
        public static float b2Atan2(float y, float x) { return default(float); }
        public static Box2D.Common.b2Vec2 b2Clamp(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 low, Box2D.Common.b2Vec2 high) { return default(Box2D.Common.b2Vec2); }
        public static float b2Clamp(float a, float low, float high) { return default(float); }
        [System.ObsoleteAttribute("Use the ref b2Cross")]
        public static float b2Cross(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(float); }
        public static float b2Cross(ref Box2D.Common.b2Vec2 a, ref Box2D.Common.b2Vec2 b) { return default(float); }
        public static Box2D.Common.b2Vec2 b2Cross(ref Box2D.Common.b2Vec2 a, float s) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec3 b2Cross(Box2D.Common.b2Vec3 a, Box2D.Common.b2Vec3 b) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Vec3 b2Cross(ref Box2D.Common.b2Vec3 a, ref Box2D.Common.b2Vec3 b) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Vec2 b2Cross(float s, ref Box2D.Common.b2Vec2 a) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2Cross(float ax, float ay, float s) { return default(Box2D.Common.b2Vec2); }
        public static float b2Cross(float ax, float ay, float bx, float by) { return default(float); }
        public static float b2Distance(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(float); }
        public static float b2Distance(ref Box2D.Common.b2Vec2 a, ref Box2D.Common.b2Vec2 b) { return default(float); }
        public static float b2DistanceSquared(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(float); }
        public static float b2DistanceSquared(ref Box2D.Common.b2Vec2 a, ref Box2D.Common.b2Vec2 b) { return default(float); }
        [System.ObsoleteAttribute("Use the ref b2Dot instead")]
        public static float b2Dot(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(float); }
        public static float b2Dot(ref Box2D.Common.b2Vec2 a, ref Box2D.Common.b2Vec2 b) { return default(float); }
        public static float b2Dot(Box2D.Common.b2Vec3 a, Box2D.Common.b2Vec3 b) { return default(float); }
        public static float b2Dot(ref Box2D.Common.b2Vec3 a, ref Box2D.Common.b2Vec3 b) { return default(float); }
        public static float b2Dot(float ax, float ay, float bx, float by) { return default(float); }
        public static float b2InvSqrt(float x) { return default(float); }
        public static bool b2IsPowerOfTwo(int x) { return default(bool); }
        public static bool b2IsValid(float x) { return default(bool); }
        public static Box2D.Common.b2Vec2 b2Max(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(Box2D.Common.b2Vec2); }
        public static void b2Max(ref Box2D.Common.b2Vec2 a, ref Box2D.Common.b2Vec2 b, out Box2D.Common.b2Vec2 output) { output = default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2Min(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(Box2D.Common.b2Vec2); }
        public static void b2Min(ref Box2D.Common.b2Vec2 a, ref Box2D.Common.b2Vec2 b, out Box2D.Common.b2Vec2 output) { output = default(Box2D.Common.b2Vec2); }
        public static float b2MixFriction(float friction1, float friction2) { return default(float); }
        public static float b2MixRestitution(float restitution1, float restitution2) { return default(float); }
        public static Box2D.Common.b2Mat22 b2Mul(Box2D.Common.b2Mat22 A, Box2D.Common.b2Mat22 B) { return default(Box2D.Common.b2Mat22); }
        public static Box2D.Common.b2Vec2 b2Mul(Box2D.Common.b2Mat22 A, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2Mul(ref Box2D.Common.b2Mat22 A, ref Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec3 b2Mul(Box2D.Common.b2Mat33 A, Box2D.Common.b2Vec3 v) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Rot b2Mul(Box2D.Common.b2Rot q, Box2D.Common.b2Rot r) { return default(Box2D.Common.b2Rot); }
        public static Box2D.Common.b2Vec2 b2Mul(Box2D.Common.b2Rot q, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2Mul(ref Box2D.Common.b2Rot q, ref Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Transform b2Mul(Box2D.Common.b2Transform A, Box2D.Common.b2Transform B) { return default(Box2D.Common.b2Transform); }
        public static Box2D.Common.b2Vec2 b2Mul(Box2D.Common.b2Transform T, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2Mul(ref Box2D.Common.b2Transform T, ref Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2Mul22(Box2D.Common.b2Mat33 A, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Mat22 b2MulT(Box2D.Common.b2Mat22 A, Box2D.Common.b2Mat22 B) { return default(Box2D.Common.b2Mat22); }
        public static Box2D.Common.b2Vec2 b2MulT(ref Box2D.Common.b2Mat22 A, ref Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Rot b2MulT(Box2D.Common.b2Rot q, Box2D.Common.b2Rot r) { return default(Box2D.Common.b2Rot); }
        public static Box2D.Common.b2Vec2 b2MulT(Box2D.Common.b2Rot q, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2MulT(ref Box2D.Common.b2Rot q, ref Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Transform b2MulT(Box2D.Common.b2Transform A, Box2D.Common.b2Transform B) { return default(Box2D.Common.b2Transform); }
        public static Box2D.Common.b2Vec2 b2MulT(Box2D.Common.b2Transform T, Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 b2MulT(ref Box2D.Common.b2Transform T, ref Box2D.Common.b2Vec2 v) { return default(Box2D.Common.b2Vec2); }
        public static int b2NextPowerOfTwo(int x) { return default(int); }
        public static float b2Sqrt(float x) { return default(float); }
        public static void b2Swap<T>(ref T a, ref T b) { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Rot
    {
        public float c;
        public static Box2D.Common.b2Rot Identity;
        public float s;
        public b2Rot(float angle) { throw new System.NotImplementedException(); }
        public float GetAngle() { return default(float); }
        public Box2D.Common.b2Vec2 GetXAxis() { return default(Box2D.Common.b2Vec2); }
        public Box2D.Common.b2Vec2 GetYAxis() { return default(Box2D.Common.b2Vec2); }
        public void Set(float angle) { }
        public void SetIdentity() { }
    }
    public partial class b2Settings
    {
        public static float b2_aabbExtension;
        public static Box2D.Common.b2Vec2 b2_aabbExtensionVec;
        public static float b2_aabbMultiplier;
        public static float b2_alphaEpsilon;
        public static readonly float b2_angularSleepTolerance;
        public static float b2_angularSlop;
        public static readonly float b2_baumgarte;
        public static float b2_epsilon;
        public static float b2_epsilonSqrd;
        public static readonly float b2_linearSleepTolerance;
        public static float b2_linearSlop;
        public static readonly float b2_maxAngularCorrection;
        public static float b2_maxFloat;
        public static readonly float b2_maxLinearCorrection;
        public static int b2_maxManifoldPoints;
        public static int b2_maxPolygonVertices;
        public static readonly float b2_maxRotation;
        public static readonly float b2_maxRotationSquared;
        public static int b2_maxSubSteps;
        public static readonly int b2_maxTOIContacts;
        public static readonly float b2_maxTranslation;
        public static readonly float b2_maxTranslationSquared;
        public static float b2_pi;
        public static float b2_polygonRadius;
        public static readonly float b2_timeToSleep;
        public static readonly float b2_toiBaugarte;
        public static readonly float b2_velocityThreshold;
        public static Box2D.Common.b2Version b2_version;
        public b2Settings() { }
        public static void b2Log(string s, params System.Object[] p) { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Sweep
    {
        public float a;
        public float a0;
        public float alpha0;
        public Box2D.Common.b2Vec2 c;
        public Box2D.Common.b2Vec2 c0;
        public Box2D.Common.b2Vec2 localCenter;
        public static Box2D.Common.b2Sweep Zero;
        public void Advance(float alpha) { }
        public static Box2D.Common.b2Sweep Create() { return default(Box2D.Common.b2Sweep); }
        public void Defaults() { }
        public void GetTransform(out Box2D.Common.b2Transform xfb, float beta) { xfb = default(Box2D.Common.b2Transform); }
        public void Normalize() { }
    }
    public partial class b2Timer
    {
        public b2Timer() { }
        public float GetMilliseconds() { return default(float); }
        public void Reset() { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Transform
    {
        public static Box2D.Common.b2Transform Identity;
        public Box2D.Common.b2Vec2 p;
        public Box2D.Common.b2Rot q;
        public b2Transform(Box2D.Common.b2Vec2 position, Box2D.Common.b2Rot rotation) { throw new System.NotImplementedException(); }
        public void Set(Box2D.Common.b2Vec2 position, float angle) { }
        public void SetIdentity() { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Vec2
    {
        public float x;
        public float y;
        public static Box2D.Common.b2Vec2 Zero;
        public b2Vec2(float x_, float y_) { throw new System.NotImplementedException(); }
        public float Length { get { return default(float); } }
        public float LengthSquared { get { return default(float); } }
        public bool Equals(Box2D.Common.b2Vec2 o) { return default(bool); }
        public override bool Equals(object obj) { return default(bool); }
        public override int GetHashCode() { return default(int); }
        [System.ObsoleteAttribute("Use the property accessor")]
        public float GetLength() { return default(float); }
        [System.ObsoleteAttribute("Use the property accessor instead")]
        public float GetLengthSquared() { return default(float); }
        public bool IsValid() { return default(bool); }
        public Box2D.Common.b2Vec2 NegUnitCross() { return default(Box2D.Common.b2Vec2); }
        public float Normalize() { return default(float); }
        public static Box2D.Common.b2Vec2 operator +(Box2D.Common.b2Vec2 v1, Box2D.Common.b2Vec2 v2) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 operator /(Box2D.Common.b2Vec2 v1, float a) { return default(Box2D.Common.b2Vec2); }
        public static bool operator ==(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(bool); }
        public static bool operator !=(Box2D.Common.b2Vec2 a, Box2D.Common.b2Vec2 b) { return default(bool); }
        public static Box2D.Common.b2Vec2 operator *(Box2D.Common.b2Vec2 v1, float a) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 operator *(float a, Box2D.Common.b2Vec2 v1) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 operator -(Box2D.Common.b2Vec2 v1, Box2D.Common.b2Vec2 v2) { return default(Box2D.Common.b2Vec2); }
        public static Box2D.Common.b2Vec2 operator -(Box2D.Common.b2Vec2 b) { return default(Box2D.Common.b2Vec2); }
        public void Set(float x_, float y_) { }
        public void SetZero() { }
        public Box2D.Common.b2Vec2 Skew() { return default(Box2D.Common.b2Vec2); }
        public override string ToString() { return default(string); }
        public Box2D.Common.b2Vec2 UnitCross() { return default(Box2D.Common.b2Vec2); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Vec3
    {
        public float x;
        public float y;
        public float z;
        public b2Vec3(float ix, float iy, float iz) { throw new System.NotImplementedException(); }
        public bool IsValid() { return default(bool); }
        public float Length() { return default(float); }
        public float LengthSquared() { return default(float); }
        public float Normalize() { return default(float); }
        public static Box2D.Common.b2Vec3 operator +(Box2D.Common.b2Vec3 v1, Box2D.Common.b2Vec3 v2) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Vec3 operator *(Box2D.Common.b2Vec3 v1, float a) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Vec3 operator *(float a, Box2D.Common.b2Vec3 v1) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Vec3 operator -(Box2D.Common.b2Vec3 v1, Box2D.Common.b2Vec3 v2) { return default(Box2D.Common.b2Vec3); }
        public static Box2D.Common.b2Vec3 operator -(Box2D.Common.b2Vec3 b) { return default(Box2D.Common.b2Vec3); }
        public void Set(float ix, float iy, float iz) { }
        public void SetZero() { }
        public Box2D.Common.b2Vec3 Skew() { return default(Box2D.Common.b2Vec3); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Version
    {
        public int major;
        public int minor;
        public int revision;
        public b2Version(int m, int i, int r) { throw new System.NotImplementedException(); }
    }
}
namespace Box2D.Dynamics
{
    public partial class b2Body : System.IComparable<Box2D.Dynamics.b2Body>
    {
        public float AngularDamping;
        public Box2D.Dynamics.b2BodyFlags BodyFlags;
        public Box2D.Dynamics.b2BodyType BodyType;
        public Box2D.Dynamics.Contacts.b2ContactEdge ContactList;
        public int FixtureCount;
        public Box2D.Dynamics.b2Fixture FixtureList;
        public Box2D.Common.b2Vec2 Force;
        public float GravityScale;
        public float InvertedI;
        public float InvertedMass;
        public int IslandIndex;
        public Box2D.Dynamics.Joints.b2JointEdge JointList;
        public float LinearDamping;
        protected float m_angularVelocity;
        public float m_I;
        public Box2D.Common.b2Vec2 m_linearVelocity;
        public float Mass;
        public Box2D.Dynamics.b2Body Next;
        public Box2D.Dynamics.b2Body Prev;
        public float SleepTime;
        public Box2D.Common.b2Sweep Sweep;
        public float Torque;
        public Box2D.Common.b2Transform Transform;
        public object UserData;
        public Box2D.Dynamics.b2World World;
        public b2Body(Box2D.Dynamics.b2BodyDef bd, Box2D.Dynamics.b2World world) { }
        public float Angle { get { return default(float); } }
        public float AngularVelocity { get { return default(float); } set { } }
        public float Inertia { get { return default(float); } }
        public Box2D.Common.b2Vec2 LinearVelocity { get { return default(Box2D.Common.b2Vec2); } set { } }
        public Box2D.Common.b2Vec2 LocalCenter { get { return default(Box2D.Common.b2Vec2); } }
        public Box2D.Common.b2Vec2 Position { get { return default(Box2D.Common.b2Vec2); } }
        public Box2D.Common.b2Vec2 WorldCenter { get { return default(Box2D.Common.b2Vec2); } }
        public Box2D.Common.b2Transform XF { get { return default(Box2D.Common.b2Transform); } set { } }
        public virtual void Advance(float alpha) { }
        public virtual void ApplyAngularImpulse(float impulse) { }
        public virtual void ApplyForce(Box2D.Common.b2Vec2 force, Box2D.Common.b2Vec2 point) { }
        public virtual void ApplyForceToCenter(Box2D.Common.b2Vec2 force) { }
        public virtual void ApplyLinearImpulse(Box2D.Common.b2Vec2 impulse, Box2D.Common.b2Vec2 point) { }
        public virtual void ApplyTorque(float torque) { }
        public int CompareTo(Box2D.Dynamics.b2Body b2) { return default(int); }
        public virtual Box2D.Dynamics.b2Fixture CreateFixture(Box2D.Collision.Shapes.b2Shape shape, float density) { return default(Box2D.Dynamics.b2Fixture); }
        public virtual Box2D.Dynamics.b2Fixture CreateFixture(Box2D.Dynamics.b2FixtureDef def) { return default(Box2D.Dynamics.b2Fixture); }
        public virtual void DestroyFixture(Box2D.Dynamics.b2Fixture fixture) { }
        public virtual void Dump() { }
        public virtual Box2D.Common.b2Vec2 GetLinearVelocityFromLocalPoint(Box2D.Common.b2Vec2 localPoint) { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLinearVelocityFromWorldPoint(Box2D.Common.b2Vec2 worldPoint) { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLocalPoint(Box2D.Common.b2Vec2 worldPoint) { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLocalVector(Box2D.Common.b2Vec2 worldVector) { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Collision.Shapes.b2MassData GetMassData() { return default(Box2D.Collision.Shapes.b2MassData); }
        public virtual Box2D.Common.b2Vec2 GetWorldPoint(Box2D.Common.b2Vec2 localPoint) { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetWorldVector(Box2D.Common.b2Vec2 localVector) { return default(Box2D.Common.b2Vec2); }
        public virtual bool IsActive() { return default(bool); }
        public virtual bool IsAwake() { return default(bool); }
        public virtual bool IsBullet() { return default(bool); }
        public virtual bool IsFixedRotation() { return default(bool); }
        public virtual bool IsSleepingAllowed() { return default(bool); }
        public virtual void ResetMassData() { }
        public virtual void SetActive(bool flag) { }
        public virtual void SetAwake(bool flag) { }
        public virtual void SetBullet(bool flag) { }
        public virtual void SetFixedRotation(bool flag) { }
        public virtual void SetMassData(Box2D.Collision.Shapes.b2MassData massData) { }
        public virtual void SetSleepingAllowed(bool flag) { }
        public virtual void SetTransform(Box2D.Common.b2Vec2 position, float angle) { }
        public virtual void SetType(Box2D.Dynamics.b2BodyType type) { }
        public virtual bool ShouldCollide(Box2D.Dynamics.b2Body other) { return default(bool); }
        public virtual void SynchronizeFixtures() { }
        public virtual void SynchronizeTransform() { }
    }
    public partial class b2BodyDef
    {
        public bool active;
        public bool allowSleep;
        public float angle;
        public float angularDamping;
        public float angularVelocity;
        public bool awake;
        public bool bullet;
        public bool fixedRotation;
        public float gravityScale;
        public float linearDamping;
        public Box2D.Common.b2Vec2 linearVelocity;
        public Box2D.Common.b2Vec2 position;
        public Box2D.Dynamics.b2BodyType type;
        public object userData;
        public b2BodyDef() { }
        public void Defaults() { }
    }
    [System.FlagsAttribute]
    public enum b2BodyFlags
    {
        e_activeFlag = 32,
        e_autoSleepFlag = 4,
        e_awakeFlag = 2,
        e_bulletFlag = 8,
        e_fixedRotationFlag = 16,
        e_islandFlag = 1,
        e_toiFlag = 64,
    }
    public enum b2BodyType
    {
        b2_dynamicBody = 2,
        b2_kinematicBody = 1,
        b2_staticBody = 0,
    }
    public partial class b2ContactFilter
    {
        public static Box2D.Dynamics.b2ContactFilter b2_defaultFilter;
        public b2ContactFilter() { }
        public virtual bool ShouldCollide(Box2D.Dynamics.b2Fixture fixtureA, Box2D.Dynamics.b2Fixture fixtureB) { return default(bool); }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2ContactImpulse
    {
        public int count;
        public System.Single[] normalImpulses;
        public System.Single[] tangentImpulses;
        public static Box2D.Dynamics.b2ContactImpulse Create() { return default(Box2D.Dynamics.b2ContactImpulse); }
    }
    public abstract partial class b2ContactListener
    {
        public static Box2D.Dynamics.b2ContactListener b2_defaultListener;
        protected b2ContactListener() { }
        public virtual void BeginContact(Box2D.Dynamics.Contacts.b2Contact contact) { }
        public virtual void EndContact(Box2D.Dynamics.Contacts.b2Contact contact) { }
        public abstract void PostSolve(Box2D.Dynamics.Contacts.b2Contact contact, ref Box2D.Dynamics.b2ContactImpulse impulse);
        public abstract void PreSolve(Box2D.Dynamics.Contacts.b2Contact contact, Box2D.Collision.b2Manifold oldManifold);
    }
    public partial class b2ContactManager
    {
        public b2ContactManager() { }
        public Box2D.Collision.b2BroadPhase BroadPhase { get { return default(Box2D.Collision.b2BroadPhase); } set { } }
        public int ContactCount { get { return default(int); } set { } }
        public Box2D.Dynamics.b2ContactFilter ContactFilter { get { return default(Box2D.Dynamics.b2ContactFilter); } set { } }
        public Box2D.Dynamics.Contacts.b2Contact ContactList { get { return default(Box2D.Dynamics.Contacts.b2Contact); } set { } }
        public Box2D.Dynamics.b2ContactListener ContactListener { get { return default(Box2D.Dynamics.b2ContactListener); } set { } }
        public void AddPair(ref Box2D.Dynamics.b2FixtureProxy proxyA, ref Box2D.Dynamics.b2FixtureProxy proxyB) { }
        public void Collide() { }
        public void Destroy(Box2D.Dynamics.Contacts.b2Contact c) { }
        public void FindNewContacts() { }
    }
    public abstract partial class b2DestructionListener
    {
        protected b2DestructionListener() { }
        public abstract void SayGoodbye(Box2D.Dynamics.b2Fixture fixture);
        public abstract void SayGoodbye(Box2D.Dynamics.Joints.b2Joint joint);
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Filter
    {
        public ushort categoryBits;
        public static Box2D.Dynamics.b2Filter Default;
        public int groupIndex;
        public ushort maskBits;
        public b2Filter(ushort cat, ushort mask, int group) { throw new System.NotImplementedException(); }
    }
    public partial class b2Fixture
    {
        public Box2D.Dynamics.b2Body Body;
        public float Density;
        public float Friction;
        public Box2D.Dynamics.b2Fixture Next;
        public float Restitution;
        public Box2D.Collision.Shapes.b2Shape Shape;
        public object UserData;
        public b2Fixture() { }
        public Box2D.Dynamics.b2Filter Filter { get { return default(Box2D.Dynamics.b2Filter); } set { } }
        public bool IsSensor { get { return default(bool); } set { } }
        public Box2D.Dynamics.b2FixtureProxy[] Proxies { get { return default(Box2D.Dynamics.b2FixtureProxy[]); } }
        public int ProxyCount { get { return default(int); } }
        public Box2D.Collision.Shapes.b2ShapeType ShapeType { get { return default(Box2D.Collision.Shapes.b2ShapeType); } }
        public void Create(Box2D.Dynamics.b2Body body, Box2D.Dynamics.b2FixtureDef def) { }
        public virtual void CreateProxies(Box2D.Collision.b2BroadPhase broadPhase, Box2D.Common.b2Transform xf) { }
        public virtual void Destroy() { }
        public virtual void DestroyProxies(Box2D.Collision.b2BroadPhase broadPhase) { }
        public virtual void Dump(int bodyIndex) { }
        public virtual Box2D.Collision.b2AABB GetAABB(int childIndex) { return default(Box2D.Collision.b2AABB); }
        public virtual Box2D.Collision.Shapes.b2MassData GetMassData() { return default(Box2D.Collision.Shapes.b2MassData); }
        public virtual bool RayCast(out Box2D.Collision.b2RayCastOutput output, Box2D.Collision.b2RayCastInput input, int childIndex) { output = default(Box2D.Collision.b2RayCastOutput); return default(bool); }
        public virtual void Refilter() { }
        public virtual void SetFilterData(Box2D.Dynamics.b2Filter filter) { }
        public virtual void Synchronize(Box2D.Collision.b2BroadPhase broadPhase, ref Box2D.Common.b2Transform transform1, ref Box2D.Common.b2Transform transform2) { }
        public virtual bool TestPoint(Box2D.Common.b2Vec2 p) { return default(bool); }
    }
    public partial class b2FixtureDef
    {
        public float density;
        public Box2D.Dynamics.b2Filter filter;
        public float friction;
        public bool isSensor;
        public float restitution;
        public Box2D.Collision.Shapes.b2Shape shape;
        public object userData;
        public b2FixtureDef() { }
        public void Defaults() { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2FixtureProxy
    {
        public Box2D.Collision.b2AABB aabb;
        public int childIndex;
        public Box2D.Dynamics.b2Fixture fixture;
        public int proxyId;
    }
    public partial class b2Island
    {
        public Box2D.Dynamics.b2Body[] m_bodies;
        public int m_bodyCapacity;
        public int m_bodyCount;
        public int m_contactCapacity;
        public int m_contactCount;
        public Box2D.Dynamics.Contacts.b2Contact[] m_contacts;
        public int m_jointCapacity;
        public int m_jointCount;
        public Box2D.Dynamics.Joints.b2Joint[] m_joints;
        public Box2D.Dynamics.b2ContactListener m_listener;
        public b2Island(int bodyCapacity, int contactCapacity, int jointCapacity, Box2D.Dynamics.b2ContactListener listener) { }
        public void Add(Box2D.Dynamics.b2Body body) { }
        public void Add(Box2D.Dynamics.Contacts.b2Contact contact) { }
        public void Add(Box2D.Dynamics.Joints.b2Joint joint) { }
        public void Clear() { }
        public void Report(Box2D.Dynamics.Contacts.b2ContactConstraint[] constraints) { }
        public void Reset(int bodyCapacity, int contactCapacity, int jointCapacity, Box2D.Dynamics.b2ContactListener listener) { }
        public void Solve(Box2D.Dynamics.b2TimeStep step, Box2D.Common.b2Vec2 gravity, bool allowSleep) { }
        public void SolveTOI(ref Box2D.Dynamics.b2TimeStep subStep, int toiIndexA, int toiIndexB) { }
    }
    public abstract partial class b2QueryCallback
    {
        protected b2QueryCallback() { }
        public abstract bool ReportFixture(Box2D.Dynamics.b2Fixture fixture);
    }
    public abstract partial class b2RayCastCallback
    {
        protected b2RayCastCallback() { }
        public abstract float ReportFixture(Box2D.Dynamics.b2Fixture fixture, Box2D.Common.b2Vec2 point, Box2D.Common.b2Vec2 normal, float fraction);
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2SolverData
    {
        public Box2D.Dynamics.b2TimeStep step;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2TimeStep
    {
        public float dt;
        public float dtRatio;
        public float inv_dt;
        public int positionIterations;
        public int velocityIterations;
        public bool warmStarting;
    }
    public partial class b2World
    {
        public b2World(Box2D.Common.b2Vec2 gravity) { }
        public bool AllowSleep { get { return default(bool); } set { } }
        public int BodyCount { get { return default(int); } set { } }
        public Box2D.Dynamics.b2Body BodyList { get { return default(Box2D.Dynamics.b2Body); } set { } }
        public int ContactCount { get { return default(int); } }
        public Box2D.Dynamics.b2ContactManager ContactManager { get { return default(Box2D.Dynamics.b2ContactManager); } set { } }
        public Box2D.Dynamics.b2WorldFlags Flags { get { return default(Box2D.Dynamics.b2WorldFlags); } set { } }
        public Box2D.Common.b2Vec2 Gravity { get { return default(Box2D.Common.b2Vec2); } set { } }
        public bool IsLocked { get { return default(bool); } }
        public int JointCount { get { return default(int); } set { } }
        public Box2D.Dynamics.Joints.b2Joint JointList { get { return default(Box2D.Dynamics.Joints.b2Joint); } set { } }
        public void ClearForces() { }
        public Box2D.Dynamics.b2Body CreateBody(Box2D.Dynamics.b2BodyDef def) { return default(Box2D.Dynamics.b2Body); }
        public Box2D.Dynamics.Joints.b2Joint CreateJoint(Box2D.Dynamics.Joints.b2JointDef def) { return default(Box2D.Dynamics.Joints.b2Joint); }
        public void DestroyBody(Box2D.Dynamics.b2Body b) { }
        public void DestroyJoint(Box2D.Dynamics.Joints.b2Joint j) { }
        public void DrawDebugData() { }
        public void DrawJoint(Box2D.Dynamics.Joints.b2Joint joint) { }
        public void DrawShape(Box2D.Dynamics.b2Fixture fixture, ref Box2D.Common.b2Transform xf, Box2D.Common.b2Color color) { }
        public void Dump() { }
        public bool GetAllowSleeping() { return default(bool); }
        public bool GetContinuousPhysics() { return default(bool); }
        public int GetProxyCount() { return default(int); }
        public bool GetSubStepping() { return default(bool); }
        public int GetTreeBalance() { return default(int); }
        public int GetTreeHeight() { return default(int); }
        public float GetTreeQuality() { return default(float); }
        public bool GetWarmStarting() { return default(bool); }
        public void QueryAABB(Box2D.Dynamics.b2QueryCallback callback, Box2D.Collision.b2AABB aabb) { }
        public void RayCast(Box2D.Dynamics.b2RayCastCallback callback, Box2D.Common.b2Vec2 point1, Box2D.Common.b2Vec2 point2) { }
        public void SetAllowSleeping(bool flag) { }
        public void SetContactFilter(Box2D.Dynamics.b2ContactFilter filter) { }
        public void SetContactListener(Box2D.Dynamics.b2ContactListener listener) { }
        public void SetContinuousPhysics(bool flag) { }
        public void SetDebugDraw(Box2D.Common.b2Draw debugDraw) { }
        public void SetDestructionListener(Box2D.Dynamics.b2DestructionListener listener) { }
        public void SetSubStepping(bool flag) { }
        public void SetWarmStarting(bool flag) { }
        public void Solve(Box2D.Dynamics.b2TimeStep step) { }
        public void SolveTOI(Box2D.Dynamics.b2TimeStep step) { }
        public void Step(float dt, int velocityIterations, int positionIterations) { }
    }
    [System.FlagsAttribute]
    public enum b2WorldFlags : short
    {
        e_clearForces = (short)4,
        e_locked = (short)2,
        e_newFixture = (short)1,
    }
    public partial class b2WorldQueryWrapper : Box2D.Dynamics.Ib2QueryCallback
    {
        public b2WorldQueryWrapper() { }
        public Box2D.Collision.b2BroadPhase BroadPhase { get { return default(Box2D.Collision.b2BroadPhase); } set { } }
        public Box2D.Dynamics.b2QueryCallback Callback { get { return default(Box2D.Dynamics.b2QueryCallback); } set { } }
        public bool QueryCallback(int proxyId) { return default(bool); }
    }
    public partial class b2WorldRayCastWrapper : Box2D.Dynamics.Ib2RayCastCallback
    {
        public b2WorldRayCastWrapper() { }
        public Box2D.Collision.b2BroadPhase BroadPhase { get { return default(Box2D.Collision.b2BroadPhase); } set { } }
        public Box2D.Dynamics.b2RayCastCallback Callback { get { return default(Box2D.Dynamics.b2RayCastCallback); } set { } }
        public float RayCastCallback(ref Box2D.Collision.b2RayCastInput input, int proxyId) { return default(float); }
    }
    public partial interface Ib2QueryCallback
    {
        bool QueryCallback(int proxyId);
    }
    public partial interface Ib2RayCastCallback
    {
        float RayCastCallback(ref Box2D.Collision.b2RayCastInput input, int proxyId);
    }
}
namespace Box2D.Dynamics.Contacts
{
    public partial class b2ConstraintPoint
    {
        public Box2D.Common.b2Vec2 localPoint;
        public float normalImpulse;
        public float normalMass;
        public Box2D.Common.b2Vec2 rA;
        public Box2D.Common.b2Vec2 rB;
        public float tangentImpulse;
        public float tangentMass;
        public float velocityBias;
        public b2ConstraintPoint() { }
    }
    public partial class b2Contact : Box2D.b2ReusedObject<Box2D.Dynamics.Contacts.b2Contact>
    {
        public Box2D.Dynamics.b2Fixture FixtureA;
        public Box2D.Dynamics.b2Fixture FixtureB;
        public Box2D.Dynamics.Contacts.b2ContactFlags Flags;
        public float Friction;
        public float m_toi;
        public int m_toiCount;
        public Box2D.Dynamics.Contacts.b2Contact Next;
        public Box2D.Dynamics.Contacts.b2ContactEdge NodeA;
        public Box2D.Dynamics.Contacts.b2ContactEdge NodeB;
        public Box2D.Dynamics.Contacts.b2Contact Prev;
        public float Restitution;
        protected static Box2D.Dynamics.Contacts.b2ContactRegister[,] s_registers;
        public b2Contact() { }
        public b2Contact(Box2D.Dynamics.b2Fixture fA, int indexA, Box2D.Dynamics.b2Fixture fB, int indexB) { }
        protected virtual float b2MixFriction(float friction1, float friction2) { return default(float); }
        protected float b2MixRestitution(float restitution1, float restitution2) { return default(float); }
        public static Box2D.Dynamics.Contacts.b2Contact Create(Box2D.Dynamics.b2Fixture fixtureA, int indexA, Box2D.Dynamics.b2Fixture fixtureB, int indexB) { return default(Box2D.Dynamics.Contacts.b2Contact); }
        public void Evaluate(Box2D.Collision.b2Manifold manifold, ref Box2D.Common.b2Transform xfA, ref Box2D.Common.b2Transform xfB) { }
        public virtual void FlagForFiltering() { }
        public virtual int GetChildIndexA() { return default(int); }
        public virtual int GetChildIndexB() { return default(int); }
        public virtual Box2D.Dynamics.b2Fixture GetFixtureA() { return default(Box2D.Dynamics.b2Fixture); }
        public virtual Box2D.Dynamics.b2Fixture GetFixtureB() { return default(Box2D.Dynamics.b2Fixture); }
        public virtual float GetFriction() { return default(float); }
        public virtual Box2D.Collision.b2Manifold GetManifold() { return default(Box2D.Collision.b2Manifold); }
        public virtual Box2D.Dynamics.Contacts.b2Contact GetNext() { return default(Box2D.Dynamics.Contacts.b2Contact); }
        public virtual Box2D.Dynamics.Contacts.b2Contact GetPrev() { return default(Box2D.Dynamics.Contacts.b2Contact); }
        public virtual float GetRestitution() { return default(float); }
        public virtual void GetWorldManifold(ref Box2D.Collision.b2WorldManifold worldManifold) { }
        protected void Init(Box2D.Dynamics.b2Fixture fA, int indexA, Box2D.Dynamics.b2Fixture fB, int indexB) { }
        public virtual bool IsEnabled() { return default(bool); }
        public virtual bool IsTouching() { return default(bool); }
        public virtual void ResetFriction() { }
        public virtual void ResetRestitution() { }
        public virtual void SetEnabled(bool flag) { }
        public virtual void SetFriction(float friction) { }
        public virtual void SetRestitution(float restitution) { }
        public virtual void Update(Box2D.Dynamics.b2ContactListener listener) { }
    }
    public partial class b2ContactConstraint
    {
        public float a;
        public Box2D.Dynamics.b2Body BodyA;
        public Box2D.Dynamics.b2Body BodyB;
        public Box2D.Common.b2Vec2 c;
        public Box2D.Dynamics.Contacts.b2Contact contact;
        public float friction;
        public float invIA;
        public float invIB;
        public float invMassA;
        public float invMassB;
        public Box2D.Common.b2Mat22 K;
        public Box2D.Common.b2Vec2 localCenterA;
        public Box2D.Common.b2Vec2 localCenterB;
        public Box2D.Common.b2Vec2 localNormal;
        public Box2D.Common.b2Vec2 localPoint;
        public Box2D.Common.b2Vec2 normal;
        public Box2D.Common.b2Mat22 normalMass;
        public int pointCount;
        public Box2D.Dynamics.Contacts.b2ConstraintPoint[] points;
        public float radiusA;
        public float radiusB;
        public float restitution;
        public Box2D.Collision.b2ManifoldType type;
        public Box2D.Common.b2Vec2 v;
        public float w;
        public b2ContactConstraint() { }
    }
    public partial class b2ContactEdge
    {
        public Box2D.Dynamics.Contacts.b2Contact Contact;
        public bool hasNext;
        public bool hasPrev;
        public Box2D.Dynamics.Contacts.b2ContactEdge Next;
        public Box2D.Dynamics.b2Body Other;
        public Box2D.Dynamics.Contacts.b2ContactEdge Prev;
        public b2ContactEdge() { }
    }
    [System.FlagsAttribute]
    public enum b2ContactFlags : uint
    {
        e_bulletHitFlag = (uint)16,
        e_enabledFlag = (uint)4,
        e_filterFlag = (uint)8,
        e_islandFlag = (uint)1,
        e_toiFlag = (uint)32,
        e_touchingFlag = (uint)2,
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2ContactRegister
    {
        public Box2D.Dynamics.Contacts.ContactType contactType;
        public bool isPrimary;
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2ContactSolverDef
    {
        public Box2D.Dynamics.Contacts.b2Contact[] contacts;
        public int count;
        public Box2D.Dynamics.b2TimeStep step;
    }
    public enum ContactType
    {
        b2ChainAndCircleContact = 5,
        b2ChainAndPolygonContact = 6,
        b2CircleContact = 0,
        b2EdgeAndCircleContact = 3,
        b2EdgeAndPolygonContact = 4,
        b2PolygonAndCircleContact = 1,
        b2PolygonContact = 2,
    }
}
namespace Box2D.Dynamics.Joints
{
    public partial class b2DistanceJoint : Box2D.Dynamics.Joints.b2Joint
    {
        public b2DistanceJoint(Box2D.Dynamics.Joints.b2DistanceJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2DistanceJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public float dampingRatio;
        public float frequencyHz;
        public float length;
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public b2DistanceJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body b1, Box2D.Dynamics.b2Body b2, Box2D.Common.b2Vec2 anchor1, Box2D.Common.b2Vec2 anchor2) { }
    }
    public partial class b2FrictionJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected float m_angularImpulse;
        protected float m_angularMass;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected Box2D.Common.b2Vec2 m_linearImpulse;
        protected Box2D.Common.b2Mat22 m_linearMass;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected float m_maxForce;
        protected float m_maxTorque;
        protected Box2D.Common.b2Vec2 m_rA;
        protected Box2D.Common.b2Vec2 m_rB;
        public b2FrictionJoint(Box2D.Dynamics.Joints.b2FrictionJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetMaxForce() { return default(float); }
        public virtual float GetMaxTorque() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public virtual void SetMaxForce(float force) { }
        public virtual void SetMaxTorque(float torque) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2FrictionJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public float maxForce;
        public float maxTorque;
        public b2FrictionJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body bA, Box2D.Dynamics.b2Body bB, Box2D.Common.b2Vec2 anchor) { }
    }
    public partial class b2GearJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected Box2D.Dynamics.b2Body m_bodyC;
        protected Box2D.Dynamics.b2Body m_bodyD;
        protected float m_constant;
        protected float m_iA;
        protected float m_iB;
        protected float m_iC;
        protected float m_iD;
        protected float m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected int m_indexC;
        protected int m_indexD;
        protected Box2D.Dynamics.Joints.b2Joint m_joint1;
        protected Box2D.Dynamics.Joints.b2Joint m_joint2;
        protected Box2D.Common.b2Vec2 m_JvAC;
        protected Box2D.Common.b2Vec2 m_JvBD;
        protected float m_JwA;
        protected float m_JwB;
        protected float m_JwC;
        protected float m_JwD;
        protected Box2D.Common.b2Vec2 m_lcA;
        protected Box2D.Common.b2Vec2 m_lcB;
        protected Box2D.Common.b2Vec2 m_lcC;
        protected Box2D.Common.b2Vec2 m_lcD;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localAnchorC;
        protected Box2D.Common.b2Vec2 m_localAnchorD;
        protected Box2D.Common.b2Vec2 m_localAxisC;
        protected Box2D.Common.b2Vec2 m_localAxisD;
        protected float m_mA;
        protected float m_mass;
        protected float m_mB;
        protected float m_mC;
        protected float m_mD;
        protected float m_ratio;
        protected float m_referenceAngleA;
        protected float m_referenceAngleB;
        protected Box2D.Dynamics.Joints.b2JointType m_typeA;
        protected Box2D.Dynamics.Joints.b2JointType m_typeB;
        public b2GearJoint(Box2D.Dynamics.Joints.b2GearJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Dynamics.Joints.b2Joint GetJoint1() { return default(Box2D.Dynamics.Joints.b2Joint); }
        public virtual Box2D.Dynamics.Joints.b2Joint GetJoint2() { return default(Box2D.Dynamics.Joints.b2Joint); }
        public virtual float GetRatio() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public virtual void SetRatio(float ratio) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2GearJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public Box2D.Dynamics.Joints.b2Joint joint1;
        public Box2D.Dynamics.Joints.b2Joint joint2;
        public float ratio;
        public b2GearJointDef() { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2Jacobian
    {
        public float AngularA;
        public float AngularB;
        public Box2D.Common.b2Vec2 Linear;
    }
    public partial class b2Joint
    {
        public Box2D.Dynamics.b2Body m_bodyA;
        public Box2D.Dynamics.b2Body m_bodyB;
        protected bool m_collideConnected;
        public Box2D.Dynamics.Joints.b2JointEdge m_edgeA;
        public Box2D.Dynamics.Joints.b2JointEdge m_edgeB;
        protected int m_index;
        public bool m_islandFlag;
        protected Box2D.Dynamics.Joints.b2JointType m_type;
        protected object m_userData;
        public Box2D.Dynamics.Joints.b2Joint Next;
        public Box2D.Dynamics.Joints.b2Joint Prev;
        public b2Joint(Box2D.Dynamics.Joints.b2JointDef def) { }
        public int Index { get { return default(int); } set { } }
        public static Box2D.Dynamics.Joints.b2Joint Create(Box2D.Dynamics.Joints.b2JointDef def) { return default(Box2D.Dynamics.Joints.b2Joint); }
        public virtual void Dump() { }
        public virtual Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Dynamics.b2Body GetBodyA() { return default(Box2D.Dynamics.b2Body); }
        public virtual Box2D.Dynamics.b2Body GetBodyB() { return default(Box2D.Dynamics.b2Body); }
        public virtual bool GetCollideConnected() { return default(bool); }
        public virtual Box2D.Dynamics.Joints.b2JointType GetJointType() { return default(Box2D.Dynamics.Joints.b2JointType); }
        public virtual Box2D.Dynamics.Joints.b2Joint GetNext() { return default(Box2D.Dynamics.Joints.b2Joint); }
        public virtual object GetUserData() { return default(object); }
        public virtual void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public bool IsActive() { return default(bool); }
        public virtual void SetUserData(object data) { }
        public virtual bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public virtual void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2JointDef
    {
        public Box2D.Dynamics.b2Body BodyA;
        public Box2D.Dynamics.b2Body BodyB;
        public bool CollideConnected;
        public Box2D.Dynamics.Joints.b2JointType JointType;
        public object UserData;
        public b2JointDef() { }
    }
    public partial class b2JointEdge
    {
        public Box2D.Dynamics.Joints.b2Joint Joint;
        public Box2D.Dynamics.Joints.b2JointEdge Next;
        public Box2D.Dynamics.b2Body Other;
        public Box2D.Dynamics.Joints.b2JointEdge Prev;
        public b2JointEdge() { }
    }
    [System.FlagsAttribute]
    public enum b2JointType : short
    {
        e_distanceJoint = (short)16,
        e_frictionJoint = (short)1024,
        e_gearJoint = (short)128,
        e_mouseJoint = (short)64,
        e_prismaticJoint = (short)8,
        e_pulleyJoint = (short)32,
        e_revoluteJoint = (short)4,
        e_ropeJoint = (short)2048,
        e_unknownJoint = (short)1,
        e_weldJoint = (short)512,
        e_wheelJoint = (short)256,
    }
    [System.FlagsAttribute]
    public enum b2LimitState : short
    {
        e_atLowerLimit = (short)2,
        e_atUpperLimit = (short)4,
        e_equalLimits = (short)8,
        e_inactiveLimit = (short)1,
    }
    public partial class b2MouseJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected float m_beta;
        protected Box2D.Common.b2Vec2 m_C;
        protected float m_dampingRatio;
        protected float m_frequencyHz;
        protected float m_gamma;
        protected Box2D.Common.b2Vec2 m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIB;
        protected float m_invMassB;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected Box2D.Common.b2Mat22 m_mass;
        protected float m_maxForce;
        protected Box2D.Common.b2Vec2 m_rB;
        protected Box2D.Common.b2Vec2 m_targetA;
        public b2MouseJoint(Box2D.Dynamics.Joints.b2MouseJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetDampingRatio() { return default(float); }
        public virtual float GetFrequency() { return default(float); }
        public virtual float GetMaxForce() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetTarget() { return default(Box2D.Common.b2Vec2); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public virtual void SetDampingRatio(float ratio) { }
        public virtual void SetFrequency(float hz) { }
        public virtual void SetMaxForce(float force) { }
        public virtual void SetTarget(Box2D.Common.b2Vec2 target) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2MouseJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public float dampingRatio;
        public float frequencyHz;
        public float maxForce;
        public Box2D.Common.b2Vec2 target;
        public b2MouseJointDef() { }
    }
    public partial class b2PrismaticJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected float InvertedIA;
        protected float InvertedIB;
        protected float InvertedMassA;
        protected float InvertedMassB;
        protected float m_a1;
        protected float m_a2;
        protected Box2D.Common.b2Vec2 m_axis;
        protected bool m_enableLimit;
        protected bool m_enableMotor;
        protected Box2D.Common.b2Vec3 m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected Box2D.Common.b2Mat33 m_K;
        protected Box2D.Dynamics.Joints.b2LimitState m_limitState;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected Box2D.Common.b2Vec2 m_localXAxisA;
        protected Box2D.Common.b2Vec2 m_localYAxisA;
        protected float m_lowerTranslation;
        protected float m_maxMotorForce;
        protected float m_motorImpulse;
        protected float m_motorMass;
        protected float m_motorSpeed;
        protected Box2D.Common.b2Vec2 m_perp;
        protected float m_referenceAngle;
        protected float m_s1;
        protected float m_s2;
        protected float m_upperTranslation;
        public b2PrismaticJoint(Box2D.Dynamics.Joints.b2PrismaticJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public virtual void EnableLimit(bool flag) { }
        public virtual void EnableMotor(bool flag) { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetJointSpeed() { return default(float); }
        public virtual float GetJointTranslation() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetLocalAnchorA() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLocalAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLocalAxisA() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLocalXAxisA() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetLowerLimit() { return default(float); }
        public virtual float GetMaxMotorForce() { return default(float); }
        public virtual float GetMotorForce(float inv_dt) { return default(float); }
        public virtual float GetMotorSpeed() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public virtual float GetReferenceAngle() { return default(float); }
        public virtual float GetUpperLimit() { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public virtual bool IsLimitEnabled() { return default(bool); }
        public virtual bool IsMotorEnabled() { return default(bool); }
        public virtual void SetLimits(float lower, float upper) { }
        public virtual void SetMaxMotorForce(float force) { }
        public virtual void SetMotorSpeed(float speed) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2PrismaticJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public bool enableLimit;
        public bool enableMotor;
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public Box2D.Common.b2Vec2 localAxisA;
        public float lowerTranslation;
        public float maxMotorForce;
        public float motorSpeed;
        public float referenceAngle;
        public float upperTranslation;
        public b2PrismaticJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body bA, Box2D.Dynamics.b2Body bB, Box2D.Common.b2Vec2 anchor, Box2D.Common.b2Vec2 axis) { }
    }
    public partial class b2PulleyJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected float m_constant;
        protected Box2D.Common.b2Vec2 m_groundAnchorA;
        protected Box2D.Common.b2Vec2 m_groundAnchorB;
        protected float m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected float m_lengthA;
        protected float m_lengthB;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected float m_mass;
        protected Box2D.Common.b2Vec2 m_rA;
        protected float m_ratio;
        protected Box2D.Common.b2Vec2 m_rB;
        protected Box2D.Common.b2Vec2 m_uA;
        protected Box2D.Common.b2Vec2 m_uB;
        public b2PulleyJoint(Box2D.Dynamics.Joints.b2PulleyJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetGroundAnchorA() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetGroundAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetLengthA() { return default(float); }
        public virtual float GetLengthB() { return default(float); }
        public virtual float GetRatio() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2PulleyJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public Box2D.Common.b2Vec2 groundAnchorA;
        public Box2D.Common.b2Vec2 groundAnchorB;
        public float lengthA;
        public float lengthB;
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public float ratio;
        public b2PulleyJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body bA, Box2D.Dynamics.b2Body bB, Box2D.Common.b2Vec2 groundA, Box2D.Common.b2Vec2 groundB, Box2D.Common.b2Vec2 anchorA, Box2D.Common.b2Vec2 anchorB, float r) { }
    }
    public partial class b2RevoluteJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected bool m_enableLimit;
        protected bool m_enableMotor;
        protected Box2D.Common.b2Vec3 m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected Box2D.Dynamics.Joints.b2LimitState m_limitState;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected float m_lowerAngle;
        protected Box2D.Common.b2Mat33 m_mass;
        protected float m_maxMotorTorque;
        protected float m_motorImpulse;
        protected float m_motorMass;
        protected float m_motorSpeed;
        protected Box2D.Common.b2Vec2 m_rA;
        protected Box2D.Common.b2Vec2 m_rB;
        protected float m_referenceAngle;
        protected float m_upperAngle;
        public b2RevoluteJoint(Box2D.Dynamics.Joints.b2RevoluteJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public virtual void EnableLimit(bool flag) { }
        public virtual void EnableMotor(bool flag) { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetJointAngle() { return default(float); }
        public virtual float GetJointSpeed() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetLocalAnchorA() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetLocalAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetLowerLimit() { return default(float); }
        public virtual float GetMaxMotorTorque() { return default(float); }
        public virtual float GetMotorSpeed() { return default(float); }
        public virtual float GetMotorTorque(float inv_dt) { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public virtual float GetReferenceAngle() { return default(float); }
        public virtual float GetUpperLimit() { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public virtual bool IsLimitEnabled() { return default(bool); }
        public virtual bool IsMotorEnabled() { return default(bool); }
        public virtual void SetLimits(float lower, float upper) { }
        public virtual void SetMaxMotorTorque(float torque) { }
        public virtual void SetMotorSpeed(float speed) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2RevoluteJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public bool enableLimit;
        public bool enableMotor;
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public float lowerAngle;
        public float maxMotorTorque;
        public float motorSpeed;
        public float referenceAngle;
        public float upperAngle;
        public b2RevoluteJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body bA, Box2D.Dynamics.b2Body bB, Box2D.Common.b2Vec2 anchor) { }
    }
    public partial class b2RopeJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected float m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected float m_length;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected float m_mass;
        protected float m_maxLength;
        protected Box2D.Common.b2Vec2 m_rA;
        protected Box2D.Common.b2Vec2 m_rB;
        protected Box2D.Dynamics.Joints.b2LimitState m_state;
        protected Box2D.Common.b2Vec2 m_u;
        public b2RopeJoint(Box2D.Dynamics.Joints.b2RopeJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Dynamics.Joints.b2LimitState GetLimitState() { return default(Box2D.Dynamics.Joints.b2LimitState); }
        public Box2D.Common.b2Vec2 GetLocalAnchorA() { return default(Box2D.Common.b2Vec2); }
        public Box2D.Common.b2Vec2 GetLocalAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetMaxLength() { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public void SetMaxLength(float length) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2RopeJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public float maxLength;
        public b2RopeJointDef() { }
    }
    public partial class b2WeldJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected float m_bias;
        protected float m_dampingRatio;
        protected float m_frequencyHz;
        protected float m_gamma;
        protected Box2D.Common.b2Vec3 m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected Box2D.Common.b2Mat33 m_mass;
        protected Box2D.Common.b2Vec2 m_rA;
        protected Box2D.Common.b2Vec2 m_rB;
        protected float m_referenceAngle;
        public b2WeldJoint(Box2D.Dynamics.Joints.b2WeldJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public float DampingRatio { get { return default(float); } set { } }
        public virtual float Frequency { get { return default(float); } set { } }
        public virtual Box2D.Common.b2Vec2 LocalAnchorA { get { return default(Box2D.Common.b2Vec2); } set { } }
        public virtual Box2D.Common.b2Vec2 LocalAnchorB { get { return default(Box2D.Common.b2Vec2); } set { } }
        public virtual float ReferenceAngle { get { return default(float); } set { } }
        public override void Dump() { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2WeldJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public float dampingRatio;
        public float frequencyHz;
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public float referenceAngle;
        public b2WeldJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body bA, Box2D.Dynamics.b2Body bB, Box2D.Common.b2Vec2 anchor) { }
    }
    public partial class b2WheelJoint : Box2D.Dynamics.Joints.b2Joint
    {
        protected Box2D.Common.b2Vec2 m_ax;
        protected Box2D.Common.b2Vec2 m_ay;
        protected float m_bias;
        protected float m_dampingRatio;
        protected bool m_enableMotor;
        protected float m_frequencyHz;
        protected float m_gamma;
        protected float m_impulse;
        protected int m_indexA;
        protected int m_indexB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected Box2D.Common.b2Vec2 m_localAnchorA;
        protected Box2D.Common.b2Vec2 m_localAnchorB;
        protected Box2D.Common.b2Vec2 m_localCenterA;
        protected Box2D.Common.b2Vec2 m_localCenterB;
        protected Box2D.Common.b2Vec2 m_localXAxisA;
        protected Box2D.Common.b2Vec2 m_localYAxisA;
        protected float m_mass;
        protected float m_maxMotorTorque;
        protected float m_motorImpulse;
        protected float m_motorMass;
        protected float m_motorSpeed;
        protected float m_sAx;
        protected float m_sAy;
        protected float m_sBx;
        protected float m_sBy;
        protected float m_springImpulse;
        protected float m_springMass;
        public b2WheelJoint(Box2D.Dynamics.Joints.b2WheelJointDef def) : base(default(Box2D.Dynamics.Joints.b2JointDef)) { }
        public override void Dump() { }
        public virtual void EnableMotor(bool flag) { }
        public override Box2D.Common.b2Vec2 GetAnchorA() { return default(Box2D.Common.b2Vec2); }
        public override Box2D.Common.b2Vec2 GetAnchorB() { return default(Box2D.Common.b2Vec2); }
        public virtual float GetJointSpeed() { return default(float); }
        public virtual float GetJointTranslation() { return default(float); }
        public virtual float GetMaxMotorTorque() { return default(float); }
        public virtual float GetMotorSpeed() { return default(float); }
        public virtual float GetMotorTorque(float inv_dt) { return default(float); }
        public virtual Box2D.Common.b2Vec2 GetReactionForce(float inv_dt) { return default(Box2D.Common.b2Vec2); }
        public virtual float GetReactionTorque(float inv_dt) { return default(float); }
        public virtual float GetSpringDampingRatio() { return default(float); }
        public virtual float GetSpringFrequencyHz() { return default(float); }
        public override void InitVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
        public virtual bool IsMotorEnabled() { return default(bool); }
        public virtual void SetMaxMotorTorque(float torque) { }
        public virtual void SetMotorSpeed(float speed) { }
        public virtual void SetSpringDampingRatio(float ratio) { }
        public virtual void SetSpringFrequencyHz(float hz) { }
        public override bool SolvePositionConstraints(Box2D.Dynamics.b2SolverData data) { return default(bool); }
        public override void SolveVelocityConstraints(Box2D.Dynamics.b2SolverData data) { }
    }
    public partial class b2WheelJointDef : Box2D.Dynamics.Joints.b2JointDef
    {
        public float dampingRatio;
        public bool enableMotor;
        public float frequencyHz;
        public Box2D.Common.b2Vec2 localAnchorA;
        public Box2D.Common.b2Vec2 localAnchorB;
        public Box2D.Common.b2Vec2 localAxisA;
        public float maxMotorTorque;
        public float motorSpeed;
        public b2WheelJointDef() { }
        public void Initialize(Box2D.Dynamics.b2Body bA, Box2D.Dynamics.b2Body bB, Box2D.Common.b2Vec2 anchor, Box2D.Common.b2Vec2 axis) { }
    }
}
namespace Box2D.Rope
{
    public partial class b2Rope
    {
        public b2Rope() { }
        public virtual void Draw(Box2D.Common.b2Draw draw) { }
        public void Initialize(Box2D.Rope.b2RopeDef def) { }
        public void SetAngle(float angle) { }
        public void Step(float h, int iterations) { }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct b2RopeDef
    {
        public int count;
        public float damping;
        public Box2D.Common.b2Vec2 gravity;
        public float k2;
        public float k3;
        public System.Single[] masses;
        public Box2D.Common.b2Vec2[] vertices;
    }
}
