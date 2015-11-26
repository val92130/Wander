class SoundManager {

    dayAmbientSound: Phaser.Sound;
    nightAmbientSound: Phaser.Sound;
    rainAmbientSound: Phaser.Sound;
    game: Phaser.Game;
    state: GameState;

    constructor(game: Phaser.Game, state: GameState) {
        this.game = game;
        this.state = state;
    }

    preload() {
        this.game.load.audio("dayAmbient", ["Content/Game/Sounds/ambient-day.mp3", "Content/Game/Sounds/ambient-day.ogg"]);
        this.game.load.audio("nightAmbient", ["Content/Game/Sounds/ambient-night.mp3", "Content/Game/Sounds/ambient-night.ogg"]);
        this.game.load.audio("rainAmbient", ["Content/Game/Sounds/ambient-rain.mp3", "Content/Game/Sounds/ambient-rain.ogg"]);
    }

    create() {
        this.dayAmbientSound = this.game.add.audio("dayAmbient");
        this.nightAmbientSound = this.game.add.audio("nightAmbient");
        this.rainAmbientSound = this.game.add.audio("rainAmbient");

        this.dayAmbientSound.loop = true;
        this.nightAmbientSound.loop = true;
        this.rainAmbientSound.loop = true;

        this.dayAmbientSound.volume = 0;
        this.nightAmbientSound.volume = 0;
        this.rainAmbientSound.volume = 0;
    }


    update() {
        if (this.state.dayNightCycle.isDay) {
            if (!this.dayAmbientSound.isPlaying) this.dayAmbientSound.play();           
            this.nightAmbientSound.volume = Lerp(0, this.nightAmbientSound.volume, 0.01);
            this.dayAmbientSound.volume = Lerp(1, this.dayAmbientSound.volume, 0.01);
            if (this.nightAmbientSound.volume <= 0 && this.nightAmbientSound.isPlaying) this.nightAmbientSound.stop();

        } else {
            if (!this.nightAmbientSound.isPlaying) this.nightAmbientSound.play();   
            this.nightAmbientSound.volume = Lerp(1, this.nightAmbientSound.volume, 0.01);
            this.dayAmbientSound.volume = Lerp(0, this.dayAmbientSound.volume, 0.01);
            if (this.dayAmbientSound.volume <= 0 && this.dayAmbientSound.isPlaying) this.dayAmbientSound.stop();
        }

        if (this.state.isRaining()) {
            if (!this.rainAmbientSound.isPlaying) this.rainAmbientSound.play();
            this.rainAmbientSound.volume = Lerp(1, this.rainAmbientSound.volume, 0.01);
        } else {
            this.rainAmbientSound.volume = Lerp(0, this.rainAmbientSound.volume, 0.01);
            if (this.rainAmbientSound.volume <= 0) {
                this.rainAmbientSound.stop();
            }
        }
    }
}