using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPhotobooth.Common.Gestures
{
    /// <summary>
    /// Represents a single gesture segment which uses relative positioning of body parts to detect a gesture.
    /// </summary>
    public interface IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>A GesturePartResult based on whether the gesture part has been completed.</returns>
        GesturePartResult Update(Body body);
    }
}
