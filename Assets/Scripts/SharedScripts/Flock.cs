//using UnityEngine;
//using System.Collections;
//using Unity.Collections;
//using System.Collections.Generic;
////using UnityEngine.Jobs;
////using Unity.Jobs;
////using Unity.Mathematics;

////using Unity.Entities;
////using Unity.Transforms;
////using Eustress.FlockJobs;


//////class DelayFrameBarrier : BarrierSystem
//////{}

//////public struct Distance : IComponentData { public float3 Value; }
//////////public struct EarlyPosition : IComponentData { public float3 Value; }

//////////public struct FlockMoveJob: IJobParallelForTransform
//////////{

//////////    public float deltaTime;
//////////    public float RandomNumNorm;

//////////    public Vector3 targetLocalPos;

//////////    public float centerWeight;
//////////    public float velocityWeight;
//////////    public float separationWeight;
//////////    public float followWeight;
//////////    public float randomizeWeight;

//////////    public float maxVelocity;
//////////    public float minVelocity;

//////////    public Vector3 flockCenter;
//////////    public Vector3 flockVelocity;

//////////   // public NativeArray<FlockBoidVelocity> flockRBVelocities;

//////////    [ReadOnly]
//////////    public EntityCommandBuffer earlyPositionCommandBuffer;



//////////    public void Execute(int index, TransformAccess flockTransform)
//////////    {
//////////        EarlyPosition earlyPos = new EarlyPosition { Value = 0 };


//////////       earlyPositionCommandBuffer.SetComponent(earlyPos = new EarlyPosition { Value= flockTransform.position});

//////////        var deltaVelocity = flockTransform.position - (Vector3)earlyPos.Value/deltaTime;

//////////        Vector3 dVelocity = new Vector3(deltaVelocity.x, deltaVelocity.y, deltaVelocity.z);

//////////        Vector3 center = flockCenter - flockTransform.localPosition;          // cohesion
//////////        Vector3 velocity = flockVelocity - dVelocity;           // alignment
//////////        Vector3 follow = targetLocalPos - flockTransform.localPosition; // follow leader
//////////        Vector3 separation = Vector3.zero;                                          // separation

//////////        Vector3 randomize = new Vector3((RandomNumNorm * 2) - 1, (RandomNumNorm * 2) - 1, (RandomNumNorm * 2) - 1);

//////////        randomize.Normalize();

//////////        var steer = centerWeight * center +
//////////                    velocityWeight * velocity +
//////////                    separationWeight * separation +
//////////                    followWeight * follow +
//////////                    randomizeWeight * randomize;


//////////        Vector3 relativePos = steer * deltaTime;

//////////        if (float.IsNaN(relativePos.x))
//////////            return;

//////////        if (relativePos != Vector3.zero)
//////////            dVelocity = relativePos;




//////////        // enforce minimum and maximum speeds for the boids
//////////        float speed = dVelocity.magnitude;
//////////        if (speed >= maxVelocity)
//////////        {
//////////            dVelocity = dVelocity.normalized * maxVelocity;
//////////        }
//////////        else if (speed < minVelocity)
//////////        {
//////////            dVelocity = dVelocity.normalized * minVelocity;
//////////        }

//////////        flockTransform.position += dVelocity ;

//////////    }

//////////}
/////// <summary>
/////// This script is the modification of the implementation of the Boids behaviors from http://www.unifycommunity.com/wiki/index.php?title=Flocking
/////// </summary>
//////public struct FlockBoid : IComponentData { }
////namespace Eustress.Jobs
////{
////    //public class Flocks : MonoBehaviour { }


////    public class FlockingBehavior { }


////    //[UpdateAfter(typeof(FollowTargetSystem))]
////    //[UpdateBefore(typeof(HeadingSystem))]


////    //[UpdateBefore(typeof(Position))]
////    // [UpdateInGroup(typeof(FlockingBehavior))]
////    // [UpdateAfter(typeof(HeadingSystem))]
////    //UpdateAfter(typeof(CopyTransformFromGameObject))]
////    public class FlockSystem : JobComponentSystem
////   {


////        public float centerWeight = 8f;
////        public float velocityWeight = 5f;
////        public float separationWeight = 500f;
////        public float followWeight = 1f;
////        public float randomizeWeight = 10f;

////        public float maxVelocity = 5f;
////        public float minVelocity = 50f;

