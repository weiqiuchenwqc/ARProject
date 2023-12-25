/******************************************************************************
 * DESCRIPTION: 阴影投射着色器
 * 
 *     Copyright (c) 2018, 谭伟俊 (TanWeijun)
 *     All rights reserved
 * 
 * COMPANY: Metek
 * CREATED: 2018.07.04, 14:35, CST
*******************************************************************************/

Shader "Studio1/ShadowCaster"
{
   Properties {
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
    
            ZWrite On
            ZTest LEqual
            //Cull Off
  
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            uniform fixed _Cutoff;
            uniform sampler2D _MainTex;
            uniform half4 _MainTex_ST;

            struct v2f
            {
                V2F_SHADOW_CASTER;
                half2 uv : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
    
            v2f vert(appdata_base v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
    
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D( _MainTex, i.uv );
                clip(texColor.a - _Cutoff);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
