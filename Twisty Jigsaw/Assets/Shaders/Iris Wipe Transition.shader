Shader "Custom/Iris Wipe Transition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_WipeAmount("Wipe Amount", Float) = 0.5
		_WipeCenter("Wipe Center", Vector) = (0, 0, 0, 0)
		//_AspectRatio("Aspect Ratio", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
			//sampler2D _WipeTex;
			float _WipeAmount;
			//float _AspectRatio;
			float4 _WipeCenter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//float2 correctedUV = float2((i.uv.x - 0.5) * _AspectRatio + 0.5f, i.uv.y);
				float2 scaledUV = i.uv * _ScreenParams;
				float2 vectorFromCenter = scaledUV - _WipeCenter.xy;
				// I don't take the square root because it would be slow and unnecessary
				float distanceSquared = vectorFromCenter.x * vectorFromCenter.x + vectorFromCenter.y * vectorFromCenter.y;
				float cutoffDistanceSquared = _WipeAmount * _WipeAmount * (_ScreenParams.x * _ScreenParams.x + _ScreenParams.y * _ScreenParams.y);
				clip(distanceSquared - cutoffDistanceSquared);


                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
