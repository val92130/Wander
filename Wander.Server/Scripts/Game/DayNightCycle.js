var DayNightCycle = (function () {
    function DayNightCycle(game) {
        this.nightAmbient = 0.48;
        this.rainAmbient = 0.3;
        this.game = game;
        this.ambient = 0;
    }
    DayNightCycle.prototype.create = function () {
        this.overlay = this.game.add.sprite(0, 0, "Overlay");
        this.overlay.width = this.game.camera.width;
        this.overlay.height = this.game.camera.height;
        this.rainOverlay = this.game.add.sprite(0, 0, "Overlay");
        this.rainOverlay.width = this.game.camera.width;
        this.rainOverlay.height = this.game.camera.height;
        this.overlay.filters = [new NightShader(this.game, null, null)];
        this.rainOverlay.filters = [new NightShader(this.game, null, null)];
        this.isDay = true;
    };
    DayNightCycle.prototype.update = function () {
        if (currentState.isRaining()) {
            this.rainOverlay.alpha = 1;
            this.rainOverlay.filters[0].uniforms.ambient.value = Lerp(this.rainAmbient, this.rainOverlay.filters[0].uniforms.ambient.value, 0.01);
        }
        else {
            this.rainOverlay.filters[0].uniforms.ambient.value = Lerp(0, this.rainOverlay.filters[0].uniforms.ambient.value, 0.001);
            if (this.rainOverlay.filters[0].uniforms.ambient.value <= 0) {
                this.rainOverlay.alpha = 0;
            }
        }
        if (this.isDay) {
            this.ambient = Lerp(0, this.ambient, 0.001);
            this.overlay.filters[0].uniforms.ambient.value = this.ambient;
            if (this.ambient == 0) {
            }
        }
        else {
            //this.overlay.alpha = 1;
            this.ambient = Lerp(this.nightAmbient, this.ambient, 0.001);
            this.overlay.filters[0].uniforms.ambient.value = this.ambient;
            this.rainOverlay.alpha = 0;
        }
    };
    DayNightCycle.prototype.setNight = function () {
        this.isDay = false;
    };
    DayNightCycle.prototype.setDay = function () {
        this.isDay = true;
    };
    return DayNightCycle;
})();
//# sourceMappingURL=DayNightCycle.js.map