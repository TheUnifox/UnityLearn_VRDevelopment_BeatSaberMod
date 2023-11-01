//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Hidden/SkyGradient" {
Properties {
_GradientTex ("Texture", 2D) = "white" { }
_Color ("Color", Color) = (1,1,1,1)
}
SubShader {
 Pass {
  Blend One One, One One
  ZTest Always
  ZWrite Off
  Cull Off
  GpuProgramID 45673
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "UNITY_SINGLE_PASS_STEREO" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" "UNITY_SINGLE_PASS_STEREO" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "UNITY_SINGLE_PASS_STEREO" "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" "UNITY_SINGLE_PASS_STEREO" "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "UNITY_SINGLE_PASS_STEREO" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" "UNITY_SINGLE_PASS_STEREO" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "UNITY_SINGLE_PASS_STEREO" "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "ACES_TONE_MAPPING" "UNITY_SINGLE_PASS_STEREO" "USE_TONE_MAPPING" }
"// shader disassembly not supported on DXBC"
}
}
}
}
}