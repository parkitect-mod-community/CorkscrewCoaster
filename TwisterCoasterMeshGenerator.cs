using System;
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
        base.trackWidth = 0.45234f;
    }

    public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.prepare(trackSegment, putMeshOnGO);
        putMeshOnGO.GetComponent<Renderer>().sharedMaterial = this.material;
        this.centerTubeExtruder = new TubeExtruder(centerTubeRadius, centerTubeVertCount);
        this.centerTubeExtruder.setUV(15, 15);
        this.leftTubeExtruder = new TubeExtruder(sideTubesRadius, sideTubesVertCount);
        this.leftTubeExtruder.setUV(15, 15);
        this.rightTubeExtruder = new TubeExtruder(sideTubesRadius, sideTubesVertCount);
        this.rightTubeExtruder.setUV(15, 15);
        this.collisionMeshExtruder = new BoxExtruder(base.trackWidth, 0.02666f);
        this.buildVolumeMeshExtruder = new BoxExtruder(base.trackWidth, 0.8f);
        this.buildVolumeMeshExtruder.closeEnds = true;
    }

    public override void sampleAt(TrackSegment4 trackSegment, float t)
    {
        base.sampleAt(trackSegment, t);
        Vector3 normal = trackSegment.getNormal(t);
        Vector3 trackPivot = base.getTrackPivot(trackSegment.getPoint(t), normal);
        Vector3 tangentPoint = trackSegment.getTangentPoint(t);
        Vector3 normalized = Vector3.Cross(normal, tangentPoint).normalized;
        Vector3 middlePoint = trackPivot + normalized * base.trackWidth / 2f;
        Vector3 middlePoint2 = trackPivot - normalized * base.trackWidth / 2f;
        Vector3 vector = trackPivot + normal * this.getCenterPointOffsetY();
        this.centerTubeExtruder.extrude(vector, tangentPoint, normal);
        this.leftTubeExtruder.extrude(middlePoint, tangentPoint, normal);
        this.rightTubeExtruder.extrude(middlePoint2, tangentPoint, normal);
        this.collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
        if (this.liftExtruder != null)
        {
            this.liftExtruder.extrude(vector - normal * (0.06613f + this.chainLiftHeight / 2f), tangentPoint, normal);
        }
    }

    public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
    {
        base.afterExtrusion(trackSegment, putMeshOnGO);
    }

    public override Mesh getMesh(GameObject putMeshOnGO)
    {
        return default(MeshCombiner).start().add(new Extruder[]
        {
            this.centerTubeExtruder,
            this.leftTubeExtruder,
            this.rightTubeExtruder
        }).end(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Mesh getCollisionMesh(GameObject putMeshOnGO)
    {
        return this.collisionMeshExtruder.getMesh(putMeshOnGO.transform.worldToLocalMatrix);
    }

    public override Extruder getBuildVolumeMeshExtruder()
    {
        return this.buildVolumeMeshExtruder;
    }

    public override float getCenterPointOffsetY()
    {
        return 0.27f;
    }

    public override float trackOffsetY()
    {
        return 0.35f;
    }

    protected override float getTunnelOffsetY()
    {
        return 0.2f;
    }

    public override float getTunnelWidth()
    {
        return 0.7f;
    }

    public override float getTunnelHeight()
    {
        return 1f;
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
