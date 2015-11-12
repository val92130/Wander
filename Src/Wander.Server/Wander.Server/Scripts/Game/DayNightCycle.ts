class DayNightCycle
{
    overlay: Phaser.Sprite;
    game: Phaser.Game;
    NightShader: NightShader;
    ambient: number;
    isDay: boolean;
    nightAmbient: number;

    constructor(game: Phaser.Game) {
        this.nightAmbient = 0.45;
        this.game = game;
        this.ambient = this.nightAmbient;
    }


    create() {
        this.overlay = this.game.add.sprite(0, 0, "Overlay");
        this.overlay.width = this.game.camera.width;
        this.overlay.height = this.game.camera.height;


        this.overlay.filters = [new NightShader(this.game, null, null)];
        this.isDay = true;
    }

    update() {
        
        if (this.isDay) {
            this.ambient = Lerp(0, this.ambient, 0.01);
            this.overlay.filters[0].uniforms.ambient.value = this.ambient;
            if (this.ambient == 0) {
                this.overlay.alpha = 0;
            }
        } else {
            this.overlay.alpha = 1;
            this.ambient = Lerp(this.nightAmbient, this.ambient, 0.01);
            this.overlay.filters[0].uniforms.ambient.value = this.ambient;
        }
        
    }

    setNight() {
        this.isDay = false;
    }

    setDay() {
        this.isDay = true;
    }
}