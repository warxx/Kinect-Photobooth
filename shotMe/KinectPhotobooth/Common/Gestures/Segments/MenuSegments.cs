using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPhotobooth.Common.Gestures.Segments
{
    /// <summary>
    /// The first part of a <see cref="GestureType.Menu"/> gesture.
    /// </summary>
    public class MenuSegment1 : IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        public GesturePartResult Update(Body body)
        {
            // Left and right hands below hip
            if (body.Joints[JointType.HandLeft].Position.Y < body.Joints[JointType.SpineBase].Position.Y && body.Joints[JointType.HandRight].Position.Y < body.Joints[JointType.SpineBase].Position.Y)
            {
                // left hand 0.3 to left of center hip
                if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.SpineBase].Position.X - 0.3)
                {
                    // left hand 0.2 to left of left elbow
                    if (body.Joints[JointType.HandLeft].Position.X < body.Joints[JointType.ElbowLeft].Position.X - 0.2)
                    {
                        return GesturePartResult.Succeeded;
                    }
                }

                return GesturePartResult.Undetermined;
            }

            return GesturePartResult.Failed;
        }
    }
}
