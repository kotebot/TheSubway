using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Constants
    {
        public const float ChunkTunnelLength = 2.75f;
        public const float StationLength = ChunkTunnelLength * 8;
        public const float CountCarriage =  5;
        public const float MiddleTrain = StationLength / CountCarriage + 0.5f;//0.5f - magick;

        public const string Score = "Score";
    }

}
