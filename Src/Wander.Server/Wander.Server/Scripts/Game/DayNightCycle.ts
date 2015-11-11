class DayNightCycle
{
    overlay: Phaser.Sprite;
    game: Phaser.Game;
    NightShader: NightShader;
    ambient: number;
    isDay: boolean;

    constructor(game: Phaser.Game) {
        this.game = game;
        this.ambient = 0.80;

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
            if (this.ambient > 0) {
                this.ambient -= 0.01;
                this.overlay.filters[0].uniforms.ambient.value = this.ambient;
            }
        } else {
            if (this.ambient < 0.80) {
                this.ambient += 0.01;
                this.overlay.filters[0].uniforms.ambient.value = this.ambient;
            }
        }
        

        
    }

    setNight() {
        this.isDay = false;
    }

    setDay() {
        this.isDay = true;
    }
}