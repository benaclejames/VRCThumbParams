using System;
using System.Collections;
using MelonLoader;
using ThumbParams;
using UnityEngine;
using ParamLib;

[assembly: MelonInfo(typeof(MainMod), "ThumbParams", "1.2.2", "benaclejames")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ThumbParams
{
    public class MainMod : MelonMod
    {
        private IntBaseParam rightThumbParam = new IntBaseParam("RightThumb"), leftThumbParam = new IntBaseParam("LeftThumb");

        public override void VRChat_OnUiManagerInit()
        {
            MelonCoroutines.Start(UpdateParamStores());
            MelonLogger.Msg(ConsoleColor.Cyan, "Initialized Sucessfully!");
        }
        

        IEnumerator UpdateParamStores()
        {
            for (;;)
            {
                yield return new WaitForSeconds(2);
                rightThumbParam.ResetParam();
                leftThumbParam.ResetParam();
            }
        }

        public override void OnUpdate()
        {
            if (VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadLeft"] == null ||
                VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadRight"] == null)
                return;

            leftThumbParam.ParamValue = (int)ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_VRCInput_0[
                    "ThumbSpreadLeft"].field_Public_Single_0);
            
            rightThumbParam.ParamValue = (int)ConvertToThumbState(VRCInputManager
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