////        //public Vector3 flockCenter;
////        //public Vector3 flockVelocity;

////        public struct Target
////        {
////         public ComponentDataArray<FlockBoidTarget> flockBoidEntity;
////         [ReadOnly] public ComponentDataArray<Position> positionData;
////         [ReadOnly] public ComponentDataArray<Rotation> rotationData;
////         [ReadOnly] public EntityArray entity;
////        }
////        [Inject] public Target flockTarget;

////        public ComponentGroup m_Flocks;
////        //public ComponentGroup m_MainGroup;

////        protected override void OnCreateManager(int capacity)
////        {

////         m_Flocks = GetComponentGroup((typeof(Position)), (typeof(Heading)), (typeof(Rotation)), ComponentType.ReadOnly(typeof(MoveSpeed)));

////        // m_MainGroup = GetComponentGroup(typeof(SpawnFlocks), typeof(Position));
////        }

////        float3 targetPos;
////        quaternion targetRot;


////       public JobHandle flockJobHandle;

////        EndFrameBarrier endFramebarrier;
////        protected override JobHandle OnUpdate(JobHandle inputDeps)
////        {

////            //flockJobHandle.Complete();
////            // EntityCommandBuffer endFrameCBuffer = endFramebarrier.CreateCommandBuffer();
////            // targetPos = endFrameCBuffer. flockTarget.positionData[0].Value;
////            targetPos = flockTarget.positionData[0].Value;
////           // targetRot = flockTarget.rotationData[0].Value;

////            FlockJob flockJob = new FlockJob
////            {
////                //rotations = m_Flocks.GetComponentDataArray<Rotation>(),
////                positions = m_Flocks.GetComponentDataArray<Position>(),
////                moveSpeed = m_Flocks.GetComponentDataArray<MoveSpeed>(),
////                minVelocity = this.minVelocity,
////                maxVelocity = this.maxVelocity,
////                centerWeight = this.centerWeight,
////                velocityWeight = this.velocityWeight,
////                separationWeight = this.separationWeight,
////                followWeight = this.followWeight,
////                randomizeWeight = this.randomizeWeight,
////                targetPos = targetPos,


////                RandomNumNorm = UnityEngine.Random.value,
////                deltaTime = Time.deltaTime,
////            };

////           // flockJobHandle = flockJob.Schedule(m_Flocks.CalculateLength(), 10, inputDeps);
////           // flockJobHandle.Complete();
////            return flockJobHandle;
////        }
////        // public float3 targetPos = 

////        //public float3 targetPos =flockta// flockTarget.positionData[0].Value;

////        //FlockJob flockJob = new FlockJob
////        //{
////        //    //targetPos = 



////        //}




////        struct FlockJob : IJobParallelFor
////        {
////            //public ComponentDataArray<Rotation> rotations;

////            public ComponentDataArray<Position> positions;
////            //public ComponentDataArray<Position> lastFramepositions;
////            //public ComponentDataArray<Position> CurrentFramepositions;
////            [ReadOnly]
////            public ComponentDataArray<MoveSpeed> moveSpeed;

////            //[ReadOnly]
////            //public float3 targetPos;
////            // [DeallocateOnJobCompletion]
////            // [ReadOnly] public ComponentDataArray<Position> 
////            public float3 targetPos;
////            public quaternion targetRot;

////            public float deltaTime;
////            public float RandomNumNorm;

////            public float centerWeight;
////            public float velocityWeight;
////            public float separationWeight;
////            public float followWeight;
////            public float randomizeWeight;

////            public float maxVelocity;
////            public float minVelocity;

////            public float3 flockCenter;
////            public float3 flockVelocity;

////            // public NativeArray<FlockBoidVelocity> flockRBVelocities;

////        //    [ReadOnly]
////          //  public EntityCommandBuffer earlyPositionCommandBuffer;


////            float3 center;
////            float3 velocity;
////            float3 follow;
////            float3 separation;
////            float3 dVelocity;
////            Vector3 steer;

////            float3 randomize;
////            float3 relativePos;
////            public void Execute(int i)
////            {
////                var targetPosition = targetPos;//[0].Value;
////                                               //dVelocity = Vector3.Magnitude((targetPosition - positions[i].Value) * moveSpeed[i].speed) * deltaTime;// / deltaTime;
////                                               ////dVelocity = Vector3.Magnitude(CurrentFramepositions[i].Value - lastFramepositions[i].Value / deltaTime);
////                                               //center = flockCenter - positions[i].Value;  // cohesion
////                                               //velocity = flockVelocity - dVelocity;          // alignment
////                                               //follow = targetPosition - positions[i].Value; // follow leader
////                                               //separation = float3.zero;                                          // separation



