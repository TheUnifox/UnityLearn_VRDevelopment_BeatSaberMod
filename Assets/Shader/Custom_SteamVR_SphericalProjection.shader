//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Custom/SteamVR_SphericalProjection" {
Properties {
_MainTex ("Base (RGB)", 2D) = "white" { }
_N ("N (normal of plane)", Vector) = (0,0,0,0)
_Phi0 ("Phi0", Float) = 0
_Phi1 ("Phi1", Float) = 1
_Theta0 ("Theta0", Float) = 0
_Theta1 ("Theta1", Float) = 1
_UAxis ("uAxis", Vector) = (0,0,0,0)
_VAxis ("vAxis", Vector) = (0,0,0,0)
_UOrigin ("uOrigin", Vector) = (0,0,0,0)
_VOrigin ("vOrigin", Vector) = (0,0,0,0)
_UScale ("uScale", Float) = 1
_VScale ("vScale", Float) = 1
}
SubShader {
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Fog {
   Mode Off
  }
  GpuProgramID 29809
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "UNITY_SINGLE_PASS_STEREO" }
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
}
}
}
}