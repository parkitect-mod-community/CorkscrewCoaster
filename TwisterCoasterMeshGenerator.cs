/**
* Copyright 2019 Michael Pollind
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using UnityEngine;

namespace CorkscrewCoaster
{
    public class TwisterCoasterMeshGenerator : MeshGenerator
    {
        private const float SideTubesRadius = 0.02666f;

        private const int SideTubesVertCount = 6;

        private const float CenterTubeRadius = 0.08032f;

        private const int CenterTubeVertCount = 8;

        private const float BuildVolumeHeight = 0.8f;

        private TubeExtruder centerTubeExtruder;

        private BoxExtruder collisionMeshExtruder;

        private TubeExtruder leftTubeExtruder;

        private TubeExtruder rightTubeExtruder;

        protected override void Initialize()
        {
            base.Initialize();
            trackWidth = 0.45234f;
        }

        public override void prepare(TrackSegment4 trackSegment, GameObject putMeshOnGo)
        {
            base.prepare(trackSegment, putMeshOnGo);
            putMeshOnGo.GetComponent<Renderer>().sharedMaterial = material;
            centerTubeExtruder = new TubeExtruder(CenterTubeRadius, CenterTubeVertCount);
            centerTubeExtruder.setUV(14, 14);
            leftTubeExtruder = new TubeExtruder(SideTubesRadius, SideTubesVertCount);
            leftTubeExtruder.setUV(14, 15);
            rightTubeExtruder = new TubeExtruder(SideTubesRadius, SideTubesVertCount);
            rightTubeExtruder.setUV(14, 15);
            collisionMeshExtruder = new BoxExtruder(trackWidth, 0.02666f);
            buildVolumeMeshExtruder = new BoxExtruder(trackWidth, 0.8f);
            buildVolumeMeshExtruder.closeEnds = true;
        }

        public override void sampleAt(TrackSegment4 trackSegment, float t)
        {
            base.sampleAt(trackSegment, t);
            var normal = trackSegment.getNormal(t);
            var trackPivot = getTrackPivot(trackSegment.getPoint(t), normal);
            var tangentPoint = trackSegment.getTangentPoint(t);
            var normalized = Vector3.Cross(normal, tangentPoint).normalized;
            var middlePoint = trackPivot + normalized * trackWidth / 2f;
            var middlePoint2 = trackPivot - normalized * trackWidth / 2f;
            var vector = trackPivot + normal * getCenterPointOffsetY();
            centerTubeExtruder.extrude(vector, tangentPoint, normal);
            leftTubeExtruder.extrude(middlePoint, tangentPoint, normal);
            rightTubeExtruder.extrude(middlePoint2, tangentPoint, normal);
            collisionMeshExtruder.extrude(trackPivot, tangentPoint, normal);
            if (liftExtruder != null)
                liftExtruder.extrude(vector - normal * (0.06713f + chainLiftHeight / 2f), tangentPoint, normal);
        }

        public override void afterExtrusion(TrackSegment4 trackSegment, GameObject putMeshOnGO)
        {
            base.afterExtrusion(trackSegment, putMeshOnGO);
        }

        public override Mesh getMesh(GameObject putMeshOnGo)
        {
            return MeshCombiner.start().add(centerTubeExtruder, leftTubeExtruder, rightTubeExtruder)
                .end(putMeshOnGo.transform.worldToLocalMatrix);
        }

        public override Mesh getCollisionMesh(GameObject putMeshOnGo)
        {
            return collisionMeshExtruder.getMesh(putMeshOnGo.transform.worldToLocalMatrix);
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
            return 0.3f;
        }
        

        public override float getTunnelWidth(TrackSegment4 trackSegment, float t)
        {
            return 0.6f;
        }

        public override float getTunnelHeight()
        {
            return 1.3f;
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
}