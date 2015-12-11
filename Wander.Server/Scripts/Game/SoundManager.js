var SoundManager = (function () {
    function SoundManager(game, state) {
        this.game = game;
        this.state = state;
    }
    SoundManager.prototype.preload = function () {
        this.game.load.audio("dayAmbient", ["Content/Game/Sounds/ambient-day.mp3", "Content/Game/Sounds/ambient-day.ogg"]);
        this.game.load.audio("nightAmbient", ["Content/Game/Sounds/ambient-night.mp3", "Content/Game/Sounds/ambient-night.ogg"]);
        this.game.load.audio("rainAmbient", ["Content/Game/Sounds/ambient-rain.mp3", "Content/Game/Sounds/ambient-rain.ogg"]);
        this.game.load.audio("footstep", "Content/Game/Sounds/footstep.ogg");
    };
    SoundManager.prototype.create = function () {
        this.dayAmbientSound = this.game.add.audio("dayAmbient");
        this.nightAmbientSound = this.game.add.audio("nightAmbient");
        this.rainAmbientSound = this.game.add.audio("rainAmbient");
        this.dayAmbientSound.loop = true;
        this.nightAmbientSound.loop = true;
        this.rainAmbientSound.loop = true;
        this.dayAmbientSound.volume = 0;
        this.nightAmbientSound.volume = 0;
        this.rainAmbientSound.volume = 0;
        this.footstepSound = this.game.add.audio("footstep");
    };
    SoundManager.prototype.playFootStep = function (player) {
        if (currentState.map.currentPlayer == player) {
            this.footstepSound.volume = 1;
        }
        else {
            var dist = this.game.physics.arcade.distanceBetween(player.texture, currentState.map.currentPlayer.texture);
            var maxHearingDist = this.game.camera.width / 2;
            var t = 1 - (1 / (maxHearingDist)) * dist;
            t = t < 0 ? 0 : t;
            this.footstepSound.volume = t > 1 ? 1 : t;
        }
        this.footstepSound.play();
    };
    SoundManager.prototype.update = function () {
        if (this.state.dayNightCycle.isDay) {
            if (!this.dayAmbientSound.isPlaying)
                this.dayAmbientSound.play();
            this.nightAmbientSound.volume = Lerp(0, this.nightAmbientSound.volume, 0.01);
            this.dayAmbientSound.volume = Lerp(1, this.dayAmbientSound.volume, 0.01);
            if (this.nightAmbientSound.volume <= 0 && this.nightAmbientSound.isPlaying)
                this.nightAmbientSound.stop();
        }
        else {
            if (!this.nightAmbientSound.isPlaying)
                this.nightAmbientSound.play();
            this.nightAmbientSound.volume = Lerp(1, this.nightAmbientSound.volume, 0.01);
            this.dayAmbientSound.volume = Lerp(0, this.dayAmbientSound.volume, 0.01);
            if (this.dayAmbientSound.volume <= 0 && this.dayAmbientSound.isPlaying)
                this.dayAmbientSound.stop();
        }
        if (this.state.isRaining()) {
            if (!this.rainAmbientSound.isPlaying)
                this.rainAmbientSound.play();
            this.rainAmbientSound.volume = Lerp(1, this.rainAmbientSound.volume, 0.01);
        }
        else {
            this.rainAmbientSound.volume = Lerp(0, this.rainAmbientSound.volume, 0.01);
            if (this.rainAmbientSound.volume <= 0) {
                this.rainAmbientSound.stop();
            }
        }
    };
    return SoundManager;
})();
