varying vec4 v_color;
varying vec2 v_texCoord0;
 
//texture samplers
uniform sampler2D u_sampler2D; //diffuse map
 
//additional parameters for the shader
uniform vec4 ambientColor;

 
void main() {
    vec4 diffuseColor = texture2D(u_sampler2D, v_texCoord0) * v_color;
    vec3 ambient = ambientColor.rgb * ambientColor.a;
    vec3 final = v_color.rgb * diffuseColor.rgb * ambient;
    gl_FragColor = vec4(final, diffuseColor.a);
}