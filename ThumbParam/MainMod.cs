using System;
using System.Collections;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace ThumbParam
{
    public class MainMod : MelonMod
    {
        private AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique rightThumb, leftThumb;

        public override void VRChat_OnUiManagerInit()
        {
            MelonCoroutines.Start(UpdateParamStores());
            MelonLogger.Log(ConsoleColor.Cyan, "Initialized Sucessfully!");
        }

        IEnumerator UpdateParamStores()
        {
            for (;;)
            {
                yield return new WaitForSeconds(2);
                var tempRightThumb = GetParamIndex("RightThumb");
                var tempLeftThumb = GetParamIndex("LeftThumb");
                rightThumb = tempRightThumb != -1
                    ? (AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique) tempRightThumb
                    : AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique.None;
                leftThumb = tempLeftThumb != -1
                    ? (AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique) tempLeftThumb
                    : AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique.None;
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0?.field_Private_VRC_AnimationController_0?.field_Private_AvatarAnimParamController_0?.field_Private_AvatarPlayableController_0 == null)
                    continue;
                
                AvatarPlayableController controller = VRCPlayer.field_Internal_Static_VRCPlayer_0
                    .field_Private_VRC_AnimationController_0
                    .field_Private_AvatarAnimParamController_0
                    .field_Private_AvatarPlayableController_0;
                if (rightThumb != AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique.None)
                {
                    controller.Method_Public_Void_Int32_Boolean_0(tempRightThumb, true);
                    controller.Method_Public_Boolean_Int32_Boolean_PDM_0(tempRightThumb, true);
                    controller.Method_Public_Void_Int32_Boolean_1(tempRightThumb, true);
                    controller.Method_Public_Void_Int32_Boolean_2(tempRightThumb, true);
                }
                if (leftThumb != AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique.None)
                {
                    controller.Method_Public_Void_Int32_Boolean_0(tempLeftThumb, true);
                    controller.Method_Public_Boolean_Int32_Boolean_PDM_0(tempLeftThumb, true);
                    controller.Method_Public_Void_Int32_Boolean_1(tempLeftThumb, true);
                    controller.Method_Public_Void_Int32_Boolean_2(tempLeftThumb, true);
                }
            }
        }
        
        private static int GetParamIndex(string paramName)
        {
            VRCExpressionParameters.Parameter[] parameters = new VRCExpressionParameters.Parameter[0];

            if (VRCPlayer
                .field_Internal_Static_VRCPlayer_0?.prop_VRCAvatarManager_0?.prop_VRCAvatarDescriptor_0?.expressionParameters?.parameters != null)
            {
                parameters = VRCPlayer
                    .field_Internal_Static_VRCPlayer_0
                    .prop_VRCAvatarManager_0.prop_VRCAvatarDescriptor_0.expressionParameters
                    .parameters;

            }
            else
                return -1;
            
            var index = -1;
            for (var i = 0; i < parameters.Length; i++)
            {
                VRCExpressionParameters.Parameter param = parameters[i];
                if (param.name == null)
                    return -1;
                if (param.name == paramName)
                {
                    index = i;
                }
            }

            return index;
        }
        
        public override void OnUpdate()
        {
            if (VRCInputManager.field_Private_Static_Dictionary_2_String_ObjectPublicStSiBoSiObBoSiObStSiUnique_0[
                    "ThumbSpreadLeft"] == null ||
                VRCInputManager.field_Private_Static_Dictionary_2_String_ObjectPublicStSiBoSiObBoSiObStSiUnique_0[
                    "ThumbSpreadRight"] == null)
                return;

            var leftThumb = ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_ObjectPublicStSiBoSiObBoSiObStSiUnique_0[
                    "ThumbSpreadLeft"].field_Private_Single_6);

            GameObject.Find("InputManager").GetComponent<VRCInputProcessorKeyboard>().field_Private_ObjectPublicStSiBoSiObBoSiObStSiUnique_23 = new ObjectPublicStSiBoSiObBoSiObStSiUnique("");

            var rightThumb = ConvertToThumbState(VRCInputManager
                .field_Private_Static_Dictionary_2_String_ObjectPublicStSiBoSiObBoSiObStSiUnique_0[
                    "ThumbSpreadRight"].field_Private_Single_6);
            
            AvatarAnimParamController controller = null;
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0?.field_Private_VRC_AnimationController_0?.
                field_Private_AvatarAnimParamController_0 != null)
            {
                controller = VRCPlayer.field_Internal_Static_VRCPlayer_0
                    .field_Private_VRC_AnimationController_0.field_Private_AvatarAnimParamController_0;
            }
            
            SetParameter(controller, this.leftThumb, (int) leftThumb);
            SetParameter(controller, this.rightThumb, (int) rightThumb);
        }

        private void SetParameter(AvatarAnimParamController controller,
            AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique param, int state)
        {
            if (controller?.field_Private_AvatarPlayableController_0 != null && param != AvatarPlayableController.EnumNPublicSealedvaStNoSt18StStStStStUnique.None)
                controller.field_Private_AvatarPlayableController_0
                    .Method_Public_Void_EnumNPublicSealedvaStNoSt18StStStStStUnique_Single_0(
                        param, state);
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