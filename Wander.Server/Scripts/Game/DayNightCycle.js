var DayNightCycle = (function () {
    function DayNightCycle(game) {
        this.nightAmbient = 0.45;
        this.game = game;
        this.ambient = this.nightAmbient;
    }
    DayNightCycle.prototype.create = function () {
        this.overlay = this.game.add.sprite(0, 0, "Overlay");
        this.overlay.width = this.game.camera.width;
        this.overlay.height = this.game.camera.height;
        this.overlay.filters = [new NightShader(this.game, null, null)];
        this.isDay = true;
    };
    DayNightCycle.prototype.update = function () {
        if (this.isDay) {
            this.ambient = Lerp(0, this.ambient, 0.01);
            this.overlay.filters[0].uniforms.ambient.value = this.ambient;
            if (this.ambient == 0) {
                this.overlay.alpha = 0;
            }
        }
        else {
            this.overlay.alpha = 1;
            this.ambient = Lerp(this.nightAmbient, this.ambient, 0.01);
            this.overlay.filters[0].uniforms.ambient.value = this.ambient;
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
