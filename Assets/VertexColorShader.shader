Shader "Custom/VertexColorShader"
{
    Properties
    {
    }

    SubShader
    {
        Pass
        {
            // Set the rendering queue to be drawn after the opaque objects
            // This is necessary to ensure that the vertex colors are blended correctly
            // with the objects that are drawn behind them
            //Queue 1000

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            VertexOutput vert(VertexInput input)
            {
                VertexOutput output;

                output.vertex = UnityObjectToClipPos(input.vertex);
                output.color = input.color;

                return output;
            }

            fixed4 frag(VertexOutput input) : SV_Target
            {
                return input.color;
            }
            ENDCG
        }
    }
}
