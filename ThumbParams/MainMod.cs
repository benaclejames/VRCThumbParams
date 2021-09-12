using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using MelonLoader;
using ThumbParams;
using UnityEngine;
using ParamLib;

[assembly: MelonInfo(typeof(MainMod), "ThumbParams", "1.2.5", "benaclejames")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ThumbParams
{
    public class MainMod : MelonMod
    {
        private readonly IntBaseParam _rightThumbParam = new IntBaseParam("RightThumb"), _leftThumbParam = new IntBaseParam("LeftThumb");

        private Assembly _assemblyCSharp;
        private Type _uiManager;
        private MethodInfo _uiManagerInstance;
        private bool _shouldCheckUiManager;
        
        public override void OnApplicationStart()
        {
            _assemblyCSharp = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
            _shouldCheckUiManager = typeof(MelonMod).GetMethod("VRChat_OnUiManagerInit") == null;
        }
        
        public override void VRChat_OnUiManagerInit() => UiManagerInit();
        
        private void UiManagerInit()
        {
            MelonCoroutines.Start(UpdateParamStores());
            MelonLogger.Msg(ConsoleColor.Cyan, "Initialized Successfully!");
        }

        private void CheckUiManager()
        {
            if (_assemblyCSharp == null) return;
            
            if (_uiManager == null) _uiManager = _assemblyCSharp.GetType("VRCUiManager");
            if (_uiManager == null) {
                _shouldCheckUiManager = false;
                return;
            }
            
            if (_uiManagerInstance == null)
                _uiManagerInstance = _uiManager.GetMethods().First(x => x.ReturnType == _uiManager);
            if (_uiManagerInstance == null)
            {
                _shouldCheckUiManager = false;
                return;
            }

            if (_uiManagerInstance.Invoke(null, new object[0]) == null)
                return;

            _shouldCheckUiManager = false;
            UiManagerInit();
        }
        
        private IEnumerator UpdateParamStores()
        {
            for (;;)
            {
                yield return new WaitForSeconds(2);
                _rightThumbParam.ResetParam();
                _leftThumbParam.ResetParam();
            }
        }

        public override void OnUpdate()
        {
            if (_shouldCheckUiManager) CheckUiManager();
            
            if (VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadLeft"] == null ||
                VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadRight"] == null)
                return;

            _leftThumbParam.ParamValue = (int)ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadLeft"].field_Public_Single_0);
            
            _rightThumbParam.ParamValue = (int)ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadRight"].field_Public_Single_0);
        }

        enum ThumbState
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
    }
}