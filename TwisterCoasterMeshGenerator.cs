using UnityEngine;

public class TwisterCoasterMeshGenerator : MeshGenerator
{
    private const float sideTubesRadius = 0.02666f;

    private const int sideTubesVertCount = 6;

    private const float centerTubeRadius = 0.08032f;

    private const int centerTubeVertCount = 8;

    private const float buildVolumeHeight = 0.8f;
    
    private TubeExtruder centerTubeExtruder;

    private TubeExtruder leftTubeExtruder;

    private TubeExtruder rightTubeExtruder;

    private BoxExtruder collisionMeshExtruder;

    protected override void Initialize()
    {
        base.Initialize();
        trackWidth = 0.45234f;
        
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = material;
        centerTubeExtruder = new TubeExtruder(centerTubeRadius, centerTubeVertCount);
        centerTubeExtruder.setUV(14, 14);
        leftTubeExtruder = new TubeExtruder(sideTubesRadius, sideTubesVertCount);
        leftTubeExtruder.setUV(14, 15);
        rightTubeExtruder = new TubeExtruder(sideTubesRadius, sideTubesVertCount);
        rightTubeExtruder.setUV(14, 15);
        collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
        buildVolumeMeshExtruder = new BoxExtruder(trackWidth, 0.8f);
        buildVolumeMeshExtruder.closeEnds = true;
    }

    public override void sampleAt(TrackSegment4 trackSegment, float t)
    {
        base.sampleAt(trackSegment, t);
        Vector3 normal = trackSegment.getNormal(t);
        Vector3 trackPivot = getTrackPivot(trackSegment.getPoint(t), normal);
        Vector3 tangentPoint = trackSegment.getTangentPoint(t);
        Vector3 normalized = Vector3.Cross(normal, tangentPoint).normalized;
        Vector3 middlePoint = trackPivot + normalized * trackWidth / 2f;
        Vector3 middlePoint2 = trackPivot - normalized * trackWidth / 2f;
        Vector3 vector = trackPivot + normal * getCenterPointOffsetY();
        centerTubeExtruder.extrude(vector, tangentPoint, normal);
        leftTubeExtruder.extrude(middlePoint, tangentPoint, normal);
        rightTubeExtruder.extrude(middlePoint2, tangentPoint, normal);
        collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (liftExtruder != null)
        {
            liftExtruder.extrude(vector - normal * (0.06713f + chainLiftHeight / 2f), tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return MeshCombiner.start().add(centerTubeExtruder, leftTubeExtruder, rightTubeExtruder).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return buildVolumeMeshExtruder;
    }

    public override float getCenterPointOffsetY()
    {
        return 0.27f;
    }

    public override float trackOffsetY()
    {
        return 0.35f;
    }

    public override float getTunnelOffsetY()
    {
        return 0.4f;
    }

    public override float getTunnelWidth()
    {
        return 0.6f;
    }

    public override float getTunnelHeight()
    {
        return 1.7f;
    }
    public override float getFrictionWheelOffsetY()
    {
        return 0.15f;
    }
    protected override float railHalfHeight()
    {
        return 0.02666f;
    }
}