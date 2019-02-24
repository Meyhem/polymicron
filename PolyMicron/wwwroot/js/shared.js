"use strict";
$(function () {
    $('.navbar-burger').click(function () {
        var $this = $(this);
        $this.toggleClass('is-active');
        $('#' + $this.data('target')).toggleClass('is-active');
    });

    $(".button[data-loader]").click(function () {
        var $this = $(this);
        $this.addClass('is-loading');
        $this.attr("disabled", true);
    });
});