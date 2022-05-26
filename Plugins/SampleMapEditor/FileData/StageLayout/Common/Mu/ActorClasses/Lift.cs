using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    [ByamlObject]
    public class Lift : MuObj, IRailableParams
    {
        /*
        bool IsObjPaintForResult
        bool RespawnReset
        bool WarpTargetPosBlockable
        bool WarpTargetPosCheckWall
        bool RunOverSubWeapon
        float FreeRotDeg__X
        float FreeRotDeg__Y
        float FreeRotDeg__Z
        float OneTimeRotDeg__X
        float OneTimeRotDeg__Y
        float OneTimeRotDeg__Z
        int OneTimeRotateDelay
        int OneTimeRotateFrame
        int RotateType
        int OneTimeRotateInterpolationType
        int RepeatOneTimeRotIntervalFrame

        // RailableParams

        int BindType
        bool IsClearPaintOnWarp
        bool CombineRotation
        bool CombineRailAndFreeRotate
        int RumbleType
        int RumbleTiming
        bool IsShowLiftAlertDbg
        int LiftAlertPredictFrame

        // DuplicatePoints ?
        //  getLinkDstUnitIds
        // ToSuperJumpSafePosBuffer
        //  getLinkDstUnitIds
        // ToLiftDangerousAreaBuffer
        //  getLinkDstUnitIds
        // ToNotPaintableArea
        //  getLinkDstUnitIds
        // ToLiftDemoDecoy
        //  getLinkDstUnitIds

        bool IsCalcVelGndOneMoreLong

        // PredictAlertableParams

        // (NULL) string StageViewDemoAnim

        bool IsCreateSpecificSLinkUserName
        bool IsAbleToBeCulled
         */

        [ByamlMember] public bool IsObjPaintForResult { get; set; }
        [ByamlMember] public bool RespawnReset { get; set; }
        [ByamlMember] public bool WarpTargetPosBlockable { get; set; }
        [ByamlMember] public bool WarpTargetPosCheckWall { get; set; }
        [ByamlMember] public bool RunOverSubWeapon { get; set; }
        [ByamlMember] public float FreeRotDeg__X { get; set; }
        [ByamlMember] public float FreeRotDeg__Y { get; set; }
        [ByamlMember] public float FreeRotDeg__Z { get; set; }
        [ByamlMember] public float OneTimeRotDeg__X { get; set; }
        [ByamlMember] public float OneTimeRotDeg__Y { get; set; }
        [ByamlMember] public float OneTimeRotDeg__Z { get; set; }
        [ByamlMember] public int OneTimeRotateDelay { get; set; }
        [ByamlMember] public int OneTimeRotateFrame { get; set; }
        [ByamlMember] public int RotateType { get; set; }
        [ByamlMember] public int OneTimeRotateInterpolationType { get; set; }
        [ByamlMember] public int RepeatOneTimeRotIntervalFrame { get; set; }


        // RailableParams
        [ByamlMember] public int RailableParams__StandbyFrame { get; set; }
        [ByamlMember] public int RailableParams__MoveFrame { get; set; }
        [ByamlMember] public int RailableParams__BreakFrame { get; set; }
        [ByamlMember] public int RailableParams__Patrol { get; set; }
        [ByamlMember] public int RailableParams__Interpolation { get; set; }
        [ByamlMember] public int RailableParams__Vel { get; set; }
        [ByamlMember] public float RailableParams__ConstantVelUnitLength { get; set; }
        [ByamlMember] public int RailableParams__AttCalc { get; set; }
        // End RailableParams


        [ByamlMember] public int BindType { get; set; }
        [ByamlMember] public bool IsClearPaintOnWarp { get; set; }
        [ByamlMember] public bool CombineRotation { get; set; }
        [ByamlMember] public bool CombineRailAndFreeRotate { get; set; }
        [ByamlMember] public int RumbleType { get; set; }
        [ByamlMember] public int RumbleTiming { get; set; }
        [ByamlMember] public bool IsShowLiftAlertDbg { get; set; }
        [ByamlMember] public int LiftAlertPredictFrame { get; set; }

        [ByamlMember] public bool IsCalcVelGndOneMoreLong { get; set; }

        [ByamlMember] public bool IsCreateSpecificSLinkUserName { get; set; }
        [ByamlMember] public bool IsAbleToBeCulled { get; set; }


#warning Clone func ugly
        public override Lift Clone()
        {
            Lift cloned = (Lift)base.Clone();

            cloned.IsObjPaintForResult = IsObjPaintForResult;
            cloned.RespawnReset = RespawnReset;
            cloned.WarpTargetPosBlockable = WarpTargetPosBlockable;
            cloned.WarpTargetPosCheckWall = WarpTargetPosCheckWall;
            cloned.RunOverSubWeapon = RunOverSubWeapon;
            cloned.FreeRotDeg__X = FreeRotDeg__X;
            cloned.FreeRotDeg__Y = FreeRotDeg__Y;
            cloned.FreeRotDeg__Z = FreeRotDeg__Z;
            cloned.OneTimeRotDeg__X = OneTimeRotDeg__X;
            cloned.OneTimeRotDeg__Y = OneTimeRotDeg__Y;
            cloned.OneTimeRotDeg__Z = OneTimeRotDeg__Z;
            cloned.OneTimeRotateDelay = OneTimeRotateDelay;
            cloned.OneTimeRotateFrame = OneTimeRotateFrame;
            cloned.RotateType = RotateType;
            cloned.OneTimeRotateInterpolationType = OneTimeRotateInterpolationType;
            cloned.RepeatOneTimeRotIntervalFrame = RepeatOneTimeRotIntervalFrame;
            cloned.RailableParams__StandbyFrame = RailableParams__StandbyFrame;
            cloned.RailableParams__MoveFrame = RailableParams__MoveFrame;
            cloned.RailableParams__BreakFrame = RailableParams__BreakFrame;
            cloned.RailableParams__Patrol = RailableParams__Patrol;
            cloned.RailableParams__Interpolation = RailableParams__Interpolation;
            cloned.RailableParams__Vel = RailableParams__Vel;
            cloned.RailableParams__ConstantVelUnitLength = RailableParams__ConstantVelUnitLength;
            cloned.RailableParams__AttCalc = RailableParams__AttCalc;
            cloned.BindType = BindType;
            cloned.IsClearPaintOnWarp = IsClearPaintOnWarp;
            cloned.CombineRotation = CombineRotation;
            cloned.CombineRailAndFreeRotate = CombineRailAndFreeRotate;
            cloned.RumbleType = RumbleType;
            cloned.RumbleTiming = RumbleTiming;
            cloned.IsShowLiftAlertDbg = IsShowLiftAlertDbg;
            cloned.LiftAlertPredictFrame = LiftAlertPredictFrame;
            cloned.IsCalcVelGndOneMoreLong = IsCalcVelGndOneMoreLong;
            cloned.IsCreateSpecificSLinkUserName = IsCreateSpecificSLinkUserName;
            cloned.IsAbleToBeCulled = IsAbleToBeCulled;

            return cloned;
        }
    }
}
