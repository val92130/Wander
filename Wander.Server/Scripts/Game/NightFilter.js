var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Uniforms = (function () {
    function Uniforms() {
    }
    return Uniforms;
})();
var NightShader = (function (_super) {
    __extends(NightShader, _super);
    function NightShader(game, uniforms, fragmentSource) {
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
        _super.call(this, this.game, this.uniforms, this.fragmentSrc);
    }
    return NightShader;
})(Phaser.Filter);
