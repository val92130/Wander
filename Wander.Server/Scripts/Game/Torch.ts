class Light extends Phaser.Sprite {

    state: Phaser.State;
    glow: Phaser.Image;
    glowScale: number;
    maxAlpha: number;
    size: any;
    tween: Phaser.Tween;
    constructor(state:Phaser.State, x:number,y:number, scale:number) {
        super(state.game, x, y);
        this.anchor.setTo(0.5, 0.5);
        this.glow = this.game.add.image(x, y, 'blurred-circle');
        this.glow.anchor.setTo(0.5, 0.5);
        this.glowScale = scale;

        if (this.glowScale > 1) this.glowScale = 1;
        if (this.glowScale < 0) this.glowScale = 0;
        this.maxAlpha = 0.38;

        this.glow.blendMode = PIXI.blendModes.ADD;
        this.glow.alpha = this.maxAlpha;

        this.size = { width: this.glowScale + 0.08, height: this.glowScale + 0.08 };
        this.tween = this.game.add.tween(this.size).to({ width: this.glowScale, height: this.glowScale }, 2000, "Linear", true, 0, -1);
        this.tween.yoyo(true, 0);
    }

    update() {

        this.glow.x = this.x;
        this.glow.y = this.y;

        var size = this.game.rnd.realInRange(this.glowScale, this.glowScale + 0.025);
        this.glow.scale.setTo(this.size.width, this.size.height); 
        var gameState = currentState;

        if (gameState != undefined) {
            if (gameState.dayNightCycle.isDay) {
                this.alpha = Lerp(0, this.alpha, 0.01);
                this.glow.alpha = Lerp(0, this.alpha, 0.01);
            } else {
                this.alpha = Lerp(this.maxAlpha, this.alpha, 0.01);
                this.glow.alpha = Lerp(this.maxAlpha, this.alpha, 0.01);
            }
        }

    }

}