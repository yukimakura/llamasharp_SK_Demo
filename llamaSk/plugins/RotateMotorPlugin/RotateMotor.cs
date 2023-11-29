using System.ComponentModel;
using System.Globalization;
using Microsoft.SemanticKernel;

namespace LLamaSK.Plugins.RotateMotorPlugin;
public class RotateMotor
{
    [SKFunction, Description("モーターを回すことを要求され、方向を与えられたら、モーターを回します")]
    public string Rotate([Description("方向")] string input)
    {
        return $"{input}側のモーターをまわします！boooooooo!";
    }
}
