using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPhotobooth.Common.Gestures
{
    /// <summary>
    /// The first part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //left hand in front of left Shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // Debug.WriteLine("GesturePart 0 - left hand in front of left Shoulder - PASS");
                // //left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.WriteLine("GesturePart 0 - left hand below shoulder height but above hip height - PASS");
                    // //left hand left of left Shoulder
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        // Debug.WriteLine("GesturePart 0 - left hand left of left Shoulder - PASS");
                        return GesturePartResult.Succeeded;
                    }

                    // Debug.WriteLine("GesturePart 0 - left hand left of left Shoulder - UNDETERMINED");
                    return GesturePartResult.Undetermined;
                }

                // Debug.WriteLine("GesturePart 0 - left hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.WriteLine("GesturePart 0 - left hand in front of left Shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The second part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment2 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //left hand in front of left Shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // Debug.WriteLine("GesturePart 1 - left hand in front of left Shoulder - PASS");
                // /left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // Debug.WriteLine("GesturePart 1 - left hand below shoulder height but above hip height - PASS");
                    // //left hand left of left Shoulder
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ShoulderRight].Position.X && body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        // Debug.WriteLine("GesturePart 1 - left hand left of left Shoulder - PASS");
                        return GesturePartResult.Succeeded;
                    }

                    // Debug.WriteLine("GesturePart 1 - left hand left of left Shoulder - UNDETERMINED");
                    return GesturePartResult.Undetermined;
                }

                // Debug.WriteLine("GesturePart 1 - left hand below shoulder height but above hip height - FAIL");
                return GesturePartResult.Failed;
            }

            // Debug.WriteLine("GesturePart 1 - left hand in front of left Shoulder - FAIL");
            return GesturePartResult.Failed;
        }
    }

    /// <summary>
    /// The third part of a <see cref="GestureType.SwipeRight"/> gesture.
    /// </summary>
    public class SwipeRightSegment3 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // //left hand in front of left Shoulder
            if (body.Joints[JointType.HandLeft].Position.Z < body.Joints[JointType.ElbowLeft].Position.Z && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // //left hand below shoulder height but above hip height
                if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.Head].Position.Y && body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.SpineBase].Position.Y)
                {
                    // //left hand left of left Shoulder
                    if (body.Joints[JointType.HandLeft].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }

                    return GesturePartResult.Undetermined;
                }

                return GesturePartResult.Failed;
            }

            return GesturePartResult.Failed;
        }
    }
}
