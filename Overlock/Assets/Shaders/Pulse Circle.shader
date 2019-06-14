Shader "Custom/Pulse Circle"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_PulseAmount("Pulse Amount", Float) = 0.5
		_LineWidth("Line Width", Float) = 0.5
		_MinRadius("Minimum Radius", Float) = 0.3
    }
    SubShader
    {
        Tags {
		"Queue" = "Transparent"
		"RenderType" = "Transparent" }
        LOD 100
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			fixed4 _Color;
			float _PulseAmount;
			float _LineWidth;
			float _MinRadius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float scaledPulseAmount = lerp(_MinRadius, 1, _PulseAmount);

				float2 vectorToCenter = i.uv - float2(0.5, 0.5);
				vectorToCenter *= 2;
				float distanceToCenterSquared = vectorToCenter.x * vectorToCenter.x + vectorToCenter.y * vectorToCenter.y;
				clip(scaledPulseAmount * scaledPulseAmount - distanceToCenterSquared);
				float innerRadius = scaledPulseAmount - _LineWidth;
				clip(distanceToCenterSquared - innerRadius * innerRadius);

                // sample the texture
                fixed4 col = _Color;

				float a = 1 - _PulseAmount;
				a = clamp(a, 0, 1);
				//a = a * a;
				col.a = a;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
