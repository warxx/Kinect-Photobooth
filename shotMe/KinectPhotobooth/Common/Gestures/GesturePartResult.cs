using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectPhotobooth.Common.Gestures
{
    /// <summary>
    /// Represents the gesture part recognition result.
    /// </summary>
    public enum GesturePartResult
    {
        /// <summary>
        /// Gesture part failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Gesture part succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// Gesture part result undetermined.
        /// </summary>
        Undetermined
    }
}
