# 👍 VRCThumbParams
VRChat Mod that makes Valve Index thumb positions accessible in Avatar V3 Parameters

## 💾 Installation

Simple! Drag "ThumbParam.dll" into your MelonLoader Mods folder and you're done with installation!

## 🛠 Avatar Setup

If you're here with the intention to use this mod for sign language, I reccomend checking out [ShiroXOkami' Guide](https://github.com/ShiroXOkami/ASL-Gestures-Simplified) for ASL gestures before reading this.

Now here's the part that can be tricky for some users. The mod simply reads the "touching" state of all Index controller face buttons and outputs them to two avatar params of "int" type.
You'll need to add these **case-sensitive** parameters to your avatar's base parameters:

* RightThumb
* LeftThumb

The mod will set these parameters with an integer from 0-4 representing the position of each thumb.

| Value | Real Position |
| ----- | ------------- |
| 0     | Not Touching  |
| 1     | A Button      |
| 2     | B Button      |
| 3     | Trackpad      |
| 4     | Thumbstick    |

Additionally, bool versions of the thumb positions are available to save on parameter space. They're mapped as:

* \[Left/Right]AButton
* \[Left/Right]BButton
* \[Left/Right]TrackPad
* \[Left/Right]ThumbStick

A special bool is also available to detect if the thumb is on either the A or B buttons. This is useful
on the Valve Index since generally when your thumb is resting in that area, both are being touched.

* \[Left/Right]ABButtons

From these, you're free to modify and/or play animations from your animation controllers however you wish!

## 📜 Disclaimer

As with all mods, any modification to VRChat's Client or SDK can lead to a ban or other punishment.<br>
I will not be held responsible for any punishments that you may be given for using this mod.<br> 
**USE AT YOUR OWN RISK**

## 👋 Credits
* [HerpDerpinstine/MelonLoader](https://github.com/HerpDerpinstine/MelonLoader)
