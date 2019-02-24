$(function () {

    $('.user-comment').css('visibility', 'visible');

    if (localStorage) {
        
        var commentName = localStorage.getItem('comment-name');
        if (commentName) {
            var $nameInput = $('#name-input');
            $nameInput.val(commentName);
            console.log($nameInput.val());
            $nameInput.attr("readonly", true);
        }
    }

    $('#submit-comment-button').click(function () {
        var $self = $(this);
        var name = $('#name-input').val();
        if (name && localStorage) {
            localStorage.setItem("comment-name", name);
        }

        $(this).closest('form').submit();
    });
});