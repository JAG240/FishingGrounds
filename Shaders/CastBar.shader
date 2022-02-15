Shader "Unlit/CastBar"
{
    Properties
    {
        _BarFill("BarFill", Range(0,1)) = 0
        _SuccessZonePos("SuccessZonePos", Range(0.0,1.0)) = 0
        _SuccessRange("SuccessRange", float) = 0
        _SuccessZones("SucccessZones", Range(0,4)) = 0
        _FillColor("FillColor", Color) = (0,0,0,0)
        _SuccessZoneColor("SuccessZoneColor", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _BarFill;
            float _SuccessZonePos;
            float _SuccessRange;
            float4 _FillColor;
            float4 _SuccessZoneColor;
            int _SuccessZones;

            float InvLerp(float iMin, float iMax, float v)
            {
                return (v - iMin) / (iMax - iMin);
            }

            float Remap(float iMin, float iMax, float oMin, float oMax, float v)
            {
                float t = InvLerp(iMin, iMax, v);
                return lerp(oMin, oMax, t);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float value = i.uv.y > _BarFill;
                clip(value - 0.0001);

                if (i.uv.y + _SuccessRange > _SuccessZonePos && i.uv.y - _SuccessRange < _SuccessZonePos && value)
                    return _SuccessZoneColor;

                return float4(value.xxx * _FillColor, 1);
            }

            ENDCG
        }
    }
}
