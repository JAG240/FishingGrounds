Shader "Unlit/CastBar"
{
    Properties
    {
        _BarFill("BarFill", Range(0,1)) = 0
        _SuccessZonePos("SuccessZonePos", Range(0.0,1.0)) = 0
        _SuccessRange("SuccessRange", Range(0.0, 0.4)) = 0
        _FillColor("FillColor", Color) = (0,0,0,0)

        _GreatSize("GreatSize", Range(0.0, 1.0)) = 0.2
        _GoodSize("Goodsize", Range(0.0 ,1.0)) = 0.3

        _GreatColor("GreatSuccessZoneColor", Color) = (0,0,0,0)
        _GoodColor("GoodSuccessZoneColor", Color) = (0,0,0,0)
        _OkayColor("OkaySuccessZoneColor", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags   
        { 
            "RenderType"="Opaque"
            "Queue"="Overlay"
        }

        ZTest Always

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
            float _GreatSize;
            float _GoodSize;
            float4 _FillColor;
            float4 _GreatColor;
            float4 _GoodColor;
            float4 _OkayColor;

            float4 GetZoneColor(float uvy)
            {
                if (uvy + _GreatSize > _SuccessZonePos && uvy - _GreatSize < _SuccessZonePos)
                    return _GreatColor;
                else if (uvy + _GoodSize > _SuccessZonePos && uvy - _GoodSize < _SuccessZonePos)
                    return _GoodColor;
                else
                    return _OkayColor;
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

                if (i.uv.y + _SuccessRange > _SuccessZonePos && i.uv.y - _SuccessRange < _SuccessZonePos && value)
                    return GetZoneColor(i.uv.y);

                return float4(value.xxx * _FillColor, 1);
            }

            ENDCG
        }
    }
}
