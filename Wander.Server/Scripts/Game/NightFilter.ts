class Uniforms {
    ambient: any;
}  

class NightShader extends Phaser.Filter {

    uniforms: Uniforms;

    constructor(game: Phaser.Game, uniforms: any, fragmentSource: string[]) {
        this.uniforms = new Uniforms();
        this.uniforms.ambient = { type: '1f', value: 0.85 };

        this.fragmentSrc = [


            "precision mediump float;",
            "varying vec2 vTextureCoord;",
            "uniform sampler2D uSampler;",
            "uniform float      ambient;",
            "void main(void) {",

            "vec4 texColor = texture2D(uSampler, vTextureCoord);",
            "texColor = vec4(0, 0, 0.18,ambient);",

            "gl_FragColor = texColor;",

            "}"
        ];

        super(this.game, this.uniforms, this.fragmentSrc);
    }

}