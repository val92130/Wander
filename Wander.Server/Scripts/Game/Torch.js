var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Light = (function (_super) {
    __extends(Light, _super);
    function Light(state, x, y, scale) {
        _super.call(this, state.game, x, y);
        this.anchor.setTo(0.5, 0.5);
        this.glow = this.game.add.image(x, y, 'blurred-circle');
        this.glow.anchor.setTo(0.5, 0.5);
        this.glowScale = scale;
        if (this.glowScale > 1)
            this.glowScale = 1;
        if (this.glowScale < 0)
            this.glowScale = 0;
        this.maxAlpha = 0.38;
        this.glow.blendMode = PIXI.blendModes.ADD;
        this.glow.alpha = this.maxAlpha;
        this.size = { width: this.glowScale + 0.08, height: this.glowScale + 0.08 };
        this.tween = this.game.add.tween(this.size).to({ width: this.glowScale, height: this.glowScale }, 2000, "Linear", true, 0, -1);
        this.tween.yoyo(true, 0);
    }
    Light.prototype.update = function () {
        this.glow.x = this.x;
        this.glow.y = this.y;
        var size = this.game.rnd.realInRange(this.glowScale, this.glowScale + 0.025);
        this.glow.scale.setTo(this.size.width, this.size.height);
        var gameState = currentState;
        if (gameState != undefined) {
            if (gameState.dayNightCycle.isDay) {
                this.alpha = Lerp(0, this.alpha, 0.01);
                this.glow.alpha = Lerp(0, this.alpha, 0.01);
            }
            else {
                this.alpha = Lerp(this.maxAlpha, this.alpha, 0.01);
                this.glow.alpha = Lerp(this.maxAlpha, this.alpha, 0.01);
            }
        }
    };
    return Light;
})(Phaser.Sprite);
