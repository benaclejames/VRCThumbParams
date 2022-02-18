using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using ThumbParams;
using UnityEngine;
using ParamLib;

[assembly: MelonInfo(typeof(MainMod), "ThumbParams", "1.2.7", "benaclejames")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ThumbParams
{
    public class MainMod : MelonMod
    {
        private readonly List<IMappedThumbState<BaseParam>> _mappedThumbStates = new List<IMappedThumbState<BaseParam>>();
                
        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(UpdateParamStores());
            RegisterDirectMapped("Thumb");
            RegisterSingleMapped("AButton", ThumbState.ABUTTON);
            RegisterSingleMapped("BButton", ThumbState.BBUTTON);
            RegisterSingleMapped("TrackPad", ThumbState.TRACKPAD);
            RegisterSingleMapped("ThumbStick", ThumbState.THUMBSTICK);
            RegisterMultiMapped("ABButtons", ThumbState.ABUTTON, ThumbState.BBUTTON);
            
            MelonLogger.Msg(ConsoleColor.Cyan, "Initialized Successfully!");
        }

        private void RegisterDirectMapped(string param)
        {
            _mappedThumbStates.Add(DirectMappedThumbState.Of(param, false));
            _mappedThumbStates.Add(DirectMappedThumbState.Of(param, true));
        }

        private void RegisterSingleMapped(string param, ThumbState targetState)
        {
            _mappedThumbStates.Add(SingleMappedThumbState.Of(param, false, targetState));
            _mappedThumbStates.Add(SingleMappedThumbState.Of(param, true, targetState));
        }

        private void RegisterMultiMapped(string param, params ThumbState[] targetStates)
        {
            _mappedThumbStates.Add(MultiMappedThumbState.Of(param, false, targetStates));
            _mappedThumbStates.Add(MultiMappedThumbState.Of(param, true, targetStates));
        }

        private IEnumerator UpdateParamStores()
        {
            for (;;)
            {
                yield return new WaitForSeconds(2);
                _mappedThumbStates.ForEach(mappedState => mappedState.UpdateBaseParam());
            }
        }

        public override void OnUpdate()
        {
            if (VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadLeft"] == null ||
                VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadRight"] == null)
                return;
            
            var leftState = ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadLeft"].field_Public_Single_0);
            var rightState = ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadRight"].field_Public_Single_0);
            
            _mappedThumbStates.ForEach(mappedState => mappedState.UpdateFrom(mappedState.IsLeft() ? leftState : rightState));
        }

        private enum ThumbState
        {
            THUMBUP,
            ABUTTON,
            BBUTTON,
            TRACKPAD,
            THUMBSTICK
        }

        private ThumbState ConvertToThumbState(float floatThumbState)
        {
            switch (floatThumbState)
            {
                case 0.66f:
                    return ThumbState.THUMBUP;
                case 0.7f:
                    return ThumbState.THUMBUP;
                case 0.4f:
                    return ThumbState.ABUTTON;
                case 0.35f:
                    return ThumbState.BBUTTON;
                case 0.6f:
                    return ThumbState.TRACKPAD;
                case 0.9f:
                    return ThumbState.THUMBSTICK; 
            }

            return ThumbState.THUMBUP;
        }

        private interface IMappedThumbState<out T> where T : BaseParam
        {

            void UpdateFrom(ThumbState state);

            void UpdateBaseParam();

            bool IsLeft();
        }

        private abstract class MappedThumbState<T> : IMappedThumbState<T> where T : BaseParam
        {
            protected readonly T Param;
            private readonly bool _isLeft;

            protected MappedThumbState(T param, bool isLeft)
            {
                Param = param;
                _isLeft = isLeft;
            }

            public abstract void UpdateFrom(ThumbState state);

            public void UpdateBaseParam()
            {
                Param.ResetParam();
            }

            public bool IsLeft()
            {
                return _isLeft;
            }
        }

        private class SingleMappedThumbState : MappedThumbState<BoolBaseParam>
        {
            private readonly ThumbState _targetState;

            private SingleMappedThumbState(BoolBaseParam param, bool isLeft, ThumbState targetState) : base(param, isLeft)
            {
                _targetState = targetState;
            }

            public override void UpdateFrom(ThumbState state)
            {
                Param.ParamValue = state == _targetState;
            }

            internal static MappedThumbState<BoolBaseParam> Of(string param, bool isLeft, ThumbState state)
            {
                return new SingleMappedThumbState(new BoolBaseParam((isLeft ? "Left" : "Right") + param), isLeft, state);
            }
        }

        private class MultiMappedThumbState : MappedThumbState<BoolBaseParam>
        {
            private readonly ThumbState[] _targetStates;

            private MultiMappedThumbState(BoolBaseParam param, bool isLeft, ThumbState[] targetStates) : base(param, isLeft)
            {
                _targetStates = targetStates;
            }

            public override void UpdateFrom(ThumbState state)
            {
                Param.ParamValue = _targetStates.Any(target => state == target);
            }

            internal static MappedThumbState<BoolBaseParam> Of(string param, bool isLeft, ThumbState[] states)
            {
                return new MultiMappedThumbState(new BoolBaseParam((isLeft ? "Left" : "Right") + param), isLeft, states);
            }
        }

        private class DirectMappedThumbState : MappedThumbState<IntBaseParam>
        {
            private DirectMappedThumbState(IntBaseParam param, bool isLeft) : base(param, isLeft)
            {
                
            }

            public override void UpdateFrom(ThumbState state)
            {
                Param.ParamValue = (int) state;
            }

            internal static MappedThumbState<IntBaseParam> Of(string param, bool isLeft)
            {
                return new DirectMappedThumbState(new IntBaseParam((isLeft ? "Left" : "Right") + param), isLeft);
            }
        }
    }
}