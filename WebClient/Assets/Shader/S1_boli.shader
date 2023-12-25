// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33175,y:32633,varname:node_3138,prsc:2|custl-7335-OUT,alpha-9594-OUT,refract-6858-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32275,y:32312,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_ComponentMask,id:3966,x:32760,y:33224,varname:node_3966,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8319-OUT;n:type:ShaderForge.SFN_Multiply,id:6858,x:32977,y:33291,varname:node_6858,prsc:2|A-3966-OUT,B-2077-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2077,x:32760,y:33402,ptovrint:False,ptlb:Rafraction_Power,ptin:_Rafraction_Power,varname:_Rafraction_Power,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:3363,x:31871,y:32726,ptovrint:False,ptlb:Fresnel_power,ptin:_Fresnel_power,varname:_Fresnel_power,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_NormalVector,id:8319,x:31754,y:32541,prsc:2,pt:False;n:type:ShaderForge.SFN_Fresnel,id:593,x:32045,y:32466,varname:node_593,prsc:2|NRM-8319-OUT;n:type:ShaderForge.SFN_Multiply,id:8253,x:32574,y:32735,varname:node_8253,prsc:2|A-593-OUT,B-3363-OUT,C-7241-RGB;n:type:ShaderForge.SFN_Add,id:7335,x:32873,y:32752,varname:node_7335,prsc:2|A-8253-OUT,B-2046-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6946,x:32139,y:33454,ptovrint:False,ptlb:huidu,ptin:_huidu,varname:_huidu,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:1743,x:32139,y:33209,ptovrint:False,ptlb:Fanshe,ptin:_Fanshe,varname:_Fanshe,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2903-OUT;n:type:ShaderForge.SFN_Multiply,id:3680,x:32337,y:33303,varname:node_3680,prsc:2|A-1743-RGB,B-6946-OUT;n:type:ShaderForge.SFN_Set,id:4357,x:32503,y:33270,varname:Diffuse,prsc:2|IN-3680-OUT;n:type:ShaderForge.SFN_Get,id:2046,x:32550,y:32962,varname:node_2046,prsc:2|IN-4357-OUT;n:type:ShaderForge.SFN_NormalVector,id:7867,x:31436,y:33192,prsc:2,pt:False;n:type:ShaderForge.SFN_Transform,id:6950,x:31611,y:33209,varname:node_6950,prsc:2,tffrom:0,tfto:3|IN-7867-OUT;n:type:ShaderForge.SFN_ComponentMask,id:2736,x:31773,y:33209,varname:node_2736,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-6950-XYZ;n:type:ShaderForge.SFN_RemapRange,id:2903,x:31952,y:33209,varname:node_2903,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-2736-OUT;n:type:ShaderForge.SFN_VertexColor,id:7976,x:32153,y:32971,varname:node_7976,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9594,x:32962,y:32956,varname:node_9594,prsc:2|A-7241-A,B-7976-A,C-1743-A;proporder:7241-2077-3363-6946-1743;pass:END;sub:END;*/

Shader "Studio1/S1_boli" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _Rafraction_Power ("Rafraction_Power", Float ) = 0
        _Fresnel_power ("Fresnel_power", Float ) = 2
        _huidu ("huidu", Float ) = 0
        _Fanshe ("Fanshe", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform fixed4 _Color;
            uniform half _Rafraction_Power;
            uniform half _Fresnel_power;
            uniform half _huidu;
            uniform sampler2D _Fanshe; uniform float4 _Fanshe_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (i.normalDir.rg*_Rafraction_Power);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float2 node_2903 = (mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg*0.5+0.5);
                float4 _Fanshe_var = tex2D(_Fanshe,TRANSFORM_TEX(node_2903, _Fanshe));
                float3 Diffuse = (_Fanshe_var.rgb*_huidu);
                float3 finalColor = (((1.0-max(0,dot(i.normalDir, viewDirection)))*_Fresnel_power*_Color.rgb)+Diffuse);
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,(_Color.a*i.vertexColor.a*_Fanshe_var.a)),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
