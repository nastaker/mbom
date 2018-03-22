(function ($) {

    $.fn.mask = function (options) {
        var time = options ? options : 3000;
        
        return this.each(function () {
            var _this = $(this);

            if (_this.data("isMasking")) {
                return;
            }
            _this.data("isMasking", true);

            var mask = $("<div>").css({
                position: "absolute",
                left: 0,
                top: 0,
                width: "100%",
                height: "100%",
                opacity: "0.8",
                backgroundColor: "#000",
                zIndex: 99998
            });
            var text = $("<div>").css({
                position: "absolute",
                textAlign: "center",
                fontSize: "2em",
                color: "#fff",
                zIndex: 99999
            }).html(lang.loading);
            mask.appendTo(_this);
            text.appendTo(_this);
            text.css({
                top: mask.height() / 2 - text.height() / 2,
                left: mask.width() / 2 - text.width() / 2
            });

            setTimeout(function () {
                mask.remove();
                text.remove();
                _this.data("isMasking", false);
            }, time);
        });
    }

}(jQuery))