////                //relativePos = targetPosition - positions[i].Value;
////                //separation += relativePos / (math.sqrt(Vector3.Magnitude(relativePos)));



////                //randomize = new Vector3((RandomNumNorm * 2) - 1, (RandomNumNorm * 2) - 1, (RandomNumNorm * 2) - 1);

////                //math.normalize(randomize);

////                //steer = (center * centerWeight) + (velocity * velocityWeight) + (follow * followWeight) + (randomize * randomizeWeight);


////                //relativePos = steer * deltaTime;

////                //if (float.IsNaN(relativePos.x))
////                //    return;

////                ////if (relativePos != float3.zero)
////                ////    dVelocity = relativePos;

////                //// enforce minimum and maximum speeds for the boids
////                //float speed = Vector3.Magnitude(dVelocity);

////                //if (speed >= maxVelocity)
////                //{
////                //    dVelocity = math.normalize(dVelocity) * maxVelocity;
////                //}
////                //else if (speed < minVelocity)
////                //{
////                //    dVelocity = math.normalize(dVelocity) * minVelocity;
////                //}
////                //rotations[i] = new Rotation
////                //{

////                // Value = (rotations[i].Value +
////                // math.normalize(new float3(targetPosition.x, targetPosition.y, targetPosition.z) - new float3(positions[i].Value.x, positions[i].Value.y, positions[i].Value.z)) * (deltaTime * moveSpeed[i].speed)),//positions[i].Value + dVelocity,




////                //};


////                positions[i] = new Position
////                {

////                    Value = (positions[i].Value + 
////                  math.normalize( new float3(targetPosition.x, targetPosition.y, targetPosition.z) - new float3(positions[i].Value.x, positions[i].Value.y, positions[i].Value.z)) * (deltaTime * moveSpeed[i].speed)),//positions[i].Value + dVelocity,




////                };
////                //rotations[i] = new Rotation
////                //{

////                //    Value = targetRot




////                //};


////            }
////        }
////  }
////}



//////public class FlockBoidComponent : ComponentDataWrapper<FlockBoid> { }
//public class Flock : MonoBehaviour
//{


//    internal FlockControlSystem controller;

//    private Rigidbody thisRigidBody;

//    void Awake()
//    {

//        thisRigidBody = GetComponent<Rigidbody>();

//    }



//    void Update()
//    {


//        if (controller != null)
//        {
//            Vector3 relativePos = steer() * Time.deltaTime;

//            if (float.IsNaN(relativePos.x))
//                return;

//            if (relativePos != Vector3.zero)
//                thisRigidBody.velocity = relativePos;



//            // enforce minimum and maximum speeds for the boids
//            float speed = thisRigidBody.velocity.magnitude;
//            if (speed > controller.maxVelocity)
//            {
//                thisRigidBody.velocity = thisRigidBody.velocity.normalized * controller.maxVelocity;
//            }
//            else if (speed < controller.minVelocity)
//            {
//                thisRigidBody.velocity = thisRigidBody.velocity.normalized * controller.minVelocity;
//            }
//        }
//    }

//  //  Calculate flock steering Vector based on the Craig Reynold's algorithm (Cohesion, Alignment, Follow leader and Seperation)
//private Vector3 steer()
//    {
//        Vector3 center = controller.flockCenter - transform.localPosition;          // cohesion
//        Vector3 velocity = controller.flockAverageVelocity - thisRigidBody.velocity;            // alignment
//        Vector3 follow = controller.target.localPosition - transform.localPosition; // follow leader
//        Vector3 separation = Vector3.zero;                                          // separation

//        foreach (Flock flock in controller.flockList)
//        {
//            if (flock != this)
//            {
//                Vector3 relativePos = transform.localPosition - flock.transform.localPosition;
//                separation += relativePos / (relativePos.sqrMagnitude);

//            }
//        }

//        // randomize
//        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

//        randomize.Normalize();

//        return (controller.centerWeight * center +
//                controller.velocityWeight * velocity +
//                controller.separationWeight * separation +
//                controller.followWeight * follow +
//                controller.randomizeWeight * randomize);
//    }
//}