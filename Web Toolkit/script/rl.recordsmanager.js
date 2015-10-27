(function ($) {
    $.fn.extend({
        recordDetails: function (recordManagerUrl, recordUri, options) {
            var defaults = {
                style: "width: 600px; height: 500px"
            };
            var options = $.extend(defaults, options);
            var uri = recordManagerUrl + "/record/detailsbyuri?uri=" + recordUri + "&api=true";

            $(this).append("<div style='" + options.style + "'><iframe style='width: 100%; height: 100%' src='" + uri + "' frameborder='0' width='100%' height='100%'></iframe></div>");
        },
        recordAudit: function (recordManagerUrl, recordUri, options) {
            var defaults = {
                style: "width: 700px; height: 500px"
            };
            var options = $.extend(defaults, options);
            var uri = recordManagerUrl + "/audit/record?uri=" + recordUri + "&api=true";

            $(this).append("<div style='" + options.style + "'><iframe style='width: 100%; height: 100%' src='" + uri + "' frameborder='0' width='100%' height='100%'></iframe></div>");
        },
        recordDeclaration: function (recordManagerUrl, recordUri, options) {
            var defaults = {
                style: "width: 600px; height: 260px"
            };
            var options = $.extend(defaults, options);
            var uri = recordManagerUrl + "/record/recorddeclarationbyuri?uri=" + recordUri + "&api=true";

            $(this).append("<div style='" + options.style + "'><iframe style='width: 100%; height: 100%' src='" + uri + "' frameborder='0' width='100%' height='100%'></iframe></div>");
        },
        recordClassification: function (recordManagerUrl, recordUri, options) {
            var defaults = {
                style: "width: 600px; height: 360px"
            };
            var options = $.extend(defaults, options);
            var uri = recordManagerUrl + "/record/classifyrecordbyuri?uri=" + recordUri + "&api=true";

            $(this).append("<div style='" + options.style + "'><iframe style='width: 100%; height: 100%' src='" + uri + "' frameborder='0' width='100%' height='100%'></iframe></div>");
        },
        recordRequest: function (recordManagerUrl, recordUri, options) {
            var defaults = {
                style: "width: 700px; height: 500px"
            };
            var options = $.extend(defaults, options);
            var uri = recordManagerUrl + "/request/record?uri=" + recordUri + "&api=true";

            $(this).append("<div style='" + options.style + "'><iframe style='width: 100%; height: 100%' src='" + uri + "' frameborder='0' width='100%' height='100%'></iframe></div>");
        },
        legalHold: function (recordManagerUrl, itemUri, options) {
            var defaults = {
                style: "width: 600px; height: 260px"
            };
            var options = $.extend(defaults, options);
            var uri = recordManagerUrl + "/legalhold/createapi?uri=" + itemUri + "&api=true";

            $(this).append("<div style='" + options.style + "'><iframe style='width: 100%; height 100%' src='" + uri + "' frameborder='0' width='100%' height='100%'></iframe></div>");
        }
    });
})(jQuery);